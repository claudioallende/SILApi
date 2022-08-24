using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class EstadoAlfanumericoStore
    {
        private string mapping = "Vista_estadoalfa.mpg.xml";

        public IList<EstadoAlfanumericoModel> FindByAlfanumericos(IList<string> Alfanumericos)
        {
            Alfanumericos = Alfanumericos.Select(x => x.ToUpper()).ToList();
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<EstadoAlfanumericoModel> Cupos = session.Query<EstadoAlfanumericoModel>()
                    .Where(x => Alfanumericos.Contains(x.Alfanumerico.ToUpper().Trim()))
                    .OrderBy(x => x.Fecha)
                    .ThenBy(x => x.Alfanumerico)
                    .ToList();
                HibernateUtil.Dispose();
                return Cupos;
            }
        }
    }
}