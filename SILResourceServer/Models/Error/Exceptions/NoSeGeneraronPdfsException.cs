using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    [Serializable()]
    public class NoSeGeneraronPdfsException : BusinessException
    {
        public NoSeGeneraronPdfsException() : base("Archivo PDF no generado.") { }
    }
}