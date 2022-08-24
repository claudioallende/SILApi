using NHibernate;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ServicioRelacionGranoSILGranoSTOP
    {
        private CuposGranoSTOPStore GranoSTOPStore { get; set; }
        private GranoStore GranoSILStore { get; set; }
        private RelacionGranoSILGranoSTOPStore RelacionStore { get; set; }
        private ISession session;

        public ServicioRelacionGranoSILGranoSTOP()
        {
            this.GranoSTOPStore = new CuposGranoSTOPStore();
            this.GranoSILStore = new GranoStore();
            this.RelacionStore = new RelacionGranoSILGranoSTOPStore();
            this.session = HibernateUtil.OpenSession("");
        }

        public List<RelacionGranoSILGranoSTOPCompleta> GetRelaciones(RelacionGranoSILGranoSTOPDTO relacion = null)
        {
            List<RelacionGranoSILGranoSTOPCompleta> listResult = new List<RelacionGranoSILGranoSTOPCompleta>();
            if (relacion != null && (relacion.NroGranoSIL > 0 || relacion.NroGranoSTOP > 0))
            {
                if (relacion.NroGranoSIL > 0 && relacion.NroGranoSTOP > 0)
                {
                    listResult.Add(this.RelacionStore.FindRelacionCompletaByNroGranoSILAndNroGranoSTOP(relacion.ToRelacionGranoSILGranoSTOPCompleta(), session));
                }
                else if (relacion.NroGranoSIL > 0)
                {
                    listResult.AddRange(this.RelacionStore.FindRelacionCompletaByNroGranoSIL(relacion.NroGranoSIL, session));
                }
                else
                {                                       /* relacion.NRoGranoSTOP > 0 */
                    listResult.AddRange(this.RelacionStore.FindRelacionCompletaByNroGranoSTOP(relacion.NroGranoSTOP, session));
                }
            }
            else
            {
                listResult.AddRange(this.RelacionStore.FindAllRelacionCompleta(session));
            }
            return listResult;
        }

        public RelacionGranoSILGranoSTOPCompleta AgregarRelacion(RelacionGranoSILGranoSTOPDTO nuevaRelacion) 
        {

            if (nuevaRelacion != null)
            {
                return this.RelacionStore.Save(nuevaRelacion.ToRelacionGranoSILGranoSTOPCompleta(), session);
            }
            return null;
        }

        public bool EliminarRelacion(RelacionGranoSILGranoSTOPDTO relacion)
        {
            if (relacion != null) 
            {
                return this.RelacionStore.Delete(relacion.ToRelacionGranoSILGranoSTOPCompleta(), session);
            }
            return false;
        }

        public RelacionGranoSILGranoSTOPCompleta ModificarRelacion(RelacionGranoSILGranoSTOPDTO relacion)
        {
            if (relacion != null)
            {
                return this.RelacionStore.UpdateRelacion(relacion.ToRelacionGranoSILGranoSTOPCompleta(), session);
            }
            return null;
        }
    }
}