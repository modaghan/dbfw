using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BLL
{
    public class DataRepository<T> : IDisposable, IRepository<T> where T : class
    {

        private static DataContext _dbContext;
        private static IUnitOfWork _uow;
        private static IRepository<T> IRepo;
        private bool _disposed;
        public DataRepository()
        {
            Credentials.Session.ProcessStartTime = DateTime.Now;
            _dbContext = new DataContext();
            _uow = new UnitOfWork(_dbContext);
            IRepo = _uow.GetRepository<T>();
        }
        public DataRepository(DataContext context)
        {
            Credentials.Session.ProcessStartTime = DateTime.Now;
            _dbContext = context;
            _uow = new UnitOfWork(_dbContext);
            IRepo = _uow.GetRepository<T>();
        }
        public void Inserting(T entity)
        {
            IRepo.Inserting(entity);
            _uow.SaveChanges();
        }
        public T Inserted(T entity)
        {
            Inserting(entity);
            return entity;
        }
        public void Deleting(T entity)
        {
            IRepo.Deleting(entity);
            _uow.SaveChanges();
        }
        public void SoftDeleting(T entity)
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
        public void Deleting(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            T entity = IRepo.SingleSelectByQuery(query);
            IRepo.Deleting(query);
            _uow.SaveChanges();
        }
        public T Deleted(T entity)
        {
            Deleting(entity);
            return entity;
        }
        public void Updating(T entity)
        {
            IRepo.Updating(entity);
            _uow.SaveChanges();
        }
        public T Updated(T entity)
        {
            Updating(entity);
            return entity;
        }
        public T SingleSelectByQuery(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {
            return IRepo.SingleSelectByQuery(query);
        }

        public IQueryable<T> MultiSelectByQuery(System.Linq.Expressions.Expression<Func<T, bool>> query)
        {

            return IRepo.MultiSelectByQuery(query);
        }

        public IQueryable<T> SelectByAll()
        {
            return IRepo.SelectByAll();
        }

        public T SelectRandom()
        {
            return IRepo.SelectRandom();
        }
       
        public BindingList<T> toBindingList()
        {
            return IRepo.toBindingList();
        }

        public BindingList<T> toQueryableBindingList(System.Linq.Expressions.Expression<Func<T, bool>> query)
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

        public IEnumerable<T> RunQuery(string query)
        {
            return IRepo.RunQuery(query);
        }
        public IEnumerable<T> RunQuery(string query, params object[] parameters)
        {
            return IRepo.RunQuery(query, parameters);
        }

        public int ExecuteSQL(string query)
        {
            return IRepo.ExecuteSQL(query);
        }
    }
}
