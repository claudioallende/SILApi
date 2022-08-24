using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    [Serializable()]
    public class AsignacionDeCentrosVacioException : BusinessException
    {
        public AsignacionDeCentrosVacioException() : base("El usuario autenticado no tiene centros asignados.") { }
    }
}