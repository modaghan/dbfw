namespace DevToolPack
{
    partial class DxCombo
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
            this.cmb = new DevExpress.XtraEditors.ButtonEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cmb.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cmb
            // 
            this.cmb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmb.Location = new System.Drawing.Point(0, 0);
            this.cmb.Name = "cmb";
            this.cmb.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.cmb.Properties.NullText = "[EditValue is null]";
            this.cmb.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmb.Size = new System.Drawing.Size(379, 20);
            this.cmb.TabIndex = 0;
            this.cmb.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.ButtonClick);
            // 
            // DxCombo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmb);
            this.Name = "DxCombo";
            this.Size = new System.Drawing.Size(379, 20);
            ((System.ComponentModel.ISupportInitialize)(this.cmb.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.ButtonEdit cmb;
    }
}
