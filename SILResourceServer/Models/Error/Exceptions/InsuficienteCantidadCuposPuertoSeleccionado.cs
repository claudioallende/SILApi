using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    [Serializable()]
    public class InsuficienteCantidadCuposPuertoSeleccionado : BusinessException
    {
        public InsuficienteCantidadCuposPuertoSeleccionado() : base("No es posible decrementar la cantidad de cupos ingresada ya que algunos de ellos están distribuidos para otro puerto.") { }
    }
}