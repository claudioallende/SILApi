using ResourceServer.Models.Cupo;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Error;
using ResourceServer.Models.Error.Exceptions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ServicioEditar
    {
        public ServicioEditar()
        {
            storeCupos = new CuposStore();
            storeCuposDist = new CuposDistStore();
            storeDistribucion = new DistribucionStore();
            ServicioCupo = new ServicioCupo();
        }
        private ICuposStore storeCupos;
        private ICuposDistStore storeCuposDist;
        private IDistribucionStore storeDistribucion;
        private ServicioCupo ServicioCupo { get; set; }
        private IList<Cupos> CuposModificados { get; set; }

        public void Anular(string AllIds, string Motivo, string Tipo, bool Cyo)
        {
            if (!String.IsNullOrEmpty(AllIds))
            {
                IList<long> Ids = GetIds(AllIds);
                using (ISession Session = HibernateUtil.OpenSession(new List<string>() { "Cupos.mpg.xml", "CuposDist.mpg.xml" }))
                using (ITransaction tx = Session.BeginTransaction())
                {
                    try
                    {
                        CuposModificados = storeCupos.FindByIds(Ids, Session);
                        if (Tipo == "Cupo")
                        {
                            AnularCupos(CuposModificados, Session, Motivo);
                        }
                        else if (Tipo == "Distribucion")
                        {
                            AnularDistribuciones(CuposModificados, Session, Motivo, Cyo);
                        }
                        tx.Commit();
                    }
                    catch (Exception e)
                    {
                        tx.Rollback();
                        throw e;
                    }
                }
            }
        }

        private IList<long> GetIds(string Ids)
        {
            return Ids.Split('-').Select(x => Int64.Parse(x)).ToList();
        }

        /// <summary>
        /// Anula las distribuciones de la lista de cupos que recive como parámetro. 
        /// En caso de que alguno de ellos sea cyo se buscará su cupo relacionado y se anulará.
        /// </summary>
        /// <param name="Cupos">Lista de cupos a anular distribucion</param>
        /// <param name="Session"></param>
        /// <param name="Motivo"></param>
        public void AnularDistribuciones(IList<Cupos> Cupos, ISession Session, string Motivo, bool Cyo)
        {
            ServicioDistribuir Servicio = new ServicioDistribuir();
            Servicio.BuscarYAnularDistribuciones(Cupos, Motivo, Cyo, Session);
        }

        public void AnularCupos(IList<Cupos> Cupos, ISession Session, string Motivo)
        {
            ServicioDistribucion ServicioDistribucion = new ServicioDistribucion();
            IList<CuposDist> Distribuciones = storeCuposDist.FindByUvalues(Cupos.Select(x => x.Uvcupodist).ToList(), Session);
            IList<Cupos> Encabezados = ServicioCupo.ObtenerEncabezados(Cupos, Session);
            CuposDist Distribucion = null;
            InformacionModificacionEstado Info = null;
            IList<Cupos> CuposRelacionados = this.ServicioCupo.ObtenerCuposRelacionados(Cupos, Session);
            ServicioDistribucion.RestarDistribucionesCuposRelacionados(CuposRelacionados, Session);
            //Anula Cupos de padres cyo y normales
            this.ServicioCupo.SetearCuposHijoAPadreCYO(Cupos, CuposRelacionados, Session);
            Cupos = this.ServicioCupo.ObtenerCuposPadreCYOYNormales(Cupos);
            /////////////////////////////////////////////////////////////////////////////////////
            foreach (Cupos Cupo in Cupos)
            {
                Distribucion = Distribuciones.Where(x => x.Uvalue == Cupo.Uvcupodist).FirstOrDefault();
                Info = Cupo.GetEstado().AnularCupo(Motivo, ServicioCupo.ObtenerEncabezado(Encabezados, Cupo), Session);
                if (Distribucion != null && Distribucion.Cupos > 0)
                {
                    Distribucion.Cupos -= 1;
                    Distribucion.Usuario = ResourceServer.Models.Identity.IdentityHelper.GetUsuarioLogueado();
                }
            }
            storeCuposDist.Update(Distribuciones, Session);
        }

        public string GetMotivoAnulacion(long id)
        {
            return storeCupos.FindById(id).Motbaja;
        }

        public IList<Cupos> GetCuposModificados()
        {
            return CuposModificados;
        }

    }
}