using MS.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MS.BLL
{
    public class DataRepository<T> : IDisposable, IRepository<T> where T : class
    {

        private static DataContext _dbContext;
        private static IUnitOfWork _uow;
        private static IRepository<T> IRepo;
        private bool _disposed;
        public DataRepository(DataContext context)
        {
            Credentials.Session.ProcessStartTime = DateTime.Now;
            _dbContext = context;
            _uow = new UnitOfWork(_dbContext);            
            IRepo = _uow.GetRepository<T>();
        }

        public virtual void Include(params string[] includes)
        {
            IRepo.Include(includes);
        }
        public virtual void Inserting(T entity)
        {
            IRepo.Inserting(entity);
            _uow.SaveChanges();
        }
        public virtual T Inserted(T entity)
        {
            this.Inserting(entity);
            return entity;
        }
        public virtual void Deleting(T entity)
        {
            IRepo.Deleting(entity);
            _uow.SaveChanges();
        }
        public virtual void SoftDeleting(T entity)
        {
            if (entity.GetType().GetProperties().Count(p => p.Name == "is_active") > 0)
            {
                entity.GetType().GetProperty("is_active").SetValue(entity, false);
                if (entity.GetType().GetProperties().Count(p => p.Name == "modified_date") > 0)
                    entity.GetType().GetProperty("modified_date").SetValue(entity, DateTime.Now);
                Updating(entity);
            }
            else
            {
                Deleting(entity);
            }
        }
        public virtual void Deleting(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            T entity = IRepo.SingleSelectByQuery(query);
            IRepo.Deleting(query);
            _uow.SaveChanges();
        }
        public virtual T Deleted(T entity)
        {
            Deleting(entity);
            return entity;
        }
        public virtual void Updating(T entity)
        {
            IRepo.Updating(entity);
            _uow.SaveChanges();
        }
        public virtual T Updated(T entity)
        {
            Updating(entity);
            return entity;
        }
        public virtual T SingleSelectByQuery(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            return IRepo.SingleSelectByQuery(query);
        }

        public virtual IQueryable<T> MultiSelectByQuery(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            try
            {
                return IRepo.MultiSelectByQuery(query);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public virtual IQueryable<T> SelectByAll()
        {
            return IRepo.SelectByAll();
        }

        public virtual T SelectRandom()
        {
            return IRepo.SelectRandom();
        }
       
        public virtual BindingList<T> toBindingList()
        {
            return IRepo.toBindingList();
        }

        public virtual BindingList<T> toQueryableBindingList(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            return IRepo.toQueryableBindingList(query);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext = null;
                    _uow.Dispose();
                }
            }
            _disposed = true;
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual IEnumerable<T> RunQuery(string query)
        {
            return IRepo.RunQuery(query);
        }
        public virtual IEnumerable<T> RunQuery(string query, params object[] parameters)
        {
            return IRepo.RunQuery(query, parameters);
        }

        public virtual T GetOld(object id)
        {
            return IRepo.GetOld(id);
        }
    }
}
