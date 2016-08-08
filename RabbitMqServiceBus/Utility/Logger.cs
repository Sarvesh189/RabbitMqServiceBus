using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace RabbitMqServiceBus.Utility
{
   internal static class Logger
   {
       private static readonly log4net.ILog log;
       static Logger()
       {
           log4net.Config.XmlConfigurator.Configure();
           log = log4net.LogManager.GetLogger("RabbitMqServiceBusLogger");
       }

     
       public static void WriteInfo(string message)
       {
            log.Info(message);
        }

       public static void WriteWarn(string message)
       {
           log.Warn(message);
       }

       public static void WriteError(string message)
        {
            log.Error(message);
         
        }
       

    }
}
