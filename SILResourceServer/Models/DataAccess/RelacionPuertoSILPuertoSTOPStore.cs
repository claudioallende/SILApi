using NHibernate;
using ResourceServer.Models.Error.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
  public class RelacionPuertoSILPuertoSTOPStore : IRelacionPuertoSILPuertoSTOPStore
  {
    #region private method

    /// <summary>
    /// existe esta relacion retorna true si existe un registro con el mismo nroGranoSIL y mismo nroGranoSTOP
    /// </summary>
    /// <param name="relacion"></param>
    /// <param name="session"></param>
    /// <returns></returns>
    private bool existeEstaRelacion(RelacionPuertoSILPuertoSTOP relacion, ISession session)
    {
      return this.FindCompleteRelationByPuertoSILAndPuertoSTOP(relacion.NroPuertoSIL, relacion.NroPuertoSTOP, session) != null;
    }
    private void quitarValorPorDefectoDeRelacionExistente(RelacionPuertoSILPuertoSTOP relacion, ISession session, ITransaction transaccion = null)
    {
      IList<RelacionPuertoSILPuertoSTOP> listaDeRelaciones = this.FindCompleteRelationByPuertoSTOP(relacion.NroPuertoSTOP, session);
      foreach (RelacionPuertoSILPuertoSTOP var in listaDeRelaciones)
      {
        if (var.ValorPorDefecto == 1)
        {
          var.ValorPorDefecto = 0;
          this.UpdateRelacion(var, session, transaccion);
          break;
        }
      }
    }
    private bool EsLaPrimerRelacionParaEstePuertoSTOP(long NroPuertoStop, ISession session)
    {
      if (NroPuertoStop != 0)
      {
        var query = session.Query<RelacionPuertoSILPuertoSTOP>().Where(c => c.NroPuertoSTOP == NroPuertoStop).ToList<RelacionPuertoSILPuertoSTOP>();
        return query.Count() == 0;
      }
      return true;
    }

    public RelacionPuertoSILPuertoSTOP FindRelationById(int id, ISession session)
    {
      if (id != 0)
      {
        var relacion = session.Query<RelacionPuertoSILPuertoSTOP>().Where(c => c.Id == id).FirstOrDefault();
        return relacion;
      }
      return null;
    }

    #endregion

    public bool Delete(RelacionPuertoSILPuertoSTOP relacion, ISession session)
    {
      if (relacion != null)
      {
        RelacionPuertoSILPuertoSTOP result = this.FindRelationById(relacion.Id, session);
        if (result != null)// && result.ValorPorDefecto != 1)
        {
          using (ITransaction tx = session.BeginTransaction())
          {
            try
            {
              session.Delete(result);
              tx.Commit();
              return true;
            }
            catch (Exception e)
            {
              tx.Rollback();
              throw e;
            }
          }
        }
      }
      return false;
    }

    /*si no tiene relacion return true*/
    public bool DeleteAllRelationOfPuertoSTOP(long NroPuertoSTOP, ISession session, ITransaction tx1 = null)
    {
      if (NroPuertoSTOP != 0)
      {
        List<RelacionPuertoSILPuertoSTOP> result = new List<RelacionPuertoSILPuertoSTOP>();
        result = (List<RelacionPuertoSILPuertoSTOP>)this.FindCompleteRelationByPuertoSTOP(NroPuertoSTOP, session);

        if (result.Count > 0)
        {
          ITransaction tx = (tx1 != null) ? tx1 : session.BeginTransaction();
          try
          {
            foreach (RelacionPuertoSILPuertoSTOP a in result)
            {
              session.Delete(a);
            }
            if (tx1 == null)
            {
              tx.Commit();
            }
          }
          catch (Exception e)
          {
            tx.Rollback();
            throw e;
          }
        }
      }
      return true;
    }

    public IList<RelacionPuertoSILPuertoSTOP> FindAllRelations(ISession session)
    {
      var query = session.Query<RelacionPuertoSILPuertoSTOP>();
      var joinPuerto = query.Join(session.Query<Puerto>(), RelacionCorr => RelacionCorr.NroPuertoSIL, puerto => puerto.Cuenta, (relCorr, puerto) => new { RelacionCorriente = relCorr, Puerto = puerto});
      var joinPuertoSTOP = joinPuerto.Join(session.Query<CuposPuertoSTOP>(), RelacionConPuertoSIL => RelacionConPuertoSIL.RelacionCorriente.NroPuertoSTOP, PuertoSTOP => PuertoSTOP.NroPuerto, (relacionConPuertoSIL, puertoSTOP) => new { RelacionConPuertoSIL = relacionConPuertoSIL, PuertoSTOPRelacionado = puertoSTOP });
      IList<RelacionPuertoSILPuertoSTOP> relacionCompleta = joinPuertoSTOP.Select(x =>
                                                                          new RelacionPuertoSILPuertoSTOP
                                                                          {
                                                                            NroPuertoSIL = x.RelacionConPuertoSIL.Puerto.Cuenta,
                                                                            NombrePuertoSIL = x.RelacionConPuertoSIL.Puerto.Nombre,
                                                                            NroPuertoSTOP= x.PuertoSTOPRelacionado.NroPuerto,
                                                                            NombrePuertoSTOP = x.PuertoSTOPRelacionado.NombrePuerto,
                                                                            ValorPorDefecto = x.RelacionConPuertoSIL.RelacionCorriente.ValorPorDefecto,
                                                                            Id = x.RelacionConPuertoSIL.RelacionCorriente.Id
                                                                          }).OrderBy(x => x.NroPuertoSTOP)
                                                                            .ToList();
      return relacionCompleta;
    }

    public RelacionPuertoSILPuertoSTOP FindCompleteRelationByPuertoSILAndPuertoSTOP(long NroPuertoSIL, long nroPuertoSTOP, ISession session)
    {
      if (NroPuertoSIL>0 && nroPuertoSTOP>0)
      {
        var query = session.Query<RelacionPuertoSILPuertoSTOP>()
            .Where(c => c.NroPuertoSIL == NroPuertoSIL
                    && c.NroPuertoSTOP== nroPuertoSTOP);
        var joinPuerto = query.Join(session.Query<Puerto>(), RelacionCorr => RelacionCorr.NroPuertoSIL, puerto => puerto.Cuenta, (relCorr, puerto) => new { RelacionCorriente = relCorr, Puerto = puerto });
        var joinPuertoSTOP = joinPuerto.Join(session.Query<CuposPuertoSTOP>(), RelacionConPuertoSIL => RelacionConPuertoSIL.RelacionCorriente.NroPuertoSTOP, PuertoSTOP => PuertoSTOP.NroPuerto, (relacionConPuertoSIL, puertoSTOP) => new { RelacionConPuertoSIL = relacionConPuertoSIL, PuertoSTOPRelacionado = puertoSTOP });

        RelacionPuertoSILPuertoSTOP relacionCompleta = joinPuertoSTOP.Select(x =>
                                                                            new RelacionPuertoSILPuertoSTOP
                                                                            {
                                                                              NroPuertoSIL = x.RelacionConPuertoSIL.Puerto.Cuenta,
                                                                              NombrePuertoSIL = x.RelacionConPuertoSIL.Puerto.Nombre,
                                                                              NroPuertoSTOP = x.PuertoSTOPRelacionado.NroPuerto,
                                                                              NombrePuertoSTOP = x.PuertoSTOPRelacionado.NombrePuerto,
                                                                              ValorPorDefecto = x.RelacionConPuertoSIL.RelacionCorriente.ValorPorDefecto,
                                                                              Id = x.RelacionConPuertoSIL.RelacionCorriente.Id
                                                                            }).FirstOrDefault();
        return relacionCompleta;
      }
      return null;
    }

    public RelacionPuertoSILPuertoSTOP FindCompleteRelationById(int id, ISession session)
    {
      if (id != 0)
      {
        var query = session.Query<RelacionPuertoSILPuertoSTOP>()
            .Where(c => c.Id == id);
        var joinPuerto = query.Join(session.Query<Puerto>(), RelacionCorr => RelacionCorr.NroPuertoSIL, puerto => puerto.Cuenta, (relCorr, puerto) => new { RelacionCorriente = relCorr, Puerto = puerto });
        var joinPuertoSTOP = joinPuerto.Join(session.Query<CuposPuertoSTOP>(), RelacionConPuertoSIL => RelacionConPuertoSIL.RelacionCorriente.NroPuertoSTOP, PuertoSTOP => PuertoSTOP.NroPuerto, (relacionConPuertoSIL, puertoSTOP) => new { RelacionConPuertoSIL = relacionConPuertoSIL, PuertoSTOPRelacionado = puertoSTOP });

        RelacionPuertoSILPuertoSTOP relacionCompleta = joinPuertoSTOP.Select(x =>
                                                                            new RelacionPuertoSILPuertoSTOP
                                                                            {
                                                                              NroPuertoSIL = x.RelacionConPuertoSIL.Puerto.Cuenta,
                                                                              NombrePuertoSIL = x.RelacionConPuertoSIL.Puerto.Nombre,
                                                                              NroPuertoSTOP = x.PuertoSTOPRelacionado.NroPuerto,
                                                                              NombrePuertoSTOP = x.PuertoSTOPRelacionado.NombrePuerto,
                                                                              ValorPorDefecto = x.RelacionConPuertoSIL.RelacionCorriente.ValorPorDefecto,
                                                                              Id = x.RelacionConPuertoSIL.RelacionCorriente.Id
                                                                            }).FirstOrDefault();
        return relacionCompleta;
      }
      return null;
    }

    public IList<RelacionPuertoSILPuertoSTOP> FindCompleteRelationByPuertoSIL(long NroGrano, ISession session)
    {
      if (NroGrano != 0)
      {
        var query = session.Query<RelacionPuertoSILPuertoSTOP>()
            .Where(c => c.NroPuertoSIL == NroGrano);
        var joinPuerto = query.Join(session.Query<Puerto>(), RelacionCorr => RelacionCorr.NroPuertoSIL, puerto => puerto.Cuenta, (relCorr, puerto) => new { RelacionCorriente = relCorr, Puerto = puerto });
        var joinPuertoSTOP = joinPuerto.Join(session.Query<CuposPuertoSTOP>(), RelacionConPuertoSIL => RelacionConPuertoSIL.RelacionCorriente.NroPuertoSTOP, PuertoSTOP => PuertoSTOP.NroPuerto, (relacionConPuertoSIL, puertoSTOP) => new { RelacionConPuertoSIL = relacionConPuertoSIL, PuertoSTOPRelacionado = puertoSTOP });

        IList<RelacionPuertoSILPuertoSTOP> relacionCompleta = joinPuertoSTOP.Select(x =>
                                                                            new RelacionPuertoSILPuertoSTOP
                                                                            {
                                                                              NroPuertoSIL = x.RelacionConPuertoSIL.Puerto.Cuenta,
                                                                              NombrePuertoSIL = x.RelacionConPuertoSIL.Puerto.Nombre,
                                                                              NroPuertoSTOP = x.PuertoSTOPRelacionado.NroPuerto,
                                                                              NombrePuertoSTOP = x.PuertoSTOPRelacionado.NombrePuerto,
                                                                              ValorPorDefecto = x.RelacionConPuertoSIL.RelacionCorriente.ValorPorDefecto,
                                                                              Id = x.RelacionConPuertoSIL.RelacionCorriente.Id
                                                                            }).ToList();
        return relacionCompleta;
      }
      return null;
    }

    public IList<RelacionPuertoSILPuertoSTOP> FindCompleteRelationByPuertoSTOP(long NroGrano, ISession session)
    {
      if (NroGrano != 0)
      {
        var query = session.Query<RelacionPuertoSILPuertoSTOP>()
            .Where(c => c.NroPuertoSTOP == NroGrano);
        var joinPuerto = query.Join(session.Query<Puerto>(), RelacionCorr => RelacionCorr.NroPuertoSIL, puerto => puerto.Cuenta, (relCorr, puerto) => new { RelacionCorriente = relCorr, Puerto = puerto });
        var joinPuertoSTOP = joinPuerto.Join(session.Query<CuposPuertoSTOP>(), RelacionConPuertoSIL => RelacionConPuertoSIL.RelacionCorriente.NroPuertoSTOP, PuertoSTOP => PuertoSTOP.NroPuerto, (relacionConPuertoSIL, puertoSTOP) => new { RelacionConPuertoSIL = relacionConPuertoSIL, PuertoSTOPRelacionado = puertoSTOP });

        IList<RelacionPuertoSILPuertoSTOP> relacionCompleta = joinPuertoSTOP.Select(x =>
                                                                            new RelacionPuertoSILPuertoSTOP
                                                                            {
                                                                              NroPuertoSIL = x.RelacionConPuertoSIL.Puerto.Cuenta,
                                                                              NombrePuertoSIL = x.RelacionConPuertoSIL.Puerto.Nombre,
                                                                              NroPuertoSTOP = x.PuertoSTOPRelacionado.NroPuerto,
                                                                              NombrePuertoSTOP = x.PuertoSTOPRelacionado.NombrePuerto,
                                                                              ValorPorDefecto = x.RelacionConPuertoSIL.RelacionCorriente.ValorPorDefecto,
                                                                              Id = x.RelacionConPuertoSIL.RelacionCorriente.Id
                                                                            }).ToList();
        return relacionCompleta;
      }
      return null;
    }

    public RelacionPuertoSILPuertoSTOP Save(RelacionPuertoSILPuertoSTOP nuevaRelacion, ISession session)
    {
      if (nuevaRelacion != null)
      {
        IList<RelacionPuertoSILPuertoSTOP> listaDeRelaciones = new List<RelacionPuertoSILPuertoSTOP>();
        if (!this.existeEstaRelacion(nuevaRelacion, session))
        {
          using (ITransaction tx = session.BeginTransaction())
          {
            try
            {
              if (nuevaRelacion.ValorPorDefecto == 1)
              {       /*cambiamos valor por defecto*/
                this.quitarValorPorDefectoDeRelacionExistente(nuevaRelacion, session, tx);
              }
              else
              {
                if (this.EsLaPrimerRelacionParaEstePuertoSTOP(nuevaRelacion.NroPuertoSTOP, session))
                {
                  nuevaRelacion.ValorPorDefecto = 1;
                }
              }
              session.Save(nuevaRelacion);
              tx.Commit();
              return this.FindCompleteRelationByPuertoSILAndPuertoSTOP(nuevaRelacion.NroPuertoSIL, nuevaRelacion.NroPuertoSTOP, session);
            }
            catch (NHibernate.Exceptions.GenericADOException e)
            {
              if (e.GetBaseException().Message.Contains("ORA-00001"))
              {
                tx.Rollback();
                HibernateUtil.Dispose();
                throw new RegistroDuplicadoException("La relación");
              }
              else
              {
                tx.Rollback();
                HibernateUtil.Dispose();
                throw e;
              }
            }
            catch (Exception e)
            {
              tx.Rollback();
              HibernateUtil.Dispose();
              throw e;
            }
          }
        }
        throw new RegistroDuplicadoException("La relación");
      }
      return null;
    }

    public RelacionPuertoSILPuertoSTOP UpdateRelacion(RelacionPuertoSILPuertoSTOP relacion, ISession session, ITransaction transaccion = null)
    {
      RelacionPuertoSILPuertoSTOP result = new RelacionPuertoSILPuertoSTOP();
      if (relacion != null)
      {
        RelacionPuertoSILPuertoSTOP RelacionesExistente = new RelacionPuertoSILPuertoSTOP();
        RelacionesExistente = this.FindCompleteRelationByPuertoSILAndPuertoSTOP(relacion.NroPuertoSIL, relacion.NroPuertoSTOP, session);
        if (RelacionesExistente != null)
        {
          /*cambia solo valor por defecto*/
          if (RelacionesExistente.Equals(relacion) && RelacionesExistente.ValorPorDefecto != relacion.ValorPorDefecto && RelacionesExistente.Id == relacion.Id)
          { 
            using (ITransaction tx = session.BeginTransaction())
            {
              /*cambia solo valor por defecto*/
              this.quitarValorPorDefectoDeRelacionExistente(relacion, session, tx);
              if (relacion.ValorPorDefecto == 1)
              {
                this.Update(relacion, session, tx);
              }
              tx.Commit();
              result = this.FindCompleteRelationByPuertoSILAndPuertoSTOP(relacion.NroPuertoSIL, relacion.NroPuertoSTOP, session);
            }
          }
          /*si no entro al If anterior es porque la modificacion introducia una relacion que ya existia en BD*/
          return result;
        }
        else
        {
          RelacionesExistente = this.FindCompleteRelationById(relacion.Id, session);
          if (RelacionesExistente != null)
          {
            using (ITransaction tx = session.BeginTransaction())
            {
              /*si lo que cambia es SIL es directo y este va por defecto sii para el mismo GranoSTOP no hay una relacion*/
              if (RelacionesExistente.NroPuertoSIL != relacion.NroPuertoSIL && RelacionesExistente.NroPuertoSTOP == relacion.NroPuertoSTOP)
              {
                //si ya hay reac para el mismo granoSTOP
                if (this.FindCompleteRelationByPuertoSTOP(relacion.NroPuertoSTOP, session).Count > 0)
                {
                  if (relacion.ValorPorDefecto == 1)
                  {
                    this.quitarValorPorDefectoDeRelacionExistente(relacion, session, tx);
                  }
                  this.Update(relacion, session, tx);
                }
                else
                {
                  relacion.ValorPorDefecto = 1;
                  this.Update(relacion, session, tx);
                }


              }       /*si lo que cambia es STOP analiza si cambia o no el valor por defecto*/
              else if (RelacionesExistente.NroPuertoSIL == relacion.NroPuertoSIL && RelacionesExistente.NroPuertoSTOP != relacion.NroPuertoSTOP)
              {
                if (relacion.ValorPorDefecto == 1)
                {
                  this.quitarValorPorDefectoDeRelacionExistente(relacion, session, tx);
                }
                this.Update(relacion, session, tx);
              }
              tx.Commit();
            }
            result = this.FindCompleteRelationByPuertoSILAndPuertoSTOP(relacion.NroPuertoSIL, relacion.NroPuertoSTOP, session);
            return result;
          }
        }
      }
      return null;
    }

    public void Update(RelacionPuertoSILPuertoSTOP relacion, ISession session, ITransaction transaccion = null)
    {
      ITransaction tx = transaccion ?? session.BeginTransaction();
      try
      {
        session.Update(relacion);
        if (transaccion == null)
        {
          tx.Commit();
          session.Transaction.Dispose();
        }
      }
      catch (NHibernate.Exceptions.GenericADOException e)
      {
        if (e.GetBaseException().Message.Contains("ORA-00001"))
        {
          tx.Rollback();
          HibernateUtil.Dispose();
          throw new RegistroDuplicadoException("La relación");
        }
        else
        {
          tx.Rollback();
          HibernateUtil.Dispose();
          throw e;
        }
      }
      catch (Exception e)
      {
        tx.Rollback();
        HibernateUtil.Dispose();
        throw e;
      }
    }
  }
}