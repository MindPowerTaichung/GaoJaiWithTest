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
        static string logPath = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/AccessLog{0:yyyyMM}.txt", DateTime.Now));
        public static void SetLogPath(string path)
        {
            logPath = path;
        }

        public static void WriteEntry(string userName, AccessAction action, string entityName, string data) {
            string msg = string.Format("{0:yyyyMMdd HH:mm:ss} {1} {2} {3} {4}\r\n",
                DateTime.Now,
                userName,
                action,
                entityName,
                data);            

            //確認或建立App_Data目錄
            if (!Directory.Exists(Path.GetDirectoryName(logPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));

            //寫入Log
            File.AppendAllText(logPath, msg);
        }
    }
}