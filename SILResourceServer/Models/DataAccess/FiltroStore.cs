using ResourceServer.Models.Filtro;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class FiltroStore : IFiltroStore
    {
        private string mapping = "Filtro.mpg.xml";

        public FiltroDistribucion FindByKey(long CodigoGrano, long CuentaPuerto, long CuentaComprador, long Uvdist)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                FiltroDistribucion filtro = session.Query<FiltroDistribucion>()
                    .Where(x => 
                        x.CodigoGrano == CodigoGrano && 
                        x.CuentaPuerto == CuentaPuerto && 
                        x.CuentaComprador == CuentaComprador && 
                        x.Uvdist == Uvdist
                    )
                    .FirstOrDefault();
                HibernateUtil.Dispose();
                return filtro;
            }
        }

        public void SaveOrUpdate(FiltroDistribucion filtro)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    session.SaveOrUpdate(filtro);
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