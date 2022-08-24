using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    [Serializable()]
    public class CuposNoAnulablesException : BusinessException
    {
        public CuposNoAnulablesException(IList<Cupos> CuposNoAnulables, int CantidadCuposIngresada) : base(string.Format("Cupos no anulables: {0}; Cantidad ingresada: {1}", string.Join(",", CuposNoAnulables.Select(x => x.Nrocupo)), CantidadCuposIngresada)) { }
    }
}