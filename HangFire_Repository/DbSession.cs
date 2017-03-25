using HangFire_IRepository;
using System.Collections.Generic;
using System.Linq;
using HangFire_EFModel;

namespace HangFire_Repository
{
    public class DbSession : IDbSession
    {
        private readonly HangFire_DevEntities _dbContext;
        public DbSession(HangFire_DevEntities dbContext) {

            _dbContext = dbContext;
        }
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
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
