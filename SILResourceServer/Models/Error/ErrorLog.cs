using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error
{
    public class ErrorLog
    {
        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger("SIL");
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public static void Write(Exception ex)
        {
            log.Error(ex);
        }
        public static void Write(string message, Exception ex)
        {
            log.Error(message, ex);
        }
        public static void Write(string message)
        {
            log.Error(message);
        }
        public static void WriteFile(string FileName, string Message)
        {
            string strPath = HttpContext.Current.Server.MapPath("~") + string.Format(@"/Models/Error/{0}.txt", FileName);
            if (!File.Exists(strPath))
            {
                File.Create(strPath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(strPath))
            {
                sw.WriteLine("=============Error Logging ===========");
                sw.WriteLine("===========Start============= " + DateTime.Now);
                sw.WriteLine(Message);
                sw.WriteLine("===========End============= " + DateTime.Now);
            }
        }
        public static void WriteWarning(Exception ex)
        {
            log.Warn(ex);
        }
    }
}