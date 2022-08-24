using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class VistaCtoCorreStore: IVistaCtoCorreStore
    {
        private string mapping = "VistaCtoCorre.mpg.xml";
        public VistaCtoCorre FindById(string id)
        {
            throw new NotImplementedException();
        }

        public IList<VistaCtoCorre> FindAll()
        {
            throw new NotImplementedException();
        }

        public IList<VistaCtoCorre> FindByCompctaAndVendctaAndCodcentroAndGranoAndFechaent
            (long compcta, long vendcta, string codcentro, int grano, long fechaent)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<VistaCtoCorre> contratos = session.Query<VistaCtoCorre>()
                    .Where(c =>
                        c.Compcta == compcta
                        && c.Vendcta == vendcta
                        && c.Codcentro == codcentro
                        && c.Grano == grano
                        && (fechaent == 0 ? true : c.Fechaent <= fechaent))
                    .ToList();
                HibernateUtil.Dispose();
                return contratos;
            }
        }

        [Obsolete("Le sacamos el destino, reemplazar por FindByCompctaAndVendctaAndCodcentroAndGranoAndFechaent")]
        public IList<VistaCtoCorre> FindByCompctaAndVendctaAndCtadestinoAndCodcentroAndGranoAndFechaent
            (long compcta, long vendcta, long ctadestino, string codcentro, int grano, long fechaent)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<VistaCtoCorre> contratos = session.Query<VistaCtoCorre>()
                    .Where(c => 
                        c.Compcta == compcta 
                        && c.Vendcta == vendcta
                        //&& c.Ctadestino == ctadestino
                        && c.Codcentro == codcentro
                        && c.Grano == grano
                        && (fechaent == 0 ? true : c.Fechaent <= fechaent))
                    .ToList();
                HibernateUtil.Dispose();
                return contratos;
            }
        }
    }
}