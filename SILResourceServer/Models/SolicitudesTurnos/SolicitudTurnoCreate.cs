using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.SolicitudesTurnos
{
  public class SolicitudTurnoCreate
  {
    public long? CuentaComprador { get; set; }
    public long CuentaVendedor { get; set; }
    public long? CuentaDestino { get; set; }
    public TipoDestino? TipoDestino { get; set; }
    public int CodigoGrano { get; set; }
    public bool EsFuturo { get; set; }
    public string Centro { get; set; }
    public string Observacion { get; set; }
    public IEnumerable<SolicitudTurnoDiaCreate> SolicitudesDias { get; set; }

    public string GetError()
    {
      return SolicitudesDias.Where(x => !string.IsNullOrWhiteSpace(x.GetError())).FirstOrDefault()?.GetError();
    }
  }

  public class SolicitudTurnoDiaCreate
  {
    private const int Limit = 100;
    private string Error { get; set; }
    public DateTime FechaSolicitado { get; set; }
    public int Cantidad { get; set; }

    public bool Validate()
    {
      if (Cantidad < Limit)
      {
        return true;
      }
      else
      {
        Error = $"Superó el límite de {Limit}";
        return false;
      }
    }

    public string GetError()
    {
      return Error;
    }
  }
}