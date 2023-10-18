using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
  public class VistaCuposDistribuidosStore : IVistaCuposDistribuidosStore
  {
    private string mapping = "VistaCuposDistribuidos.mpg.xml";

    public void Save(VistaCuposDistribuidos c)
    {
      throw new NotImplementedException();
    }

    public void Update(int Id, VistaCuposDistribuidos c)
    {
      throw new NotImplementedException();
    }

    public void Delete(int Id)
    {
      throw new NotImplementedException();
    }

    public VistaCuposDistribuidos FindByCuentaComprador(int id)
    {
      throw new NotImplementedException();
    }

    public IList<VistaCuposDistribuidos> FindAll()
    {
      throw new NotImplementedException();
    }
    /*CA: Para la etapa 2 de la inclusion de caratula - aca debemos filtrar tbn por caratula -  debemos incluir la columna en la vista*/
    public IList<VistaCuposDistribuidos> FindByFilterOfView(VistaCuposDistribuidos CompVendDestProdCenConsignacion, string cosechaDesde,
        string cosechaHasta, int fechaDesde, int fechaEntrega, LocalType.ContratosPor contratosPor, IList<long> vendedores)
    {
      var cosechaD = Int32.Parse(cosechaDesde);
      var cosechaH = Int32.Parse(cosechaHasta);
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<VistaCuposDistribuidos> cuposDistribuidos = session.Query<VistaCuposDistribuidos>()
            .Where(c =>
                    c.Codproducto == CompVendDestProdCenConsignacion.Codproducto &&
                    c.Compcta == CompVendDestProdCenConsignacion.Compcta &&
                    ((cosechaD != 0) ? Int32.Parse(c.Cosecha) >= cosechaD : true) &&
                    ((cosechaH != 0) ? Int32.Parse(c.Cosecha) <= cosechaH : true) &&
                    ((fechaEntrega != 0) ? c.Fechaent <= fechaEntrega : true) &&
                    ((fechaDesde != 0) ? c.Fechaent >= fechaDesde : true) &&
                    c.Codcentro == CompVendDestProdCenConsignacion.Codcentro &&
                    ((CompVendDestProdCenConsignacion.Vendcta != 0 && contratosPor == LocalType.ContratosPor.Cuenta) ? c.Vendcta == CompVendDestProdCenConsignacion.Vendcta : true) &&
                    ((CompVendDestProdCenConsignacion.Vendcta != 0 && contratosPor == LocalType.ContratosPor.Cuit && vendedores != null) ? vendedores.Contains(c.Vendcta) : true) &&
                    (CompVendDestProdCenConsignacion.Cuitsolicitante != null ? c.Cuitsolicitante == CompVendDestProdCenConsignacion.Cuitsolicitante : (c.Cuitsolicitante == null) || (c.Cuitsolicitante == "")) &&
                    (CompVendDestProdCenConsignacion.Cuitintermediario != null ? c.Cuitintermediario == CompVendDestProdCenConsignacion.Cuitintermediario : (c.Cuitintermediario == null) || (c.Cuitintermediario == "")) &&
                    (CompVendDestProdCenConsignacion.Cuitrteent != null ? c.Cuitrteent == CompVendDestProdCenConsignacion.Cuitrteent : (c.Cuitrteent == null) || (c.Cuitrteent == "")) &&
                    (CompVendDestProdCenConsignacion.Cuitcorrcomp != null ? c.Cuitcorrcomp == CompVendDestProdCenConsignacion.Cuitcorrcomp : (c.Cuitcorrcomp == null) || (c.Cuitcorrcomp == "")) &&
                    (CompVendDestProdCenConsignacion.Cuitmat != null ? c.Cuitmat == CompVendDestProdCenConsignacion.Cuitmat : (c.Cuitmat == null) || (c.Cuitmat == "")) &&
                    (CompVendDestProdCenConsignacion.Cuitcorrvta != null ? c.Cuitcorrvta == CompVendDestProdCenConsignacion.Cuitcorrvta : (c.Cuitcorrvta == null) || (c.Cuitcorrvta == "")) &&
                    (CompVendDestProdCenConsignacion.Cuitrtecomercial != null ? c.Cuitrtecomercial == CompVendDestProdCenConsignacion.Cuitrtecomercial : (c.Cuitrtecomercial == null) || (c.Cuitrtecomercial == "")) &&
                    (CompVendDestProdCenConsignacion.Cuitdestinatario != null ? c.Cuitdestinatario == CompVendDestProdCenConsignacion.Cuitdestinatario : (c.Cuitdestinatario == null) || (c.Cuitdestinatario == "")) &&
                    (CompVendDestProdCenConsignacion.CuitRteComercialProductor != null ? c.CuitRteComercialProductor == CompVendDestProdCenConsignacion.CuitRteComercialProductor : (c.CuitRteComercialProductor == null) || (c.CuitRteComercialProductor == "")) &&
                    (CompVendDestProdCenConsignacion.CuitRteComercialVentaPrimaria != null ? c.CuitRteComercialVentaPrimaria == CompVendDestProdCenConsignacion.CuitRteComercialVentaPrimaria : (c.CuitRteComercialVentaPrimaria == null) || (c.CuitRteComercialVentaPrimaria == "")) &&
                    (CompVendDestProdCenConsignacion.Caratula != null ? c.Caratula == CompVendDestProdCenConsignacion.Caratula : (c.Caratula == null) || (c.Caratula == "")) &&
                    (CompVendDestProdCenConsignacion.ContactoComercial != null ? c.ContactoComercial == CompVendDestProdCenConsignacion.ContactoComercial : (c.ContactoComercial == null) || (c.ContactoComercial == ""))
                )
                .GroupBy(y =>
                            new
                            {
                              y.Compcta,
                              y.Vendcta,
                              y.Codproducto,
                              y.Codcentro,
                              y.Ctadestino,
                              y.Cuitcomp,
                              y.Comprador,
                              y.Cuitvend,
                              y.Vendedor,
                              y.Producto,
                              y.Destino,
                              y.Zonainfluencia,
                              y.Centro,
                              y.Cuposotorgados,
                              y.Ayer,
                              y.Hoy,
                              y.Dia1,
                              y.Dia2,
                              y.Dia3,
                              y.Dia4,
                              y.Dia5,
                              y.Dia6,
                              y.Dia7,
                              y.Dia8,
                              y.Dia9,
                              y.Dia10,
                              y.Dia11,
                              y.Dia12,
                              y.Dia13,
                              y.Dia14,
                              y.Dia15,
                              y.Dia16,
                              y.Dia17,
                              y.Dia18,
                              y.Dia19,
                              y.Dia20,
                              y.Cuitsolicitante,
                              y.Cuitintermediario,
                              y.Cuitrtecomercial,
                              y.Cuitcorrcomp,
                              y.Cuitmat,
                              y.Cuitcorrvta,
                              y.Cuitrteent,
                              y.Cuitdestinatario,
                              y.CuitRteComercialProductor,
                              y.CuitRteComercialVentaPrimaria,
                              y.Vendcyo,
                              y.Caratula,
                              y.ContactoComercial
                            }
                        ).OrderBy(z => z.Key.Vendedor)
                        .Select(
                            x => new VistaCuposDistribuidos
                            {
                              Compcta = x.Key.Compcta,
                              Vendcta = x.Key.Vendcta,
                              Codproducto = x.Key.Codproducto,
                              Codcentro = x.Key.Codcentro,
                              Ctadestino = x.Key.Ctadestino,
                              Cuitcomp = x.Key.Cuitcomp,
                              Comprador = x.Key.Comprador,
                              Cuitvend = x.Key.Cuitvend,
                              Vendedor = x.Key.Vendedor,
                              Producto = x.Key.Producto,
                              Destino = x.Key.Destino,
                              Zonainfluencia = x.Key.Zonainfluencia,
                              Centro = x.Key.Centro,
                              Pactado = x.Sum(p => p.Pactado),
                              Pendentrega = x.Sum(p => p.Pendentrega),
                              Pendaplicar = x.Sum(p => p.Pendaplicar),
                              Cuposadist = x.Sum(p => p.Cuposadist),
                              Cuposotorgados = x.Key.Cuposotorgados,
                              Cupostotalesadist = x.Sum(p => p.Cuposadist) - x.Key.Cuposotorgados,
                              Ayer = x.Key.Ayer,
                              Hoy = x.Key.Hoy,
                              Dia1 = x.Key.Dia1,
                              Dia2 = x.Key.Dia2,
                              Dia3 = x.Key.Dia3,
                              Dia4 = x.Key.Dia4,
                              Dia5 = x.Key.Dia5,
                              Dia6 = x.Key.Dia6,
                              Dia7 = x.Key.Dia7,
                              Dia8 = x.Key.Dia8,
                              Dia9 = x.Key.Dia9,
                              Dia10 = x.Key.Dia10,
                              Dia11 = x.Key.Dia11,
                              Dia12 = x.Key.Dia12,
                              Dia13 = x.Key.Dia13,
                              Dia14 = x.Key.Dia14,
                              Dia15 = x.Key.Dia15,
                              Dia16 = x.Key.Dia16,
                              Dia17 = x.Key.Dia17,
                              Dia18 = x.Key.Dia18,
                              Dia19 = x.Key.Dia19,
                              Dia20 = x.Key.Dia20,
                              Cuitsolicitante = x.Key.Cuitsolicitante,
                              Cuitintermediario = x.Key.Cuitintermediario,
                              Cuitrtecomercial = x.Key.Cuitrtecomercial,
                              Cuitcorrcomp = x.Key.Cuitcorrcomp,
                              Cuitmat = x.Key.Cuitmat,
                              Cuitcorrvta = x.Key.Cuitcorrvta,
                              Cuitrteent = x.Key.Cuitrteent,
                              Cuitdestinatario = x.Key.Cuitdestinatario,
                              CuitRteComercialProductor = x.Key.CuitRteComercialProductor,
                              CuitRteComercialVentaPrimaria = x.Key.CuitRteComercialVentaPrimaria,
                              Vendcyo = x.Key.Vendcyo,
                              Caratula = x.Key.Caratula,
                              ContactoComercial = x.Key.ContactoComercial,
                            }
                        )
                        .Where(x => x.Cupostotalesadist > 0)
                        .OrderByDescending(x => x.Hoy)
                        .ThenByDescending(x => x.Dia1)
                        .ThenByDescending(x => x.Dia2)
                        .ThenByDescending(x => x.Dia3)
                        .ThenByDescending(x => x.Dia4)
                        .ThenByDescending(x => x.Dia5)
                        .ThenByDescending(x => x.Dia6)
                        .ThenByDescending(x => x.Dia7)
                        .ThenByDescending(x => x.Dia8)
                        .ThenByDescending(x => x.Dia9)
                        .ThenByDescending(x => x.Dia10)
                        .ThenByDescending(x => x.Dia11)
                        .ThenByDescending(x => x.Dia12)
                        .ThenByDescending(x => x.Dia13)
                        .ThenByDescending(x => x.Dia14)
                        .ThenByDescending(x => x.Dia15)
                        .ThenByDescending(x => x.Dia16)
                        .ThenByDescending(x => x.Dia17)
                        .ThenByDescending(x => x.Dia18)
                        .ThenByDescending(x => x.Dia19)
                        .ThenByDescending(x => x.Dia20)
                        .ThenBy(x => x.Compcta)
                        .ThenBy(x => x.Vendcta)
                        .ThenBy(x => x.Vendedor)
                        .ThenBy(x => x.Codproducto)
                        .ThenBy(x => x.Ctadestino)
            .ToList();
        HibernateUtil.Dispose();
        return cuposDistribuidos;
      }
    }
    /*unificar con el metodo de arriba. luego la obtencion de un solo registro lo hago con un charAt(0) en la list*/
    public VistaCuposDistribuidos FindByProdCompVendPuerFechaCoseCentDestino(VistaCuposDistribuidos CompVendProdCentroDestFechaConsignacion, string cosechaD, string cosechaH)
    {
      var coseD = Int32.Parse(cosechaD);
      var coseH = Int32.Parse(cosechaH);
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        using (ITransaction tx = session.BeginTransaction())
        {
          VistaCuposDistribuidos cuposDistribuidos = session.Query<VistaCuposDistribuidos>()
              .Where(c =>
                      c.Codproducto == CompVendProdCentroDestFechaConsignacion.Codproducto &&
                      c.Compcta == CompVendProdCentroDestFechaConsignacion.Compcta &&
                      Int32.Parse(c.Cosecha) >= coseD &&
                      Int32.Parse(c.Cosecha) <= coseH &&
                      c.Fechaent <= CompVendProdCentroDestFechaConsignacion.Fechaent &&
                      c.Codcentro == CompVendProdCentroDestFechaConsignacion.Codcentro &&
                      (CompVendProdCentroDestFechaConsignacion.Vendcta != 0 ? c.Vendcta == CompVendProdCentroDestFechaConsignacion.Vendcta : true) &&
                      c.Ctadestino.ToString() == CompVendProdCentroDestFechaConsignacion.Ctadestino.ToString() &&
                      c.Cupostotalesadist > 0 &&
                      (CompVendProdCentroDestFechaConsignacion.Cuitsolicitante != null ? c.Cuitsolicitante == CompVendProdCentroDestFechaConsignacion.Cuitsolicitante : (c.Cuitsolicitante == null) || (c.Cuitsolicitante == "")) &&
                      (CompVendProdCentroDestFechaConsignacion.Cuitintermediario != null ? c.Cuitintermediario == CompVendProdCentroDestFechaConsignacion.Cuitintermediario : (c.Cuitintermediario == null) || (c.Cuitintermediario == "")) &&
                      (CompVendProdCentroDestFechaConsignacion.Cuitrteent != null ? c.Cuitrteent == CompVendProdCentroDestFechaConsignacion.Cuitrteent : (c.Cuitrteent == null) || (c.Cuitrteent == "")) &&
                      (CompVendProdCentroDestFechaConsignacion.Cuitcorrcomp != null ? c.Cuitcorrcomp == CompVendProdCentroDestFechaConsignacion.Cuitcorrcomp : (c.Cuitcorrcomp == null) || (c.Cuitcorrcomp == "")) &&
                      (CompVendProdCentroDestFechaConsignacion.Cuitmat != null ? c.Cuitmat == CompVendProdCentroDestFechaConsignacion.Cuitmat : (c.Cuitmat == null) || (c.Cuitmat == "")) &&
                      (CompVendProdCentroDestFechaConsignacion.Cuitcorrvta != null ? c.Cuitcorrvta == CompVendProdCentroDestFechaConsignacion.Cuitcorrvta : (c.Cuitcorrvta == null) || (c.Cuitcorrvta == "")) &&
                      (CompVendProdCentroDestFechaConsignacion.Cuitrtecomercial != null ? c.Cuitrtecomercial == CompVendProdCentroDestFechaConsignacion.Cuitrtecomercial : (c.Cuitrtecomercial == null) || (c.Cuitrtecomercial == "")) &&
                      (CompVendProdCentroDestFechaConsignacion.Cuitdestinatario != null ? c.Cuitdestinatario == CompVendProdCentroDestFechaConsignacion.Cuitdestinatario : (c.Cuitdestinatario == null) || (c.Cuitdestinatario == "")) &&
                      (CompVendProdCentroDestFechaConsignacion.CuitRteComercialProductor != null ? c.CuitRteComercialProductor == CompVendProdCentroDestFechaConsignacion.CuitRteComercialProductor : (c.CuitRteComercialProductor == null) || (c.CuitRteComercialProductor == "")) &&
                      (CompVendProdCentroDestFechaConsignacion.CuitRteComercialVentaPrimaria != null ? c.CuitRteComercialVentaPrimaria == CompVendProdCentroDestFechaConsignacion.CuitRteComercialVentaPrimaria : (c.CuitRteComercialVentaPrimaria == null) || (c.CuitRteComercialVentaPrimaria == "")) &&
                      (CompVendProdCentroDestFechaConsignacion.Caratula != null ? c.Caratula == CompVendProdCentroDestFechaConsignacion.Caratula : (c.Caratula == null) || (c.Caratula == "")) &&
                      (CompVendProdCentroDestFechaConsignacion.ContactoComercial != null ? c.ContactoComercial == CompVendProdCentroDestFechaConsignacion.ContactoComercial : (c.ContactoComercial == null) || (c.ContactoComercial == ""))
                  )
                  .GroupBy(y =>
                         new
                         {
                           y.Compcta,
                           y.Vendcta,
                           y.Codproducto,
                           y.Codcentro,
                           //y.Cosecha,
                           y.Ctadestino,
                           y.Cuitcomp,
                           y.Comprador,
                           y.Cuitvend,
                           y.Vendedor,
                           y.Producto,
                           y.Destino,
                           y.Centro,
                           y.Cuposotorgados,
                           y.Ayer,
                           y.Hoy,
                           y.Dia1,
                           y.Dia2,
                           y.Dia3,
                           y.Dia4,
                           y.Dia5,
                           y.Dia6,
                           y.Dia7,
                           y.Dia8,
                           y.Pedidosayer,
                           y.Pedidoshoy,
                           y.Pedidosdia1,
                           y.Pedidosdia2,
                           y.Pedidosdia3,
                           y.Pedidosdia4,
                           y.Pedidosdia5,
                           y.Pedidosdia6,
                           y.Pedidosdia7,
                           y.Pedidosdia8,
                           y.Cuitsolicitante,
                           y.Cuitintermediario,
                           y.Cuitrtecomercial,
                           y.Cuitcorrcomp,
                           y.Cuitmat,
                           y.Cuitcorrvta,
                           y.Cuitrteent,
                           y.Cuitdestinatario,
                           y.CuitRteComercialProductor,
                           y.CuitRteComercialVentaPrimaria,
                           y.Caratula,
                           y.ContactoComercial
                         }
                      ).OrderBy(z => z.Key.Vendedor)
                      .Select(
                          x => new VistaCuposDistribuidos
                          {
                            Compcta = x.Key.Compcta,
                            Vendcta = x.Key.Vendcta,
                            Codproducto = x.Key.Codproducto,
                            Codcentro = x.Key.Codcentro,
                            //Cosecha = x.Key.Cosecha,
                            Ctadestino = x.Key.Ctadestino,
                            Cuitcomp = x.Key.Cuitcomp,
                            Comprador = x.Key.Comprador,
                            Cuitvend = x.Key.Cuitvend,
                            Vendedor = x.Key.Vendedor,
                            Producto = x.Key.Producto,
                            Destino = x.Key.Destino,
                            Centro = x.Key.Centro,
                            Pactado = x.Sum(p => p.Pactado),
                            Pendentrega = x.Sum(p => p.Pendentrega),
                            Pendaplicar = x.Sum(p => p.Pendaplicar),
                            Cuposadist = x.Sum(p => p.Cuposadist),
                            Cuposotorgados = x.Key.Cuposotorgados,
                            Cupostotalesadist = x.Sum(p => p.Cuposadist) - x.Key.Cuposotorgados,
                            Ayer = x.Key.Ayer,
                            Hoy = x.Key.Hoy,
                            Dia1 = x.Key.Dia1,
                            Dia2 = x.Key.Dia2,
                            Dia3 = x.Key.Dia3,
                            Dia4 = x.Key.Dia4,
                            Dia5 = x.Key.Dia5,
                            Dia6 = x.Key.Dia6,
                            Dia7 = x.Key.Dia7,
                            Dia8 = x.Key.Dia8,
                            Pedidosayer = x.Key.Pedidosayer,
                            Pedidoshoy = x.Key.Pedidoshoy,
                            Pedidosdia1 = x.Key.Pedidosdia1,
                            Pedidosdia2 = x.Key.Pedidosdia2,
                            Pedidosdia3 = x.Key.Pedidosdia3,
                            Pedidosdia4 = x.Key.Pedidosdia4,
                            Pedidosdia5 = x.Key.Pedidosdia5,
                            Pedidosdia6 = x.Key.Pedidosdia6,
                            Pedidosdia7 = x.Key.Pedidosdia7,
                            Pedidosdia8 = x.Key.Pedidosdia8,
                            Cuitsolicitante = x.Key.Cuitsolicitante,
                            Cuitintermediario = x.Key.Cuitintermediario,
                            Cuitrtecomercial = x.Key.Cuitrtecomercial,
                            Cuitcorrcomp = x.Key.Cuitcorrcomp,
                            Cuitmat = x.Key.Cuitmat,
                            Cuitcorrvta = x.Key.Cuitcorrvta,
                            Cuitrteent = x.Key.Cuitrteent,
                            Cuitdestinatario = x.Key.Cuitdestinatario,
                            CuitRteComercialProductor = x.Key.CuitRteComercialProductor,
                            CuitRteComercialVentaPrimaria = x.Key.CuitRteComercialVentaPrimaria,
                            Caratula = x.Key.Caratula,
                            ContactoComercial = x.Key.ContactoComercial
                          }
                      )
              .FirstOrDefault();
          HibernateUtil.Dispose();
          return cuposDistribuidos;
        }
      }
    }//func

    public VistaCuposDistribuidos FindByProdCompVendPuerFechaCoseCentDestino(VistaCuposDistribuidos CompVendProdCentroDestFechaConsignacion, string cosechaD, string cosechaH, DateTime? FechaDesde, DateTime? FechaHasta, ISession Session)
    {
      var cosechaDe = Int32.Parse(cosechaD);
      var cosechaHa = Int32.Parse(cosechaH);
      var query = Session.Query<VistaCuposDistribuidos>()
          .Where(c =>
                  c.Codproducto == CompVendProdCentroDestFechaConsignacion.Codproducto &&
                  c.Compcta == CompVendProdCentroDestFechaConsignacion.Compcta &&
                  (Int32.Parse(c.Cosecha) >= cosechaDe) &&
                  (Int32.Parse(c.Cosecha) <= cosechaHa) &&
                  (DateUtils.n_date(FechaDesde) <= c.Fechaent) &&
                  (DateUtils.n_date(FechaHasta) >= c.Fechaent) &&
                  c.Codcentro == CompVendProdCentroDestFechaConsignacion.Codcentro &&
                  (CompVendProdCentroDestFechaConsignacion.Vendcta == c.Vendcta));
      query = CompVendProdCentroDestFechaConsignacion.GetConsignacion().FiltroConsignacion(query);
      VistaCuposDistribuidos cuposDistribuidos = query
          .GroupBy(y =>
              new
              {
                y.Compcta,
                y.Vendcta,
                y.Codproducto,
                y.Codcentro,
                y.Ctadestino,
                y.Cuitcomp,
                y.Comprador,
                y.Cuitvend,
                y.Vendedor,
                y.Producto,
                y.Destino,
                y.Zonainfluencia,
                y.Centro,
                y.Cuposotorgados,
                y.Ayer,
                y.Hoy,
                y.Dia1,
                y.Dia2,
                y.Dia3,
                y.Dia4,
                y.Dia5,
                y.Dia6,
                y.Dia7,
                y.Dia8,
                y.Dia9,
                y.Dia10,
                y.Dia11,
                y.Dia12,
                y.Dia13,
                y.Dia14,
                y.Dia15,
                y.Dia16,
                y.Dia17,
                y.Dia18,
                y.Dia19,
                y.Dia20,
                y.Cuitsolicitante,
                y.Cuitintermediario,
                y.Cuitrtecomercial,
                y.Cuitcorrcomp,
                y.Cuitmat,
                y.Cuitcorrvta,
                y.Cuitrteent,
                y.Cuitdestinatario,
                y.CuitRteComercialProductor,
                y.CuitRteComercialVentaPrimaria,
                y.Vendcyo,
                y.Caratula,
                y.ContactoComercial
              }
          )
          .Select(
              x => new VistaCuposDistribuidos
              {
                Compcta = x.Key.Compcta,
                Vendcta = x.Key.Vendcta,
                Codproducto = x.Key.Codproducto,
                Codcentro = x.Key.Codcentro,
                Ctadestino = x.Key.Ctadestino,
                Cuitcomp = x.Key.Cuitcomp,
                Comprador = x.Key.Comprador,
                Cuitvend = x.Key.Cuitvend,
                Vendedor = x.Key.Vendedor,
                Producto = x.Key.Producto,
                Destino = x.Key.Destino,
                Zonainfluencia = x.Key.Zonainfluencia,
                Centro = x.Key.Centro,
                Pactado = x.Sum(p => p.Pactado),
                Pendentrega = x.Sum(p => p.Pendentrega),
                Pendaplicar = x.Sum(p => p.Pendaplicar),
                Cuposadist = x.Sum(p => p.Cuposadist),
                Cuposotorgados = x.Key.Cuposotorgados,
                Cupostotalesadist = x.Sum(p => p.Cuposadist) - x.Key.Cuposotorgados,
                Ayer = x.Key.Ayer,
                Hoy = x.Key.Hoy,
                Dia1 = x.Key.Dia1,
                Dia2 = x.Key.Dia2,
                Dia3 = x.Key.Dia3,
                Dia4 = x.Key.Dia4,
                Dia5 = x.Key.Dia5,
                Dia6 = x.Key.Dia6,
                Dia7 = x.Key.Dia7,
                Dia8 = x.Key.Dia8,
                Dia9 = x.Key.Dia9,
                Dia10 = x.Key.Dia10,
                Dia11 = x.Key.Dia11,
                Dia12 = x.Key.Dia12,
                Dia13 = x.Key.Dia13,
                Dia14 = x.Key.Dia14,
                Dia15 = x.Key.Dia15,
                Dia16 = x.Key.Dia16,
                Dia17 = x.Key.Dia17,
                Dia18 = x.Key.Dia18,
                Dia19 = x.Key.Dia19,
                Dia20 = x.Key.Dia20,
                Cuitsolicitante = x.Key.Cuitsolicitante,
                Cuitintermediario = x.Key.Cuitintermediario,
                Cuitrtecomercial = x.Key.Cuitrtecomercial,
                Cuitcorrcomp = x.Key.Cuitcorrcomp,
                Cuitmat = x.Key.Cuitmat,
                Cuitcorrvta = x.Key.Cuitcorrvta,
                Cuitrteent = x.Key.Cuitrteent,
                Cuitdestinatario = x.Key.Cuitdestinatario,
                CuitRteComercialProductor = x.Key.CuitRteComercialProductor,
                CuitRteComercialVentaPrimaria = x.Key.CuitRteComercialVentaPrimaria,
                Vendcyo = x.Key.Vendcyo,
                Caratula = x.Key.Caratula,
                ContactoComercial = x.Key.ContactoComercial
              }
          )
          .Where(x => x.Cupostotalesadist > 0)
          .FirstOrDefault();
      return cuposDistribuidos;
    }
  }
}