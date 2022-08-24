using NHibernate;
using ResourceServer.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class ProcesoStore
    {
        /// <summary>
        /// Busca los estados de la ejecucion del proceso en la fecha indicada. No tiene en cuenta la hora.
        /// </summary>
        /// <param name="Nombre"></param>
        /// <param name="Fecha"></param>
        /// <returns></returns>
        public IList<Proceso> FindByIdProcesoAndFecha(long IdProceso, DateTime Fecha)
        {
            using (ISession session = HibernateUtil.OpenSession())
            {
                IList<Proceso> estado = session.Query<Proceso>()
                                .Where(x => x.Id == IdProceso && x.Estados.Any(y => y.FechaExec.Date == Fecha.Date))
                                .ToList();
                HibernateUtil.Dispose();
                return estado;
            }
        }

        /// <summary>
        /// Obtiene el ultimo estado del proceso.
        /// </summary>
        /// <param name="Nombre"></param>
        /// <returns></returns>
        public Proceso FindLastByIdProceso(long IdProceso)
        {
            using (ISession session = HibernateUtil.OpenSession())
            {
                Proceso Proceso = session.Query<Proceso>()
                                .Where(x => x.Id == IdProceso)
                                .OrderByDescending(x => x.Estados.Select(y => y.FechaExec))
                                .FirstOrDefault();
                HibernateUtil.Dispose();
                return Proceso;
            }
        }

        public void Save(Proceso Proceso)
        {
            using (ISession session = HibernateUtil.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    session.Save(Proceso);
                    tx.Commit();
                    HibernateUtil.Dispose();
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
        }
    }
}