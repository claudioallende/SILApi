using NHibernate;
using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Email
{
  public class EmailDistribucionContactoComercial : EmailDistribucionMultiplesPdfs
  {
    private IList<Cupos> ListaCupos { get; set; }
    public EmailDistribucionContactoComercial(IList<ICupo> ListaCupos, ISession Session)
             : base(ListaCupos, Session)
    {
      this.ListaCupos = ListaCupos.Cast<Cupos>().ToList();
    }

    protected override IList<Cupos> GetCuposInformados(long CuentaComprador, long CuentaVendedor, int CodigoGrano, long CuentaPuerto, Consignacion Consignacion, DateTime FechaInicial, DateTime FechaFin, ISession Session)
    {
      CuposStore Store = new CuposStore();
      // La lista de cupos que recibe deben tener todos el mismo contacto comercial y debe ser != null
      string ContactosComerciales = this.ListaCupos.Select(x => x.ContactoComercial).First();
      return Store.FindByContactoComercialCompradorAndVendedorAndGranoAndPuertoAndConsignacionAndInformadoAndDistribuidoBetweenDates
          (ContactosComerciales, CuentaComprador, CuentaVendedor, CodigoGrano, CuentaPuerto, Consignacion, InicioSemana, FinSemana, Session);
    }
  }
}