using ResourceServer.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    [Serializable()]
    public class ErrorEnEnvioDeEmailException: BusinessException
    {
        public ErrorEnEnvioDeEmailException(string detalleError) : base(string.Format("Warning: Al enviar Email de la {0}", detalleError)) { }
    }
}