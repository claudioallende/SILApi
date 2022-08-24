using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    [Serializable()]
    public class NeedEmailException : BusinessException
    {
        public NeedEmailException() : base("Es necesario un correo para enviar el Email.") { }
    }
}