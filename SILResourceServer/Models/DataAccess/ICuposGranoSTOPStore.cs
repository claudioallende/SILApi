using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface ICuposGranoSTOPStore
    {
        CuposGranoSTOP Save(CuposGranoSTOP c, ISession session);
        CuposGranoSTOP Update(CuposGranoSTOP c, ISession session);
        bool Delete(CuposGranoSTOP GranoSTOP, ISession session, ITransaction tx1 = null);
        /// <summary>
        /// retorna todos los elementos de la tabla CuposGranoSTOP
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        IList<CuposGranoSTOP> FindAll(ISession session);
        IList<CuposGranoSTOP> FindAll();
        /// <summary>
        /// retorna los datos del grano para el NroGrano. esta es la PK
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        CuposGranoSTOP FindByNroGrano(long NroGrano, ISession session);
    }
}
