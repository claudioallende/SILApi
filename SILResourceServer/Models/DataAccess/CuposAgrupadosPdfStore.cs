using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class CuposAgrupadosPdfStore : ICuposAgrupadosPdfStore
    {
        private string mapping = "CuposAgrupadosPdf.mpg.xml";
        private string mappingCyo = "CuposAgrupadosPdfCyo.mpg.xml";

        public IList<ICuposAgrupadosPdf> FindByCompradorAndPuertoAndGrano(long Comprador, long Puerto, int Grano)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<ICuposAgrupadosPdf> ContratosAgrupados = session.Query<CuposAgrupadosPdf>()
                    .Where(c =>
                        c.Compcta == Comprador &&
                        c.PuertoCta == Puerto &&
                        c.Grano == Grano &&
                        ((c.D0Co > 0 || c.D1Co > 0 || c.D2Co > 0 || c.D3Co > 0 || c.D4Co > 0 || c.D5Co > 0) || c.PendPdf == 1)
                    )
                    .OrderBy(x => x.PendPdf)
                    .ToList<ICuposAgrupadosPdf>();
                HibernateUtil.Dispose();
                return ContratosAgrupados;
            }
        }

        public IList<ICuposAgrupadosPdf> FindByCompradorAndPuertoAndGranoCyo(long Comprador, long Puerto, int Grano)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<ICuposAgrupadosPdf> ContratosAgrupados = session.Query<CuposAgrupadosPdfCyo>()
                    .Where(c =>
                        c.Compcta == Comprador &&
                        c.PuertoCta == Puerto &&
                        c.Grano == Grano &&
                        ((c.D0Co > 0 || c.D1Co > 0 || c.D2Co > 0 || c.D3Co > 0 || c.D4Co > 0 || c.D5Co > 0) || c.PendPdf == 1)
                    )
                    .OrderBy(x => x.PendPdf)
                    .ToList<ICuposAgrupadosPdf>();
                HibernateUtil.Dispose();
                return ContratosAgrupados;
            }
        }
    }
}