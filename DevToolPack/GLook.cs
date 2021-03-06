using DevExpress.XtraEditors;
using MS.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevToolPack
{
    public partial class GLook : LookBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Type ForeignType { get; private set; }

        private object _EditValue { get; set; }
        public object EditValue
        {
            get
            {
                return _EditValue;
            }
            set
            {
                _EditValue = value;
                OnPropertyChanged(nameof(EditValue));
            }
        }
        public object Entity { get; set; }
        public string ValueMember { get; set; }
        public string IsActiveMember { get; set; }
        public long SelectedId { get; set; }
        public IEntityView Dialog { get; set; }
        public List<object> DataList { get; private set; }
        public string ForeignKey { get; set; }
        public bool IsValidated { get; private set; }
        public string CantSaveReason { get; private set; }
        public IStatic Static { get; set; }
        public DbContext Context { get; set; }
        private string Filename;

        public event EventHandler OnClear, OnRefresh, OnAdd, OnUpdate;

        public event DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler OnCustomDisplay;

        public event EventHandler ValueChanged;

        public GLook()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                DataBindingSource = new System.Windows.Forms.BindingSource();
                cmb.Properties.DataSource = DataBindingSource;
                this.cmb.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
                this.cmb.Properties.PopupSizeable = false;
            }

        }


        public void BindData<T>(object Entity, IEntityView dialog, string foreign_key)
        {
            Bind<T>(Entity, dialog, foreign_key, null, null);
        }
        public void BindData<T>(object Entity, IEntityView dialog, string foreign_key, Type columnsRM, string grouped = null, params string[] cols)
        {
            Bind<T>(Entity, dialog, foreign_key, columnsRM, cols, grouped);
        }

        private void Bind<T>(object Entity, IEntityView dialog, string foreign_key, Type columnsRM, string[] cols, string grouped = null)
        {
            ForeignType = typeof(T);
            this.ValueMember = ValueMember ?? "id";
            this.IsActiveMember = IsActiveMember ?? "is_active";
            this.Entity = Entity;
            this.Dialog = dialog;
            cmb.DataBindings.Clear();
            ForeignKey = foreign_key;
            cmb.Properties.ValueMember = ValueMember;
            cmb.DataBindings.Add(nameof(GridLookUpEdit.EditValue), Entity, ForeignKey, true, DataSourceUpdateMode.OnPropertyChanged).Parse += Validater;
            GView.Columns.Clear();
            int index = 0;
            if (columnsRM != null && cols != null)
                foreach (string col in cols)
                    GView.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn
                    {
                        FieldName = col,
                        Name = $"col_{Entity.GetType().Name}_{ForeignType.Name}_{col}",
                        Caption = new ResourceManager(columnsRM).GetString(col),
                        Visible = true,
                        GroupIndex = col == grouped ? 1 : -1,
                        VisibleIndex = index++
                    });
            cmb.EditValue = EditValue = Entity.GetType().GetProperty(ForeignKey).GetValue(Entity);
            cmb.Properties.Buttons[1].Visible = cmb.EditValue != null;


            Filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Layouts", $"GLook_{ForeignType.Name}.xml");
            if (File.Exists(Filename))
                GView.RestoreLayoutFromXml(Filename);
            GView.Layout += GView_Layout;
        }
        private void GView_Layout(object sender, EventArgs e)
        {
            if (Filename != null)
                GView.SaveLayoutToXml(Filename);
        }
        public async Task LoadData(DbContext ctx, IStatic stc)
        {
            Context = ctx;
            Static = stc;
            if (Static.TypeExists(ForeignType))
                await LoadData(Static.GetList(ForeignType));
            else
                await LoadData();
        }


        public async Task LoadData(object dataList = null)
        {
            GView.ShowLoadingPanel();
            if (dataList == null)
                DataList = await Task.Run(() =>
                {
                    DbSet _dbSet = Context.Set(ForeignType);
                    var list = _dbSet.ToListAsync();
                    if (ForeignType.GetProperty(IsActiveMember) == null)
                        list.Result.ToList();
                    return list.Result.Where(x => (bool)x.GetType().GetProperty(IsActiveMember).GetValue(x)).ToList();
                });
            DataBindingSource.DataSource = dataList != null ? dataList : DataList;
            GView.BestFitColumns();
            if (Entity != null)
                cmb.EditValue = EditValue = Entity.GetType().GetProperty(ForeignKey).GetValue(Entity);
            GView.HideLoadingPanel();
            cmb.Refresh();
        }

        public override async Task<T> GetView<T>(T entity)
        {
            Dialog.ShowDialog();
            if (Dialog.Object != null)
            {
                object id = Dialog.Object.GetType().GetProperty("id").GetValue(Dialog.Object);
                entity.GetType().GetProperty(ForeignKey).SetValue(entity, id);
                Entity = entity;
                await LoadData();
            }
            return entity;
        }

        private void Validater(object sender, ConvertEventArgs e)
        {
            var context = new ValidationContext(Entity, serviceProvider: null, items: null);
            List<ValidationResult> ValidationResults = new List<ValidationResult>();
            IsValidated = Validator.TryValidateObject(Entity, context, ValidationResults, true);
            CantSaveReason = string.Empty;
            foreach (ValidationResult validationResult in ValidationResults)
            {
                CantSaveReason += $"• {validationResult.ErrorMessage}\n";
            }
        }
        public override async Task<object> GetSelected()
        {
            if ((EditValue = cmb.EditValue) == null)
                return null;
            return await Task.Run(() =>
            {
                DbSet _dbSet = Context.Set(ForeignType);
                return _dbSet.Find(EditValue);
            });
        }

        private void cmb_EditValueChanged(object sender, EventArgs e)
        {
            cmb.Properties.Buttons[1].Visible = cmb.EditValue != null;
            EditValue = cmb.EditValue;
            SelectedId = cmb.EditValue.ToLong();
            if (ValueChanged != null)
                this.ValueChanged(this, e);
        }

        private void GView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            OnCustomDisplay?.Invoke(sender, e);
        }

        private void ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            switch (e.Button.Kind)
            {
                case DevExpress.XtraEditors.Controls.ButtonPredefines.Search:
                    OnRefresh?.Invoke(this, e);
                    break;

                case DevExpress.XtraEditors.Controls.ButtonPredefines.Plus:
                    OnAdd?.Invoke(this, e);
                    break;

                case DevExpress.XtraEditors.Controls.ButtonPredefines.Clear:
                    cmb.EditValue = null;
                    OnClear?.Invoke(this, e);
                    break;

                case DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph:
                    OnUpdate?.Invoke(this, e);
                    break;

                default:
                    break;
            }
        }
    }
}