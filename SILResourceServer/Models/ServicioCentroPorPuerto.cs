using NHibernate;
using ResourceServer.Models.DataAccess;
using ResourceServer.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ServicioCentroPorPuerto
    {
        ISession session;

        public ServicioCentroPorPuerto() 
        {
            session = HibernateUtil.OpenSession();
        }
    
    #region MetodosABMs
        public IList<CentroPorPuertoDTO> GetRelacionPuertoCentro(CentroPorPuertoDTO filtro = null) 
        {
            CentroPorPuertoStore CxPStore = new CentroPorPuertoStore();
            IList<CuposCentroPorPuerto> listaDeElementos = new List<CuposCentroPorPuerto>();
            if (filtro != null && (filtro.IdTerminal != 0 || !string.IsNullOrEmpty(filtro.CodigoCentro)))
            {
                if (filtro.IdTerminal != 0 && !string.IsNullOrEmpty(filtro.CodigoCentro))
                {
                    listaDeElementos = CxPStore.FindByPuertoAndCentro(filtro.IdTerminal, filtro.CodigoCentro, session);
                }
                else
                {
                    if (filtro.IdTerminal != 0)
                    {
                        listaDeElementos = CxPStore.FindByPuerto(filtro.IdTerminal, session);
                    }
                    else
                    {
                        listaDeElementos = CxPStore.FindByCentro(filtro.CodigoCentro, session);
                    }
                }
            }
            else {
                listaDeElementos = CxPStore.FindAll(session);
            }
            IList<CentroPorPuertoDTO> listaDeObjDTO = new List<CentroPorPuertoDTO>();
            if (listaDeElementos.Count > 0) 
            {
                foreach (CuposCentroPorPuerto rel in listaDeElementos)
                {
                    CentroPorPuertoDTO Relacion = new CentroPorPuertoDTO(rel);
                    Relacion = this.BuscarNombrePuertoNombreCentro(Relacion, session);
                    listaDeObjDTO.Add(Relacion);
                }
            }
            return listaDeObjDTO;
        }

        public IList<Centro> GetCentros(String filtro = null)
        {
            CentroStore centroStore = new CentroStore();
            IList<Centro> centros = string.IsNullOrEmpty(filtro) ? centroStore.FindAllCenter(session) : centroStore.FindByCentro(filtro, session);
            return centros;
        }

        public Puerto GetPuertoPorIdTerminal(long filtro)
        {
            PuertoStore puertoStore = new PuertoStore();
            Puerto puerto= new Puerto();
            return puertoStore.FindByIdTerminal(filtro, session);        
        }

        public IList<Puerto> GetPuerto(string Text = "")
        {
            IList<Puerto> puertos = new List<Puerto>();
            PuertoStore puertoStore = new PuertoStore();
            if (Text != "")
            {
                if (EsNumero(Text))
                {
                    return puertoStore.FindPuertoStartsWithCuentaLimit(Int64.Parse(Text), this.session, 10);
                }
                else
                {
                    return puertoStore.FindPuertoLikeIgnoreCaseNombreLimit(Text, this.session, 10);
                }
            }
            else 
            {
                return puertoStore.FindAll(session);
            }
        }

        public bool EliminarRelacion(CentroPorPuertoDTO filtro)
        {
            CentroPorPuertoStore CxPStore = new CentroPorPuertoStore();
            CxPStore.Delete(filtro.ToCentroPorPuerto(), session);
            return true;
        }

        public CentroPorPuertoDTO Update(CentroPorPuertoDTO filtro)
        {
            CentroPorPuertoStore CxPStore = new CentroPorPuertoStore();
            CxPStore.Update(filtro.ToCentroPorPuerto(), session);
            return filtro;
        }

        public CentroPorPuertoDTO Save(CentroPorPuertoDTO relacion)
        {
            CentroPorPuertoStore CxPStore = new CentroPorPuertoStore();
            relacion.Id = CxPStore.Save(relacion.ToCentroPorPuerto(), session);
            if (relacion.Id != 0) 
            {
                relacion = this.BuscarNombrePuertoNombreCentro(relacion, session);
            }
            return relacion;
        }
    #endregion

    #region MetodosComplementarios

        public CentroPorPuertoDTO BuscarNombrePuertoNombreCentro(CentroPorPuertoDTO realcionCorriente, ISession session)
        {
            PuertoStore puertoStore = new PuertoStore();
            CentroStore centroStore = new CentroStore();
            Puerto puerto = puertoStore.FindByIdTerminal(realcionCorriente.IdTerminal, session);
            realcionCorriente.CuentaTerminal = puerto != null ? puerto.Cuenta : 0;
            realcionCorriente.NombreTerminal = puerto != null ? puerto.Nombre : "";
            Centro centro = centroStore.FindByCentro(realcionCorriente.CodigoCentro, session).FirstOrDefault();
            realcionCorriente.NombreCentro = centro != null ? centro.Nombre : "";
            return realcionCorriente;
        }

        public bool EsNumero(string Text)
        {
            int n;
            return int.TryParse(Text, out n);
        }

    #endregion
    }
}