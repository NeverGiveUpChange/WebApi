using System;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using HangFire_EFModel;

namespace HangFire_Repository
{
    public class BaseRepository<T> where T : class, new()
    {
         private  readonly HangFire_DevEntities _dbContext;
        public BaseRepository(HangFire_DevEntities dbContext)
        {
            _dbContext = dbContext; 
        }
         
  
        public T Add(T entity)
        {
            _dbContext.Entry<T>(entity).State = EntityState.Added;
        
            return entity;
        }
        public T Remove(T entity, bool isPhysicalDel)
        {
            return isPhysicalDel ? PhysicalRemove(entity) : Update(entity);
        }
        public T Update(T entity)
        {
        

            _dbContext.Entry<T>(entity).State = EntityState.Modified;
            return entity;
        }
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return _dbContext.Set<T>().Where<T>(whereLambda);
        }
        public IQueryable<T> LoadPageEntities<K>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, K>> orderByLambda)
        {
          
            var tempQueryable = _dbContext.Set<T>().Where<T>(whereLambda);
            if (isAsc)
            {
                tempQueryable.OrderBy<T, K>(orderByLambda);
            }
            else
            {
                tempQueryable.OrderByDescending<T, K>(orderByLambda);
            }
            totalCount = tempQueryable.Count();
            return tempQueryable.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        private T PhysicalRemove(T entity)
        {
        
            _dbContext.Entry<T>(entity).State = EntityState.Deleted;
            return entity;
        }
  

    }
}
