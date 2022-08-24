using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class WriteLog
    {
        public static void Write(string FileName, string Mensaje)
        {
            string strPath = HttpContext.Current.Server.MapPath("~") + @"/Models/Error/" + FileName;
            if (!File.Exists(strPath))
            {
                File.Create(strPath).Dispose();
            }
            using (StreamWriter sw = File.AppendText(strPath))
            {
                sw.WriteLine("=============Log===========");
                sw.WriteLine("===========Start============= " + DateTime.Now);
                sw.WriteLine(Mensaje);
                sw.WriteLine("===========End============= " + DateTime.Now);
            }
        }
    }
}