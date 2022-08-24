using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public class ServicioGrano
    {
        public IList<Grano> ObtenerGranos()
        {
            GranoStore Store = new GranoStore();
            return Store.FindAll();
        }

        //public IList<Grano> ObtenerGranos(long CuentaVendedor)
        //{
        //    GranoStore Store = new GranoStore();
        //    return Store.FindBy();
        //}
    }
}