using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    [Serializable()]
    public class CupoSinDistribucionException : BusinessException
    {
        public CupoSinDistribucionException() : base("El Cupo seleccionado no está distribuido.") { }
    }
}