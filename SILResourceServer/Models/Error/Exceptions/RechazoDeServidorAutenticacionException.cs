using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    [Serializable()]
    public class RechazoDeServidorAutenticacionException : BusinessException
    {
        public RechazoDeServidorAutenticacionException() : base("Servidor de autenticación rechazó usuario y contraseñas ingresados.") { }
    }
}