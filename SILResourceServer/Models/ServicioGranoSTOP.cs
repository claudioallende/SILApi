using NHibernate;
using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;

namespace ResourceServer.Models
{
  public class ServicioGranoSTOP
    {
        private CuposGranoSTOPStore GranoSTOPStore { get; set; }
        private ISession session;

        public ServicioGranoSTOP()
        {
            this.GranoSTOPStore = new CuposGranoSTOPStore();
            this.session = HibernateUtil.OpenSession("");
        }

        public List<CuposGranoSTOP> GetGranos(CuposGranoSTOP grano = null)
        {
            List<CuposGranoSTOP> listResult = new List<CuposGranoSTOP>();
            if (grano != null)
            {
                listResult.Add(this.GranoSTOPStore.FindByNroGrano(grano.NroGrano, session));
            }
            else 
            {
                listResult = (List<CuposGranoSTOP>)this.GranoSTOPStore.FindAll(session);
            }

           return listResult;
        }

        public CuposGranoSTOP AgregarGrano(CuposGranoSTOP grano) 
        {
            if (grano != null)
            {
                return this.GranoSTOPStore.Save(grano, session);                
            }
            return null;
        }

        public bool EliminarGrano(CuposGranoSTOP grano)
        {
            bool result = false;
            if (grano != null) 
            {
                ITransaction tx = session.BeginTransaction();
                try
                {       
                    IRelacionGranoSILGranoSTOPStore relaStore = new RelacionGranoSILGranoSTOPStore();
                    if (relaStore.DeleteTodasLasRelacionesDeGranoSTOP(grano.NroGrano, session, tx))
                    {
                        result = this.GranoSTOPStore.Delete(grano, session, tx);
                    }
                    tx.Commit();
                } 
                catch (Exception e) {
                    tx.Rollback();
                    throw e;
                }
                return result;
            }
            return false;
        }

        public CuposGranoSTOP ModificarGrano(CuposGranoSTOP grano)
        {
            if (grano != null)
            {
                return this.GranoSTOPStore.Update(grano,session);
            }
            return null;
        }
    }
}