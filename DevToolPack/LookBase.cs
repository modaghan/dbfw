using System.Threading.Tasks;

namespace DevToolPack
{
    public class LookBase : DevExpress.XtraEditors.XtraUserControl
    {
        public virtual async Task<T> GetView<T>(T entity)
        {
            return entity;
        }
    }
}