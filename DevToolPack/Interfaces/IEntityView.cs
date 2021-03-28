using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevToolPack
{
    public interface IEntityView<T>
    {
        T Entity { get; set; }
        T NochangedEntity { get; set; }
        List<T> Entities { get; set; }
    }
    public interface IEntityView
    {
        object Object { get; set; }
        DialogResult ShowDialog();
    }
}
