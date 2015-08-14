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
        public override void Log(ExceptionLoggerContext context)
        {
            string msg = string.Format("{0:yyyyMMdd HH:mm:ss} {1} {2} {3}\r\n",
                DateTime.Now,
                context.Request.Method,
                context.Request.RequestUri,
                context.Exception);
            
            string logPath = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/ErrorLog{0:yyyyMM}.txt", DateTime.Now));

            //確認或建立App_Data目錄
            if (!Directory.Exists(Path.GetDirectoryName(logPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));

            //寫入Log
            File.AppendAllText(logPath, msg);
        }
    }
}