using NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
  public class CupoInStopAndNotSILStore : ICupoInStopAndNotSILStore
  {

    public void Save(CupoInStopAndNotSIL c)
    {
      throw new NotImplementedException();
    }

    public void Save(CupoInStopAndNotSIL c, ISession session)
    {
      throw new NotImplementedException();
    }

    public void Save(CupoInStopAndNotSIL c, ISession session, ITransaction transaccion)
    {
      throw new NotImplementedException();
    }

    public void Update(long Id, CupoInStopAndNotSIL c, ISession session)
    {
      try
      {
        session.Update(c, Id);
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public void Delete(long Id)
    {
      throw new NotImplementedException();
    }

    public void Delete(CupoInStopAndNotSIL cupo, ISession session)
    {
      using (ITransaction tx = session.BeginTransaction())
      {
        try
        {
          session.Delete(cupo);
          tx.Commit();
        }
        catch (Exception e)
        {
          tx.Rollback();
          throw e;
        }
      }
    }

    public void Delete(CupoInStopAndNotSIL cupo, ISession session, ITransaction transaccion)
    {
      try
      {
        session.Delete(cupo);
      }
      catch (Exception e)
      {
        transaccion.Rollback();
        throw e;
      }
    }

    public CupoInStopAndNotSIL FindByAlfa(string alfa, ISession session)
    {
      CupoInStopAndNotSIL cupos = session.Query<CupoInStopAndNotSIL>()
          .Where(x => x.Fecha >= DateTime.Today
                      && x.Nrocupo == alfa
                      && (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro)))
          .FirstOrDefault();
      return cupos;
    }

    public IList<CupoInStopAndNotSIL> FindByIds(IList<long> Ids, ISession session)
    {
      throw new NotImplementedException();
    }
    public IList<CupoInStopAndNotSIL> FindByGranoPuertoCompradorVendedor(CupoInStopAndNotSIL compPuertoGrano, ISession session)
    {
      var cupos = session.Query<CupoInStopAndNotSIL>()
                          .Where(x => x.Fecha >= DateTime.Today
                                   && x.CodigoEstadoCupo == "1"
                                   //&& x.Idterminal == compPuertoGrano.Idterminal 
                                   //&& x.grano == compPuertoGrano.grano
                                   && x.Idterminal == 36
                                   && ((compPuertoGrano.Cuitdestinatario != null && compPuertoGrano.Cuitdestinatario != "") ? x.Cuitdestinatario == compPuertoGrano.Cuitdestinatario : true)   //compcta
                                   && ((compPuertoGrano.Cuitremcomercial != null && compPuertoGrano.Cuitremcomercial != "") ? x.Cuitremcomercial == compPuertoGrano.Cuitremcomercial : true)   //vendcta
                                   && (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro))
                                  )
                                  .ToList();
      return cupos;
    }

    /// <summary>
    /// retorna todos los registros de la stopNoCorre que estan disponibles para ser autorizados y que cumplen
    /// con el filtro pasado como parametro filtro
    /// </summary>
    /// <param name="filtro"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    public IList<CupoInStopAndNotSIL> FindByFiltro(CupoInStopAndNotSIL filtro, ISession session)
    {
      var query = session.Query<CupoInStopAndNotSIL>()
          .Where(x => x.Fecha >= DateTime.Today);
      if (filtro.Idterminal != 0)
      {
        query = query.Where(x => x.Idterminal == filtro.Idterminal);
      }
      if (filtro.Grano != 0)
      {
        query = query.Where(x => x.Grano == filtro.Grano);
      }
      if (filtro.Cuitorigen != null && filtro.Cuitorigen != "")
      {
        query = query.Where(x => x.Cuitorigen == filtro.Cuitorigen);
      }
      if (filtro.Cuitintermediario != null && filtro.Cuitintermediario != "")
      {
        query = query.Where(x => x.Cuitintermediario == filtro.Cuitintermediario);
      }
      if (filtro.Cuitremcomercial != null && filtro.Cuitremcomercial != "")
      {
        query = query.Where(x => x.Cuitremcomercial == filtro.Cuitremcomercial);
      }
      if (filtro.Cuitcorredorc != null && filtro.Cuitcorredorc != "")
      {
        query = query.Where(x => x.Cuitcorredorc == filtro.Cuitcorredorc);
      }
      if (filtro.Cuitmercadoatermino != null && filtro.Cuitmercadoatermino != "")
      {
        query = query.Where(x => x.Cuitmercadoatermino == filtro.Cuitmercadoatermino);
      }
      if (filtro.Cuitcorredorv != null && filtro.Cuitcorredorv != "")
      {
        query = query.Where(x => x.Cuitcorredorv == filtro.Cuitcorredorv);
      }
      if (filtro.Cuitrepresentanteentregador != null && filtro.Cuitrepresentanteentregador != "")
      {
        query = query.Where(x => x.Cuitrepresentanteentregador == filtro.Cuitrepresentanteentregador);
      }
      if (filtro.Cuitdestinatario != null && filtro.Cuitdestinatario != "")
      {
        query = query.Where(x => x.Cuitdestinatario == filtro.Cuitdestinatario);
      }
      if (filtro.Cuitdestino != null && filtro.Cuitdestino != "")
      {
        query = query.Where(x => x.Cuitdestino == filtro.Cuitdestino);
      }
      if (filtro.Cuitintermediarioflete != null && filtro.Cuitintermediarioflete != "")
      {
        query = query.Where(x => x.Cuitintermediarioflete == filtro.Cuitintermediarioflete);
      }
      if (filtro.Cuittransportista != null && filtro.Cuittransportista != "")
      {
        query = query.Where(x => x.Cuittransportista == filtro.Cuittransportista);
      }
      if (filtro.Cuitchofer != null && filtro.Cuitchofer != "")
      {
        query = query.Where(x => x.Cuitchofer == filtro.Cuitchofer);
      }
      if (filtro.Fecha != null && filtro.Fecha != default(DateTime))
      {
        query = query.Where(x => x.Fecha == filtro.Fecha);
      }
      if (filtro.Idcupo != 0)
      {
        query = query.Where(x => x.Idcupo == filtro.Idcupo);
      }
      if (filtro.Id != 0)
      {
        query = query.Where(x => x.Id == filtro.Id);
      }
      if (filtro.Nrocupo != null && filtro.Nrocupo != "")
      {
        query = query.Where(x => x.Nrocupo == filtro.Nrocupo);
      }
      var cupos = query.Where(x => x.CodigoEstadoCupo == "1"
                    && x.Centro == filtro.Centro
                    && (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro))
            ).ToList();
      return cupos;
    }

    /// <summary>
    /// retorna todos los registros de la stopNoCorre que estan disponibles para ser autorizados y que cumplen
    /// con el filtro pasado como parametro filtro. y que ademas ACA interviene como Comprador/corredorV/corredorC
    /// </summary>
    /// <param name="filtro"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    public IList<CupoInStopAndNotSIL> FindByFiltroAndACACorredorCORDestinatarioORCorredorV(CupoInStopAndNotSIL filtro, ISession session)
    {
      string cuitAca = ConfigurationManager.AppSettings["cuitCorredorComprador"].Replace("-", "");
      var cupos = session.Query<CupoInStopAndNotSIL>()
          .Where(x => x.Fecha >= DateTime.Today
              && ((filtro.Idterminal != 0) ? x.Idterminal == filtro.Idterminal : true)
              && ((filtro.Grano != 0) ? x.Grano == filtro.Grano : true)
              && ((!string.IsNullOrEmpty(filtro.Cuitorigen)) ? x.Cuitorigen == filtro.Cuitorigen : true)
              && ((!string.IsNullOrEmpty(filtro.Cuitintermediario)) ? x.Cuitintermediario == filtro.Cuitintermediario : true)
              && (x.Cuitremcomercial == filtro.Cuitremcomercial)
              && (x.Cuitcorredorc == filtro.Cuitcorredorc)
              && ((!string.IsNullOrEmpty(filtro.Cuitmercadoatermino)) ? x.Cuitmercadoatermino == filtro.Cuitmercadoatermino : true)
              && (x.Cuitcorredorv == filtro.Cuitcorredorv)
              && ((!string.IsNullOrEmpty(filtro.Cuitrepresentanteentregador)) ? x.Cuitrepresentanteentregador == filtro.Cuitrepresentanteentregador : true)
              && ((!string.IsNullOrEmpty(filtro.Cuitdestinatario)) ? x.Cuitdestinatario == filtro.Cuitdestinatario : true)
              && ((!string.IsNullOrEmpty(filtro.Cuitdestino)) ? x.Cuitdestino == filtro.Cuitdestino : true)
              && ((!string.IsNullOrEmpty(filtro.Cuitintermediarioflete)) ? x.Cuitintermediarioflete == filtro.Cuitintermediarioflete : true)
              && ((!string.IsNullOrEmpty(filtro.Cuittransportista)) ? x.Cuittransportista == filtro.Cuittransportista : true)
              && ((!string.IsNullOrEmpty(filtro.Cuitchofer)) ? x.Cuitchofer == filtro.Cuitchofer : true)
              && ((filtro.Fecha != null && filtro.Fecha != default(DateTime)) ? x.Fecha == filtro.Fecha : true)
              && ((filtro.Idcupo != 0) ? x.Idcupo == filtro.Idcupo : true)
              && ((filtro.Id != 0) ? x.Id == filtro.Id : true)
              && ((!string.IsNullOrEmpty(filtro.Nrocupo)) ? x.Nrocupo == filtro.Nrocupo : true)
              && ((!string.IsNullOrEmpty(filtro.Centro)) ? x.Centro == filtro.Centro : true)
              && x.CodigoEstadoCupo == "1"
              && (x.Cuitdestinatario == cuitAca || x.Cuitcorredorc == cuitAca || x.Cuitcorredorv == cuitAca)
              && (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro))
      ).ToList();
      return cupos;
    }

    /// <summary>
    /// retorna todos los registros de la stopNoCorre que estan disponibles para ser autorizados y posteriormente agregados a SIL
    /// </summary>
    /// <param name="Session"></param>
    /// <returns></returns>
    public IList<CupoInStopAndNotSIL> FindAll(ISession Session)
    {
      var cupos = Session.Query<CupoInStopAndNotSIL>()
          .Where(x => x.Fecha >= DateTime.Today
                  && x.CodigoEstadoCupo == "1"
                  && (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro))
                  )
              .ToList();
      return cupos;
    }

    /// <summary>
    /// Retorna todos los registros de la stopNoCorre que estan disponibles para ser autorizados y posteriormente agregados a SIL
    /// en los que ACA intervenga como Comprador/corredorV/corredorC
    /// </summary>
    /// <param name="Session"></param>
    /// <returns></returns>
    public IList<CupoInStopAndNotSIL> FindAllWithACACorredorCORDestinatarioORCorredorV(ISession Session)
    {
      string cuitAca = ConfigurationManager.AppSettings["cuitCorredorComprador"].Replace("-", "");
      var cupos = Session.Query<CupoInStopAndNotSIL>()
              .Where(x => x.Fecha >= DateTime.Today
                  && x.CodigoEstadoCupo == "1"
                  && (x.Cuitdestinatario == cuitAca || x.Cuitcorredorc == cuitAca || x.Cuitcorredorv == cuitAca)
                  && (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro))
              )
          .ToList();
      return cupos;
    }

    public IList<CupoInStopAndNotSIL> FindByAlfasWithACACorredorCORDestinatarioORCorredorV(CupoInStopAndNotSIL filtro, IList<string> alfas, ISession Session)
    {
      string cuitAca = ConfigurationManager.AppSettings["cuitCorredorComprador"].Replace("-", "");
      List<CupoInStopAndNotSIL> cupos = Session.Query<CupoInStopAndNotSIL>()
          .Where(x => x.Fecha >= DateTime.Today
              && ((filtro.Idterminal != 0) ? x.Idterminal == filtro.Idterminal : true)
              && ((filtro.Grano != 0) ? x.Grano == filtro.Grano : true)
              && ((filtro.Cuitorigen != null && filtro.Cuitorigen != "") ? x.Cuitorigen == filtro.Cuitorigen : true)
              && ((filtro.Cuitintermediario != null && filtro.Cuitintermediario != "") ? x.Cuitintermediario == filtro.Cuitintermediario : true)
              && ((filtro.Cuitremcomercial != null && filtro.Cuitremcomercial != "") ? x.Cuitremcomercial == filtro.Cuitremcomercial : true)
              && ((filtro.Cuitcorredorc != null && filtro.Cuitcorredorc != "") ? x.Cuitcorredorc == filtro.Cuitcorredorc : true)
              && ((filtro.Cuitmercadoatermino != null && filtro.Cuitmercadoatermino != "") ? x.Cuitmercadoatermino == filtro.Cuitmercadoatermino : true)
              && ((filtro.Cuitcorredorv != null && filtro.Cuitcorredorv != "") ? x.Cuitcorredorv == filtro.Cuitcorredorv : true)
              && ((filtro.Cuitrepresentanteentregador != null && filtro.Cuitrepresentanteentregador != "") ? x.Cuitrepresentanteentregador == filtro.Cuitrepresentanteentregador : true)
              && ((filtro.Cuitdestinatario != null && filtro.Cuitdestinatario != "") ? x.Cuitdestinatario == filtro.Cuitdestinatario : true)
              && ((filtro.Cuitdestino != null && filtro.Cuitdestino != "") ? x.Cuitdestino == filtro.Cuitdestino : true)
              && ((filtro.Cuitintermediarioflete != null && filtro.Cuitintermediarioflete != "") ? x.Cuitintermediarioflete == filtro.Cuitintermediarioflete : true)
              && ((filtro.Cuittransportista != null && filtro.Cuittransportista != "") ? x.Cuittransportista == filtro.Cuittransportista : true)
              && ((filtro.Cuitchofer != null && filtro.Cuitchofer != "") ? x.Cuitchofer == filtro.Cuitchofer : true)
              && ((filtro.Fecha != null && filtro.Fecha != default(DateTime)) ? x.Fecha == filtro.Fecha : true)
              && ((filtro.Idcupo != 0) ? x.Idcupo == filtro.Idcupo : true)
              && ((filtro.Id != 0) ? x.Id == filtro.Id : true)
              && x.Centro == filtro.Centro
              && x.CodigoEstadoCupo == "1"
              && (x.Cuitdestinatario == cuitAca || x.Cuitcorredorc == cuitAca || x.Cuitcorredorv == cuitAca)
              && alfas.Contains(x.Nrocupo)
              && (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro))
      ).ToList();
      return cupos;
    }
  }
}