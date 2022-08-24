using NHibernate;
using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models.Configuracion
{
    public class ServicioConfiguracionCuentaCYO
    {
        public string SaveOrUpdateCuentaCYO(CuentaCYOViewModel CuentaCYO)
        {
            IDTablaStore Store = new DTablaStore();
            return Store.SaveOrUpdateCuentaCYO(CuentaCYO);
        }

        public string DeleteCuentaCYO(CuentaCYOViewModel CuentaCYO)
        {
            IDTablaStore Store = new DTablaStore();
            return Store.BuscarYBorrarCuentaCYO(CuentaCYO);
        }

        public bool EsCuentaYOrden(string Cuenta, ISession session = null)
        {
            IDTablaStore Store = new DTablaStore();
            DTabla busqueda = Store.FindByEntidadAndClave("CUPCYO", Cuenta, session);
            return (busqueda != null);
        }

        public bool EsCuentaYOrden(string Cuenta, string Cuit)
        {
            Cuit = Cuit.Replace("-", string.Empty);
            Cuit = Cuit.Insert(2, "-");
            Cuit = Cuit.Insert(Cuit.Length - 1, "-");
            IDTablaStore Store = new DTablaStore();
            DTabla busqueda = Store.FindByEntidadAndClaveAndOrdenAndValor("CUPCYO", Cuenta, "CYO03", Cuit);
            return (busqueda != null);
        }
    }
}