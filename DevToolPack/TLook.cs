using DevExpress.XtraEditors;
using MS.BLL;
using MSDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevToolPack
{
    public partial class TLook : LookBase,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public Type ForeignType { get; private set; }
        public object Entity { get; set; }
        private object _EditValue { get; set; }
        public object EditValue {
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
        public string ValueMember { get; set; }
        public string ParentValueMember { get; set; }
        public string IsActiveMember { get; set; }
        public long SelectedId { get; set; }
        public IEntityView Dialog { get; set; }
        public List<object> DataList { get; private set; }
        public string ForeignKey { get; set; }
        public bool IsValidated { get; private set; }
        public string CantSaveReason { get; private set; }
        public string NullText {
            get
            {
                return cmb.Properties.NullText;
            }
            set
            {
                cmb.Properties.ShowNullValuePrompt = value != null ? ShowNullValuePromptOptions.NullValue:ShowNullValuePromptOptions.Default;
                cmb.Properties.NullText = cmb.Properties.NullValuePrompt = value;
            }
        }

        public event EventHandler OnClear, OnRefresh, OnAdd, OnUpdate;

        public event DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler OnCustomDisplay;

        public event EventHandler ValueChanged;
        DbContext Context;

        public TLook()
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
        /// <summary>
        /// TLook Bağlama
        /// </summary>
        /// <typeparam name="T">Taşınacak Tür</typeparam>
        /// <param name="Entity">İlşkilenecek Entity</param>
        /// <param name="dialog">Crud Editörü</param>
        /// <param name="foreign_key">İlişkilenecek Entity'deki foreign key</param>
        /// <param name="parentValueMember">Türdeki üst dizin propertysi</param>
        /// <param name="columnsRM">Localization Resource</param>
        /// <param name="cols">Gösterilecek sütunlar</param>
        public void BindData<T>(object Entity, IEntityView dialog, string foreign_key, string parentValueMember, Type columnsRM = null, params string[] cols)
        {
            ForeignType = typeof(T);
            this.ValueMember = ValueMember ?? "id";
            this.ParentValueMember = parentValueMember;
            this.IsActiveMember = IsActiveMember ?? "is_active";
            this.Entity = Entity;
            this.Dialog = dialog;
            cmb.DataBindings.Clear();
            ForeignKey = foreign_key;
            cmb.Properties.ValueMember = ValueMember;
            cmb.DataBindings.Add(nameof(GridLookUpEdit.EditValue), Entity, ForeignKey, true, DataSourceUpdateMode.OnPropertyChanged).Parse += Validater;
            TView.Columns.Clear();
            TView.ParentFieldName = ParentValueMember;
            TView.KeyFieldName = ValueMember;
            if (columnsRM != null)
            {
                int index = 0;
                foreach (string col in cols)
                    TView.Columns.Add(new DevExpress.XtraTreeList.Columns.TreeListColumn
                    {
                        FieldName = col,
                        Name = $"col_{Entity.GetType().Name}_{ForeignType.Name}_{col}",
                        Caption = new ResourceManager(columnsRM).GetString(col),
                        Visible = true,
                        VisibleIndex = index++
                    });
            }
            cmb.EditValue = EditValue = Entity.GetType().GetProperty(ForeignKey).GetValue(Entity);
            cmb.Properties.Buttons[1].Visible = cmb.EditValue != null;
        }

        public async Task LoadData(DbContext ctx = null)
        {
            if (ctx != null)
                this.Context = ctx;
            TView.ShowLoadingPanel();
            DataList = await Task.Run(() =>
            {
                DbSet _dbSet = this.Context.Set(ForeignType);
                var list = _dbSet.ToListAsync();
                if (ForeignType.GetProperty(IsActiveMember) == null)
                    list.Result.ToList();
                return list.Result.Where(x => (bool)x.GetType().GetProperty(IsActiveMember).GetValue(x)).ToList();
            });
            DataBindingSource.DataSource = DataList;
            TView.BestFitColumns();
            if (Entity != null)
                cmb.EditValue = EditValue = Entity.GetType().GetProperty(ForeignKey).GetValue(Entity);
            TView.HideLoadingPanel();
            cmb.Refresh();
        }

        public async Task<T> GetView<T>(T entity)
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

        public async Task<object> GetSelected()
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
                this.ValueChanged(sender, e);
        }

        private void TView_CustomColumnDisplayText(object sender, DevExpress.XtraTreeList.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Column.FieldName.Equals(ParentValueMember))
                    e.DisplayText = DataList.GetForeignString(e.Value);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToJson());
            }
        }

        private void GView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            OnCustomDisplay?.Invoke(sender, e);
        }

        private async void ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            switch (e.Button.Kind)
            {
                case DevExpress.XtraEditors.Controls.ButtonPredefines.Search:
                    await LoadData();
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