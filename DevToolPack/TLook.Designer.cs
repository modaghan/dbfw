namespace DevToolPack
{
    partial class TLook
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.DataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cmb = new DevExpress.XtraEditors.TreeListLookUpEdit();
            this.TView = new DevExpress.XtraTreeList.TreeList();
            ((System.ComponentModel.ISupportInitialize)(this.DataBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TView)).BeginInit();
            this.SuspendLayout();
            // 
            // cmb
            // 
            this.cmb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmb.Location = new System.Drawing.Point(0, 0);
            this.cmb.Name = "cmb";
            this.cmb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Clear),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Search)});
            this.cmb.Properties.DataSource = this.DataBindingSource;
            this.cmb.Properties.TreeList = this.TView;
            this.cmb.Size = new System.Drawing.Size(379, 20);
            this.cmb.TabIndex = 0;
            this.cmb.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.ButtonClick);
            this.cmb.EditValueChanged += new System.EventHandler(this.cmb_EditValueChanged);
            // 
            // TView
            // 
            this.TView.KeyFieldName = "id";
            this.TView.Location = new System.Drawing.Point(0, 0);
            this.TView.Name = "TView";
            this.TView.OptionsView.ShowIndentAsRowStyle = true;
            this.TView.ParentFieldName = "parent_id";
            this.TView.Size = new System.Drawing.Size(400, 200);
            this.TView.TabIndex = 0;
            this.TView.CustomColumnDisplayText += new DevExpress.XtraTreeList.CustomColumnDisplayTextEventHandler(this.TView_CustomColumnDisplayText);
            // 
            // TLook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmb);
            this.Name = "TLook";
            this.Size = new System.Drawing.Size(379, 20);
            ((System.ComponentModel.ISupportInitialize)(this.DataBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource DataBindingSource;
        private DevExpress.XtraTreeList.TreeList TView;
        public DevExpress.XtraEditors.TreeListLookUpEdit cmb;
    }
}
