using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ServicioVarios
    {
        public IList<DTabla> GetMonedas()
        {
            IDTablaStore store = new DTablaStore();
            return store.findClaveValorByEntidadAndOrden("MONEDA", "MN2").Where(x => x.Clave != null && !string.IsNullOrEmpty(x.Clave.Trim())).ToList();
        }

        public IList<DTabla> GetMonedas(IList<string> ids)
        {
            IDTablaStore store = new DTablaStore();
            return store.findClaveValorByEntidadAndOrden("MONEDA", "MN2").Where(x => ids.Contains(x.Clave)).ToList();
        }

        public IList<DTabla> GetCondicionMercaderia()
        {
            IDTablaStore store = new DTablaStore();
            return store.findClaveValorByEntidadAndOrden("CALIDAD", "CA2").Where(x => x.Clave != null && !string.IsNullOrEmpty(x.Clave.Trim())).ToList();
        }

        public IList<DTabla> GetCondicionMercaderia(IList<string> ids)
        {
            IDTablaStore store = new DTablaStore();
            return store.findClaveValorByEntidadAndOrden("CALIDAD", "CA2").Where(x => ids.Contains(x.Clave)).ToList();
        }

        public IList<DTabla> GetTipoCuenta()
        {
            IDTablaStore store = new DTablaStore();
            return store.findClaveValorByEntidadAndOrden("STPROVEE", "02FA").Where(x => x.Clave != null && !string.IsNullOrEmpty(x.Clave.Trim())).ToList();
        }

        public IList<DTabla> GetTipoCuenta(IList<string> ids)
        {
            IDTablaStore store = new DTablaStore();
            return store.findClaveValorByEntidadAndOrden("STPROVEE", "02FA").Where(x => ids.Contains(x.Clave)).ToList();
        }

        public IList<DTabla> GetTipoOperacion()
        {
            IDTablaStore store = new DTablaStore();
            return store.findClaveValorByEntidadAndOrden("TIP_OPER", "OP2").Where(x => x.Clave != null && !string.IsNullOrEmpty(x.Clave.Trim())).ToList();
        }

        public IList<DTabla> GetTipoOperacion(IList<string> ids)
        {
            IDTablaStore store = new DTablaStore();
            return store.findClaveValorByEntidadAndOrden("TIP_OPER", "OP2").Where(x => ids.Contains(x.Clave)).ToList();
        }

        public IList<TbCoper> GetZonaComercial()
        {
            TbCoperStore store = new TbCoperStore();
            return store.FindAll();
        }

        public IList<TbCoper> GetZonaComercial(IList<string> ids)
        {
            TbCoperStore store = new TbCoperStore();
            return store.FindAll().Where(x => ids.Contains(x.Codigo)).ToList();
        }
    }
}