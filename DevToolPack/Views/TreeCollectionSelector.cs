using DevExpress.Utils;
using DevExpress.Utils.Behaviors.Common;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevToolPack.Views
{
    public partial class TreeCollectionSelector : DevExpress.XtraEditors.XtraForm
    {
        private string Filename;
        public Type BaseType { get; set; }
        public TreeCollectionSelector()
        {
            InitializeComponent();
        }
        public void Set<T>()
        {

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string layoutPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Layouts");
            Directory.CreateDirectory(layoutPath);
            Filename = Path.Combine(layoutPath, $"{nameof(TreeCollectionSelector)}_{BaseType.Name}_Layout.xml");
            if (File.Exists(Filename))
                treelist.RestoreLayoutFromXml(Filename);
            treelist.Layout += View_Layout;
        }
        private void View_Layout(object sender, EventArgs e)
        {
            if (Filename != null)
                treelist.SaveLayoutToXml(Filename);
        }
    }
}