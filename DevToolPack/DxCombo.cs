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
    public partial class DxCombo : DevExpress.XtraEditors.XtraUserControl, INotifyPropertyChanged
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
        public IEntityCollection Collection { get; set; }
        public string ForeignKey { get; set; }
        public bool IsValidated { get; private set; }
        public string CantSaveReason { get; private set; }
        DbContext Context;

        public event EventHandler OnChanged;
        public event EventHandler OnCollectionOpen;

        public DxCombo()
        {

            this.ValueMember = "id";
            this.IsActiveMember = "is_active";
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                this.cmb.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
                this.cmb.EditValueChanged += GLook_OnChanged;
            }
        }

        private void GLook_OnChanged(object sender, EventArgs e)
        {
            OnChanged?.Invoke(this, null);
        }

        public void BindData<T>(object Entity, IEntityCollection selector, string foreign_key)
        {
            Bind<T>(Entity, selector, foreign_key);
        }

        private void Bind<T>(object Entity, IEntityCollection selector, string foreign_key)
        {
            ForeignType = typeof(T);
            this.Entity = Entity;
            this.ForeignKey = foreign_key;
            this.Collection = selector;
            cmb.DataBindings.Add(nameof(GridLookUpEdit.EditValue), Entity, ForeignKey, true, DataSourceUpdateMode.OnPropertyChanged).Parse += Validater;
            cmb.EditValue = EditValue = Entity.GetType().GetProperty(ForeignKey).GetValue(Entity);
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

        private void ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            switch (e.Button.Kind)
            {
                case DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis:                    
                    OnCollectionOpen?.Invoke(this, e);
                    break;

                default:
                    break;
            }
        }
    }
}