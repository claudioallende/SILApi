using ResourceServer.Models.DataAccess;
using ResourceServer.Models.Vista;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResourceServer.Models
{
    public class IndexCupoViewModel
    {
        private ICuposAgrupadosStore StoreCupos;

        public IndexCupoViewModel()
        {
            StoreCupos = new CuposAgrupadosStore();
        }

        public int ProductoSeleccionado { get; set; }
        public IList<ICuposAgrupados> CuposAgrupados { get; set; }
        public IList<ICuposAgrupados> CuposAgrupadosCyO { get; set; }
        [Display(Name = "Producto")]
        public virtual IEnumerable<SelectListItem> Productos
        {
            get
            {
                IGranoStore store = new GranoStore();
                var granos = store.FindAll()
                            .Select(x =>
                                    new SelectListItem
                                    {
                                        Value = x.Id.ToString(),
                                        Text = x.Nombre
                                    });

                return new SelectList(granos, "Value", "Text");
            }
        }
        [Display(Name = "Centro")]
        public virtual IEnumerable<SelectListItem> Centros
        {
            get
            {
                ICentroStore store = new CentroStore();
                var centros = store.FindAll()
                            .Select(x =>
                                    new SelectListItem
                                    {
                                        Value = x.Id.ToString(),
                                        Text = x.Nombre,
                                        Selected = CentroSeleccionado != null && CentroSeleccionado.Contains(x.Id.ToString())
                                    });

                return new SelectList(centros, "Value", "Text");
            }
        }
        public IList<string> CentroSeleccionado { get; set; }
        public bool TieneFiltroGrano { get; set; }
        public string Comprador { get; set; }
        public string Vendedor { get; set; }

        //Ya lo pase a SIL en DK
        public void Filtro()
        {
            if (!string.IsNullOrEmpty(this.Vendedor))
            {
                this.CuposAgrupados = StoreCupos.FindByFiltro(this, this.Vendedor);
                this.CuposAgrupadosCyO = StoreCupos.FindByFiltroCyo(this, this.Vendedor);
            }
            else
            {
                this.CuposAgrupados = StoreCupos.FindByFiltro(this);
                this.CuposAgrupadosCyO = StoreCupos.FindByFiltroCyo(this);
            }
        }
        public void SinFiltro()
        {
            CuposAgrupados = (IList<ICuposAgrupados>)StoreCupos.FindAll();
            CuposAgrupadosCyO = (IList<ICuposAgrupados>)StoreCupos.FindOnlyCyO();
        }
        public IList<string> OcultarColumnas()
        {
            IList<string> Columnas = new List<string>();
            if (ProductoSeleccionado != 0)
            {
                Columnas.Add("Grano");
                TieneFiltroGrano = true;
            }
            else
            {
                TieneFiltroGrano = false;
            }
            return Columnas;
        }
    }
}