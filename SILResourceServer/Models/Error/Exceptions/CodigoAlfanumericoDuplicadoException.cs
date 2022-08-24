using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    [Serializable()]
    public class CodigoAlfanumericoDuplicadoException : BusinessException
    {
        public CodigoAlfanumericoDuplicadoException(string codigo) : base(string.Format("El código alfanumérico {0} ya existe, por favor ingrese otro.", codigo)) { }
    }
}