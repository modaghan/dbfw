using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepository<T> where T : class
    {
        void Include(params string[] includes);
        void Inserting(T entity);
        T Inserted(T entity);
        T SingleSelectByQuery(System.Linq.Expressions.Expression<Func<T, bool>> query);
        IQueryable<T> MultiSelectByQuery(System.Linq.Expressions.Expression<Func<T, bool>> query);
        IQueryable<T> SelectByAll();
        T SelectRandom();
        void Updating(T entity);
        T Updated(T entity);
        void Deleting(T entity);
        void Deleting(System.Linq.Expressions.Expression<Func<T, bool>> query);
        T Deleted(T entity);
        BindingList<T> toBindingList();
        BindingList<T> toQueryableBindingList(System.Linq.Expressions.Expression<Func<T, bool>> query);
        IEnumerable<T> RunQuery(string query);
        IEnumerable<T> RunQuery(string query, params object[] parameters);
    }
}
