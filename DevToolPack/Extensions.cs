using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevToolPack
{
    public static class Extensions
    {
        public static Binding Bind(this BaseEdit edit, object obj, string property)
        {
            edit.DataBindings.Clear();
            return edit.DataBindings.Add(nameof(edit.EditValue), obj, property, true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
