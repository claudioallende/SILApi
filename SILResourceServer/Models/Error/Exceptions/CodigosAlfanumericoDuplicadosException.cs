using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    [Serializable()]
    public class CodigosAlfanumericoDuplicadosException: BusinessException
    {
        public CodigosAlfanumericoDuplicadosException(string codigos) : base(string.Format("Los códigos alfanumérico [{0}] ya existen, por favor verifique si son los correctos.", codigos)) { }
    }
}