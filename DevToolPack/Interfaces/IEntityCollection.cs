using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevToolPack
{
    public interface IEntityCollection<T>: IEntityCollection
    {
        event EventHandler OnSingleSelected;
        event EventHandler OnMultipleSelected;
        List<T> Entities { get; set; }
        List<T> SelectedEntities { get; set; }
        T SelectedEntity { get; set; }
    }
    public interface IEntityCollection
    {
    }
}
