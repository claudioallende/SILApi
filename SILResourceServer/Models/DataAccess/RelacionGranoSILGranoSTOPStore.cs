using NHibernate;
using ResourceServer.Models.DTO;
using ResourceServer.Models.Error.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class RelacionGranoSILGranoSTOPStore : IRelacionGranoSILGranoSTOPStore
    {
        /// <summary>
        /// antes de hacer el save verifica si existe, para el mismo nroGranoSTOP alguna relacion. 
        /// si la relacion que estoy dando de alta es el valor por defecto. primero hago update del anterior valor por defecto 
        /// y luego el save de la nueva Relacion. la cual sera el nuevo valor por defecto
        /// </summary>
        /// <param name="nuevaRelacion"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public RelacionGranoSILGranoSTOPCompleta Save(RelacionGranoSILGranoSTOPCompleta nuevaRelacion, ISession session)
        {
            if (nuevaRelacion != null)
            {
                IList <RelacionGranoSILGranoSTOPCompleta> listaDeRelaciones = new List<RelacionGranoSILGranoSTOPCompleta>();
                if(!this.existeEstaRelacion(nuevaRelacion,session))
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
                                if(this.EsLaPrimerRelacionParaEsteGranoSTOP(nuevaRelacion.NroGranoSTOP,session))
                                {
                                    nuevaRelacion.ValorPorDefecto = 1;
                                }
                            }
                            session.Save(nuevaRelacion);
                            tx.Commit();
                            return this.FindRelacionCompletaByNroGranoSILAndNroGranoSTOP(nuevaRelacion, session);
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

        /// <summary>
        /// existe esta relacion retorna true si existe un registro con el mismo nroGranoSIL y mismo nroGranoSTOP
        /// </summary>
        /// <param name="relacion"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool existeEstaRelacion(RelacionGranoSILGranoSTOPCompleta relacion, ISession session) 
        {
            return (this.FindRelacionCompletaByNroGranoSILAndNroGranoSTOP(relacion, session) != null);
        }

        public void quitarValorPorDefectoDeRelacionExistente(RelacionGranoSILGranoSTOPCompleta relacion, ISession session, ITransaction transaccion = null) 
        {
            IList<RelacionGranoSILGranoSTOPCompleta> listaDeRelaciones = this.FindRelacionCompletaByNroGranoSTOP(relacion.NroGranoSTOP, session);
            foreach (RelacionGranoSILGranoSTOPCompleta var in listaDeRelaciones)
            {
                if (var.ValorPorDefecto == 1)
                {
                    var.ValorPorDefecto = 0;
                    this.Update(var, session, transaccion);
                    break;
                }
            }
        }

        public bool EsLaPrimerRelacionParaEsteGranoSTOP(long NroGranoSTOP, ISession session) 
        {
            if (NroGranoSTOP != 0) 
            {
                var query = session.Query<RelacionGranoSILGranoSTOPCompleta>()
                   .Where(c => c.NroGranoSTOP == NroGranoSTOP)
                   .ToList<RelacionGranoSILGranoSTOPCompleta>();
                return query.Count() == 0;
            }
            return true;     
        }

        /// <summary>
        /// si lo que cambia es el nrgrano SIL no problem.!
        /// si son otros casos lo defini en el cuaderno d bit
        /// </summary>
        /// <param name="relacion"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public RelacionGranoSILGranoSTOPCompleta UpdateRelacion(RelacionGranoSILGranoSTOPCompleta relacion, ISession session)
        {
            RelacionGranoSILGranoSTOPCompleta result = new RelacionGranoSILGranoSTOPCompleta();
            if (relacion != null)
            {
                RelacionGranoSILGranoSTOPCompleta RelacionesExistente = new RelacionGranoSILGranoSTOPCompleta();
                RelacionesExistente = this.FindRelacionCompletaByNroGranoSILAndNroGranoSTOP(relacion, session);
                if (RelacionesExistente != null)
                {
                                    /*cambia solo valor por defecto*/
                    if (RelacionesExistente.Equals(relacion) && RelacionesExistente.ValorPorDefecto != relacion.ValorPorDefecto && RelacionesExistente.Id==relacion.Id)
                    {           /*cambio valor por defecto*/
                        using (ITransaction tx = session.BeginTransaction())
                        {
                                    /*cambia solo valor por defecto*/
                            this.quitarValorPorDefectoDeRelacionExistente(relacion, session, tx);
                            if (relacion.ValorPorDefecto == 1)
                            {
                                this.Update(relacion, session, tx);
                            }
                            tx.Commit();
                            result = this.FindRelacionCompletaByNroGranoSILAndNroGranoSTOP(relacion, session);
                        }
                    }   
                    /*si no entro al If anterior es porque la modificacion introducia una relacion que ya existia en BD*/
                    return result;
                }else
                    {
                        RelacionesExistente = this.FindRelacionCompletaById(relacion.Id, session);
                        if (RelacionesExistente != null)
                        {
                            using (ITransaction tx = session.BeginTransaction())
                            {
                                            /*si lo que cambia es SIL es directo y este va por defecto sii para el mismo GranoSTOP no hay una relacion*/
                                if (RelacionesExistente.NroGranoSIL != relacion.NroGranoSIL && RelacionesExistente.NroGranoSTOP == relacion.NroGranoSTOP)
                                {
                                    //si ya hay reac para el mismo granoSTOP
                                    if (this.FindRelacionCompletaByNroGranoSTOP(relacion.NroGranoSTOP, session).Count > 0)
                                    {
                                        if (relacion.ValorPorDefecto == 1)
                                        {
                                            this.quitarValorPorDefectoDeRelacionExistente(relacion, session, tx);
                                        }
                                        this.Update(relacion, session, tx);
                                    }
                                    else {
                                        relacion.ValorPorDefecto = 1;
                                        this.Update(relacion, session, tx);
                                    }

                                    
                                }       /*si lo que cambia es STOP analiza si cambia o no el valor por defecto*/
                                else if (RelacionesExistente.NroGranoSIL == relacion.NroGranoSIL && RelacionesExistente.NroGranoSTOP != relacion.NroGranoSTOP)
                                {
                                    if (relacion.ValorPorDefecto == 1)
                                    {
                                        this.quitarValorPorDefectoDeRelacionExistente(relacion, session, tx);
                                    }
                                    this.Update(relacion, session, tx);
                                }
                                tx.Commit();
                            }
                            result = this.FindRelacionCompletaByNroGranoSILAndNroGranoSTOP(relacion, session);
                            return result;
                        }
                    }
            }
            return null;             
        }

        public void Update(RelacionGranoSILGranoSTOPCompleta relacion, ISession session, ITransaction transaccion = null)
        {
            ITransaction tx = (transaccion == null) ? session.BeginTransaction() : transaccion;
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

        /// <summary>
        /// si eliminamos alguna relacion. debemos buscar las relaciones con el mismo nroDeGranoSTOP de mayores ordenes y
        /// decrementarlas en 1
        /// </summary>
        /// <param name="relacion"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool Delete(RelacionGranoSILGranoSTOPCompleta relacion, ISession session)
        {
            if (relacion != null)
            {
                RelacionGranoSILGranoSTOPCompleta result = this.FindRelacionCompletaById(relacion.Id, session);
                if (result != null && result.ValorPorDefecto != 1) 
                {
                    using (ITransaction tx = session.BeginTransaction())
                    {
                        try
                        {
                            session.Delete(relacion);
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

        /// <summary>
        /// este metodo es el utilizado por el servicio GranoSTOP
        /// ya que al eliminar un grano de STOP debe eliminar todas sus relaciones
        /// </summary>
        /// <param name="NrogranoSTOP"></param>
        /// <param name="session"></param>
        /// <param name="tx1"></param>
        /// <returns></returns>
        public bool DeleteTodasLasRelacionesDeGranoSTOP(long NrogranoSTOP, ISession session, ITransaction tx1 = null)
        {
            if (NrogranoSTOP != 0)
            {
                List<RelacionGranoSILGranoSTOPCompleta> result = new List<RelacionGranoSILGranoSTOPCompleta>();
                result = (List<RelacionGranoSILGranoSTOPCompleta>)this.FindRelacionCompletaByNroGranoSTOP(NrogranoSTOP, session);

                if (result.Count > 0)
                {
                    ITransaction tx = (tx1 != null)? tx1 : session.BeginTransaction();
                    try
                    {
                        foreach (RelacionGranoSILGranoSTOPCompleta a in result)
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
                return true;
            }
            return false;
        }

        public IList<RelacionGranoSILGranoSTOPCompleta> FindAllRelacionCompleta(ISession session)
        {
            var query = session.Query<RelacionGranoSILGranoSTOPCompleta>();
            var joinGrano = query.Join(session.Query<Grano>(), RelacionCorr => RelacionCorr.NroGranoSIL, grano => grano.CodigoGrano, (relCorr, grano) => new { RelacionCorriente = relCorr, Grano = grano });
            var joinGranoSTOP = joinGrano.Join(session.Query<CuposGranoSTOP>(), RelacionConGranoSIL => RelacionConGranoSIL.RelacionCorriente.NroGranoSTOP, GranoSTOP => GranoSTOP.NroGrano, (relacionConGranoSIL, granoSTOP) => new { RelacionadoConGranoSIL = relacionConGranoSIL, GranoSTOPRelacionado = granoSTOP });
            IList<RelacionGranoSILGranoSTOPCompleta> relacionCompleta = joinGranoSTOP.Select(x =>
                                                                                new RelacionGranoSILGranoSTOPCompleta
                                                                                {
                                                                                    NroGranoSIL = x.RelacionadoConGranoSIL.Grano.CodigoGrano,
                                                                                    nombreGranoSil = x.RelacionadoConGranoSIL.Grano.Nombre,
                                                                                    NroGranoSTOP = x.GranoSTOPRelacionado.NroGrano,
                                                                                    nombreGranoSTOP = x.GranoSTOPRelacionado.NombreGrano,
                                                                                    ValorPorDefecto = x.RelacionadoConGranoSIL.RelacionCorriente.ValorPorDefecto,
                                                                                    Id = x.RelacionadoConGranoSIL.RelacionCorriente.Id
                                                                                }).OrderBy(x=> x.NroGranoSTOP)
                                                                                  .ToList();
            return relacionCompleta;
        }

        public IList<RelacionGranoSILGranoSTOPCompleta> FindRelacionCompletaByNroGranoSTOP(long NroGrano, ISession session)
        {
            if (NroGrano != 0)
            {
                var query = session.Query<RelacionGranoSILGranoSTOPCompleta>()
                    .Where(c => c.NroGranoSTOP == NroGrano);
                var joinGrano = query.Join(session.Query<Grano>(), RelacionCorr => RelacionCorr.NroGranoSIL, grano => grano.CodigoGrano, (relCorr, grano) => new { RelacionCorriente = relCorr, Grano = grano });
                var joinGranoSTOP = joinGrano.Join(session.Query<CuposGranoSTOP>(), RelacionConGranoSIL => RelacionConGranoSIL.RelacionCorriente.NroGranoSTOP, GranoSTOP => GranoSTOP.NroGrano, (relacionConGranoSIL, granoSTOP) => new { RelacionadoConGranoSIL = relacionConGranoSIL, GranoSTOPRelacionado = granoSTOP });
                IList<RelacionGranoSILGranoSTOPCompleta> relacionCompleta = joinGranoSTOP.Select(x =>
                                                                                    new RelacionGranoSILGranoSTOPCompleta
                                                                                    {
                                                                                        NroGranoSIL = x.RelacionadoConGranoSIL.Grano.CodigoGrano,
                                                                                        nombreGranoSil = x.RelacionadoConGranoSIL.Grano.Nombre,
                                                                                        NroGranoSTOP = x.GranoSTOPRelacionado.NroGrano,
                                                                                        nombreGranoSTOP = x.GranoSTOPRelacionado.NombreGrano,
                                                                                        ValorPorDefecto = x.RelacionadoConGranoSIL.RelacionCorriente.ValorPorDefecto,
                                                                                        Id = x.RelacionadoConGranoSIL.RelacionCorriente.Id
                                                                                    }).ToList();
                return relacionCompleta;
            }
            return null;
        }

        public IList<RelacionGranoSILGranoSTOPCompleta> FindRelacionCompletaByNroGranoSIL(long NroGrano, ISession session)
        {
            if (NroGrano != 0)
            {
                var query = session.Query<RelacionGranoSILGranoSTOPCompleta>()
                    .Where(c => c.NroGranoSIL == NroGrano);
                var joinGrano = query.Join(session.Query<Grano>(), RelacionCorr => RelacionCorr.NroGranoSIL, grano => grano.CodigoGrano, (relCorr, grano) => new { RelacionCorriente = relCorr, Grano = grano });
                var joinGranoSTOP = joinGrano.Join(session.Query<CuposGranoSTOP>(), RelacionConGranoSIL => RelacionConGranoSIL.RelacionCorriente.NroGranoSTOP, GranoSTOP => GranoSTOP.NroGrano, (relacionConGranoSIL, granoSTOP) => new { RelacionadoConGranoSIL = relacionConGranoSIL, GranoSTOPRelacionado = granoSTOP });
                IList<RelacionGranoSILGranoSTOPCompleta> relacionCompleta = joinGranoSTOP.Select(x =>
                                                                                    new RelacionGranoSILGranoSTOPCompleta
                                                                                    {
                                                                                        NroGranoSIL = x.RelacionadoConGranoSIL.Grano.CodigoGrano,
                                                                                        nombreGranoSil = x.RelacionadoConGranoSIL.Grano.Nombre,
                                                                                        NroGranoSTOP = x.GranoSTOPRelacionado.NroGrano,
                                                                                        nombreGranoSTOP = x.GranoSTOPRelacionado.NombreGrano,
                                                                                        ValorPorDefecto = x.RelacionadoConGranoSIL.RelacionCorriente.ValorPorDefecto,
                                                                                        Id = x.RelacionadoConGranoSIL.RelacionCorriente.Id
                                                                                    }).ToList();
                return relacionCompleta;
            }
            return null;
        }

        public RelacionGranoSILGranoSTOPCompleta FindRelacionCompletaByNroGranoSILAndNroGranoSTOP(RelacionGranoSILGranoSTOPCompleta relacion, ISession session)
        {
            if (relacion != null)
            {
                var query = session.Query<RelacionGranoSILGranoSTOPCompleta>()
                    .Where(c => c.NroGranoSIL == relacion.NroGranoSIL
                            && c.NroGranoSTOP == relacion.NroGranoSTOP);              
                var joinGrano = query.Join(session.Query<Grano>(), RelacionCorr => RelacionCorr.NroGranoSIL, grano => grano.CodigoGrano, (relCorr, grano) => new { RelacionCorriente = relCorr, Grano = grano });
                var joinGranoSTOP = joinGrano.Join(session.Query<CuposGranoSTOP>(), RelacionConGranoSIL => RelacionConGranoSIL.RelacionCorriente.NroGranoSTOP, GranoSTOP => GranoSTOP.NroGrano, (relacionConGranoSIL, granoSTOP) => new { RelacionadoConGranoSIL = relacionConGranoSIL, GranoSTOPRelacionado = granoSTOP });
                RelacionGranoSILGranoSTOPCompleta relacionCompleta = joinGranoSTOP.Select(x =>
                                                                                    new RelacionGranoSILGranoSTOPCompleta 
                                                                                    {
                                                                                        NroGranoSIL = x.RelacionadoConGranoSIL.Grano.CodigoGrano,
                                                                                        nombreGranoSil = x.RelacionadoConGranoSIL.Grano.Nombre,
                                                                                        NroGranoSTOP = x.GranoSTOPRelacionado.NroGrano,
                                                                                        nombreGranoSTOP = x.GranoSTOPRelacionado.NombreGrano,
                                                                                        ValorPorDefecto= x.RelacionadoConGranoSIL.RelacionCorriente.ValorPorDefecto,
                                                                                        Id = x.RelacionadoConGranoSIL.RelacionCorriente.Id
                                                                                    }).FirstOrDefault();
                return relacionCompleta;
            }
            return null;
        }

        public RelacionGranoSILGranoSTOPCompleta FindRelacionCompletaById(int id, ISession session)
        {
            if (id != 0)
            {
                var query = session.Query<RelacionGranoSILGranoSTOPCompleta>()
                    .Where(c => c.Id == id);
                var joinGrano = query.Join(session.Query<Grano>(), RelacionCorr => RelacionCorr.NroGranoSIL, grano => grano.CodigoGrano, (relCorr, grano) => new { RelacionCorriente = relCorr, Grano = grano });
                var joinGranoSTOP = joinGrano.Join(session.Query<CuposGranoSTOP>(), RelacionConGranoSIL => RelacionConGranoSIL.RelacionCorriente.NroGranoSTOP, GranoSTOP => GranoSTOP.NroGrano, (relacionConGranoSIL, granoSTOP) => new { RelacionadoConGranoSIL = relacionConGranoSIL, GranoSTOPRelacionado = granoSTOP });
                RelacionGranoSILGranoSTOPCompleta relacionCompleta = joinGranoSTOP.Select(x =>
                                                                                    new RelacionGranoSILGranoSTOPCompleta
                                                                                    {
                                                                                        NroGranoSIL = x.RelacionadoConGranoSIL.Grano.CodigoGrano,
                                                                                        nombreGranoSil = x.RelacionadoConGranoSIL.Grano.Nombre,
                                                                                        NroGranoSTOP = x.GranoSTOPRelacionado.NroGrano,
                                                                                        nombreGranoSTOP = x.GranoSTOPRelacionado.NombreGrano,
                                                                                        ValorPorDefecto = x.RelacionadoConGranoSIL.RelacionCorriente.ValorPorDefecto,
                                                                                        Id = x.RelacionadoConGranoSIL.RelacionCorriente.Id
                                                                                    }).FirstOrDefault();
                return relacionCompleta;
            }
            return null;
        }
    }
}