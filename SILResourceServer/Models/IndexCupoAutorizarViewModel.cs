using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResourceServer.Models
{
    public class IndexCupoAutorizarViewModel
    {
        public IList<ICuposAgrupados> CuposAgrupados { get; set; }
        [Display(Name = "Producto")]
        public virtual IEnumerable<SelectListItem> Productos
        {
            get
            {
                ICuposGranoSTOPStore storeST = new CuposGranoSTOPStore();
                var granos = storeST.FindAll()
                                .Select(x =>
                                    new SelectListItem
                                    {
                                        Value = x.NroGrano.ToString(),
                                        Text = x.NombreGrano
                                    });

                return new SelectList(granos, "Value", "Text");
            }
            private set { }
        }
    }
}