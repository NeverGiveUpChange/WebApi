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
         static ILog logInfo = log4net.LogManager.GetLogger("Info");
         static ILog LogError = log4net.LogManager.GetLogger("Error");
         public static void Info(string message)
         {
                 logInfo.Info(message);
         }
         public static void Error(Exception ex)
         {
  
                 LogError.Error(ex.Message.ToString() + "\r\n" + ex.Source.ToString() + "\r\n" +ex.TargetSite==null?"":ex.TargetSite.ToString()+ "\r\n" + ex.StackTrace.ToString());
         }
    }
}
