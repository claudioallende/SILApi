using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.DataAccess
{
    public class DistribucionStore : IDistribucionStore
    {
        /*03-09: lo llamaremos una vez por cada cupo distribuido*/
        public int AgregarDistribucion(CuposDist cuposDist, Cupos cupoCuerpo, Cupos cupoEncabezado, Cupos nuevoCuerpoCYO, Cupos editoCuerpoCYOOrigen)
        {
            List<string> listaDeMapeos = new List<string>(new string[] { "CuposDist.mpg.xml", "Cupos.mpg.xml"});
            using (ISession session = HibernateUtil.OpenSession(listaDeMapeos))
            using (ITransaction tx = session.BeginTransaction()) 
            {
                try {
                    if (cuposDist != null && cupoCuerpo != null && cupoEncabezado != null) {
                        CuposDistStore abmDistStore = new CuposDistStore();
                        Int64 uvalueCupoDist = abmDistStore.Save(cuposDist, session, tx);
                        CuposStore abmCuposCorre = new CuposStore();
                        abmCuposCorre.Update(cupoEncabezado.Id, cupoEncabezado, session, tx);
                        cupoCuerpo.Uvcupodist = uvalueCupoDist;
                        abmCuposCorre.Update(cupoCuerpo.Id, cupoCuerpo, session, tx);
                        if (nuevoCuerpoCYO != null) {
                            if (nuevoCuerpoCYO.Id == 0)
                            {
                                abmCuposCorre.Save(nuevoCuerpoCYO, session, tx);
                            }
                            else {
                                abmCuposCorre.Delete(nuevoCuerpoCYO, session, tx);
                            }
                        }
                        if (editoCuerpoCYOOrigen != null) {
                            abmCuposCorre.Update(editoCuerpoCYOOrigen.Id, editoCuerpoCYOOrigen, session, tx);
                        }
                        tx.Commit();
                    }
                    HibernateUtil.Dispose();
                    return 1;
                }
                catch (Exception e) {
                    tx.Rollback(); 
                    HibernateUtil.Dispose();
                    throw e;
                }
            }
        }

        public int AnularDistribucion (CuposDist cuposDist, Cupos cupoCuerpo, Cupos cupoEncabezado)
        {
            List<string> listaDeMapeos = new List<string>(new string[] { "CuposDist.mpg.xml", "Cupos.mpg.xml"});
            using (ISession session = HibernateUtil.OpenSession(listaDeMapeos))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    //Eliminar si CuposDist tiene Cupos en 0
                    if (cuposDist != null) {
                        CuposDistStore abmDistStore = new CuposDistStore();
                        if (cuposDist.Cupos == 0)
                        {
                            abmDistStore.Delete(cuposDist, session);
                        }
                        else
                        {
                            abmDistStore.Update(cuposDist, session);
                        }
                        
                        CuposStore abmCuposCorre = new CuposStore();
                        abmCuposCorre.AnularCupoDistribuido(cupoCuerpo, cupoEncabezado, session);
                        
                        tx.Commit();
                    }
                    HibernateUtil.Dispose();
                    return 1;
                }
                catch (Exception e)
                {
                    tx.Rollback();
                    HibernateUtil.Dispose();
                    throw e;
                }

            
            }
        }

        public void AnularDistribucionCYO(CuposDist cuposDist, Cupos cupoCuerpoCYO, Cupos cupoCuerpoRelacionado, Cupos cupoEncabezado)
        {
            List<string> listaDeMapeos = new List<string>(new string[] { "CuposDist.mpg.xml", "Cupos.mpg.xml" });
            using (ISession session = HibernateUtil.OpenSession(listaDeMapeos))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    //Eliminar si CuposDist tiene Cupos en 0
                    if (cuposDist != null)
                    {
                        CuposDistStore abmDistStore = new CuposDistStore();
                        if (cuposDist.Cupos == 0)
                        {
                            abmDistStore.Delete(cuposDist, session);
                        }
                        else
                        {
                            abmDistStore.Update(cuposDist, session);
                        }

                        CuposStore abmCuposCorre = new CuposStore();
                        abmCuposCorre.Update(session, cupoCuerpoCYO);
                        abmCuposCorre.Update(session, cupoCuerpoRelacionado);
                        abmCuposCorre.Update(session, cupoEncabezado);

                        tx.Commit();
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

        public void Delete(string Id)
        {
            throw new NotImplementedException();
        }

        //public void DistribuirCupo(Cupos CupoEncabezado, Cupos CupoCuerpo, CuposDist Distribucion)
        //{
        //    List<string> listaDeMapeos = new List<string>(new string[] { "CuposDist.mpg.xml", "Cupos.mpg.xml" });
        //    using (ISession session = HibernateUtil.OpenSession(listaDeMapeos))
        //    using (ITransaction tx = session.BeginTransaction())
        //    {
        //        try
        //        {
        //            //Eliminar si CuposDist tiene Cupos en 0
        //            if (Distribucion != null)
        //            {
        //                CuposDistStore DistribucionStore = new CuposDistStore();
        //                if (Distribucion.Cupos == 0)
        //                {
        //                    DistribucionStore.Delete(Distribucion, session);
        //                }
        //                else
        //                {
        //                    DistribucionStore.Update(Distribucion, session);
        //                }
        //
        //                CuposStore CuposStore = new CuposStore();
        //                CuposStore.Update(session, CupoCuerpo);
        //                CuposStore.Update(session, CupoEncabezado);
        //
        //                tx.Commit();
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            tx.Rollback();
        //            throw e;
        //        }
        //    }
        //}


        public void Update(CuposDist Distribucion, Cupos CupoCuerpo, Cupos CupoEncabezado)
        {
            List<string> listaDeMapeos = new List<string>(new string[] { "CuposDist.mpg.xml", "Cupos.mpg.xml" });
            using (ISession session = HibernateUtil.OpenSession(listaDeMapeos))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    //Eliminar si CuposDist tiene Cupos en 0
                    if (Distribucion != null)
                    {
                        CuposDistStore DistribucionStore = new CuposDistStore();
                        if (Distribucion.Cupos == 0)
                        {
                            DistribucionStore.Delete(Distribucion, session);
                        }
                        else
                        {
                            DistribucionStore.Update(Distribucion, session);
                        }

                        CuposStore CupoStore = new CuposStore();
                        CupoStore.Update(session, CupoCuerpo);
                        CupoStore.Update(session, CupoEncabezado);

                        tx.Commit();
                        HibernateUtil.Dispose();
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

        public void Update(CuposDist Distribucion, Cupos CupoPadreCyO, Cupos CupoHijoCyO, Cupos CupoEncabezado)
        {
            List<string> listaDeMapeos = new List<string>(new string[] { "CuposDist.mpg.xml", "Cupos.mpg.xml" });
            using (ISession session = HibernateUtil.OpenSession(listaDeMapeos))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    //Eliminar si CuposDist tiene Cupos en 0
                    if (Distribucion != null)
                    {
                        CuposDistStore DistribucionStore = new CuposDistStore();
                        if (Distribucion.Cupos == 0)
                        {
                            DistribucionStore.Delete(Distribucion, session);
                        }
                        else
                        {
                            DistribucionStore.Update(Distribucion, session);
                        }

                        CuposStore CupoStore = new CuposStore();
                        CupoStore.Update(session, CupoPadreCyO);
                        CupoStore.Delete(session, CupoHijoCyO);
                        CupoStore.Update(session, CupoEncabezado);

                        tx.Commit();
                        HibernateUtil.Dispose();
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

        public void NuevaDistribucion(CuposDist Distribucion, Cupos CupoCuerpo, Cupos CupoEncabezado, ISession session)
        {
            List<string> listaDeMapeos = new List<string>(new string[] { "CuposDist.mpg.xml", "Cupos.mpg.xml" });
            {
                //Eliminar si CuposDist tiene Cupos en 0
                if (Distribucion != null)
                {
                    CuposDistStore DistribucionStore = new CuposDistStore();
                    //Crea o actualiza en caso de que contenga comprador, vendedor, grano, destino, centro y fecha iguales a una 
                    //distribucion ya existente
                    long Uvalue = DistribucionStore.Save(Distribucion, session);

                    CuposStore CupoStore = new CuposStore();
                    CupoCuerpo.Uvcupodist = Uvalue;
                    CupoStore.Update(session, CupoCuerpo);
                    CupoStore.Update(session, CupoEncabezado);
                }
            }
        }
        
        public void NuevaDistribucion(Cupos CupoCuerpo, long Uvdist, ISession session)
        {
            //Eliminar si CuposDist tiene Cupos en 0
            if (Uvdist != 0)
            {
                CuposStore CupoStore = new CuposStore();
                CupoCuerpo.Uvcupodist = Uvdist;
                CupoStore.Update(session, CupoCuerpo);
            }
        }


        public void NuevaDistribucion(CuposDist Distribucion, Cupos CupoCuerpo)
        {
            List<string> listaDeMapeos = new List<string>(new string[] { "CuposDist.mpg.xml", "Cupos.mpg.xml" });
            using (ISession session = HibernateUtil.OpenSession(listaDeMapeos))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    //Eliminar si CuposDist tiene Cupos en 0
                    if (Distribucion != null)
                    {
                        CuposDistStore DistribucionStore = new CuposDistStore();
                        //Crea o actualiza en caso de que contenga comprador, vendedor, grano, destino, centro y fecha iguales a una 
                        //distribucion ya existente
                        long Uvalue = DistribucionStore.Save(Distribucion, session);

                        CuposStore CupoStore = new CuposStore();
                        CupoCuerpo.Uvcupodist = Uvalue;
                        CupoStore.Update(session, CupoCuerpo);

                        tx.Commit();
                        HibernateUtil.Dispose();
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


        public void AnularCupoCyO(CuposDist Distribucion, Cupos CupoPadreCyO, Cupos CupoHijoCyO, Cupos CupoEncabezado)
        {
            List<string> listaDeMapeos = new List<string>(new string[] { "CuposDist.mpg.xml", "Cupos.mpg.xml" });
            using (ISession session = HibernateUtil.OpenSession(listaDeMapeos))
            using (ITransaction tx = session.BeginTransaction())
            {
                try
                {
                    //Eliminar si CuposDist tiene Cupos en 0
                    if (Distribucion != null)
                    {
                        CuposDistStore abmDistStore = new CuposDistStore();
                        if (Distribucion.Cupos == 0)
                        {
                            abmDistStore.Delete(Distribucion, session);
                        }
                        else
                        {
                            abmDistStore.Update(Distribucion, session);
                        }

                        CuposStore abmCuposCorre = new CuposStore();
                        abmCuposCorre.AnularCupoDistribuido(CupoPadreCyO, CupoEncabezado, session);
                        abmCuposCorre.Update(session, CupoHijoCyO);
                        //abmCuposCorre.Delete(CupoHijoCyO, session);

                        tx.Commit();
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


        public void NuevaDistribucion(CuposDist Distribucion, Cupos CupoCuerpo, Cupos CupoEncabezado)
        {
            throw new NotImplementedException();
        }

        public void NuevaDistribucion(long Distribucion, Cupos Cupo, ISession Session)
        {
            throw new NotImplementedException();
        }
    }
}