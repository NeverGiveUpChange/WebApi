using HangFire_IRepository;
using System.Collections.Generic;
using System.Linq;
using HangFire_EFModel;
using System.Transactions;
using System;

namespace HangFire_Repository
{
    public class DbSession : IDbSession
    {
        private readonly HangFire_DevEntities _dbContext;
        private bool _isNotSubmit = false;
        private TransactionScope _transactionScope;
   

        public bool IsNotSubmit { get => _isNotSubmit; set => _isNotSubmit = value; }
        public TransactionScope TransactionScope { get => _transactionScope; private set => _transactionScope = value; }

        public DbSession(HangFire_DevEntities dbContext)
        {

            _dbContext = dbContext;
        }
        public int? SaveChanges()  
        {
            if (!IsNotSubmit)
            {

                return _dbContext.SaveChanges();
            }
            else
            {
                return null;
            }
        }
        public TransactionScope Begin()
        {
            TransactionScope= new TransactionScope();
            return TransactionScope;
        }
        public void Complete()
        {
            TransactionScope.Complete();
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {

            return _dbContext.Database.ExecuteSqlCommand(sql, parameters);
        }
        public List<T> SqlSqlQuery<T>(string sql, params object[] parameters)
        {
            return _dbContext.Database.SqlQuery<T>(sql, parameters).ToList<T>();
        }

    
    }
}
