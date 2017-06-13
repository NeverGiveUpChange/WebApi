
using HangFire_IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Service
{
   public  class BaseService<T> where T:class,new()
    {
       private  IDbSession _dbSession { get; set; }
       private IBaseRepository<T> _currentRepository { get; set; }
        public BaseService(IDbSession dbSession, IBaseRepository<T> currentRepository)
        {
            _dbSession = dbSession;
            _currentRepository = currentRepository;
            _dbSession.IsNotSubmit = true;
         
        }
     public   T Add(T entity)
       {
           var  addEntity= _currentRepository.Add(entity);
           _dbSession.SaveChanges();
           return addEntity;
       }
      public  T Remove(T entity, bool isPhysicalDel)
        {
            var removeEntity = _currentRepository.Remove(entity, isPhysicalDel);
            _dbSession.SaveChanges();
            return removeEntity;
        }
      public  T Update(T entity) 
        { 
            var updateEntity= _currentRepository.Update(entity);
            _dbSession.SaveChanges();
            return updateEntity;
        }
       public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda) 
        { 
            return _currentRepository.LoadEntities(whereLambda); 
        }


        public IQueryable<T> LoadPageEntities<K>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, K>> orderByLambda)
        {
            return _currentRepository.LoadPageEntities(pageIndex, pageSize,out totalCount, whereLambda, isAsc, orderByLambda);
        }
    }
}
