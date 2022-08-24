using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface ICuposAgrupadosPdfStore
    {
        /// <summary>
        /// Busca en base de datos los Contratos distribuidos agrupados por comprador, puerto, grano, destino y si fue informado o no.
        /// </summary>
        /// <param name="Comprador"></param>
        /// <param name="Puerto"></param>
        /// <param name="Grano"></param>
        /// <returns></returns>
        IList<ICuposAgrupadosPdf> FindByCompradorAndPuertoAndGrano(long Comprador, long Puerto, int Grano);
        /// <summary>
        /// Busca en base de datos los Contratos distribuidos agrupados por comprador, puerto, grano, destino y si fue informado o no.
        /// </summary>
        /// <param name="Comprador"></param>
        /// <param name="Puerto"></param>
        /// <param name="Grano"></param>
        /// <returns></returns>
        IList<ICuposAgrupadosPdf> FindByCompradorAndPuertoAndGranoCyo(long Comprador, long Puerto, int Grano);
    }
}
