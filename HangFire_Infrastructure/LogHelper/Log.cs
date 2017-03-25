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
        public static ILog _logInfo = LogManager.GetLogger("Info");
        public static ILog _LogError = LogManager.GetLogger("Error");
        public  static ILog  _logDatabaseTimeout = LogManager.GetLogger("DatabaseTimeoutInfo");
        public static ILog _logDatabaseError = LogManager.GetLogger("DatabaseError");
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
