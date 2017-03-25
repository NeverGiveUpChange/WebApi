using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_IRepository
{
    public interface IBaseRepository<T> where T : class,new()
    {
     
        T Add(T entity);
        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="entity">需要删除的实体</param>
        /// <param name="isPhysicalDel">是否物理删除</param>
        /// <returns></returns>
        T Remove(T entity, bool isPhysicalDel);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">需要更新的实体</param>
        /// <returns></returns>
        T Update(T entity);
      
        /// <summary>
        /// 简单查询
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <returns></returns>

        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="K">排序类别</typeparam>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="totalCount">总条数</param>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <param name="isAsc">排序规则</param>
        /// <param name="orderByLambda">排序表达式</param>
        /// <returns></returns>

        IQueryable<T> LoadPageEntities<K>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, bool isAsc, Expression<Func<T, K>> orderByLambda);



    }
}
