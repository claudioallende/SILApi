using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    [Serializable()]
    public class RegistroDuplicadoException : BusinessException
    {
        public RegistroDuplicadoException(string dato) : base(string.Format("{0} que intenta incorporar ya existe, verifique si los datos ingresados son los correctos", dato)) { }
    }
}