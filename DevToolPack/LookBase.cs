using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DevToolPack
{
    public class LookBase : DevExpress.XtraEditors.XtraUserControl
    {
        public virtual Task LoadData(DbContext ctx = null)
        {
            return default;
        }
        public virtual Task<object> GetSelected()
        {
            return default;
        }
        public virtual Task<T> GetView<T>(T entity)
        {
            return default;
        }
    }
}