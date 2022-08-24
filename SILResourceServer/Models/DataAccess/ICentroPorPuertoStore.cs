using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface ICentroPorPuertoStore
    {
        long Save(CuposCentroPorPuerto c, ISession session);
        CuposCentroPorPuerto Update(CuposCentroPorPuerto c, ISession session);
        void Delete(CuposCentroPorPuerto PuertoCentro, ISession session);
        CuposCentroPorPuerto FindById(long Id, ISession session);
        IList<CuposCentroPorPuerto> FindAll(ISession session);
        /// <summary>
        /// Filtra los CuposCentroPorPuerto filtrando por el codigo del Centro 
        /// </summary>
        /// <param name="CodigoCentro"></param>
        /// <returns></returns>
        IList<CuposCentroPorPuerto> FindByCentro(string CodigoCentro, ISession session);
        /// <summary>
        /// Filtra los CuposCentroPorPuerto filtrando por IdTerminal del Puerto
        /// </summary>
        /// <param name="IdTerminal"></param>
        /// <returns></returns>
        IList<CuposCentroPorPuerto> FindByPuerto(long IdTerminal, ISession session);
        IList<CuposCentroPorPuerto> FindByPuertoAndCentro(long IdTerminal, string codigoCentro, ISession session);
    }
}
