using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Auditoria
{
    public class ServicioAuditoria
    {
        private CupAuditStore Store { get; set; }
        public ServicioAuditoria()
        {
            Store = new CupAuditStore();
        }
        public IList<AuditoriaCuposCorre> ObtenerAuditoriaCupo(long IdCupo)
        {
            return Store.FindAuditoriaFromCuposcorre(IdCupo).SelectMany(w =>
                w.ListaCupAuditHijo.DefaultIfEmpty(),
                (padre, hijo) => new AuditoriaCuposCorre
                {
                    Usuario = padre.Usuario,
                    TipoAudit = padre.TipoAudit,
                    Vnew = hijo == null ? null : hijo.Vnew,
                    Vold = hijo == null ? null : hijo.Vold,
                    Fecha = DateUtils.date_c(padre.Fecha).ToString("dd/MM/yyyy"),
                    Hora = DateUtils.hora_c(padre.Hora).ToString("HH:mm"),
                    Observacion = padre.Observacion
                })
                .ToList();
        }

        public IList<AuditoriaCuposCorre> ObtenerAuditoriaCupo(string Usuario = null, DateTime? FechaDesde = null, DateTime? FechaHasta = null, string OperacionSeleccionada = null)
        {
            var query_response = Store.FindAuditoriaFromCuposcorre(Usuario, FechaDesde, FechaHasta, OperacionSeleccionada);
            var response = query_response.SelectMany(w =>
                w.ListaCupAuditHijo.DefaultIfEmpty(),
                (padre, hijo) => new AuditoriaCuposCorre
                {
                    Usuario = padre.Usuario,
                    TipoAudit = padre.TipoAudit,
                    Vnew = hijo == null ? null : hijo.Vnew,
                    Vold = hijo == null ? null : hijo.Vold,
                    Fecha = DateUtils.date_c(padre.Fecha).ToString("dd/MM/yyyy"),
                    Hora = DateUtils.hora_c(padre.Hora).ToString("HH:mm"),
                    Observacion = padre.Observacion,
                    Alfanumerico = padre.Cupo.Nrocupo
                })
                .ToList();
            return response;
        }
    }
}