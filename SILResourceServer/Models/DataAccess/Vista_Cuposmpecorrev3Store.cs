using ResourceServer.Models.Contratos;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class Vista_Cuposmpecorrev3Store
    {
        private string mapping = "Vista_CuposMpeCorreV3.mpg.xml";

        public IList<Vista_CuposMpeCorreV3> FindByCompctaAndVendctaAndProductoAndCodcentro(long Compcta, long Vendcta,
            int Producto, string Codcentro)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Vista_CuposMpeCorreV3> detalles = session.Query<Vista_CuposMpeCorreV3>()
                    .Where(x =>
                        x.Compcta == Compcta &&
                        x.Vendcta == Vendcta &&
                        x.Producto == Producto &&
                        x.Codcentro == Codcentro)
                    .ToList();
                HibernateUtil.Dispose();
                return detalles;
            }
        }

        [Obsolete("Le sacamos el destino y la cosecha, reemplazar por FindByCompctaAndVendctaAndProductoAndCodcentro")]
        public IList<Vista_CuposMpeCorreV3> FindByCompctaAndVendctaAndProductoAndCtadestinoAndCosechaAndCodcentro(long Compcta, long Vendcta,
            int Producto, long Ctadestino, string Cosecha, string Codcentro)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Vista_CuposMpeCorreV3> detalles = session.Query<Vista_CuposMpeCorreV3>()
                    .Where(x => 
                        x.Compcta == Compcta &&
                        x.Vendcta == Vendcta &&
                        x.Producto == Producto &&
                        //x.Ctadestino == Ctadestino &&
                        //x.Cosecha == Cosecha &&
                        x.Codcentro == Codcentro)
                    .ToList();
                HibernateUtil.Dispose();
                return detalles;
            }
        }
    }
}