using ResourceServer.Models.Auditoria;
using NHibernate;
using NHibernate.SqlCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class CupAuditStore
    {
        public CupAudit FindByUvalue(long Uvalue)
        {
            CupAudit auditoria = HibernateUtil.OpenSession().Query<CupAudit>()
                .Where(c => c.Uvalue == Uvalue)
                .FirstOrDefault();
            HibernateUtil.Dispose();
            return auditoria;
        }

        public IList<CupAudit> FindByIdInterno(long IdInterno)
        {
            IList<CupAudit> auditorias = HibernateUtil.OpenSession().Query<CupAudit>()
                .Where(c => c.IdInterno == IdInterno)
                .ToList();
            HibernateUtil.Dispose();
            return auditorias;
        }

        public IList<CupAudit> FindByIdInternoFromCuposcorre(long IdInterno)
        {
            IList<CupAudit> auditorias = HibernateUtil.OpenSession().Query<CupAudit>()
                .Where(c => c.IdInterno == IdInterno && c.Tabla.ToUpper() == "CUPOSCORRE")
                .ToList();
            HibernateUtil.Dispose();
            return auditorias;
        }

        public IList<CupAudit> FindAuditoriaFromCuposcorre(long IdInterno)
        {
            ISession session = HibernateUtil.OpenSession();
            CupAudit Padre = null;
            CupAuditHijo Hijo = null;
            IList<CupAudit> auditorias = session.QueryOver<CupAudit>(() => Padre)
                .Left
                .JoinAlias(
                    () => Padre.ListaCupAuditHijo,
                    () => Hijo
                )
                .Where(() => Padre.IdInterno == IdInterno && Padre.Tabla == "CUPOSCORRE" && (Hijo.Campo == null || Hijo.Campo == "STATUS"))
                .OrderBy(x => x.Fecha).Asc
                .OrderBy(x => x.Hora).Asc
                .List();
            HibernateUtil.Dispose();
            return auditorias;
        }

        public IList<CupAudit> FindAuditoriaFromCuposcorre(string Usuario = null, DateTime? FechaDesde = null, DateTime? FechaHasta = null, string OperacionSeleccionada = null)
        {
            ISession session = HibernateUtil.OpenSession();
            CupAudit Padre = null;
            CupAuditHijo Hijo = null;
            Cupos Cupo = null;
            var query = session.QueryOver<CupAudit>(() => Padre)
                .Left
                .JoinAlias(padre => padre.ListaCupAuditHijo, () => Hijo)
                .JoinAlias(padre => padre.Cupo, () => Cupo)
                .Where(padre => padre.Tabla == "CUPOSCORRE" && (Hijo.Campo == null || Hijo.Campo == "STATUS"));
            if (!string.IsNullOrEmpty(Usuario)) query = query.WhereRestrictionOn(padre => padre.Usuario).IsInsensitiveLike(Usuario, NHibernate.Criterion.MatchMode.Anywhere);
            if (FechaDesde != null) query = query.Where(padre => padre.Fecha >= DateUtils.n_date(FechaDesde));
            if (FechaHasta != null) query = query.Where(padre => padre.Fecha <= DateUtils.n_date(FechaHasta));
            if (!string.IsNullOrEmpty(OperacionSeleccionada))
            {
                ResourceServer.Models.Cupo.TransicionDeEstado transicion = ResourceServer.Models.Cupo.TransicionesDeEstadoFactory.GetTransicion(OperacionSeleccionada);
                query = query.Where(padre => padre.TipoAudit == transicion.Tipo);
                if (!string.IsNullOrEmpty(transicion.VOld)) query = query.Where(padre => Hijo.Vold == transicion.VOld);
                if (!string.IsNullOrEmpty(transicion.VNew)) query = query.Where(padre => Hijo.Vnew == transicion.VNew);
                //if (OperacionSeleccionada.ToLower() == "alta") query = query.Where(padre => padre.TipoAudit == "ALT");
                //if (OperacionSeleccionada.ToLower() == "distribucion") query = query.Where(padre => padre.TipoAudit == "MOD" && Hijo.Vold == "0" && Hijo.Vnew == "4");
                //if (OperacionSeleccionada.ToLower() == "cancelacion") query = query.Where(padre => padre.TipoAudit == "MOD" && Hijo.Vold == "4" && Hijo.Vnew == "0");
                //if (OperacionSeleccionada.ToLower() == "anulacion") query = query.Where(padre => padre.TipoAudit == "MOD" && Hijo.Vnew == "3");
            }
            query = query.OrderBy(x => x.Fecha).Asc
                .OrderBy(x => x.Hora).Asc;
            IList<CupAudit> auditorias = query.List();
            HibernateUtil.Dispose();
            return auditorias;
        }

    }
}