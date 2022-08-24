using ResourceServer.Models.Auditoria;
using NHibernate;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class CupAuditHijoStore
    {
        //public IList<CupAuditHijo> FindCambioEstadoByIdInternoFromCuposcorre(long IdInterno)
        //{
        //    ISession session = HibernateUtil.OpenSession();
        //    IList<CupAuditHijo> auditorias = session.Query<CupAuditHijo>()
        //        .Join(session.Query<CupAudit>(), hijo => hijo.UvaluePadre, padre => padre.Uvalue, (hijo, padre) => new { Hijo = hijo, Padre = padre })
        //        .Where(c => c.Padre.IdInterno == IdInterno && c.Padre.Tabla.ToUpper() == "CUPOSCORRE" && c.Hijo.Campo == "STATUS")
        //        //.Select
        //        .ToList();
        //    HibernateUtil.Dispose();
        //    return auditorias;
        //}

        public IList<CupAuditHijo> FindAuditoriaFromCuposcorre(long IdInterno)
        {
            ISession session = HibernateUtil.OpenSession();
            CupAudit Padre = null;
            CupAuditHijo Hijo = null;
            IList<CupAuditHijo> auditorias = session.QueryOver<CupAuditHijo>(() => Hijo)
                .JoinEntityAlias(
                    () => Padre,
                    () => Padre.Uvalue == Hijo.UvaluePadre && Hijo.Campo == "STATUS",
                    JoinType.RightOuterJoin
                )
                .Where(() => Padre.IdInterno == IdInterno && Padre.Tabla == "CUPOSCORRE")
                .List();
            HibernateUtil.Dispose();
            return auditorias;
        }
    }
}