using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    [Serializable()]
    public class CuposPorConsignacionExcedidosException : BusinessException
    {
        public CuposPorConsignacionExcedidosException() : base("La cantidad de cupos ingresadas excede el máximo disponible.") { }
    }
}