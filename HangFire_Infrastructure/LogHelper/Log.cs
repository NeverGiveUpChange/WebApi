using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangFire_Infrastructure.LogHelper
{
    public class Log
    {
        private static ILog _logInfo =LogManager.GetLogger("Info");
        private static ILog _LogError = LogManager.GetLogger("Error");
        private  static ILog  _logDatabaseTimeout = LogManager.GetLogger("DatabaseTimeoutInfo");
        private static ILog _logDatabaseError = LogManager.GetLogger("DatabaseError");
        public static ILog LogInfo { get => _logInfo; private set => _logInfo = value; }
        public static ILog LogError { get => _LogError; private set => _LogError = value; }
        public static ILog LogDatabaseTimeout { get => _logDatabaseTimeout; private set => _logDatabaseTimeout = value; }
        public static ILog LogDatabaseError { get => _logDatabaseError; private set => _logDatabaseError = value; }

        public static void Info(string message,ILog  logInfo)
        {
            logInfo.Info(message);
        }
        public static void Error(Exception ex,ILog logError)
        {

            logError.Error(ex.Message.ToString() + "\r\n" + ex.Source.ToString() + "\r\n" + ex.TargetSite == null ? "" : ex.TargetSite.ToString() + "\r\n" + ex.StackTrace.ToString());
        }
    }
}
