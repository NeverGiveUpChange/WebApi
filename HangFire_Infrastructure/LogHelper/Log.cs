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
        private static ILog _logInfo = log4net.LogManager.GetLogger("Info");
        private static ILog _LogError = log4net.LogManager.GetLogger("Error");

        public static ILog LogInfo { get => _logInfo; set => _logInfo = value; }
        public static ILog LogError { get => _LogError; set => _LogError = value; }

        public static void Info(string message)
        {
            LogInfo.Info(message);
        }
        public static void Error(Exception ex)
        {

            LogError.Error(ex.Message.ToString() + "\r\n" + ex.Source.ToString() + "\r\n" + ex.TargetSite == null ? "" : ex.TargetSite.ToString() + "\r\n" + ex.StackTrace.ToString());
        }
    }
}
