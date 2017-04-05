using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HangFire_Infrastructure.LogHelper
{
    /// <summary>
    /// 实现Aop 对Ef执行的sql记录
    /// </summary>
    public class DataBaseLogger : IDbCommandInterceptor
    {
        static readonly ConcurrentDictionary<DbCommand, DateTime> MStartTime = new ConcurrentDictionary<DbCommand, DateTime>();
        private static void OnStart(DbCommand command)
        {
            MStartTime.TryAdd(command, DateTime.Now);
        }
        private static void LogDatabase<T>(DbCommand command, DbCommandInterceptionContext<T> interceptionContext)
        {

            DateTime startTime;
            TimeSpan duration;
            //得到此command的开始时间
            MStartTime.TryRemove(command, out startTime);
            if (startTime != default(DateTime))
            {
                duration = DateTime.Now - startTime;
            }
            else
                duration = TimeSpan.Zero;

            var parameters = new StringBuilder();
            //循环获取执行语句的参数值
            foreach (DbParameter param in command.Parameters)
            {
                parameters.AppendLine(param.ParameterName + " " + param.DbType + " = " + param.Value);
            }

            if (interceptionContext.Exception != null)
            {
                
                //记录错误sql
                Log.Error(interceptionContext.Exception, Log.LogDatabaseError);
            }
            else if (duration.TotalSeconds > 1)
            {

                Log.Info(command.CommandText, Log.LogDatabaseTimeout);
                //记录超时sql
            }
            else
            {
                //不对正常sql记录
            }

        }
        #region  执行增删改
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            LogDatabase(command, interceptionContext);
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            OnStart(command);
        }


        #endregion
        #region 执行查询
        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            OnStart(command);
        }
        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {

            LogDatabase(command, interceptionContext);
        }
        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            LogDatabase(command, interceptionContext);
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {

            OnStart(command);
        }
        #endregion


    }
}
