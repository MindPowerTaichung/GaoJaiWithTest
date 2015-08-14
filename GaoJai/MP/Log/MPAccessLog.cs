using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MPERP2015.MP.Log
{
    public enum AccessAction
    {
        Create,
        Update,
        Delete,
        PasswordChanged
    }

    public class MPAccessLog
    {
        static string getLogPath() {
            string logDir;
            if (HttpContext.Current != null)
            {
                logDir = HttpContext.Current.Server.MapPath("~/App_Data");
            }
            else
            {
                logDir = Environment.CurrentDirectory +"\\Log" ;
            }

            //確認或建立App_Data目錄
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            return Path.Combine(logDir,string.Format("AccessLog{0:yyyyMM}.txt", DateTime.Now));
        }

        public static void WriteEntry(string userName, AccessAction action, string entityName, string data) {
            string msg = string.Format("{0:yyyyMMdd HH:mm:ss} {1} {2} {3} {4}\r\n",
                DateTime.Now,
                userName,
                action,
                entityName,
                data);            

            //寫入Log
            File.AppendAllText(getLogPath(), msg);
        }
    }
}