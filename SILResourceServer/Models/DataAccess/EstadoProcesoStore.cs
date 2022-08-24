using NHibernate;
using ResourceServer.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class EstadoProcesoStore
    {
        /// <summary>
        /// Crea una sesion aparte, ya que en caso de que la otra falle, esta pueda intentar guardar el estado en otra sesion.
        /// </summary>
        /// <param name="Estado"></param>
        public void Save(EstadoProceso Estado)
        {
            using (ISession session = HibernateUtil.GetSessionFactorySingleton().OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    session.Save(Estado);
                    tx.Commit();
                    session.Close();
                    session.Dispose();
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    session.Close();
                    session.Dispose();
                    throw e;
                }
            }
        }
    }
}