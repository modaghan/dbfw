using DevExpress.XtraEditors;
using MS.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
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
        public string ParentMember { get; set; }
        public Type ForeignType { get; private set; }
        public long SelectedId { get; set; }
        public IEntityView Dialog { get; set; }
        public List<object> DataList { get; private set; }
        public string ForeignKey { get; set; }
        public bool IsValidated { get; private set; }
        public string CantSaveReason { get; private set; }
        DbContext Context;

        public event EventHandler OnClear;

        public event DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler OnCustomDisplay;

        public event EventHandler ValueChanged;
        public event EventHandler OnChanged;

        public GLook()
        {
            InitializeComponent();
            this.cmb.EditValueChanged += GLook_OnChanged;
        }

        private void GLook_OnChanged(object sender, EventArgs e)
        {
            OnChanged?.Invoke(this, null);
        }
        public void BindData<T>(object Entity, IEntityView dialog, string foreign_key, string parentValueMember = null)
        {
            ParentMember = parentValueMember;
            Bind<T>(Entity, dialog, foreign_key);
        }

        private void Bind<T>(object Entity, IEntityView dialog, string foreign_key)
        {
            ForeignType = typeof(T);
            this.Entity = Entity;
            this.Dialog = dialog;
            cmb.DataBindings.Clear();
            ForeignKey = foreign_key;
            cmb.DataBindings.Add(nameof(ButtonEdit.EditValue), Entity, ForeignKey, true, DataSourceUpdateMode.OnPropertyChanged).Parse += Validater;
            cmb.EditValue = EditValue = Entity.GetType().GetProperty(ForeignKey).GetValue(Entity);
            cmb.Properties.Buttons[0].Visible = cmb.EditValue != null;
        }
        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
        }

        public override async Task LoadData(DbContext ctx = null)
        {
            if (ctx != null)
                Context = ctx;
        }

        public async Task LoadEntity<T>(DbContext ctx = null)
        {
            try
            {
                if (ctx != null)
                    Context = ctx;
                await Task.Run(() =>
                {
                    DbSet _dbSet = this.Context.Set(ForeignType);
                    var objEnt = _dbSet.Find(EditValue.ToLong());
                    if (objEnt != null && objEnt.GetType().Equals(typeof(T)))
                    {
                        T foreignEntity = (T)objEnt;
                        cmb.Text = foreignEntity.ToStr();
                    }
                });
            }
            catch (Exception ex)
            {
                 
            }
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
        public override async Task<object> GetSelected()
        {
            if ((EditValue = cmb.EditValue) == null)
                return null;
            return await Task.Run(() =>
            {
                try
                {
                    DbSet _dbSet = Context.Set(ForeignType);
                    if (!EditValue.GetType().Equals(typeof(long)))
                        return null;
                    return _dbSet.Find(EditValue);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        private void cmb_EditValueChanged(object sender, EventArgs e)
        {
            cmb.Properties.Buttons[0].Visible = cmb.EditValue != null;
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

                case DevExpress.XtraEditors.Controls.ButtonPredefines.Clear:
                    cmb.EditValue = null;
                    OnClear?.Invoke(this, e);
                    break;
                case DevExpress.XtraEditors.Controls.ButtonPredefines.Search:
                    if(ParentMember == null)
                    {
                        ForeignSelector foreignSelector = new ForeignSelector(Context, ForeignType);
                        if (foreignSelector.ShowDialog() == DialogResult.OK)
                        {
                            cmb.EditValue = EditValue = foreignSelector.Entity.GetType().GetProperty("id").GetValue(foreignSelector.Entity);
                            cmb.Text = foreignSelector.Entity.ToStr();
                        }
                    }
                    else
                    {
                        ForeignTSelector foreignSelector = new ForeignTSelector(Context, ForeignType,ParentMember);
                        if (foreignSelector.ShowDialog() == DialogResult.OK)
                        {
                            cmb.EditValue = EditValue = foreignSelector.Entity.GetType().GetProperty("id").GetValue(foreignSelector.Entity);
                            cmb.Text = foreignSelector.Entity.ToStr();
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    }
}