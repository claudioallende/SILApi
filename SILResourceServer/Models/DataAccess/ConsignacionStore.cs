using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
  public class ConsignacionStore : IConsignacionStore
  {
    private string mapping = "Cupos.mpg.xml";

    public IList<Consignacion> FindByGranoAndCompAndVendAndPuerto(int grano, long compcta, long vendcta, long puerto)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<Consignacion> cupo = session.Query<Cupos>()
            .Where(x =>
                x.Compcta == compcta &&
                x.Vendcta == vendcta &&
                x.Grano == grano &&
                x.Fecha.Date >= DateTime.Now.Date &&
                x.Puerto == puerto &&
                x.Tipo == 1
            ).Select(x =>
                new Consignacion
                {
                  Cuitsolicitante = x.Cuitsolicitante.Trim(),
                  Nomsolicitante = x.Nomsolicitante.Trim(),
                  Cuitintermediario = x.Cuitintermediario.Trim(),
                  Nomintermediario = x.Nomintermediario.Trim(),
                  Cuitrtecomercial = x.Cuitrtecomercial.Trim(),
                  Nomrtecomercial = x.Nomrtecomercial.Trim(),
                  Cuitcorrcomp = x.Cuitcorrcomp.Trim(),
                  Nomcorrcomp = x.Nomcorrcomp.Trim(),
                  Cuitmat = x.Cuitmat.Trim(),
                  Nommat = x.Nommat.Trim(),
                  Cuitcorrvta = x.Cuitcorrvta.Trim(),
                  Nomcorrvta = x.Nomcorrvta.Trim(),
                  Cuitrteent = x.Cuitrteent.Trim(),
                  Nomrteent = x.Nomrteent.Trim(),
                  Cuitdestinatario = x.Cuitdestinatario.Trim(),
                  Nomdestinatario = x.Nomdestinatario.Trim(),
                  CuitRteComercialProductor = x.CuitRteComercialProductor.Trim(),
                  NomRteComercialProductor = x.NomRteComercialProductor.Trim(),
                  CuitRteComercialVentaPrimaria = x.CuitRteComercialVentaPrimaria.Trim(),
                  NomRteComercialVentaPrimaria = x.NomRteComercialVentaPrimaria.Trim()
                })
            .GroupBy(y =>
                new
                {
                  y.Cuitsolicitante,
                  y.Nomsolicitante,
                  y.Cuitintermediario,
                  y.Nomintermediario,
                  y.Cuitrtecomercial,
                  y.Nomrtecomercial,
                  y.Cuitcorrcomp,
                  y.Nomcorrcomp,
                  y.Cuitmat,
                  y.Nommat,
                  y.Cuitcorrvta,
                  y.Nomcorrvta,
                  y.Cuitrteent,
                  y.Nomrteent,
                  y.Cuitdestinatario,
                  y.Nomdestinatario,
                  y.CuitRteComercialProductor,
                  y.NomRteComercialProductor,
                  y.CuitRteComercialVentaPrimaria,
                  y.NomRteComercialVentaPrimaria
                }
            )
            .Select(z => new Consignacion
            {
              Cuitsolicitante = z.Key.Cuitsolicitante,
              Nomsolicitante = z.Key.Nomsolicitante,
              Cuitintermediario = z.Key.Cuitintermediario,
              Nomintermediario = z.Key.Nomintermediario,
              Cuitrtecomercial = z.Key.Cuitrtecomercial,
              Nomrtecomercial = z.Key.Nomrtecomercial,
              Cuitcorrcomp = z.Key.Cuitcorrcomp,
              Nomcorrcomp = z.Key.Nomcorrcomp,
              Cuitmat = z.Key.Cuitmat,
              Nommat = z.Key.Nommat,
              Cuitcorrvta = z.Key.Cuitcorrvta,
              Nomcorrvta = z.Key.Nomcorrvta,
              Cuitrteent = z.Key.Cuitrteent,
              Nomrteent = z.Key.Nomrteent,
              Cuitdestinatario = z.Key.Cuitdestinatario,
              Nomdestinatario = z.Key.Nomdestinatario,
              CuitRteComercialProductor = z.Key.CuitRteComercialProductor,
              NomRteComercialProductor = z.Key.NomRteComercialProductor,
              CuitRteComercialVentaPrimaria = z.Key.CuitRteComercialVentaPrimaria,
              NomRteComercialVentaPrimaria = z.Key.NomRteComercialVentaPrimaria
            })
            .ToList();
        HibernateUtil.Dispose();
        return cupo;
      }
    }


    public IList<Consignacion> FindByGranoAndCompAndVendAndPuertoInStatus(int grano, long compcta, long vendcta, long puerto, int[] status)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<Consignacion> cupo = session.Query<Cupos>()
            .Join(session.Query<Cupos>(), encab => encab.Id, cuerpo => cuerpo.Idorigen, (encab, cuerpo) => new { Encabezado = encab, Cuerpo = cuerpo })
            .Where(x =>
                (((compcta != 0) ? x.Encabezado.Compcta == compcta : true) || ((compcta != 0) ? x.Cuerpo.Compcta == compcta : true)) &&
                x.Cuerpo.Vendcta == vendcta &&
                ((grano != 0) ? x.Cuerpo.Grano == grano : true) &&
                x.Cuerpo.Fecha.Date >= DateTime.Now.Date &&
                ((puerto != 0) ? x.Cuerpo.Puerto == puerto : true) &&
                x.Cuerpo.Tipo == 1 &&
                status.Contains(x.Cuerpo.Status)
            ).Select(x =>
                new Consignacion
                {
                  Cuitsolicitante = x.Cuerpo.Cuitsolicitante.Trim(),
                  Nomsolicitante = x.Cuerpo.Nomsolicitante.Trim(),
                  Cuitintermediario = x.Cuerpo.Cuitintermediario.Trim(),
                  Nomintermediario = x.Cuerpo.Nomintermediario.Trim(),
                  Cuitrtecomercial = x.Cuerpo.Cuitrtecomercial.Trim(),
                  Nomrtecomercial = x.Cuerpo.Nomrtecomercial.Trim(),
                  Cuitcorrcomp = x.Cuerpo.Cuitcorrcomp.Trim(),
                  Nomcorrcomp = x.Cuerpo.Nomcorrcomp.Trim(),
                  Cuitmat = x.Cuerpo.Cuitmat.Trim(),
                  Nommat = x.Cuerpo.Nommat.Trim(),
                  Cuitcorrvta = x.Cuerpo.Cuitcorrvta.Trim(),
                  Nomcorrvta = x.Cuerpo.Nomcorrvta.Trim(),
                  Cuitrteent = x.Cuerpo.Cuitrteent.Trim(),
                  Nomrteent = x.Cuerpo.Nomrteent.Trim(),
                  Cuitdestinatario = x.Cuerpo.Cuitdestinatario.Trim(),
                  Nomdestinatario = x.Cuerpo.Nomdestinatario.Trim(),
                  CuitRteComercialProductor = x.Cuerpo.CuitRteComercialProductor.Trim(),
                  NomRteComercialProductor = x.Cuerpo.NomRteComercialProductor.Trim(),
                  CuitRteComercialVentaPrimaria = x.Cuerpo.CuitRteComercialVentaPrimaria.Trim(),
                  NomRteComercialVentaPrimaria = x.Cuerpo.NomRteComercialVentaPrimaria.Trim()
                })
            .GroupBy(y =>
                new
                {
                  y.Cuitsolicitante,
                  y.Nomsolicitante,
                  y.Cuitintermediario,
                  y.Nomintermediario,
                  y.Cuitrtecomercial,
                  y.Nomrtecomercial,
                  y.Cuitcorrcomp,
                  y.Nomcorrcomp,
                  y.Cuitmat,
                  y.Nommat,
                  y.Cuitcorrvta,
                  y.Nomcorrvta,
                  y.Cuitrteent,
                  y.Nomrteent,
                  y.Cuitdestinatario,
                  y.Nomdestinatario,
                  y.CuitRteComercialProductor,
                  y.NomRteComercialProductor,
                  y.CuitRteComercialVentaPrimaria,
                  y.NomRteComercialVentaPrimaria
                }
            )
            .Select(z => new Consignacion
            {
              Cuitsolicitante = z.Key.Cuitsolicitante,
              Nomsolicitante = z.Key.Nomsolicitante,
              Cuitintermediario = z.Key.Cuitintermediario,
              Nomintermediario = z.Key.Nomintermediario,
              Cuitrtecomercial = z.Key.Cuitrtecomercial,
              Nomrtecomercial = z.Key.Nomrtecomercial,
              Cuitcorrcomp = z.Key.Cuitcorrcomp,
              Nomcorrcomp = z.Key.Nomcorrcomp,
              Cuitmat = z.Key.Cuitmat,
              Nommat = z.Key.Nommat,
              Cuitcorrvta = z.Key.Cuitcorrvta,
              Nomcorrvta = z.Key.Nomcorrvta,
              Cuitrteent = z.Key.Cuitrteent,
              Nomrteent = z.Key.Nomrteent,
              Cuitdestinatario = z.Key.Cuitdestinatario,
              Nomdestinatario = z.Key.Nomdestinatario,
              CuitRteComercialProductor = z.Key.CuitRteComercialProductor,
              NomRteComercialProductor = z.Key.NomRteComercialProductor,
              CuitRteComercialVentaPrimaria = z.Key.CuitRteComercialVentaPrimaria,
              NomRteComercialVentaPrimaria = z.Key.NomRteComercialVentaPrimaria
            })
            .ToList();
        HibernateUtil.Dispose();
        return cupo;
      }
    }


    public IList<Consignacion> FindByGranoAndCompAndVendAndPuertoNotInStatus(int grano, long compcta, long vendcta, long puerto, int[] notstatus)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<Consignacion> cupo = session.Query<Cupos>()
            .Where(x =>
                x.Compcta == compcta &&
                x.Vendcta == vendcta &&
                x.Grano == grano &&
                x.Fecha.Date >= DateTime.Now.Date &&
                x.Puerto == puerto &&
                x.Tipo == 1 &&
                !notstatus.Contains(x.Status)
            ).Select(x =>
                new Consignacion
                {
                  Cuitsolicitante = x.Cuitsolicitante.Trim(),
                  Nomsolicitante = x.Nomsolicitante.Trim(),
                  Cuitintermediario = x.Cuitintermediario.Trim(),
                  Nomintermediario = x.Nomintermediario.Trim(),
                  Cuitrtecomercial = x.Cuitrtecomercial.Trim(),
                  Nomrtecomercial = x.Nomrtecomercial.Trim(),
                  Cuitcorrcomp = x.Cuitcorrcomp.Trim(),
                  Nomcorrcomp = x.Nomcorrcomp.Trim(),
                  Cuitmat = x.Cuitmat.Trim(),
                  Nommat = x.Nommat.Trim(),
                  Cuitcorrvta = x.Cuitcorrvta.Trim(),
                  Nomcorrvta = x.Nomcorrvta.Trim(),
                  Cuitrteent = x.Cuitrteent.Trim(),
                  Nomrteent = x.Nomrteent.Trim(),
                  Cuitdestinatario = x.Cuitdestinatario.Trim(),
                  Nomdestinatario = x.Nomdestinatario.Trim(),
                  CuitRteComercialProductor = x.CuitRteComercialProductor.Trim(),
                  NomRteComercialProductor = x.NomRteComercialProductor.Trim(),
                  CuitRteComercialVentaPrimaria = x.CuitRteComercialVentaPrimaria.Trim(),
                  NomRteComercialVentaPrimaria = x.NomRteComercialVentaPrimaria.Trim(),
                  CondicionGrano = x.CondicionGrano.Trim()
                })
            .GroupBy(y =>
                new
                {
                  y.Cuitsolicitante,
                  y.Nomsolicitante,
                  y.Cuitintermediario,
                  y.Nomintermediario,
                  y.Cuitrtecomercial,
                  y.Nomrtecomercial,
                  y.Cuitcorrcomp,
                  y.Nomcorrcomp,
                  y.Cuitmat,
                  y.Nommat,
                  y.Cuitcorrvta,
                  y.Nomcorrvta,
                  y.Cuitrteent,
                  y.Nomrteent,
                  y.Cuitdestinatario,
                  y.Nomdestinatario,
                  y.CuitRteComercialProductor,
                  y.NomRteComercialProductor,
                  y.CuitRteComercialVentaPrimaria,
                  y.NomRteComercialVentaPrimaria,
                  y.CondicionGrano
                }
            )
            .Select(z => new Consignacion
            {
              Cuitsolicitante = z.Key.Cuitsolicitante,
              Nomsolicitante = z.Key.Nomsolicitante,
              Cuitintermediario = z.Key.Cuitintermediario,
              Nomintermediario = z.Key.Nomintermediario,
              Cuitrtecomercial = z.Key.Cuitrtecomercial,
              Nomrtecomercial = z.Key.Nomrtecomercial,
              Cuitcorrcomp = z.Key.Cuitcorrcomp,
              Nomcorrcomp = z.Key.Nomcorrcomp,
              Cuitmat = z.Key.Cuitmat,
              Nommat = z.Key.Nommat,
              Cuitcorrvta = z.Key.Cuitcorrvta,
              Nomcorrvta = z.Key.Nomcorrvta,
              Cuitrteent = z.Key.Cuitrteent,
              Nomrteent = z.Key.Nomrteent,
              Cuitdestinatario = z.Key.Cuitdestinatario,
              Nomdestinatario = z.Key.Nomdestinatario,
              CuitRteComercialProductor = z.Key.CuitRteComercialProductor,
              NomRteComercialProductor = z.Key.NomRteComercialProductor,
              CuitRteComercialVentaPrimaria = z.Key.CuitRteComercialVentaPrimaria,
              NomRteComercialVentaPrimaria = z.Key.NomRteComercialVentaPrimaria,
              CondicionGrano = z.Key.CondicionGrano
            })
            .ToList();
        HibernateUtil.Dispose();
        return cupo;
      }
    }

    public IList<Consignacion> FindByGranoAndCompAndVendAndPuertoAndVendcyoNotInStatus(int grano, long comp, long vend, long puerto, long[] vendcyo, int[] notstatus)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<Consignacion> cupo = session.Query<Cupos>()
            .Where(x =>
                x.Compcta == comp &&
                x.Vendcta == vend &&
                x.Grano == grano &&
                x.Fecha.Date >= DateTime.Now.Date &&
                x.Puerto == puerto &&
                x.Tipo == 1 &&
                vendcyo.Contains(x.Vendcyo) &&
                !notstatus.Contains(x.Status)
            ).Select(x =>
                new Consignacion
                {
                  Cuitsolicitante = x.Cuitsolicitante.Trim(),
                  Nomsolicitante = x.Nomsolicitante.Trim(),
                  Cuitintermediario = x.Cuitintermediario.Trim(),
                  Nomintermediario = x.Nomintermediario.Trim(),
                  Cuitrtecomercial = x.Cuitrtecomercial.Trim(),
                  Nomrtecomercial = x.Nomrtecomercial.Trim(),
                  Cuitcorrcomp = x.Cuitcorrcomp.Trim(),
                  Nomcorrcomp = x.Nomcorrcomp.Trim(),
                  Cuitmat = x.Cuitmat.Trim(),
                  Nommat = x.Nommat.Trim(),
                  Cuitcorrvta = x.Cuitcorrvta.Trim(),
                  Nomcorrvta = x.Nomcorrvta.Trim(),
                  Cuitrteent = x.Cuitrteent.Trim(),
                  Nomrteent = x.Nomrteent.Trim(),
                  Cuitdestinatario = x.Cuitdestinatario.Trim(),
                  Nomdestinatario = x.Nomdestinatario.Trim(),
                  CuitRteComercialProductor = x.CuitRteComercialProductor.Trim(),
                  NomRteComercialProductor = x.NomRteComercialProductor.Trim(),
                  CuitRteComercialVentaPrimaria = x.CuitRteComercialVentaPrimaria.Trim(),
                  NomRteComercialVentaPrimaria = x.NomRteComercialVentaPrimaria.Trim(),
                  Caratula = x.Caratula.Trim(),
                  ContactoComercial = x.ContactoComercial.Trim(),
                  CondicionGrano = x.CondicionGrano.Trim()
                })
            .GroupBy(y =>
                new
                {
                  y.Cuitsolicitante,
                  y.Nomsolicitante,
                  y.Cuitintermediario,
                  y.Nomintermediario,
                  y.Cuitrtecomercial,
                  y.Nomrtecomercial,
                  y.Cuitcorrcomp,
                  y.Nomcorrcomp,
                  y.Cuitmat,
                  y.Nommat,
                  y.Cuitcorrvta,
                  y.Nomcorrvta,
                  y.Cuitrteent,
                  y.Nomrteent,
                  y.Cuitdestinatario,
                  y.Nomdestinatario,
                  y.CuitRteComercialProductor,
                  y.NomRteComercialProductor,
                  y.CuitRteComercialVentaPrimaria,
                  y.NomRteComercialVentaPrimaria,
                  y.Caratula,
                  y.ContactoComercial,
                  y.CondicionGrano
                }
            )
            .Select(z => new Consignacion
            {
              Cuitsolicitante = z.Key.Cuitsolicitante,
              Nomsolicitante = z.Key.Nomsolicitante,
              Cuitintermediario = z.Key.Cuitintermediario,
              Nomintermediario = z.Key.Nomintermediario,
              Cuitrtecomercial = z.Key.Cuitrtecomercial,
              Nomrtecomercial = z.Key.Nomrtecomercial,
              Cuitcorrcomp = z.Key.Cuitcorrcomp,
              Nomcorrcomp = z.Key.Nomcorrcomp,
              Cuitmat = z.Key.Cuitmat,
              Nommat = z.Key.Nommat,
              Cuitcorrvta = z.Key.Cuitcorrvta,
              Nomcorrvta = z.Key.Nomcorrvta,
              Cuitrteent = z.Key.Cuitrteent,
              Nomrteent = z.Key.Nomrteent,
              Cuitdestinatario = z.Key.Cuitdestinatario,
              Nomdestinatario = z.Key.Nomdestinatario,
              CuitRteComercialProductor = z.Key.CuitRteComercialProductor,
              NomRteComercialProductor = z.Key.NomRteComercialProductor,
              CuitRteComercialVentaPrimaria = z.Key.CuitRteComercialVentaPrimaria,
              NomRteComercialVentaPrimaria = z.Key.NomRteComercialVentaPrimaria,
              Caratula = z.Key.Caratula,
              ContactoComercial = z.Key.ContactoComercial,
              CondicionGrano = z.Key.CondicionGrano
            })
            .ToList();
        HibernateUtil.Dispose();
        return cupo;
      }
    }

    public IList<Consignacion> FindByGranoAndCompAndVendAndPuertoAndVendcyo(int grano, long comp, long vend, long puerto, long[] vendcyo)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<Consignacion> cupo = session.Query<Cupos>()
            .Where(x =>
                x.Compcta == comp &&
                x.Vendcta == vend &&
                x.Grano == grano &&
                x.Fecha.Date >= DateTime.Now.Date &&
                x.Puerto == puerto &&
                x.Tipo == 1 &&
                vendcyo.Contains(x.Vendcyo)
            ).Select(x =>
                new Consignacion
                {
                  Cuitsolicitante = x.Cuitsolicitante.Trim(),
                  Nomsolicitante = x.Nomsolicitante.Trim(),
                  Cuitintermediario = x.Cuitintermediario.Trim(),
                  Nomintermediario = x.Nomintermediario.Trim(),
                  Cuitrtecomercial = x.Cuitrtecomercial.Trim(),
                  Nomrtecomercial = x.Nomrtecomercial.Trim(),
                  Cuitcorrcomp = x.Cuitcorrcomp.Trim(),
                  Nomcorrcomp = x.Nomcorrcomp.Trim(),
                  Cuitmat = x.Cuitmat.Trim(),
                  Nommat = x.Nommat.Trim(),
                  Cuitcorrvta = x.Cuitcorrvta.Trim(),
                  Nomcorrvta = x.Nomcorrvta.Trim(),
                  Cuitrteent = x.Cuitrteent.Trim(),
                  Nomrteent = x.Nomrteent.Trim(),
                  Cuitdestinatario = x.Cuitdestinatario.Trim(),
                  Nomdestinatario = x.Nomdestinatario.Trim(),
                  CuitRteComercialProductor = x.CuitRteComercialProductor.Trim(),
                  NomRteComercialProductor = x.NomRteComercialProductor.Trim(),
                  CuitRteComercialVentaPrimaria = x.CuitRteComercialVentaPrimaria.Trim(),
                  NomRteComercialVentaPrimaria = x.NomRteComercialVentaPrimaria.Trim()
                })
            .GroupBy(y =>
                new
                {
                  y.Cuitsolicitante,
                  y.Nomsolicitante,
                  y.Cuitintermediario,
                  y.Nomintermediario,
                  y.Cuitrtecomercial,
                  y.Nomrtecomercial,
                  y.Cuitcorrcomp,
                  y.Nomcorrcomp,
                  y.Cuitmat,
                  y.Nommat,
                  y.Cuitcorrvta,
                  y.Nomcorrvta,
                  y.Cuitrteent,
                  y.Nomrteent,
                  y.Cuitdestinatario,
                  y.Nomdestinatario,
                  y.CuitRteComercialProductor,
                  y.NomRteComercialProductor,
                  y.CuitRteComercialVentaPrimaria,
                  y.NomRteComercialVentaPrimaria
                }
            )
            .Select(z => new Consignacion
            {
              Cuitsolicitante = z.Key.Cuitsolicitante,
              Nomsolicitante = z.Key.Nomsolicitante,
              Cuitintermediario = z.Key.Cuitintermediario,
              Nomintermediario = z.Key.Nomintermediario,
              Cuitrtecomercial = z.Key.Cuitrtecomercial,
              Nomrtecomercial = z.Key.Nomrtecomercial,
              Cuitcorrcomp = z.Key.Cuitcorrcomp,
              Nomcorrcomp = z.Key.Nomcorrcomp,
              Cuitmat = z.Key.Cuitmat,
              Nommat = z.Key.Nommat,
              Cuitcorrvta = z.Key.Cuitcorrvta,
              Nomcorrvta = z.Key.Nomcorrvta,
              Cuitrteent = z.Key.Cuitrteent,
              Nomrteent = z.Key.Nomrteent,
              Cuitdestinatario = z.Key.Cuitdestinatario,
              Nomdestinatario = z.Key.Nomdestinatario,
              CuitRteComercialProductor = z.Key.CuitRteComercialProductor,
              NomRteComercialProductor = z.Key.NomRteComercialProductor,
              CuitRteComercialVentaPrimaria = z.Key.CuitRteComercialVentaPrimaria,
              NomRteComercialVentaPrimaria = z.Key.NomRteComercialVentaPrimaria
            }
            )
            .ToList();
        HibernateUtil.Dispose();
        return cupo;
      }
    }

    public IList<Consignacion> FindByGranoAndCompAndVendAndPuertoAndVendcyoAndCentroAndCentrodist(int grano, long comp, long vend, long puerto, FiltroCentro Filtro, long[] vendcyo)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        IList<Consignacion> cupo = session.Query<Cupos>()
            .Where(x =>
                x.Compcta == comp &&
                x.Vendcta == vend &&
                x.Grano == grano &&
                x.Fecha.Date >= DateTime.Now.Date &&
                x.Puerto == puerto &&
                x.Tipo == 1 &&
                x.Centro == Filtro.CentroOrigen &&
                x.Centrodist == Filtro.CentroDistribucion &&
                vendcyo.Contains(x.Vendcyo)
            ).Select(x =>
                new Consignacion
                {
                  Cuitsolicitante = x.Cuitsolicitante.Trim(),
                  Nomsolicitante = x.Nomsolicitante.Trim(),
                  Cuitintermediario = x.Cuitintermediario.Trim(),
                  Nomintermediario = x.Nomintermediario.Trim(),
                  Cuitrtecomercial = x.Cuitrtecomercial.Trim(),
                  Nomrtecomercial = x.Nomrtecomercial.Trim(),
                  Cuitcorrcomp = x.Cuitcorrcomp.Trim(),
                  Nomcorrcomp = x.Nomcorrcomp.Trim(),
                  Cuitmat = x.Cuitmat.Trim(),
                  Nommat = x.Nommat.Trim(),
                  Cuitcorrvta = x.Cuitcorrvta.Trim(),
                  Nomcorrvta = x.Nomcorrvta.Trim(),
                  Cuitrteent = x.Cuitrteent.Trim(),
                  Nomrteent = x.Nomrteent.Trim(),
                  Cuitdestinatario = x.Cuitdestinatario.Trim(),
                  Nomdestinatario = x.Nomdestinatario.Trim(),
                  CuitRteComercialProductor = x.CuitRteComercialProductor.Trim(),
                  NomRteComercialProductor = x.NomRteComercialProductor.Trim(),
                  CuitRteComercialVentaPrimaria = x.CuitRteComercialVentaPrimaria.Trim(),
                  NomRteComercialVentaPrimaria = x.NomRteComercialVentaPrimaria.Trim(),
                  Caratula = x.Caratula.Trim(),
                  ContactoComercial = x.ContactoComercial.Trim(),
                  CondicionGrano = x.CondicionGrano.Trim()
                })
            .GroupBy(y =>
                new
                {
                  y.Cuitsolicitante,
                  y.Nomsolicitante,
                  y.Cuitintermediario,
                  y.Nomintermediario,
                  y.Cuitrtecomercial,
                  y.Nomrtecomercial,
                  y.Cuitcorrcomp,
                  y.Nomcorrcomp,
                  y.Cuitmat,
                  y.Nommat,
                  y.Cuitcorrvta,
                  y.Nomcorrvta,
                  y.Cuitrteent,
                  y.Nomrteent,
                  y.Cuitdestinatario,
                  y.Nomdestinatario,
                  y.CuitRteComercialProductor,
                  y.NomRteComercialProductor,
                  y.CuitRteComercialVentaPrimaria,
                  y.NomRteComercialVentaPrimaria,
                  y.Caratula,
                  y.ContactoComercial,
                  y.CondicionGrano
                }
            )
            .Select(z => new Consignacion
            {
              Cuitsolicitante = z.Key.Cuitsolicitante,
              Nomsolicitante = z.Key.Nomsolicitante,
              Cuitintermediario = z.Key.Cuitintermediario,
              Nomintermediario = z.Key.Nomintermediario,
              Cuitrtecomercial = z.Key.Cuitrtecomercial,
              Nomrtecomercial = z.Key.Nomrtecomercial,
              Cuitcorrcomp = z.Key.Cuitcorrcomp,
              Nomcorrcomp = z.Key.Nomcorrcomp,
              Cuitmat = z.Key.Cuitmat,
              Nommat = z.Key.Nommat,
              Cuitcorrvta = z.Key.Cuitcorrvta,
              Nomcorrvta = z.Key.Nomcorrvta,
              Cuitrteent = z.Key.Cuitrteent,
              Nomrteent = z.Key.Nomrteent,
              Cuitdestinatario = z.Key.Cuitdestinatario,
              Nomdestinatario = z.Key.Nomdestinatario,
              CuitRteComercialProductor = z.Key.CuitRteComercialProductor,
              NomRteComercialProductor = z.Key.NomRteComercialProductor,
              CuitRteComercialVentaPrimaria = z.Key.CuitRteComercialVentaPrimaria,
              NomRteComercialVentaPrimaria = z.Key.NomRteComercialVentaPrimaria,
              Caratula = z.Key.Caratula,
              ContactoComercial = z.Key.ContactoComercial,
              CondicionGrano = z.Key.CondicionGrano
            }
            )
            .ToList();
        HibernateUtil.Dispose();
        return cupo;
      }
    }
  }
}