using NHibernate;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Error.Exceptions;
using ResourceServer.Models.SolicitudesTurnos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceServer.Models
{
  public class SolicitudTurnoService
  {
    private SolicitudTurnoStore SolicitudTurnoStore { get; set; }

    public SolicitudTurnoService()
    {
      SolicitudTurnoStore = new SolicitudTurnoStore();
    }

    public IEnumerable<SolicitudTurnoView> GetByVendedor(long cuentaVendedor)
    {
      return SolicitudTurnoStore.GetByVendedor(cuentaVendedor, DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(7).Date);
    }

    public IEnumerable<SolicitudTurnoView> GetAll(DateTime? desde, DateTime? hasta, bool futuro)
    {
      return SolicitudTurnoStore.GetAll(desde, hasta, futuro);
    }

    public async Task AddRequest(SolicitudTurnoCreate solicitudTurnoCreate)
    {
      using (ISession session = HibernateUtil.OpenSession())
      using (ITransaction tx = session.BeginTransaction())
      {
        if (solicitudTurnoCreate.SolicitudesDias.Any(x => !x.Validate()))
          throw new BusinessException(solicitudTurnoCreate.GetError());

        try
        {
          if (solicitudTurnoCreate.SolicitudesDias != null && solicitudTurnoCreate.SolicitudesDias.Count() > 0)
          {
            IList<Task> allTasks = new List<Task>();
            foreach (SolicitudTurnoDiaCreate solicitudTurnoDia in solicitudTurnoCreate.SolicitudesDias)
            {
              for (int i = 0; i < solicitudTurnoDia.Cantidad; i++)
              {
                SolicitudTurno solicitud = new SolicitudTurno
                {
                  CuentaVendedor = solicitudTurnoCreate.CuentaVendedor,
                  CuentaComprador = solicitudTurnoCreate.CuentaComprador,
                  CodigoGrano = solicitudTurnoCreate.CodigoGrano,
                  CuentaDestino = solicitudTurnoCreate.CuentaDestino,
                  FechaCreacion = DateTime.Now,
                  FechaSolicitado = solicitudTurnoDia.FechaSolicitado,
                  EsFuturo = solicitudTurnoCreate.EsFuturo,
                  CodigoCentro = solicitudTurnoCreate.Centro,
                  CodigoEstado = 0,
                  Observacion = solicitudTurnoCreate.Observacion,
                  TipoDestino = TipoDestino.ZonaPortuaria,
                  CupoId = null
                };

                allTasks.Add(SolicitudTurnoStore.AddAsync(solicitud, session));
              }
            }

            await Task.WhenAll(allTasks);

            await tx.CommitAsync();
          }
        }
        catch
        {
          await tx.RollbackAsync();
          throw;
        }
      }
    }
  }
}