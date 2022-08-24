using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceServer.Models.DataAccess
{
    interface IRelacionGranoSILGranoSTOPStore
    {
        RelacionGranoSILGranoSTOPCompleta Save(RelacionGranoSILGranoSTOPCompleta nuevaRelacion, ISession session);
        RelacionGranoSILGranoSTOPCompleta UpdateRelacion(RelacionGranoSILGranoSTOPCompleta relacion, ISession session);
        void Update(RelacionGranoSILGranoSTOPCompleta relacion, ISession session, ITransaction transaccion = null);
        bool Delete(RelacionGranoSILGranoSTOPCompleta relacion, ISession session);
        bool DeleteTodasLasRelacionesDeGranoSTOP(long NrogranoSTOP, ISession session, ITransaction tx1 = null);
        /// <summary>
        /// retorna todos los elementos de la tabla RelacionGranoSILGranoSTOP
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        IList<RelacionGranoSILGranoSTOPCompleta> FindAllRelacionCompleta(ISession session);
        /// <summary>
        /// retorna las relaciones que tiene el grano e stop ingresado
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        IList<RelacionGranoSILGranoSTOPCompleta> FindRelacionCompletaByNroGranoSTOP(long NroGrano, ISession session);
        /// <summary>
        /// retorna las relaciones que tiene el grano e SIL ingresado
        /// </summary>
        /// <param name="NroGrano"></param>
        /// <param name="session"></param>
        /// <returns></returns>        
        IList<RelacionGranoSILGranoSTOPCompleta> FindRelacionCompletaByNroGranoSIL(long NroGrano, ISession session);
        /// <summary>
        /// retorna la relacion enre el GranoSTOP y el GranoSIL pasdo como parametro
        /// </summary>
        /// <param name="NroGrano"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        RelacionGranoSILGranoSTOPCompleta FindRelacionCompletaByNroGranoSILAndNroGranoSTOP(RelacionGranoSILGranoSTOPCompleta relacion, ISession session);
        RelacionGranoSILGranoSTOPCompleta FindRelacionCompletaById(int id, ISession session);
    }
}
