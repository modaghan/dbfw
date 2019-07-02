using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        private string[] _includes;
        public GenericRepository(DataContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext can not be null.");
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public void Include(params string[] includes)
        {
            this._includes = includes;
        }
        #region CREATE
        public void Inserting(T entity)
        {
            entity = _dbSet.Add(entity);
        }
        public void InsertAsync(T entity)
        {
            entity = _dbSet.Add(entity);
        }
        public T Inserted(T entity)
        {
            Inserting(entity);
            return entity;
        }
        #endregion

        #region READ
        public T GetOld(object id)
        {
            return _dbSet.Find(id);
        }
        public T SingleSelectByQuery(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            return Include(MultiSelectByQuery(query)).FirstOrDefault();
        }

        public IQueryable<T> MultiSelectByQuery(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            return Include(SelectByAll()).Where(query);
        }

        public IQueryable<T> SelectByAll()
        {
            return _dbSet;
        }
        public IQueryable<T> Include(IQueryable<T> list)
        {
            if (_includes!=null &&_includes.Length > 0)
                foreach (string inc in _includes)
                {
                    list = list.Include(inc);
                }
            return list;
        }

        public T SelectRandom()
        {
            var list = _dbSet.ToList();
            Random Rnd = new Random(DateTime.Now.Millisecond);
            return list[Rnd.Next(list.Count)];
        }
        #endregion

        #region UPDATE
        public void Updating(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        public void UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        public T Updated(T entity)
        {
            Updating(entity);
            return entity;
        }
        #endregion

        #region DELETE
        public void Deleting(T entity)
        {
            DbEntityEntry dbEntityEntry = _dbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
            }
        }
        public void DeleteAsync(T entity)
        {
            DbEntityEntry dbEntityEntry = _dbContext.Entry(entity);

            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                _dbSet.Attach(entity);
                _dbSet.Remove(entity);
            }
        }
        public void Deleting(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            T entity = _dbSet.Where(query).SingleOrDefault();
            Deleting(entity);
        }
        public void DeleteAsync(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            T entity = _dbSet.Where(query).SingleOrDefault();
            DeleteAsync(entity);
        }
        public T Deleted(T entity)
        {
            Deleting(entity);
            return entity;
        }
        #endregion

        #region BINDING LISTS
        public BindingList<T> toBindingList()
        {
            BindingList<T> entities = new BindingList<T>();
            foreach (T t in SelectByAll())
            {
                entities.Add(t);
            }
            return entities;
        }

        public BindingList<T> toQueryableBindingList(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            BindingList<T> entities = new BindingList<T>();
            foreach (T t in SelectByAll().Where(query))
            {
                entities.Add(t);
            }
            return entities;
        }
        #endregion

        #region QUERY EXECUTERS
        public IEnumerable<T> RunQuery(string query)
        {
            return _dbContext.Database.SqlQuery<T>(query);
        }
        public IEnumerable<T> RunQuery(string query, params object[] parameters)
        {
            return _dbContext.Database.SqlQuery<T>(query, parameters);
        }
        #endregion


    }
}
