using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Error.Exceptions
{
    [Serializable()]
    public class CupoBloqueadoParaDistribuirException : BusinessException
    {
        public CupoBloqueadoParaDistribuirException()
            : base("El cupo se encuentra bloqueado para distribuir debido a que su hijo cuenta y orden está pendiente de informar.")
        {

        }
    }
}