using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.BLL
{
    public interface IStatic
    {
        bool TypeExists(Type type);
        object GetList(Type type);
        List<T> GetList<T>();
        void SetProperty<T>();
        void SetProperty(string type);
        void Fill();
    }
}
