using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class TbCoperStore
    {
        public IList<TbCoper> FindAll()
        {
            using (ISession session = HibernateUtil.OpenSession())
            {
                IList<TbCoper> list = session.Query<TbCoper>()
                    .Where(x => x.Codigo != null && x.Descripcion != null)
                    .ToList();
                HibernateUtil.Dispose();
                return list;
            }
        }
    }
}