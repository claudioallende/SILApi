using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class CuposDistStore : ICuposDistStore
    {        
        private string mapping = "CuposDist.mpg.xml";
        
        public void Save(CuposDist c)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    if (c != null)
                    {
                        CuposDist distBD = FindByCompVendGranoDestCentroFechaConsignacion(c.Ctacomp, c.CtaVend, c.Grano, c.Destino, c.Centro, c.Fecha, c.GetConsignacion());
                        if(distBD != null){
                            distBD.Cupos = distBD.Cupos + c.Cupos;
                            session.Update(distBD);
                            tx.Commit();
                        }else{
                            session.Save(c);
                            tx.Commit();
                        }
                    }
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

        public Int64 Save(CuposDist c, ISession sesion, ITransaction transaccion)
        {
            Int64 uvalue;
            try
            {
                if (c != null)
                {
                    CuposDist distBD = FindByCompVendGranoDestCentroFechaConsignacion(c.Ctacomp, c.CtaVend, c.Grano, c.Destino, c.Centro, c.Fecha, c.GetConsignacion());
                    if (distBD != null)
                    {
                        distBD.Cupos = distBD.Cupos + c.Cupos;
                        sesion.Update(distBD);
                        return distBD.Uvalue;
                        //transaccion.Commit();
                    }
                    else
                    {
                        uvalue = (Int64)sesion.Save(c);
                        return uvalue;
                        //transaccion.Commit();
                    }
                }
                return 0;
                //HibernateUtil.Dispose();
            }
            catch (Exception e)
            {
                //transaccion.Rollback();
                throw e;
            }
        }

        //Ver si hace Save si devuelve el Uvalue generado
        public long Save(CuposDist c, ISession sesion)
        {
            try
            {
                if (c != null)
                {
                    sesion.SaveOrUpdate(c);
                    return c.Uvalue;
                }
                return 0;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void Update(Int64 Id, CuposDist c)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction()) 
            { 
                try 
                {
                    session.Update(c,c.Uvalue);
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

        public CuposDist FindByCompVendGranoDestCentroFecha(Int64 ctacomp, Int64 ctaVend, int grano, Int64 destino, string centro, DateTime fecha )
        {
            using (ISession session = HibernateUtil.OpenSession(mapping)) { 
                CuposDist cupodist = session.Query<CuposDist>()
                                  .Where(x =>
                                          ((ctacomp != 0) ? x.Ctacomp == ctacomp : true) &&
                                          ((ctaVend != 0) ? x.CtaVend == ctaVend : true) &&
                                          ((grano != 0) ? x.Grano == grano : true) &&
                                          ((destino != 0) ? x.Destino == destino : true) &&
                                          ((centro != "") ? x.Centro == centro : true) &&
                                          ((fecha != null) ? x.Fecha.Date == fecha.Date : true)
                                      )
                                   .FirstOrDefault();
                            //session.Dispose();
                HibernateUtil.Dispose();
                return cupodist;
            }
        }

        public CuposDist FindByCompVendGranoDestCentroFechaConsignacion(long ctacomp, long ctaVend, int grano, long destino, string centro, DateTime fecha, Consignacion Consignacion)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var query = session.Query<CuposDist>()
                                  .Where(x =>
                                        x.Ctacomp == ctacomp &&
                                        x.CtaVend == ctaVend &&
                                        x.Grano == grano &&
                                        x.Destino == destino &&
                                        x.Centro == centro &&
                                        x.Fecha.Date == fecha.Date);
                CuposDist cupodist = Consignacion.FiltroConsignacion(query).FirstOrDefault();
                HibernateUtil.Dispose();
                return cupodist;
            }
        }

        CuposDist ICuposDistStore.FindByUvalue(long uvalue)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                CuposDist cupodist = session.Query<CuposDist>()
                                  .Where(x => x.Uvalue == uvalue)
                                   .FirstOrDefault();
                //session.Dispose();
                HibernateUtil.Dispose();
                return cupodist;
            }
        }

        IList<CuposDist> ICuposDistStore.FindAll()
        {
            throw new NotImplementedException();
        }

        public void Delete(long Uvalue)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    session.Delete(Uvalue);
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

        //Borra pero el commit de la transaccion se hace en otro lado
        public void Delete(CuposDist cupodist, ISession session)
        {
            session.Delete(cupodist);
        }


        public IList<CuposDist> FindByUvalues(IList<long> Uvalues)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                IList<CuposDist> cupodist = session.Query<CuposDist>()
                    .Where(x => Uvalues.Contains(x.Uvalue))
                    .ToList();
                HibernateUtil.Dispose();
                return cupodist;
            }
        }


        public IList<CuposDist> FindByCompVendGranoDestCentroFechasConsignacion(long CuentaComprador, long CuentaVendedor, int CodigoGrano, long Destino, string Centro, IList<DateTime> Fechas, Consignacion Consignacion)
        {
            using (ISession session = HibernateUtil.OpenSession(mapping))
            {
                var query = session.Query<CuposDist>()
                        .Where(x =>
                                x.Ctacomp == CuentaComprador &&
                                x.CtaVend == CuentaVendedor &&
                                x.Grano == CodigoGrano &&
                                x.Destino == Destino &&
                                x.Centro == Centro);
                IList<CuposDist> cupodist = Consignacion.FiltroConsignacion(query).ToList();
                HibernateUtil.Dispose();
                return cupodist;
            }
        }


        void ICuposDistStore.Save(CuposDist c, ISession session)
        {
            session.Save(c);
        }

        public void Update(CuposDist Distribucion, ISession Session)
        {
            Session.Update(Distribucion);
        }


        public CuposDist FindByUvalue(long uvalue, ISession Session)
        {
            return Session.Query<CuposDist>()
                .Where(x => x.Uvalue == uvalue)
                .FirstOrDefault();
        }

        public void Update(IList<CuposDist> Distribuciones, ISession Session)
        {
            foreach (CuposDist Distribucion in Distribuciones)
            {
                Session.Update(Distribucion);
            }
        }

        public CuposDist FindByCompVendGranoDestCentroFechaConsignacion(long ctacomp, long ctaVend, int grano, long destino, string centro, DateTime fecha, Consignacion Consignacion, ISession Session)
        {
            var query = Session.Query<CuposDist>()
                .Where(x =>
                    x.Ctacomp == ctacomp &&
                    x.CtaVend == ctaVend &&
                    x.Grano == grano &&
                    x.Destino == destino &&
                    x.Centro == centro &&
                    x.Fecha.Date == fecha.Date);
            CuposDist cupodist = Consignacion.FiltroConsignacion(query).FirstOrDefault();
            return cupodist;
        }
        
        public IList<CuposDist> FindByUvalues(IList<long> Uvalues, ISession Session)
        {
            IList<CuposDist> cupodist = Session.Query<CuposDist>()
                    .Where(x => Uvalues.Contains(x.Uvalue))
                    .ToList();
            return cupodist;
        }


        public IList<CuposDist> FindByCompVendGranoDestCentroFechasConsignacion(long CuentaComprador, long CuentaVendedor, int CodigoGrano, long Destino, string Centro, IList<DateTime> Fechas, Consignacion Consignacion, ISession Session)
        {
            var query = Session.Query<CuposDist>()
                .Where(x =>
                        x.Ctacomp == CuentaComprador &&
                        x.CtaVend == CuentaVendedor &&
                        x.Grano == CodigoGrano &&
                        x.Destino == Destino &&
                        x.Centro == Centro &&
                        Fechas.Contains(x.Fecha));
            IList<CuposDist> cupodist = Consignacion.FiltroConsignacion(query).ToList();
            return cupodist;
        }
    }
}