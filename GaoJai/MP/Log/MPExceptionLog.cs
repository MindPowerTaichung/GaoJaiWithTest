using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace MPERP2015.MP.Log
{
    public class MPExceptionLog : ExceptionLogger
    {
        string getLogPath()
        {
            string logDir;
            if (HttpContext.Current != null)
            {
                logDir = HttpContext.Current.Server.MapPath("~/App_Data");
            }
            else
            {
                logDir = Environment.CurrentDirectory + "\\Log";
            }

            //確認或建立App_Data目錄            
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            return Path.Combine(logDir, string.Format("ErrorLog{0:yyyyMM}.txt", DateTime.Now));
        }

        public override void Log(ExceptionLoggerContext context)
        {
            string msg = string.Format("{0:yyyyMMdd HH:mm:ss} {1} {2} {3}\r\n",
                DateTime.Now,
                context.Request.Method,
                context.Request.RequestUri,
                context.Exception);            

            //寫入Log
            File.AppendAllText(getLogPath(), msg);
        }
    }
}