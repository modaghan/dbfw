
namespace DevToolPack.Views
{
    partial class TreeCollectionSelector
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.treelist = new DevExpress.XtraTreeList.TreeList();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.treelist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            this.SuspendLayout();
            // 
            // treelist
            // 
            this.treelist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treelist.Location = new System.Drawing.Point(0, 0);
            this.treelist.Name = "treelist";
            this.treelist.Size = new System.Drawing.Size(785, 466);
            this.treelist.TabIndex = 0;
            // 
            // TreeCollectionSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(785, 466);
            this.Controls.Add(this.treelist);
            this.IconOptions.ShowIcon = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TreeCollectionSelector";
            this.ShowInTaskbar = false;
            this.Text = "TreeCollectionSelector";
            ((System.ComponentModel.ISupportInitialize)(this.treelist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraTreeList.TreeList treelist;
        private System.Windows.Forms.BindingSource bs;
    }
}