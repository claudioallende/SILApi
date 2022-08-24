using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class VistaCuposmpecorreStore
    {
        private string mapping = "VistaCuposmpecorre.mpg.xml";

        public IList<VistaCuposmpecorre> FindByProductoAndCompctaAndCentroAndVendcta
            (int Producto, long Compcta, string Centro, long Vendcta)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<VistaCuposmpecorre> list = session.Query<VistaCuposmpecorre>()
                    .Where(x => 
                        x.Producto == Producto && 
                        x.Compcta == Compcta && 
                        x.Centro == Centro && 
                        x.Vendcta == Vendcta && 
                        x.Ctadestino == null
                    )
                    .ToList();
                HibernateUtil.Dispose();
                return list;
            }
        }
    }
}