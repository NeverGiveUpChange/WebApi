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

        public bool IsNotSubmit { get => _isNotSubmit; set => _isNotSubmit = value; }

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
