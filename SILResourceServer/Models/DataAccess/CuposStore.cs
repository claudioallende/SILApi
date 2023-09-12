using ResourceServer.Models.Error;
using ResourceServer.Models.Error.Exceptions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace ResourceServer.Models.DataAccess
{
    public class CuposStore : ICuposStore
    {
        private string mapping = "Cupos.mpg.xml";
        //private ISession session;
        //private ITransaction tx;

        public CuposStore()
        {
            //session = HibernateUtil.OpenSession(mapping);
            //tx = session.BeginTransaction();
        }
        [Obsolete]
        public void Save(Cupos c)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public Int64 Saverow(Cupos c)
        {
            throw new NotImplementedException();
        }

        public void Save(Cupos c, ISession session, ITransaction transaccion)
        {
            try
            {
                if (c != null)
                {
                    session.Save(c);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Update(Int64 Id, Cupos c)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    session.Update(c, Id);
                    tx.Commit();
                    HibernateUtil.Dispose();
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
        }

        public void Update(Int64 Id, Cupos c, ISession session, ITransaction transaccion)
        {
            session.Update(c, Id);
            //tx.Commit();
            //HibernateUtil.Dispose();
        }

        public void Delete(Int64 Id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Cupos cupo, ISession session, ITransaction transaccion)
        {
            session.Delete(cupo);
        }

        public void Delete(ISession session, Cupos cupo)
        {
            if (cupo.Pdf == 0)
            {
                session.Delete(cupo);
            }
        }

        public Cupos FindById(Int64 Id)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                Cupos cupo = session.Query<Cupos>()
                                .Where(x => x.Id == Id)
                                .FirstOrDefault();
                HibernateUtil.Dispose();
                return cupo;
            }
        }

        public IList<Cupos> FindAll()
        {
            throw new NotImplementedException();
        }

        public IList<Cupos> FindByGranoAndPuertoAndCompAndVendGroupByGranoAndPuertoAndCompAndVend(int grano, long puerto, Int64 comp, Int64 vend)
        {
            throw new NotImplementedException();
        }

        public void Update(Cupos c)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    session.Update(c);
                    tx.Commit();
                    HibernateUtil.Dispose();
                }
                catch (Exception e)
                {

                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
        }

        /*llamar a este metodo una vez por cupo*/
        public Int64 AgregarCupo(List<List<Cupos>> cuposDeLaSemana, Cupos encabezado, Cupos encabezadoAModificar)
        {
            Int64 idEncabezado = encabezado.Id;
            Cupos CupoCreado = null; //Guardo el cupo que se está por crear para luego poder informar, en caso de error, cuál fue.
            int NroCuposDeLaSemana = 0;
            foreach (List<Cupos> dia in cuposDeLaSemana)
            {
                NroCuposDeLaSemana += dia.Count();
            }
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    foreach (List<Cupos> dia in cuposDeLaSemana)
                    {
                        foreach (Cupos c in dia)
                        {
                            CupoCreado = c;
                            if (encabezadoAModificar != null)
                            {
                                session.Save(c);
                            }
                            else
                            {
                                if (idEncabezado == 0)
                                {
                                    /*el cuerpo viene sin idOrigen*/
                                    encabezado.Cuposrecibidos = NroCuposDeLaSemana;
                                    idEncabezado = (Int64)session.Save(encabezado);
                                    encabezado.Id = idEncabezado;
                                    c.Idorigen = idEncabezado;
                                    session.Save(c);
                                }
                                else
                                {
                                    c.Idorigen = idEncabezado;
                                    session.Save(c);
                                }
                            }
                        }
                    }

                    if (encabezadoAModificar != null)
                    {
                        encabezadoAModificar.Cuposrecibidos += NroCuposDeLaSemana;
                        session.Update(encabezadoAModificar);
                    }
                    tx.Commit();
                    HibernateUtil.Dispose();
                    return idEncabezado;
                }
                catch (NHibernate.Exceptions.GenericADOException e)
                {
                    if (e.GetBaseException().Message.Contains("ORA-00001"))
                    {
                        tx.Rollback();
                        HibernateUtil.Dispose();
                        throw new CodigoAlfanumericoDuplicadoException(CupoCreado.Nrocupo);
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

        public void Anular(Cupos cupo)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                cupo.Status = 3;
                try
                {
                    session.Update(cupo);
                    tx.Commit();
                    HibernateUtil.Dispose();
                }
                catch (Exception e)
                {

                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
        }

        /*Anula pero el commit de la transaccion se hace en otro lado*/
        public void AnularCupoDistribuido(Cupos cupo, Cupos encabezado, ISession session)
        {
            session.Update(cupo);
            session.Update(encabezado);
        }

        public void Update(ISession session, Cupos cupo)
        {
            session.Update(cupo);
        }

        /*Numero de cuerpos por comp-vend-grano-fecha-puerto-CUITS-status, agrupados por cuits*/
        public IList<Counter<Cupos>> FindNumberOfConsignacionesForKey(Int64 compcta, Int64 vendcta, int grano, DateTime fecha, long puerto, Cupos DatosCupoBuscar)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var query = session.Query<Cupos>()
                               .Where(x =>
                                   ((compcta != 0) ? x.Compcta == compcta : true) &&
                                    //((vendcta != 0) ? x.Vendcta == vendcta : true) &&
                                    x.Vendcta == vendcta &&
                                    ((grano != 0) ? x.Grano == grano : true) &&
                                    x.Fecha.Date == fecha.Date &&
                                    ((puerto != 0) ? x.Puerto == puerto : true) &&
                                    x.Tipo == 1 &&
                                    x.Status != 3 &&
                                    x.Status != 4 &&
                                    x.Status != 5 &&
                                    (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centrodist) ||
                                    ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro))
                                   );
                IList<Counter<Cupos>> cupo = DatosCupoBuscar.GetConsignacion().FiltroConsignacionIgnoreIfIsNull(query)
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
                                .Select(z =>
                                    new Counter<Cupos>
                                    {
                                        Value = new Cupos
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
                                        },
                                        Count = z.Count()
                                    }
                                )
                                .ToList();
                HibernateUtil.Dispose();
                return cupo;
            }
        }

        /*Numero de cuerpos por comp-vend-grano-fecha-puerto-status, agrupados por cuits (Consignaciones)
         si el vendedor esta en cero pone true en el select*/
        public IList<Counter<Cupos>> FindConsignacionesForKey(Int64 compcta, Int64 vendcta, int grano, DateTime? fechaDesde, DateTime? fechaHasta, long puerto)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Counter<Cupos>> cupo = session.Query<Cupos>()
                    .Join(session.Query<Cupos>(), encab => encab.Id, cuerpo => cuerpo.Idorigen, (encab, cuerpo) => new { Encabezado = encab, Cuerpo = cuerpo })
                    .Where(x =>
                        (((compcta != 0) ? x.Encabezado.Compcta == compcta : true) || ((compcta != 0) ? x.Cuerpo.Compcta == compcta : true)) &&
                            //((vendcta != 0) ? x.Cuerpo.Vendcta == vendcta : true) &&
                        x.Cuerpo.Vendcta == vendcta &&
                        ((grano !=  0)? x.Cuerpo.Grano == grano : true) &&
                        x.Cuerpo.Fecha.Date >= DateTime.Now.Date &&
                        x.Cuerpo.Fecha.Date <= DateTime.Now.Date.AddDays(5) &&
                        ((puerto != 0) ? x.Cuerpo.Puerto == puerto : true) &&
                        x.Cuerpo.Tipo == 1 &&
                            //x.Cuerpo.Status != 4 &&
                        x.Cuerpo.Status != 3
                    ).Select(x =>
                        new Cupos
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
                    .Select(z =>
                        new Counter<Cupos>
                        {
                            Value = new Cupos
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
                            },
                            Count = z.Count()
                        }
                    )
                    .ToList();
                HibernateUtil.Dispose();
                return cupo;
            }
        }

        public IList<Counter<Cupos>> FindConsignacionesForKey(Int64 compcta, Int64 vendcta, int grano, DateTime? fechaDesde, DateTime? fechaHasta, long puerto, ISession Session)
        {
            IList<Counter<Cupos>> cupo = Session.Query<Cupos>()
                .Join(Session.Query<Cupos>(), encab => encab.Id, cuerpo => cuerpo.Idorigen, (encab, cuerpo) => new { Encabezado = encab, Cuerpo = cuerpo })
                .Where(x =>
                    (((compcta != 0) ? x.Encabezado.Compcta == compcta : true) || ((compcta != 0) ? x.Cuerpo.Compcta == compcta : true)) &&
                        //((vendcta != 0) ? x.Cuerpo.Vendcta == vendcta : true) &&
                    x.Cuerpo.Vendcta == vendcta &&
                    ((grano != 0) ? x.Cuerpo.Grano == grano : true) &&
                    x.Cuerpo.Fecha.Date >= DateTime.Now.Date &&
                    x.Cuerpo.Fecha.Date <= DateTime.Now.Date.AddDays(5) &&
                    ((puerto != 0) ? x.Cuerpo.Puerto == puerto : true) &&
                    x.Cuerpo.Tipo == 1 &&
                        //x.Cuerpo.Status != 4 &&
                    x.Cuerpo.Status != 3
                ).Select(x =>
                    new Cupos
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
                .Select(z =>
                    new Counter<Cupos>
                    {
                        Value = new Cupos
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
                        },
                        Count = z.Count()
                    }
                )
                .ToList();
            return cupo;
        }

        public IList<Counter<Cupos>> FindConsignacionesForKeyGroupByFechaCyO(Int64 compcta, Int64 vendcta, int grano, long puerto)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Counter<Cupos>> cupo = session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == compcta &&
                        x.Vendcta == vendcta &&
                        x.Grano == grano &&
                        x.Fecha.Date >= DateTime.Now.Date &&
                        x.Puerto == puerto &&
                        x.Tipo == 1 &&
                        x.Status != 4 &&
                        x.Status != 5 &&
                        x.Status != 3 &&
                        (x.Vendcyo != 0 && x.Vendcyo == x.Compcta) &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centrodist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro))
                    )
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
                            y.Fecha
                        }
                    )
                    .Select(z =>
                        new Counter<Cupos>
                        {
                            Value = new Cupos
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
                                Fecha = z.Key.Fecha
                            },
                            Count = z.Count()
                        }
                    )
                    .ToList();
                HibernateUtil.Dispose();
                return cupo;
            }
        }
        /*si el vendedor esta en cero, busca con vendedores cero*/
        /*Numero de cuerpos por comp-vend-grano-fecha(5 dias)-puerto-status de los 5 dias despues de hoy*/
        public IList<Counter<Cupos>> FindConsignacionesForKeyGroupByFecha(Int64 compcta, Int64 vendcta, int grano, long puerto)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Counter<Cupos>> cupo = session.Query<Cupos>()
                    .Join(session.Query<Cupos>(), encab => encab.Id, cuerpo => cuerpo.Idorigen, (encab, cuerpo) => new { Encabezado = encab, Cuerpo = cuerpo })
                    .Where(x =>
                        ((x.Encabezado.Compcta == compcta) || (x.Cuerpo.Compcta == compcta)) &&
                        x.Cuerpo.Vendcta == vendcta &&
                        x.Cuerpo.Grano == grano &&
                        x.Cuerpo.Fecha.Date >= DateTime.Now.Date &&
                        x.Cuerpo.Puerto == puerto &&
                        x.Cuerpo.Tipo == 1 &&
                        x.Cuerpo.Status != 4 &&
                        x.Cuerpo.Status != 5 &&
                        x.Cuerpo.Status != 3 &&
                        (x.Cuerpo.Vendcyo == 0 || x.Cuerpo.Vendcyo == x.Cuerpo.Vendcta) &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Cuerpo.Centrodist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Cuerpo.Centro))
                    )
                    .GroupBy(y =>
                        new
                        {
                            y.Cuerpo.Cuitsolicitante,
                            y.Cuerpo.Nomsolicitante,
                            y.Cuerpo.Cuitintermediario,
                            y.Cuerpo.Nomintermediario,
                            y.Cuerpo.Cuitrtecomercial,
                            y.Cuerpo.Nomrtecomercial,
                            y.Cuerpo.Cuitcorrcomp,
                            y.Cuerpo.Nomcorrcomp,
                            y.Cuerpo.Cuitmat,
                            y.Cuerpo.Nommat,
                            y.Cuerpo.Cuitcorrvta,
                            y.Cuerpo.Nomcorrvta,
                            y.Cuerpo.Cuitrteent,
                            y.Cuerpo.Nomrteent,
                            y.Cuerpo.Cuitdestinatario,
                            y.Cuerpo.Nomdestinatario,
                            y.Cuerpo.CuitRteComercialProductor,
                            y.Cuerpo.NomRteComercialProductor,
                            y.Cuerpo.CuitRteComercialVentaPrimaria,
                            y.Cuerpo.NomRteComercialVentaPrimaria,
                            y.Cuerpo.Fecha
                        }
                    )
                    .Select(z =>
                        new Counter<Cupos>
                        {
                            Value = new Cupos
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
                                Fecha = z.Key.Fecha
                            },
                            Count = z.Count()
                        }
                    )
                    .ToList();
                HibernateUtil.Dispose();
                return cupo;
            }
        }

        /*Lista de cuerpos por comp-vend-grano-fecha-puerto-CUITS-status, para una consignacion dada*/
        public IList<Cupos> FindForKey(Int64 compcta, Int64 vendcta, int grano, DateTime fecha, long puerto, Cupos DatosCupoBuscar)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var query = session.Query<Cupos>()
                    .Where(x =>
                        ((compcta != 0) ? (Int64)x.Compcta == compcta : true) &&
                        ((vendcta != 0) ? (Int64)x.Vendcta == vendcta : true) &&
                        ((grano != 0) ? x.Grano == grano : true) &&
                        ((fecha != null) ? x.Fecha.Date == fecha.Date : true) &&
                        ((puerto != 0) ? x.Puerto == puerto : true) &&
                        x.Tipo == 1 &&
                        x.Status != 3 &&
                        x.Status != 4 &&
                        x.Status != 5 &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centrodist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro))
                    );
                IList<Cupos> cuerpos = DatosCupoBuscar.GetConsignacion().FiltroConsignacionIgnoreIfIsNull(query).ToList();
                HibernateUtil.Dispose();
                return cuerpos;
            }
        }

        /*Lista de cuerpos por comp-vend-grano-fecha-puerto-CUITS-status, agrupados por los clave y consignacion*/
        public IList<Cupos> FindIdorigenForConsignacionDistinct(Cupos cupoPadreNuevo)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var query = session.Query<Cupos>()
                               .Where(x =>
                                   ((cupoPadreNuevo.Compcta != 0) ? x.Compcta == cupoPadreNuevo.Compcta : true) &&
                                   x.Vendcta == cupoPadreNuevo.Vendcta &&
                                   ((cupoPadreNuevo.Grano != 0) ? x.Grano == cupoPadreNuevo.Grano : true) &&
                                   x.Fecha.Date == cupoPadreNuevo.Fecha.Date &&
                                   ((cupoPadreNuevo.Puerto != 0) ? x.Puerto == cupoPadreNuevo.Puerto : true) &&
                                   x.Tipo == 1 &&
                                   x.Status != 3
                                );
                IList<Cupos> cupo = cupoPadreNuevo.GetConsignacion().FiltroConsignacion(query).GroupBy(y =>
                                    new
                                    {
                                        y.Compcta,
                                        y.Vendcta,
                                        y.Grano,
                                        y.Fecha,
                                        y.Puerto,
                                        y.Status,
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
                                        y.Idorigen
                                    }
                                )
                                .Select(z =>
                                    new Cupos
                                        {
                                            Compcta = z.Key.Compcta,
                                            Vendcta = z.Key.Vendcta,
                                            Grano = z.Key.Grano,
                                            Fecha = z.Key.Fecha,
                                            Puerto = z.Key.Puerto,
                                            Status = z.Key.Status,
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
                                            Idorigen = z.Key.Idorigen
                                        }
                                )
                                .ToList();
                HibernateUtil.Dispose();
                return cupo;
            }
        }

        /*Lista de cuerpos por comp-vend-grano-fecha-puerto-CUITS-status(param)-tipo(param)*/
        public IList<Cupos> FindForKeyStatusAndType(Int64 compcta, Int64 vendcta, int grano, DateTime fecha, long puerto, Cupos DatosCupoBuscar, int status, int tipo)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var query = session.Query<Cupos>()
                    .Where(x =>
                        (Int64)x.Compcta == compcta &&
                        ((vendcta != 0) ? (Int64)x.Vendcta == vendcta : true) &&
                        x.Grano == grano &&
                        ((fecha != null) ? x.Fecha.Date == fecha.Date : true) &&
                        x.Puerto == puerto &&
                        x.Tipo == tipo &&
                        x.Status == status
                    );
                IList<Cupos> cuerpos = DatosCupoBuscar.GetConsignacion().FiltroConsignacionIgnoreIfIsNull(query)
                    .OrderByDescending(a => a.Id)
                    .ThenByDescending(x => x.Pdf)
                    .ToList();
                HibernateUtil.Dispose();
                return cuerpos;
            }
        }

        /*Numero de cuerpos por comp(encabezado o cuerpo)-vend(cuerpo)-grano(cuerpo)-fecha(5 dias)-puerto(cuerpo)-status(cuerpo)*/
        public IList<Counter<Cupos>> FindConsignacionesForKeyAndConsignacionGroupByFecha(Cupos consignacion)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Counter<Cupos>> cupo = session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == consignacion.Compcta &&
                        ((consignacion.Vendcta != 0) ? x.Vendcta == consignacion.Vendcta : true) &&
                        x.Grano == consignacion.Grano &&
                        x.Cuitsolicitante == consignacion.Cuitsolicitante &&
                        x.Cuitintermediario == consignacion.Cuitintermediario &&
                        x.Cuitrtecomercial == consignacion.Cuitrtecomercial &&
                        x.Cuitcorrcomp == consignacion.Cuitcorrcomp &&
                        x.Cuitmat == consignacion.Cuitmat &&
                        x.Cuitcorrvta == consignacion.Cuitcorrvta &&
                        x.Cuitrteent == consignacion.Cuitrteent &&
                        x.Cuitdestinatario == consignacion.Cuitdestinatario &&
                        x.CuitRteComercialProductor == consignacion.CuitRteComercialProductor &&
                        x.CuitRteComercialVentaPrimaria == consignacion.CuitRteComercialVentaPrimaria &&
                        x.Fecha.Date <= DateTime.Now.AddDays(5).Date &&
                        x.Fecha.Date >= DateTime.Now.Date &&
                        x.Puerto == consignacion.Puerto &&
                        x.Tipo == 1 &&
                        x.Status != 4 &&
                        x.Status != 3 &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centrodist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro))
                    )
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
                            y.Fecha
                        }
                    )
                    .Select(z =>
                        new Counter<Cupos>
                        {
                            Value = new Cupos
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
                                Fecha = z.Key.Fecha
                            },
                            Count = z.Count()
                        }
                    )
                    .ToList();
                HibernateUtil.Dispose();
                return cupo;
            }
        }

        /*CYO: Cuerpo por vend-grano-fecha-puerto-status-idOrigen-NroCupo(alfa) que generó otro cuerpo cyo*/
        public Cupos FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(Cupos DatosDeBusqueda)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                Cupos cupo = session.Query<Cupos>()
                    .Where(x =>
                        ((DatosDeBusqueda.Vendcta != 0) ? x.Vendcta == DatosDeBusqueda.Vendcta : true) &&
                        ((DatosDeBusqueda.Grano != 0) ? x.Grano == DatosDeBusqueda.Grano : true) &&
                        ((DatosDeBusqueda.Compcta != 0) ? x.Compcta == DatosDeBusqueda.Compcta : true) &&
                        x.Fecha.Date == DatosDeBusqueda.Fecha.Date &&
                        ((DatosDeBusqueda.Puerto != 0) ? x.Puerto == DatosDeBusqueda.Puerto : true) &&
                        ((DatosDeBusqueda.Idorigen != 0) ? x.Idorigen == DatosDeBusqueda.Idorigen : true) &&
                        ((DatosDeBusqueda.Nrocupo != "") ? x.Nrocupo == DatosDeBusqueda.Nrocupo : true) &&
                        x.Tipo == DatosDeBusqueda.Tipo &&
                        x.Status == DatosDeBusqueda.Status
                    )
                    .FirstOrDefault();
                HibernateUtil.Dispose();
                return cupo;
            }
        }

        public IList<Cupos> FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(long CuentaVendedor, int CodigoGrano, 
            long CuentaComprador, DateTime Fecha, long CuentaPuerto, long IdOrigen, IList<string> Alfanumericos, int Estado, ISession Session)
        {
            throw new NotImplementedException();
        }

        public IList<Cupos> FindByPdf(Cupos datos, int informado)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Cupos> cupo = session.Query<Cupos>()
                    .Where(x =>
                        x.Pdf == informado &&
                        x.Compcta == datos.Compcta &&
                        x.Vendcta == datos.Vendcta &&
                        x.Puerto == datos.Puerto &&
                        x.Grano == datos.Grano
                    )
                    .ToList();
                HibernateUtil.Dispose();
                return cupo;
            }
        }

        public void AnularCYO(Cupos cupocyo, string motivo)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                if (cupocyo.Status != 5)
                {
                    Cupos buscarCupo = (Cupos)cupocyo.Clone();
                    buscarCupo.Compcta = 0;
                    buscarCupo.Vendcta = 0;
                    Cupos cupoRelacionado = FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(buscarCupo);
                    if (cupoRelacionado.Status != 5)
                    {
                        cupocyo.Status = 3;
                        try
                        {
                            session.Update(cupocyo);
                            if (cupocyo.Id != cupoRelacionado.Id)
                            {
                                cupoRelacionado.Status = 3;
                                cupoRelacionado.Motbaja = motivo;
                                session.Update(cupoRelacionado);
                            }
                            tx.Commit();
                            HibernateUtil.Dispose();
                        }
                        catch (Exception e)
                        {
                            tx.Rollback();
                            HibernateUtil.Dispose();
                            throw e;
                        }
                    }
                }
                HibernateUtil.Dispose();
            }
        }


        public IList<Cupos> FindByPdf(int informado)
        {
            throw new NotImplementedException();
        }


        public IList<ICupo> FindCuerposDistribuidosNoInformadosByKey(long compcta, long vendcta, long puertocta, int grano)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<ICupo> cupos = session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == compcta &&
                        x.Vendcta == vendcta &&
                        x.Puerto == puertocta &&
                        x.Grano == grano &&
                        x.Tipo == 1 &&
                        x.Status == 4 &&
                        x.Vendcyo == 0 &&
                        x.Pdf == 1 &&
                        x.Fecha.Date >= DateTime.Now.Date
                    )
                    .ToList<ICupo>();
                HibernateUtil.Dispose();
                return cupos;
            }
        }

        public IList<DetalleCupoComplete> FindCuposDistribuidosInformadosBetweenDates(DateTime desde, DateTime hasta, IList<long> CuentaVendedor = null)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var query = session.Query<ViewInterfazSil>()
                    .Where(x => x.cuposfecha.Date >= desde.Date && x.cuposfecha.Date <= hasta.Date);
                //if (CuentaVendedor != null) query.Where(x => CuentaVendedor.Contains(x.cuposvendcta));
                if (CuentaVendedor != null)
                    query = query.Where(x => CuentaVendedor.Contains(x.cuposvendcta));

                IList<ViewInterfazSil> lista = query.ToList();

                IList<DetalleCupoComplete> detalles = lista
                    .Select(x => new DetalleCupoComplete
                    {
                        Consignacion = x.GetConsignacion(),
                        Grano = x.GetGrano(),
                        Comprador = x.GetComprador(),
                        Vendedor = x.GetVendedor(),
                        Puerto = x.GetPuerto(),
                        AlfanumericosPorDia = new List<AlfanumericosDia>() { 
                            new AlfanumericosDia {
                                Dia = x.cuposfecha,
                                Alfanumericos = new List<InformacionAlfanumerico>() {
                                    new InformacionAlfanumerico {
                                        Id = x.cuposID,
                                        Alfanumerico = x.cuposnrocupo,
                                        Observacion = x.cuposobserva
                                    }
                                }
                            }
                        },
                        CodigoEstablecimientoProcedencia = x.codigoestabproc
                    })
                    .ToList();
                return detalles;
            }
        }

        public IList<DetalleCupoComplete> FindCuposForId(IList<long> ids)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var query = session.Query<ViewInterfazSil>()
                    .Where(x => ids.Contains(x.cuposID));

                IList<ViewInterfazSil> lista = query.ToList();

                IList<DetalleCupoComplete> detalles = lista
                    .Select(x => new DetalleCupoComplete
                    {
                        Consignacion = x.GetConsignacion(),
                        Grano = x.GetGrano(),
                        Comprador = x.GetComprador(),
                        Vendedor = x.GetVendedor(),
                        Puerto = x.GetPuerto(),
                        AlfanumericosPorDia = new List<AlfanumericosDia>() { 
                            new AlfanumericosDia {
                                Dia = x.cuposfecha,
                                Alfanumericos = new List<InformacionAlfanumerico>() {
                                    new InformacionAlfanumerico {
                                        Id = x.cuposID,
                                        Alfanumerico = x.cuposnrocupo,
                                        Observacion = x.cuposobserva
                                    }
                                }
                            }
                        },
                        CodigoEstablecimientoProcedencia = x.codigoestabproc
                    })
                    .ToList();
                return detalles;
            }
        }

        public IList<ICupo> FindCuerposDistribuidosCyONoInformadosByKey(long compcta, long vendcta, long puertocta, int grano)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<ICupo> cupos = session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == compcta &&
                        x.Vendcta == vendcta &&
                        x.Puerto == puertocta &&
                        x.Grano == grano &&
                        x.Tipo == 1 &&
                        x.Status == 4 &&
                        x.Vendcyo != x.Vendcta &&
                        x.Pdf == 1 &&
                        x.Fecha.Date >= DateTime.Now.Date
                    )
                    .ToList<ICupo>();
                HibernateUtil.Dispose();
                return cupos;
            }
        }

        public IList<Cupos> FindCuerposAnuladosNoInformadosByKey(long compcta, long vendcta, long puertocta, int grano)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Cupos> cupos = session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == compcta &&
                        x.Vendcta == vendcta &&
                        x.Puerto == puertocta &&
                        x.Grano == grano &&
                        x.Tipo == 1 &&
                        (x.Status == 0 || x.Status == 3) &&
                        x.Uvcupodist != 0 &&
                        x.Vendcyo == 0 &&
                        x.Pdf == 1 &&
                        x.Fecha.Date >= DateTime.Now.Date
                    )
                    .ToList();
                HibernateUtil.Dispose();
                return cupos;
            }
        }

        public IList<Cupos> FindCuerposAnuladosCyONoInformadosByKey(long compcta, long vendcta, long puertocta, int grano)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Cupos> cupos = session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == compcta &&
                        x.Vendcta == vendcta &&
                        x.Puerto == puertocta &&
                        x.Grano == grano &&
                        x.Tipo == 1 &&
                        (x.Status == 0 || x.Status == 3) &&
                        x.Vendcyo != x.Vendcta &&
                        x.Pdf == 1 &&
                        x.Fecha.Date >= DateTime.Now.Date
                    )
                    .ToList();
                HibernateUtil.Dispose();
                return cupos;
            }
        }


        public void UpdateCupoInformado(IList<Cupos> cupos)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    foreach (var cupo in cupos)
                    {
                        cupo.Pdf = 0;
                        session.Update(cupo);
                    }
                    tx.Commit();
                    HibernateUtil.Dispose();
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
        }

        public void UpdateCupoAnuladoInformado(IList<Cupos> cupos)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    Cupos Encabezado;
                    foreach (var cupo in cupos)
                    {
                        Encabezado = FindById(cupo.Idorigen);
                        cupo.Pdf = 0;
                        cupo.Vendcta = Encabezado.Vendcta;
                        session.Update(cupo);
                    }
                    tx.Commit();
                    HibernateUtil.Dispose();
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
        }

        public void UpdateCupoInformado(Cupos cupo)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    cupo.Pdf = 0;
                    session.Update(cupo);
                    tx.Commit();
                    HibernateUtil.Dispose();
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
        }

        public void UpdateCupoAnuladoInformado(Cupos cupo)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    Cupos Encabezado = FindById(cupo.Idorigen);
                    cupo.Pdf = 0;
                    cupo.Vendcta = Encabezado.Vendcta;
                    session.Update(cupo);
                    tx.Commit();
                    HibernateUtil.Dispose();
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
        }


        public void Delete(Cupos cupo, ISession session)
        {
            session.Delete(cupo);
        }


        public Cupos FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfa(Cupos DatosDeBusqueda)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                Cupos cupo = session.Query<Cupos>()
                    .Where(x =>
                        ((DatosDeBusqueda.Vendcta != 0) ? x.Vendcta == DatosDeBusqueda.Vendcta : true) &&
                        ((DatosDeBusqueda.Grano != 0) ? x.Grano == DatosDeBusqueda.Grano : true) &&
                        ((DatosDeBusqueda.Compcta != 0) ? x.Compcta == DatosDeBusqueda.Compcta : true) &&
                        x.Fecha.Date == DatosDeBusqueda.Fecha.Date &&
                        ((DatosDeBusqueda.Puerto != 0) ? x.Puerto == DatosDeBusqueda.Puerto : true) &&
                        ((DatosDeBusqueda.Idorigen != 0) ? x.Idorigen == DatosDeBusqueda.Idorigen : true) &&
                        ((DatosDeBusqueda.Nrocupo != "") ? x.Nrocupo == DatosDeBusqueda.Nrocupo : true) &&
                        x.Tipo == DatosDeBusqueda.Tipo
                    )
                    .FirstOrDefault();
                HibernateUtil.Dispose();
                return cupo;
            }
        }


        public IList<Counter<Cupos>> FindConsignacionesAndObservacionForKey(Int64 compcta, Int64 vendcta, int grano, DateTime? fechaDesde, DateTime? fechaHasta, long puerto)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Counter<Cupos>> cupo = session.Query<Cupos>()
                    .Join(session.Query<Cupos>(), encab => encab.Id, cuerpo => cuerpo.Idorigen, (encab, cuerpo) => new { Encabezado = encab, Cuerpo = cuerpo })
                    .Where(x =>
                        (x.Encabezado.Compcta == compcta || x.Cuerpo.Compcta == compcta) &&
                        x.Cuerpo.Vendcta == vendcta &&
                        x.Cuerpo.Grano == grano &&
                        x.Cuerpo.Fecha.Date >= DateTime.Now.Date &&
                        x.Cuerpo.Fecha.Date <= DateTime.Now.Date.AddDays(5) &&
                        x.Cuerpo.Puerto == puerto &&
                        x.Cuerpo.Tipo == 1 &&
                        x.Cuerpo.Status != 3
                    )
                    .GroupBy(y =>
                        new
                        {
                            y.Cuerpo.Cuitsolicitante,
                            y.Cuerpo.Nomsolicitante,
                            y.Cuerpo.Cuitintermediario,
                            y.Cuerpo.Nomintermediario,
                            y.Cuerpo.Cuitrtecomercial,
                            y.Cuerpo.Nomrtecomercial,
                            y.Cuerpo.Cuitcorrcomp,
                            y.Cuerpo.Nomcorrcomp,
                            y.Cuerpo.Cuitmat,
                            y.Cuerpo.Nommat,
                            y.Cuerpo.Cuitcorrvta,
                            y.Cuerpo.Nomcorrvta,
                            y.Cuerpo.Cuitrteent,
                            y.Cuerpo.Nomrteent,
                            y.Cuerpo.Cuitdestinatario,
                            y.Cuerpo.Nomdestinatario,
                            y.Cuerpo.CuitRteComercialProductor,
                            y.Cuerpo.NomRteComercialProductor,
                            y.Cuerpo.CuitRteComercialVentaPrimaria,
                            y.Cuerpo.NomRteComercialVentaPrimaria,
                            y.Cuerpo.Observa
                        }
                    )
                    .Select(z => new Counter<Cupos>
                        {
                            Value = new Cupos
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
                                Observa = z.Key.Observa
                            },
                            Count = z.Count()
                        }
                    )
                    .ToList();
                HibernateUtil.Dispose();
                return cupo;
            }
        }

        public Cupos FindCupoLastObservacionByConsignacion(long compcta, long vendcta, int grano, long puerto, Consignacion Consignacion)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                Cupos cupo = session.Query<Cupos>()
                    .Join(session.Query<Cupos>(), encab => encab.Id, cuerpo => cuerpo.Idorigen, (encab, cuerpo) => new { Encabezado = encab, Cuerpo = cuerpo })
                    .Where(x =>
                        (x.Encabezado.Compcta == compcta || x.Cuerpo.Compcta == compcta) &&
                        x.Cuerpo.Vendcta == vendcta &&
                        x.Cuerpo.Grano == grano &&
                        x.Cuerpo.Fecha.Date >= DateTime.Now.Date &&
                        x.Cuerpo.Fecha.Date <= DateTime.Now.Date.AddDays(5) &&
                        x.Cuerpo.Puerto == puerto &&
                        x.Cuerpo.Tipo == 1 &&
                        x.Cuerpo.Status != 3 &&
                        x.Cuerpo.Nomsolicitante == Consignacion.Nomsolicitante &&
                        x.Cuerpo.Cuitsolicitante == Consignacion.Cuitsolicitante &&
                        x.Cuerpo.Nomintermediario == Consignacion.Nomintermediario &&
                        x.Cuerpo.Cuitintermediario == Consignacion.Cuitintermediario &&
                        x.Cuerpo.Nomrtecomercial == Consignacion.Nomrtecomercial &&
                        x.Cuerpo.Cuitrtecomercial == Consignacion.Cuitrtecomercial &&
                        x.Cuerpo.Nomcorrcomp == Consignacion.Nomcorrcomp &&
                        x.Cuerpo.Cuitcorrcomp == Consignacion.Cuitcorrcomp &&
                        x.Cuerpo.Nommat == Consignacion.Nommat &&
                        x.Cuerpo.Cuitmat == Consignacion.Cuitmat &&
                        x.Cuerpo.Nomcorrvta == Consignacion.Nomcorrvta &&
                        x.Cuerpo.Cuitcorrvta == Consignacion.Cuitcorrvta &&
                        x.Cuerpo.Nomrteent == Consignacion.Nomrteent &&
                        x.Cuerpo.Cuitrteent == Consignacion.Cuitrteent &&
                        x.Cuerpo.Nomdestinatario == Consignacion.Nomdestinatario &&
                        x.Cuerpo.Cuitdestinatario == Consignacion.Cuitdestinatario &&
                        x.Cuerpo.NomRteComercialProductor == Consignacion.NomRteComercialProductor &&
                        x.Cuerpo.CuitRteComercialProductor == Consignacion.CuitRteComercialProductor &&
                        x.Cuerpo.NomRteComercialVentaPrimaria == Consignacion.NomRteComercialVentaPrimaria &&
                        x.Cuerpo.CuitRteComercialVentaPrimaria == Consignacion.CuitRteComercialVentaPrimaria
                    )
                    .GroupBy(y =>
                        new
                        {
                            y.Cuerpo.Fecha,
                            y.Cuerpo.Cuitsolicitante,
                            y.Cuerpo.Nomsolicitante,
                            y.Cuerpo.Cuitintermediario,
                            y.Cuerpo.Nomintermediario,
                            y.Cuerpo.Cuitrtecomercial,
                            y.Cuerpo.Nomrtecomercial,
                            y.Cuerpo.Cuitcorrcomp,
                            y.Cuerpo.Nomcorrcomp,
                            y.Cuerpo.Cuitmat,
                            y.Cuerpo.Nommat,
                            y.Cuerpo.Cuitcorrvta,
                            y.Cuerpo.Nomcorrvta,
                            y.Cuerpo.Cuitrteent,
                            y.Cuerpo.Nomrteent,
                            y.Cuerpo.Cuitdestinatario,
                            y.Cuerpo.Nomdestinatario,
                            y.Cuerpo.CuitRteComercialProductor,
                            y.Cuerpo.NomRteComercialProductor,
                            y.Cuerpo.CuitRteComercialVentaPrimaria,
                            y.Cuerpo.NomRteComercialVentaPrimaria,
                            y.Cuerpo.Observa
                        }
                    )
                    .Select(z => new Cupos
                    {
                        Fecha = z.Key.Fecha,
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
                        Observa = z.Key.Observa
                    })
                    .OrderByDescending(x => x.Fecha)
                    .FirstOrDefault();
                HibernateUtil.Dispose();
                return cupo;
            }
        }


        public long LastUvdist(long CuentaComprador, long CuentaVendedor, long CuentaPuerto, int CodigoGrano)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                long uvalue = session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == CuentaComprador &&
                        x.Vendcta == CuentaVendedor &&
                        x.Puerto == CuentaPuerto &&
                        x.Grano == CodigoGrano &&
                        x.Fecha >= DateTime.Now.Date
                    )
                    .OrderByDescending(x => x.Uvcupodist)
                    .Select(x => x.Uvcupodist)
                    .FirstOrDefault();
                HibernateUtil.Dispose();
                return uvalue;
            }
        }


        public string FindLastObservacionByConsignacion(long compcta, long vendcta, int grano, long puerto, Consignacion Consignacion)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                string Obsercacion = session.Query<Cupos>()
                    .Join(session.Query<Cupos>(), encab => encab.Id, cuerpo => cuerpo.Idorigen, (encab, cuerpo) => new { Encabezado = encab, Cuerpo = cuerpo })
                    .Where(x =>
                        (x.Encabezado.Compcta == compcta || x.Cuerpo.Compcta == compcta) &&
                        x.Cuerpo.Vendcta == vendcta &&
                        x.Cuerpo.Grano == grano &&
                        x.Cuerpo.Fecha.Date >= DateTime.Now.Date &&
                        x.Cuerpo.Fecha.Date <= DateTime.Now.Date.AddDays(5) &&
                        x.Cuerpo.Puerto == puerto &&
                        x.Cuerpo.Tipo == 1 &&
                        x.Cuerpo.Status != 3 &&
                        x.Cuerpo.Nomsolicitante.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomsolicitante) ? Consignacion.Nomsolicitante.Trim() : Consignacion.Nomsolicitante) &&
                        x.Cuerpo.Cuitsolicitante == Consignacion.Cuitsolicitante &&
                        x.Cuerpo.Nomintermediario.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomintermediario) ? Consignacion.Nomintermediario.Trim() : Consignacion.Nomintermediario) &&
                        x.Cuerpo.Cuitintermediario == Consignacion.Cuitintermediario &&
                        x.Cuerpo.Nomrtecomercial.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomrtecomercial) ? Consignacion.Nomrtecomercial.Trim() : Consignacion.Nomrtecomercial) &&
                        x.Cuerpo.Cuitrtecomercial == Consignacion.Cuitrtecomercial &&
                        x.Cuerpo.Nomcorrcomp.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomcorrcomp) ? Consignacion.Nomcorrcomp.Trim() : Consignacion.Nomcorrcomp) &&
                        x.Cuerpo.Cuitcorrcomp == Consignacion.Cuitcorrcomp &&
                        x.Cuerpo.Nommat.Trim() == (!string.IsNullOrEmpty(Consignacion.Nommat) ? Consignacion.Nommat.Trim() : Consignacion.Nommat) &&
                        x.Cuerpo.Cuitmat == Consignacion.Cuitmat &&
                        x.Cuerpo.Nomcorrvta.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomcorrvta) ? Consignacion.Nomcorrvta.Trim() : Consignacion.Nomcorrvta) &&
                        x.Cuerpo.Cuitcorrvta == Consignacion.Cuitcorrvta &&
                        x.Cuerpo.Nomrteent.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomrteent) ? Consignacion.Nomrteent.Trim() : Consignacion.Nomrteent) &&
                        x.Cuerpo.Cuitrteent == Consignacion.Cuitrteent &&
                        x.Cuerpo.Nomdestinatario.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomdestinatario) ? Consignacion.Nomdestinatario.Trim() : Consignacion.Nomdestinatario) &&
                        x.Cuerpo.Cuitdestinatario == Consignacion.Cuitdestinatario &&
                        x.Cuerpo.NomRteComercialProductor.Trim() == (!string.IsNullOrEmpty(Consignacion.CuitRteComercialProductor) ? Consignacion.CuitRteComercialProductor.Trim() : Consignacion.CuitRteComercialProductor) &&
                        x.Cuerpo.CuitRteComercialProductor == Consignacion.CuitRteComercialProductor &&
                        x.Cuerpo.NomRteComercialVentaPrimaria.Trim() == (!string.IsNullOrEmpty(Consignacion.NomRteComercialVentaPrimaria) ? Consignacion.NomRteComercialVentaPrimaria.Trim() : Consignacion.NomRteComercialVentaPrimaria) &&
                        x.Cuerpo.CuitRteComercialVentaPrimaria == Consignacion.CuitRteComercialVentaPrimaria &&
                        x.Cuerpo.Caratula == (!string.IsNullOrEmpty(Consignacion.Caratula) ? Consignacion.Caratula.Trim() : Consignacion.Caratula)
                    )
                    .GroupBy(y =>
                        new
                        {
                            y.Cuerpo.Fecha,
                            y.Cuerpo.Cuitsolicitante,
                            y.Cuerpo.Nomsolicitante,
                            y.Cuerpo.Cuitintermediario,
                            y.Cuerpo.Nomintermediario,
                            y.Cuerpo.Cuitrtecomercial,
                            y.Cuerpo.Nomrtecomercial,
                            y.Cuerpo.Cuitcorrcomp,
                            y.Cuerpo.Nomcorrcomp,
                            y.Cuerpo.Cuitmat,
                            y.Cuerpo.Nommat,
                            y.Cuerpo.Cuitcorrvta,
                            y.Cuerpo.Nomcorrvta,
                            y.Cuerpo.Cuitrteent,
                            y.Cuerpo.Nomrteent,
                            y.Cuerpo.Cuitdestinatario,
                            y.Cuerpo.Nomdestinatario,
                            y.Cuerpo.CuitRteComercialProductor,
                            y.Cuerpo.NomRteComercialProductor,
                            y.Cuerpo.CuitRteComercialVentaPrimaria,
                            y.Cuerpo.NomRteComercialVentaPrimaria,
                            y.Cuerpo.Caratula,
                            y.Cuerpo.Observa
                        }
                    )
                    .Select(z => new Cupos
                    {
                        Fecha = z.Key.Fecha,
                        Observa = z.Key.Observa
                    })
                    .OrderByDescending(x => x.Fecha)
                    .Select(x => x.Observa)
                    .FirstOrDefault();
                HibernateUtil.Dispose();
                return Obsercacion;
            }
        }

    public string FindLastContactoComercialByConsignacion(long compcta, long vendcta, int grano, long puerto, Consignacion Consignacion)
    {
      using (ISession session = HibernateUtil.OpenSession(mapping))
      {
        string contactoComercial = session.Query<Cupos>()
            .Join(session.Query<Cupos>(), encab => encab.Id, cuerpo => cuerpo.Idorigen, (encab, cuerpo) => new { Encabezado = encab, Cuerpo = cuerpo })
            .Where(x =>
                (x.Encabezado.Compcta == compcta || x.Cuerpo.Compcta == compcta) &&
                x.Cuerpo.Vendcta == vendcta &&
                x.Cuerpo.Grano == grano &&
                x.Cuerpo.Fecha.Date >= DateTime.Now.Date &&
                x.Cuerpo.Fecha.Date <= DateTime.Now.Date.AddDays(5) &&
                x.Cuerpo.Puerto == puerto &&
                x.Cuerpo.Tipo == 1 &&
                x.Cuerpo.Status != 3 &&
                x.Cuerpo.Nomsolicitante.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomsolicitante) ? Consignacion.Nomsolicitante.Trim() : Consignacion.Nomsolicitante) &&
                x.Cuerpo.Cuitsolicitante == Consignacion.Cuitsolicitante &&
                x.Cuerpo.Nomintermediario.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomintermediario) ? Consignacion.Nomintermediario.Trim() : Consignacion.Nomintermediario) &&
                x.Cuerpo.Cuitintermediario == Consignacion.Cuitintermediario &&
                x.Cuerpo.Nomrtecomercial.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomrtecomercial) ? Consignacion.Nomrtecomercial.Trim() : Consignacion.Nomrtecomercial) &&
                x.Cuerpo.Cuitrtecomercial == Consignacion.Cuitrtecomercial &&
                x.Cuerpo.Nomcorrcomp.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomcorrcomp) ? Consignacion.Nomcorrcomp.Trim() : Consignacion.Nomcorrcomp) &&
                x.Cuerpo.Cuitcorrcomp == Consignacion.Cuitcorrcomp &&
                x.Cuerpo.Nommat.Trim() == (!string.IsNullOrEmpty(Consignacion.Nommat) ? Consignacion.Nommat.Trim() : Consignacion.Nommat) &&
                x.Cuerpo.Cuitmat == Consignacion.Cuitmat &&
                x.Cuerpo.Nomcorrvta.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomcorrvta) ? Consignacion.Nomcorrvta.Trim() : Consignacion.Nomcorrvta) &&
                x.Cuerpo.Cuitcorrvta == Consignacion.Cuitcorrvta &&
                x.Cuerpo.Nomrteent.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomrteent) ? Consignacion.Nomrteent.Trim() : Consignacion.Nomrteent) &&
                x.Cuerpo.Cuitrteent == Consignacion.Cuitrteent &&
                x.Cuerpo.Nomdestinatario.Trim() == (!string.IsNullOrEmpty(Consignacion.Nomdestinatario) ? Consignacion.Nomdestinatario.Trim() : Consignacion.Nomdestinatario) &&
                x.Cuerpo.Cuitdestinatario == Consignacion.Cuitdestinatario &&
                x.Cuerpo.NomRteComercialProductor.Trim() == (!string.IsNullOrEmpty(Consignacion.CuitRteComercialProductor) ? Consignacion.CuitRteComercialProductor.Trim() : Consignacion.CuitRteComercialProductor) &&
                x.Cuerpo.CuitRteComercialProductor == Consignacion.CuitRteComercialProductor &&
                x.Cuerpo.NomRteComercialVentaPrimaria.Trim() == (!string.IsNullOrEmpty(Consignacion.NomRteComercialVentaPrimaria) ? Consignacion.NomRteComercialVentaPrimaria.Trim() : Consignacion.NomRteComercialVentaPrimaria) &&
                x.Cuerpo.CuitRteComercialVentaPrimaria == Consignacion.CuitRteComercialVentaPrimaria &&
                x.Cuerpo.Caratula == (!string.IsNullOrEmpty(Consignacion.Caratula) ? Consignacion.Caratula.Trim() : Consignacion.Caratula)
            )
            .GroupBy(y =>
                new
                {
                  y.Cuerpo.Fecha,
                  y.Cuerpo.Cuitsolicitante,
                  y.Cuerpo.Nomsolicitante,
                  y.Cuerpo.Cuitintermediario,
                  y.Cuerpo.Nomintermediario,
                  y.Cuerpo.Cuitrtecomercial,
                  y.Cuerpo.Nomrtecomercial,
                  y.Cuerpo.Cuitcorrcomp,
                  y.Cuerpo.Nomcorrcomp,
                  y.Cuerpo.Cuitmat,
                  y.Cuerpo.Nommat,
                  y.Cuerpo.Cuitcorrvta,
                  y.Cuerpo.Nomcorrvta,
                  y.Cuerpo.Cuitrteent,
                  y.Cuerpo.Nomrteent,
                  y.Cuerpo.Cuitdestinatario,
                  y.Cuerpo.Nomdestinatario,
                  y.Cuerpo.CuitRteComercialProductor,
                  y.Cuerpo.NomRteComercialProductor,
                  y.Cuerpo.CuitRteComercialVentaPrimaria,
                  y.Cuerpo.NomRteComercialVentaPrimaria,
                  y.Cuerpo.Caratula,
                  y.Cuerpo.ContactoComercial
                }
            )
            .Select(z => new Cupos
            {
              Fecha = z.Key.Fecha,
              ContactoComercial = z.Key.ContactoComercial
            })
            .OrderByDescending(x => x.Fecha)
            .Select(x => x.ContactoComercial)
            .FirstOrDefault();
        HibernateUtil.Dispose();
        return contactoComercial;
      }
    }

    public IList<Cupos> FindByGranoAndCompradorAndVendedorAndPuertoAndVendcyoAndConsignacion(int Grano, long Comprador, long Vendedor, long Puerto, long Vendcyo, Consignacion Consignacion)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var query = session.Query<Cupos>()
                               .Where(x =>
                                    x.Compcta == Comprador &&
                                    x.Vendcta == Vendedor &&
                                    x.Grano == Grano &&
                                    x.Fecha.Date >= DateTime.Now.Date &&
                                    x.Puerto == Puerto &&
                                    x.Tipo == 1);
                IList<Cupos> cuerpos = Consignacion.FiltroConsignacion(query)
                                .ToList();
                HibernateUtil.Dispose();
                return cuerpos;
            }
        }


        public IList<Cupos> FindEncabezado(long CuentaComprador, long CuentaVendedor, long CuentaPuerto, int CodigoGrano, DateTime Fecha, Consignacion Consignacion, long Vendcyo, ISession Session)
        {
                var query = Session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == CuentaComprador &&
                        x.Vendcta == CuentaVendedor &&
                        x.Grano == CodigoGrano &&
                        x.Fecha.Date == Fecha.Date &&
                        x.Puerto == CuentaPuerto &&
                        x.Tipo == 0 &&
                        x.Cuitsolicitante == Consignacion.Cuitsolicitante &&
                        x.Cuitintermediario == Consignacion.Cuitintermediario &&
                        x.Cuitrtecomercial == Consignacion.Cuitrtecomercial &&
                        x.Cuitcorrcomp == Consignacion.Cuitcorrcomp &&
                        x.Cuitmat == Consignacion.Cuitmat &&
                        x.Cuitcorrvta == Consignacion.Cuitcorrvta &&
                        x.Cuitrteent == Consignacion.Cuitrteent &&
                        x.Cuitdestinatario == Consignacion.Cuitdestinatario &&
                        x.CuitRteComercialProductor == Consignacion.CuitRteComercialProductor &&
                        x.CuitRteComercialVentaPrimaria == Consignacion.CuitRteComercialVentaPrimaria &&
                        x.Status == 0 &&
                        x.Centro == ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroPorDefectoUsuarioLogueado() &&
                        x.Centrodist == ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroPorDefectoUsuarioLogueado()
                    );
                if (Vendcyo != 0) query = query.Where(x => x.Vendcyo == Vendcyo);
                IList<Cupos> cuerpos = query
                    .OrderByDescending(a => a.Id)
                    .ThenByDescending(x => x.Pdf)
                    .ToList();
                return cuerpos;
        }

        public void Save(Cupos c, ISession session)
        {
            session.Save(c);
        }

        public long SaveAndReturn(Cupos c, ISession session)
        {
            return (Int64)session.Save(c);
        }

        public void Anular(Cupos cupo, ISession session)
        {
            cupo.Status = 3;
            session.UpdateAsync(cupo);
        }

        public void AnularCYO(Cupos cupocyo, string motivo, ISession session)
        {

            if (cupocyo.Status != 5)
            {
                Cupos buscarCupo = (Cupos)cupocyo.Clone();
                buscarCupo.Compcta = 0;
                buscarCupo.Vendcta = 0;
                Cupos cupoRelacionado = FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(buscarCupo);
                if (cupoRelacionado.Status != 5)
                {
                    cupocyo.Status = 3;
                    session.Update(cupocyo);
                    if (cupocyo.Id != cupoRelacionado.Id)
                    {
                        cupoRelacionado.Status = 3;
                        cupoRelacionado.Motbaja = motivo;
                        session.Update(cupoRelacionado);
                    }
                }
            }
        }

        public void Update(Cupos cupos, ISession session)
        {
            session.Update(cupos);
        }


        public IList<Cupos> FindEncabezadosByIds(IList<long> Ids)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Cupos> Cupos = session.Query<Cupos>()
                    .Where(x => Ids.Contains(x.Id))
                    .ToList();
                HibernateUtil.Dispose();
                return Cupos;
            }
        }


        public IList<Cupos> FindByIds(IList<long> Ids, ISession Session)
        {
            IList<Cupos> cuerpos = Session.Query<Cupos>()
                .Where(x => Ids.Contains(x.Id))
                .ToList();
            return cuerpos;
        }


        public IList<Cupos> FindForKey(long CuentaComprador, long CuentaVendedor, long CuentaPuerto, int CodigoGrano, DateTime Fecha, Consignacion Consignacion, ISession Session)
        {
            var query = Session.Query<Cupos>()
                .Where(x =>
                    x.Compcta == CuentaComprador &&
                    x.Vendcta == CuentaVendedor &&
                    x.Grano == CodigoGrano &&
                    x.Fecha.Date == Fecha.Date &&
                    x.Puerto == CuentaPuerto &&
                    x.Tipo == 1 &&
                    x.Status != 3 &&
                    x.Status != 4 &&
                    x.Status != 5
                );
            IList<Cupos> cuerpos = Consignacion.FiltroConsignacion(query).ToList();
            return cuerpos;
        }


        public IList<Cupos> FindForKeyStatus(long CuentaComprador, long CuentaVendedor, long CuentaPuerto, int CodigoGrano, DateTime Fecha, Consignacion Consignacion, int Status, ISession Session)
        {
            var query = Session.Query<Cupos>()
                .Where(x =>
                    x.Compcta == CuentaComprador &&
                    x.Vendcta == CuentaVendedor &&
                    x.Grano == CodigoGrano &&
                    x.Fecha.Date == Fecha.Date &&
                    x.Puerto == CuentaPuerto &&
                    x.Tipo == 1 &&
                    x.Status == Status &&
                    (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centrodist) ||
                    ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro))
                );
            IList<Cupos> cuerpos = Consignacion.FiltroConsignacion(query)
                .OrderByDescending(a => a.Id)
                .ThenByDescending(x => x.Pdf)
                .ToList();
            return cuerpos;
        }


        public Cupos FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(Cupos DatosDeBusqueda, ISession Session)
        {
            Cupos Cupo = null;
            var select = Session.Query<Cupos>().Where(x => 
                x.Grano == DatosDeBusqueda.Grano &&
                x.Puerto == DatosDeBusqueda.Puerto &&
                x.Fecha.Date == DatosDeBusqueda.Fecha.Date &&
                x.Idorigen == DatosDeBusqueda.Idorigen &&
                x.Nrocupo == DatosDeBusqueda.Nrocupo &&
                x.Tipo == DatosDeBusqueda.Tipo &&
                (x.Status == 4 || x.Status == 5)
            );
            if (DatosDeBusqueda.Compcta != 0) {
                select = select.Where(x => x.Compcta == DatosDeBusqueda.Compcta);
            }
            if (DatosDeBusqueda.Vendcta != 0) {
                select = select.Where(x => x.Vendcta == DatosDeBusqueda.Vendcta);
            }
            Cupo = select.FirstOrDefault();
            return Cupo;
        }


        public Cupos FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfa(Cupos DatosDeBusqueda, ISession Session)
        {
            Cupos cupo = Session.Query<Cupos>()
                    .Where(x =>
                        ((DatosDeBusqueda.Vendcta != 0) ? x.Vendcta == DatosDeBusqueda.Vendcta : true) &&
                        ((DatosDeBusqueda.Grano != 0) ? x.Grano == DatosDeBusqueda.Grano : true) &&
                        ((DatosDeBusqueda.Compcta != 0) ? x.Compcta == DatosDeBusqueda.Compcta : true) &&
                        x.Fecha.Date == DatosDeBusqueda.Fecha.Date &&
                        ((DatosDeBusqueda.Puerto != 0) ? x.Puerto == DatosDeBusqueda.Puerto : true) &&
                        ((DatosDeBusqueda.Idorigen != 0) ? x.Idorigen == DatosDeBusqueda.Idorigen : true) &&
                        ((DatosDeBusqueda.Nrocupo != "") ? x.Nrocupo == DatosDeBusqueda.Nrocupo : true) &&
                        x.Tipo == DatosDeBusqueda.Tipo
                    )
                    .FirstOrDefault();
            return cupo;
        }


        public IList<Cupos> FindByNroCupos(IList<string> CodigosAlfanumericos)
        {
            IList<Cupos> CuposCodigosAlfanumericos = new List<Cupos>();
            using (ISession Session = HibernateUtil.OpenSession(mapping))
            {
                CuposCodigosAlfanumericos = Session.Query<Cupos>()
                    .Where(x => CodigosAlfanumericos.Contains(x.Nrocupo))
                    .ToList();
                HibernateUtil.Dispose();
                return CuposCodigosAlfanumericos;
            }
        }

        public IList<Counter<Cupos>> FindNumberOfConsignacionesForKey(Int64 compcta, Int64 vendcta, int grano, DateTime fecha, long puerto, Cupos DatosCupoBuscar, ISession Session)
        {
            var query = Session.Query<Cupos>()
                .Where(x =>
                    x.Compcta == compcta &&
                    x.Vendcta == vendcta &&
                    x.Grano == grano &&
                    x.Fecha.Date == fecha.Date &&
                    x.Puerto == puerto &&
                    x.Tipo == 1 &&
                    x.Status != 3 &&
                    x.Status != 4 &&
                    x.Status != 5);
            query = DatosCupoBuscar.GetConsignacion().FiltroConsignacion(query);
            IList<Counter<Cupos>> cupo = query
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
                .Select(z =>
                    new Counter<Cupos>
                    {
                        Value = new Cupos
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
                        },
                        Count = z.Count()
                    }
                )
                .ToList();
            return cupo;
        }


        public IList<Cupos> FindEncabezadosByIds(IList<long> Ids, ISession session)
        {
            IList<Cupos> Cupos = session.Query<Cupos>()
                .Where(x => Ids.Contains(x.Id))
                .ToList();
            return Cupos;
        }


        public IList<Cupos> FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(long CuentaComprador,
            long CuentaVendedor, int CodigoGrano, long CuentaPuerto, IList<DateTime> Fecha, IList<string> Alfanumericos, int Estado, ISession Session)
        {
            IList<Cupos> Cupos = Session.Query<Cupos>()
                .Where(x => 
                    (CuentaComprador == 0 ? true : x.Compcta == CuentaComprador) &&
                    (x.Vendcta == 0 ? true : x.Vendcta == CuentaVendedor) &&
                    x.Grano == CodigoGrano &&
                    x.Puerto == CuentaPuerto &&
                    Fecha.Contains(x.Fecha) &&
                    Alfanumericos.Contains(x.Nrocupo) &&
                    x.Status == Estado
                )
                .ToList();
            return Cupos;
        }


        public void Delete(IList<Cupos> Cupos, ISession Session)
        {
            foreach (Cupos Cupo in Cupos)
            {
                Session.Delete(Cupo);
            }
        }

        public IList<Cupos> FindCuerposDistribuidosNoInformadosByKey(long compcta, long vendcta, long puertocta, int grano, string centro, string centrodist)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Cupos> cupos = session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == compcta &&
                        x.Vendcta == vendcta &&
                        x.Puerto == puertocta &&
                        x.Grano == grano &&
                        x.Tipo == 1 &&
                        x.Status == 4 &&
                        x.Centro == centro &&
                        x.Centrodist == centrodist &&
                        x.Vendcyo == 0 &&
                        x.Pdf == 1 &&
                        x.Fecha.Date >= DateTime.Now.Date
                    )
                    .ToList();
                HibernateUtil.Dispose();
                return cupos;
            }
        }

        public IList<Cupos> FindCuerposDistribuidosCyONoInformadosByKey(long compcta, long vendcta, long puertocta, int grano, string centro, string centrodist)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<Cupos> cupos = session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == compcta &&
                        x.Vendcta == vendcta &&
                        x.Puerto == puertocta &&
                        x.Grano == grano &&
                        x.Tipo == 1 &&
                        x.Status == 4 &&
                        x.Centro == centro &&
                        x.Centrodist == centrodist &&
                        x.Vendcyo != x.Vendcta &&
                        x.Pdf == 1 &&
                        x.Fecha.Date >= DateTime.Now.Date
                    )
                    .ToList();
                HibernateUtil.Dispose();
                return cupos;
            }
        }

        public IList<ICupo> FindCuerposAnuladosNoInformadosByKey(long compcta, long vendcta, long puertocta, int grano, string centro, string centrodist)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<ICupo> cupos = session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == compcta &&
                        x.Vendcta == vendcta &&
                        x.Puerto == puertocta &&
                        x.Grano == grano &&
                        x.Tipo == 1 &&
                        (x.Status == 0 || x.Status == 3) &&
                        x.Centro == centro &&
                        x.Centrodist == centrodist &&
                        x.Uvcupodist != 0 &&
                        x.Vendcyo == 0 &&
                        x.Pdf == 1 &&
                        x.Fecha.Date >= DateTime.Now.Date
                    )
                    .ToList<ICupo>();
                HibernateUtil.Dispose();
                return cupos;
            }
        }

        public IList<ICupo> FindCuerposAnuladosCyONoInformadosByKey(long compcta, long vendcta, long puertocta, int grano, string centro, string centrodist)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<ICupo> cupos = session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == compcta &&
                        x.Vendcta == vendcta &&
                        x.Puerto == puertocta &&
                        x.Grano == grano &&
                        x.Tipo == 1 &&
                        (x.Status == 0 || x.Status == 3) &&
                        x.Centro == centro &&
                        x.Centrodist == centrodist &&
                        x.Vendcyo != x.Vendcta &&
                        x.Pdf == 1 &&
                        x.Fecha.Date >= DateTime.Now.Date
                    )
                    .ToList<ICupo>();
                HibernateUtil.Dispose();
                return cupos;
            }
        }

        public IList<Cupos> FindByGranoAndCompradorAndVendedorAndPuertoAndVendcyoAndConsignacionAndCentro(int Grano, long Comprador, long Vendedor, long Puerto, long Vendcyo, Consignacion Consignacion, FiltroCentro FiltroCentro)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var cupos = session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == Comprador &&
                        x.Vendcta == Vendedor &&
                        x.Grano == Grano &&
                        x.Fecha.Date >= DateTime.Now.Date &&
                        x.Puerto == Puerto &&
                        x.Tipo == 1 &&
                        x.Centro == FiltroCentro.CentroOrigen &&
                        x.Centrodist == FiltroCentro.CentroDistribucion &&
                        (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centrodist) ||
                        ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro)));
                cupos = Consignacion.FiltroConsignacion(cupos);
                IList<Cupos> cuerpos = cupos.ToList();
                HibernateUtil.Dispose();
                return cuerpos;
            }
        }

        public IList<Cupos> FindByCompAndPuertoAndGranoAndAlfaAndFechaAndVendcyo(long CuentaComprador, long CuentaPuerto, int CodigoGrano, string Alfanumerico, DateTime Fecha, long Vendcyo, ISession Session)
        {
            IList<Cupos> Cupos = Session.Query<Cupos>()
                .Where(x =>
                    x.Compcta == CuentaComprador &&
                    x.Puerto == CuentaPuerto &&
                    x.Grano == CodigoGrano &&
                    x.Nrocupo == Alfanumerico &&
                    x.Fecha.Date == Fecha.Date &&
                    x.Vendcyo == Vendcyo &&
                    x.Vendcyo == x.Compcta &&
                    x.Tipo == 1
                )
                .ToList();
            return Cupos;
        }


        public IList<Cupos> FindByCompAndPuertoAndGranoAndAlfaAndFechaAndVendcyo(long CuentaComprador, long CuentaPuerto, int CodigoGrano, IList<string> Alfanumericos, DateTime Fecha, long Vendcyo, ISession Session)
        {
            IList<Cupos> Cupos = Session.Query<Cupos>()
                .Where(x =>
                    x.Compcta == CuentaComprador &&
                    x.Puerto == CuentaPuerto &&
                    x.Grano == CodigoGrano &&
                    Alfanumericos.Contains(x.Nrocupo) &&
                    x.Fecha.Date == Fecha.Date &&
                    x.Vendcyo == Vendcyo &&
                    x.Vendcyo == x.Compcta &&
                    x.Tipo == 1
                )
                .ToList();
            return Cupos;
        }


        public IList<Cupos> FindByVendAndPuertoAndGranoAndVendcyoAndFechaAndAlfas(long CuentaVendedor, long CuentaPuerto, int CodigoGrano, IList<string> Alfanumericos, DateTime Fecha, long Vendcyo, ISession Session)
        {
            IList<Cupos> Cupos = Session.Query<Cupos>()
                .Where(x =>
                    x.Vendcta == CuentaVendedor &&
                    x.Puerto == CuentaPuerto &&
                    x.Grano == CodigoGrano &&
                    Alfanumericos.Contains(x.Nrocupo) &&
                    x.Fecha.Date == Fecha.Date &&
                    x.Vendcyo == Vendcyo &&
                    x.Vendcyo == x.Vendcta &&
                    x.Tipo == 1
                )
                .ToList();
            return Cupos;
        }


        public IList<Cupos> FindByCompradorVendedorGranoFechaConsignacion(long CuentaComprador, long CuentaVendedor, int CodigoGrano, DateTime Fecha, Consignacion Consignacion, ISession Session)
        {
            var query = Session.Query<Cupos>()
                .Where(x =>
                    x.Compcta == CuentaComprador &&
                    x.Vendcta == CuentaVendedor &&
                    x.Grano == CodigoGrano &&
                    x.Fecha.Date == Fecha.Date &&
                    x.Tipo == 1 &&
                    x.Status != 3 &&
                    x.Status != 4 &&
                    x.Status != 5
                );
            IList<Cupos> cuerpos = Consignacion.FiltroConsignacion(query).ToList();
            return cuerpos;
        }


        public IList<Cupos> FindByCompradorVendedorGranoFechaConsignacionStatus(long CuentaComprador, long CuentaVendedor, int CodigoGrano, DateTime Fecha, Consignacion Consignacion, IList<int> Status, ISession Session)
        {
            var query = Session.Query<Cupos>()
                .Where(x =>
                    x.Compcta == CuentaComprador &&
                    x.Vendcta == CuentaVendedor &&
                    x.Grano == CodigoGrano &&
                    x.Fecha.Date == Fecha.Date &&
                    x.Tipo == 1 &&
                    Status.Contains(x.Status) &&
                    (ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centrodist) ||
                    ResourceServer.Models.Identity.IdentityHelper.GetCodigoCentroUsuarioLogueado().Contains(x.Centro))
                );
            IList<Cupos> cuerpos = Consignacion.FiltroConsignacion(query)
                .OrderByDescending(a => a.Id)
                .ThenByDescending(x => x.Pdf)
                .ToList();
            return cuerpos;
        }


        public IList<Cupos> FindEncabezado(long CuentaComprador, long CuentaVendedor, long CuentaPuerto, int CodigoGrano, DateTime Fecha, Consignacion Consignacion)
        {
            throw new NotImplementedException();
        }

        public IList<Cupos> FindByPuertoAndGranoAndFechaAndAlfas(long CuentaPuerto, int CodigoGrano, IList<CodigosAlfanumericos> Codigos, ISession Session)
        {
            List<Cupos> Result = new List<Cupos>();
            foreach (CodigosAlfanumericos Codigo in Codigos)
            {
                var Cupos = Session.Query<Cupos>()
                    .Where(x =>
                        x.Puerto == CuentaPuerto &&
                        x.Grano == CodigoGrano &&
                        x.Tipo == 1 &&
                        (Codigo.obtenerCodigosAlfanumericos().Contains(x.Nrocupo) && x.Fecha.Date == Codigo.Dia.Date) &&
                        x.Status != 3
                    )
                    .ToList();
                Result.AddRange(Cupos);
            }
            return Result;
        }

        /// <summary>
        /// Consulta cupos que cumplan con los parámetros. 
        /// En caso de que los cupos sean cuenta y orden, se buscan los cupos hijos para poder mostrar los datos de consignación igual que en el PDF.
        /// </summary>
        /// <param name="CuentasVendedoras"></param>
        /// <param name="CuentaComprador"></param>
        /// <param name="CuentaPuerto"></param>
        /// <param name="CodigoGrano"></param>
        /// <param name="Consignacion"></param>
        /// <param name="InformadoStop"></param>
        /// <param name="EsCYO"></param>
        /// <returns></returns>
        public IList<DetalleCupo> FindDetalleCuposCliente(IList<long> CuentasVendedoras, long CuentaComprador, long CuentaPuerto, int CodigoGrano, Consignacion Consignacion, bool InformadoStop, bool EsCYO)
        {
            ISession Session = HibernateUtil.OpenSession();
            var query = Session.Query<Cupos>()
                .Where(x => x.Compcta == CuentaComprador &&
                    CuentasVendedoras.Contains(x.Vendcta) &&
                    x.Puerto == CuentaPuerto &&
                    x.Grano == CodigoGrano &&
                    x.Tipo == 1 &&
                    x.Fecha >= DateTime.Now.Date &&
                    x.Status != 0 && x.Status != 3
                );
            query = Consignacion.FiltroConsignacion(query);
            if (EsCYO)
            {
                query = query.Where(x => x.Vendcyo != 0 && x.Vendcyo == x.Vendcta);
                var join = query.Join(Session.Query<Cupos>(), padre => padre.Nrocupo, hijo => hijo.Nrocupo, (padre, hijo) => new { Padre = padre, Hijo = hijo });
                join = join.Where(x => x.Hijo.Compcta == x.Padre.Vendcta && x.Hijo.Grano == x.Padre.Grano && x.Hijo.Vendcyo == x.Padre.Vendcyo && x.Padre.Puerto == x.Hijo.Puerto);
                query = join.Select(x => x.Hijo).Where(x => x.Status == 4);
            }
            query = query.Where(x => x.Pdf == 0); //Cupo hijo distribuido e informado
            if (InformadoStop) query = query.Where(x => x.EstadoCupoCNRT >= 1);
            else query = query.Where(x => x.EstadoCupoCNRT < 1);

            var joingrano = query.Join(Session.Query<Grano>(), cupo => cupo.Grano, grano => grano.CodigoGrano, (cupo, grano) => new { Cupo = cupo, Grano = grano });
            var joingranopuerto = joingrano.Join(Session.Query<Puerto>(), cupograno => cupograno.Cupo.Puerto, puerto => puerto.Cuenta, (cupograno, puerto) => new { CupoGrano = cupograno, Puerto = puerto });

            var detalle = joingranopuerto.Select(x => new DetalleCupo
            { 
                Compcta = x.CupoGrano.Cupo.Compcta,
                Vendcta = x.CupoGrano.Cupo.Vendcta,
                Puerto = x.CupoGrano.Cupo.Puerto,
                Grano = x.CupoGrano.Cupo.Grano,
                Nomsolicitante = x.CupoGrano.Cupo.Nomsolicitante,
                Cuitsolicitante = x.CupoGrano.Cupo.Cuitsolicitante,
                Nomintermediario = x.CupoGrano.Cupo.Nomintermediario,
                Cuitintermediario = x.CupoGrano.Cupo.Cuitintermediario,
                Nomrtecomercial = x.CupoGrano.Cupo.Nomrtecomercial,
                Cuitrtecomercial = x.CupoGrano.Cupo.Cuitrtecomercial,
                Nomcorrcomp = x.CupoGrano.Cupo.Nomcorrcomp,
                Cuitcorrcomp = x.CupoGrano.Cupo.Cuitcorrcomp,
                Nommat = x.CupoGrano.Cupo.Nommat,
                Cuitmat = x.CupoGrano.Cupo.Cuitmat,
                Nomcorrvta = x.CupoGrano.Cupo.Nomcorrvta,
                Cuitcorrvta = x.CupoGrano.Cupo.Cuitcorrvta,
                Nomrteent = x.CupoGrano.Cupo.Nomrteent,
                Cuitrteent = x.CupoGrano.Cupo.Cuitrteent,
                Nomdestinatario = x.CupoGrano.Cupo.Nomdestinatario,
                Cuitdestinatario = x.CupoGrano.Cupo.Cuitdestinatario,
                NomRteComercialProductor = x.CupoGrano.Cupo.NomRteComercialProductor,
                CuitRteComercialProductor = x.CupoGrano.Cupo.CuitRteComercialProductor,
                NomRteComercialVentaPrimaria = x.CupoGrano.Cupo.NomRteComercialVentaPrimaria,
                CuitRteComercialVentaPrimaria = x.CupoGrano.Cupo.CuitRteComercialVentaPrimaria,
                Nrocupo = x.CupoGrano.Cupo.Nrocupo,
                Fecha = x.CupoGrano.Cupo.Fecha,
                Vendcyo = x.CupoGrano.Cupo.Vendcyo,
                EstadoCupoCNRT = x.CupoGrano.Cupo.EstadoCupoCNRT,
                Observa = x.CupoGrano.Cupo.Observa,
                NombreGrano = x.CupoGrano.Grano.Nombre,
                NombrePuerto = x.Puerto.Nombre,
                DireccionPlanta = x.Puerto.Domicilio,
                CuitPuerto = x.Puerto.Cuit,
                CPostal = x.Puerto.Cpostal,
                Planta = x.Puerto.Ruca
            });
            IList<DetalleCupo> Detalles = detalle.ToList();
            return Detalles;
        }
        
        public IList<long> FindVendedoresConCuposPendDeCTG()
        {
            using (ISession Session = HibernateUtil.OpenSession(mapping))
            {
                var vendctas = Session.Query<Cupos>()
                    .Where(x => x.EstadoCupoCNRT < 2
                        && x.Fecha >= DateTime.Now.Date
                        && x.Status == 4
                        && x.Tipo == 1
                        && x.Pdf == 0
                        )
                    .GroupBy(x => new
                    {
                        x.Vendcta
                    })
                    .Select(y => new
                    {
                        y.Key.Vendcta
                    })
                    .OrderByDescending( 
                        x => x.Vendcta
                    )
                    .ToList();
                List<long> cuentasVendedoras = vendctas.Select(x=> x.Vendcta).ToList<long>();
                return cuentasVendedoras;
            }
        }
        public IList<Cupos> FindCuposPendDeCTG(ISession Session, int dia, long vendCuenta = 0)
        {
            var cupos = Session.Query<Cupos>()
                .Where(x => x.EstadoCupoCNRT == 1
                    && x.Fecha == DateTime.Now.Date.AddDays(dia)
                    && x.Status == 4
                    && x.Vendcta != x.Vendcyo
                    && x.Tipo == 1
                    && x.Pdf == 0);
            cupos = (vendCuenta != 0) ? cupos.Where(x => x.Vendcta == vendCuenta) : cupos.Where(x => x.Vendcta != 0);
            cupos = cupos.OrderBy(x => x.Vendcta)
                .ThenBy(x => x.Fecha)
                .ThenBy(x => x.Grano);
            List<Cupos> Result = cupos.ToList();
            return Result;
        }

        public IList<DetalleCupo> FindDetalleCuposPendDeCTG(ISession Session, long vendCuenta)
        {
            var cupos = Session.Query<Cupos>()
                .Where(x => x.EstadoCupoCNRT < 2
                    && x.Fecha >= DateTime.Now.Date
                    && x.Status == 4
                    && x.Tipo == 1
                    && x.Pdf == 0);
            if (vendCuenta!=0) 
            {
                cupos = cupos.Where(x => x.Vendcta == vendCuenta);
            }
            var joingrano = cupos.Join(Session.Query<Grano>(), cupo => cupo.Grano, grano => grano.CodigoGrano, (cupo, grano) => new { Cupo = cupo, Grano = grano });
            var detalle = joingrano.Select(x => new DetalleCupo
            {
                Compcta = x.Cupo.Compcta,
                Vendcta = x.Cupo.Vendcta,
                Puerto = x.Cupo.Puerto,
                Grano = x.Cupo.Grano,
                Nomsolicitante = x.Cupo.Nomsolicitante,
                Cuitsolicitante = x.Cupo.Cuitsolicitante,
                Nomintermediario = x.Cupo.Nomintermediario,
                Cuitintermediario = x.Cupo.Cuitintermediario,
                Nomrtecomercial = x.Cupo.Nomrtecomercial,
                Cuitrtecomercial = x.Cupo.Cuitrtecomercial,
                Nomcorrcomp = x.Cupo.Nomcorrcomp,
                Cuitcorrcomp = x.Cupo.Cuitcorrcomp,
                Nommat = x.Cupo.Nommat,
                Cuitmat = x.Cupo.Cuitmat,
                Nomcorrvta = x.Cupo.Nomcorrvta,
                Cuitcorrvta = x.Cupo.Cuitcorrvta,
                Nomrteent = x.Cupo.Nomrteent,
                Cuitrteent = x.Cupo.Cuitrteent,
                Nomdestinatario = x.Cupo.Nomdestinatario,
                Cuitdestinatario = x.Cupo.Cuitdestinatario,
                NomRteComercialProductor = x.Cupo.NomRteComercialProductor,
                CuitRteComercialProductor = x.Cupo.CuitRteComercialProductor,
                NomRteComercialVentaPrimaria = x.Cupo.NomRteComercialVentaPrimaria,
                CuitRteComercialVentaPrimaria = x.Cupo.CuitRteComercialVentaPrimaria,
                Nrocupo = x.Cupo.Nrocupo,
                Fecha = x.Cupo.Fecha,
                Vendcyo = x.Cupo.Vendcyo,
                EstadoCupoCNRT = x.Cupo.EstadoCupoCNRT,
                Observa = x.Cupo.Observa,
                NombreGrano = x.Grano.Nombre
            });
            detalle = detalle.OrderByDescending(x => x.Vendcta);
            List<DetalleCupo> Result = detalle.ToList();
            return Result;
        }

        public IList<Cupos> FindByNroCupoAndCompctaAndVendcyoAndNotStatusAndFecha(IList<string> AlfanumericosPadre, DateTime Fecha, int Status, ISession Session)
        {
            return Session.Query<Cupos>()
                .Join(Session.Query<Cupos>(), CupoPadre => CupoPadre.Nrocupo, CupoHijo => CupoHijo.Nrocupo, (Padre, Hijo) => new { CupoPadre = Padre, CupoHijo = Hijo })
                .Where(x =>
                    AlfanumericosPadre.Contains(x.CupoPadre.Nrocupo)
                    && x.CupoPadre.Status != Status
                    && x.CupoPadre.Fecha == Fecha
                    && x.CupoHijo.Compcta == x.CupoPadre.Vendcta
                    && x.CupoPadre.Vendcta != 0
                    && x.CupoHijo.Vendcyo == x.CupoPadre.Vendcta
                )
                .Select(x => x.CupoHijo)
                .ToList();
        }


        public void Update(IList<Cupos> Cupos, ISession Session)
        {
            foreach (Cupos Cupo in Cupos)
            {
                Session.Update(Cupo);
            }
        }


        public IList<Cupos> FindByVendAndPuertoAndGranoAndVendcyoAndFechaAndAlfasAndUvalueNotZero(long CuentaVendedor, long CuentaPuerto, int CodigoGrano, IList<string> Alfanumericos, DateTime Fecha, long Vendcyo, ISession Session)
        {
            IList<Cupos> Cupos = Session.Query<Cupos>()
                .Where(x =>
                    x.Vendcta == CuentaVendedor &&
                    x.Puerto == CuentaPuerto &&
                    x.Grano == CodigoGrano &&
                    Alfanumericos.Contains(x.Nrocupo) &&
                    x.Fecha.Date == Fecha.Date &&
                    x.Vendcyo == Vendcyo &&
                    x.Vendcyo == x.Vendcta &&
                    x.Tipo == 1 &&
                    x.Uvcupodist != 0
                )
                .ToList();
            return Cupos;
        }

        public IList<Cupos> FindByCompAndVendAndGranoAndFechaAndPuertoAndIdOrigenAndAlfaAndStatus(long CuentaVendedor, int CodigoGrano, long CuentaComprador, DateTime Fecha, long CuentaPuerto, long IdOrigen, IList<string> Alfanumericos, IList<int> Estado, ISession Session)
        {
            IList<Cupos> Cupos = Session.Query<Cupos>()
                .Where(x =>
                    (CuentaComprador == 0 ? true : x.Compcta == CuentaComprador) &&
                    (x.Vendcta == 0 ? true : x.Vendcta == CuentaVendedor) &&
                    x.Grano == CodigoGrano &&
                    x.Puerto == CuentaPuerto &&
                    Fecha == x.Fecha &&
                    Alfanumericos.Contains(x.Nrocupo) &&
                    Estado.Contains(x.Status)
                )
                .ToList();
            return Cupos;
        }

        public IList<Counter<Cupos>> FindCuposByCompradorVendedorGranoPuertoConsignacion(long CuentaComprador, long CuentaVendedor, int Grano, long CuentaPuerto, Consignacion Consignacion, bool Cyo)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var filtro = session.Query<Cupos>()
                    .Where(x =>
                        x.Compcta == CuentaComprador &&
                        x.Vendcta == CuentaVendedor &&
                        x.Grano == Grano &&
                        x.Fecha.Date >= DateTime.Now.Date &&
                        x.Puerto == CuentaPuerto &&
                        x.Tipo == 1 &&
                        x.Status != 4 &&
                        x.Status != 5 &&
                        x.Status != 3 &&
                        (ClaimsUtil.GetListClaims("Centro").Contains(x.Centrodist)||
                        ClaimsUtil.GetListClaims("Centro").Contains(x.Centro))
                    );
                if (Cyo)
                    filtro = filtro.Where(x => x.Vendcyo != 0 && x.Vendcyo == x.Compcta);
                else
                    filtro = filtro.Where(x => x.Vendcyo == 0 || x.Vendcyo == x.Vendcta);
                filtro = Consignacion.FiltroConsignacion(filtro);
                IList<Counter<Cupos>> cupo = filtro
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
                            y.Fecha
                        }
                    )
                    .Select(z =>
                        new Counter<Cupos>
                        {
                            Value = new Cupos
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
                                Caratula= z.Key.Caratula,
                                ContactoComercial = z.Key.ContactoComercial,
                                Fecha = z.Key.Fecha
                            },
                            Count = z.Count()
                        }
                    )
                    .ToList();
                HibernateUtil.Dispose();
                return cupo;
            }
        }

        public IList<DetalleDeIncumplimientoCupo> DetalleDeIncumplimientoDeCupos(ISession Session, DateTime desde, DateTime hasta, long vendCuenta = 0)
        {
            string whereOtorgados = " x.EstadoCupoCNRT > 0";
            whereOtorgados += " and x.Fecha >= '" + desde.ToString("dd/MM/yyyy") + "'";
            whereOtorgados += " and x.Fecha < '" + hasta.ToString("dd/MM/yyyy") + "'";
            whereOtorgados += " and x.Status = 4";
            whereOtorgados += " and x.Vendcta != x.Vendcyo";
            whereOtorgados += " and x.Tipo = 1";
            whereOtorgados += " and x.Pdf = 0";
            whereOtorgados += (vendCuenta != 0) ? " and x.vendcta=" + vendCuenta : "";
            string whereCumplidos = " x.EstadoCupoCNRT > 1 ";
            whereCumplidos += " and " + whereOtorgados;

            string SQL = "select otorgados.vendcta, otorgados.grano, otorgados.fecha, otorgados.numero otorgados, NVL(cumplidos.numero, 0) cumplidos, (otorgados.numero - NVL(cumplidos.numero, 0)) perdidos,";
            SQL += " round(((otorgados.numero - NVL(cumplidos.numero, 0))*100)/otorgados.numero,2) porcentajeDePerdida";
            SQL += " from (select vendcta, grano, fecha, count(id) numero ";
            SQL += " from cuposcorre x";
            SQL += " where ";
            SQL += whereOtorgados;
            SQL += " group by vendcta, grano, fecha";
            SQL += " order by vendcta, grano, fecha) otorgados,";
            SQL += " (select vendcta, grano, fecha, count(id) numero";
            SQL += " from cuposcorre x ";
            SQL += " where ";
            SQL += whereCumplidos;
            SQL += " group by vendcta, grano, fecha ";
            SQL += " order by vendcta, grano, fecha) cumplidos";
            SQL += " where otorgados.fecha=cumplidos.fecha(+)";
            SQL += " and otorgados.vendcta = cumplidos.vendcta(+)";
            SQL += " and otorgados.grano = cumplidos.grano(+)";
            SQL += " order by vendcta, fecha desc";
            //List<ICupo> IncumplimientoDeCupos = new List<ICupo>();

            List<object> Incumplimiento = (List<object>)Session.CreateSQLQuery(SQL)
                .AddScalar("Vendcta", NHibernateUtil.Int64)
                .AddScalar("Grano", NHibernateUtil.Int32)
                .AddScalar("Fecha", NHibernateUtil.DateTime)
                .AddScalar("Otorgados", NHibernateUtil.Int32)
                .AddScalar("Cumplidos", NHibernateUtil.Int32)
                .AddScalar("Perdidos", NHibernateUtil.Int32)
                .AddScalar("PorcentajeDePerdida", NHibernateUtil.Decimal)
            .List();
            List<DetalleDeIncumplimientoCupo> IncumplimientoDeCuposaux = (List<DetalleDeIncumplimientoCupo>)Incumplimiento
                    .Select(array =>
                    new DetalleDeIncumplimientoCupo
                    {       /*faltan los controles por null*/
                        Vendcta =   (Int64)((object[])array)[0],
                        Grano =     (Int32)((object[])array)[1],
                        Fecha =     (DateTime)((object[])array)[2],
                        Otorgados = (int)((object[])array)[3],
                        Cumplidos = (int)((object[])array)[4],
                        Perdidos =  (int)((object[])array)[5],
                        PorcentajeDePerdida = (decimal)((object[])array)[6]
                    })
                    .ToList();
            return IncumplimientoDeCuposaux;
        }

        public IList<Cupos> FindByCompradorAndVendedorAndGranoAndPuertoAndConsignacionAndInformadoAndDistribuidoBetweenDates(long CuentaComprador, long CuentaVendedor, int CodigoGrano, long CuentaPuerto, Consignacion Consignacion, DateTime FechaInicial, DateTime FechaFin, ISession Session)
        {
            var Result = Session.Query<Cupos>()
                .Where(x => x.Vendcta == CuentaVendedor && x.Compcta == CuentaComprador && x.Grano == CodigoGrano && x.Puerto == CuentaPuerto && x.Pdf == 0 && x.Status == 4 && x.Vendcta != x.Vendcyo);

            Result = Consignacion.FiltroConsignacion(Result);

            Result = Result.Where(x => x.Fecha >= FechaInicial && x.Fecha <= FechaFin);

            return Result.ToList();
        }
    }
}