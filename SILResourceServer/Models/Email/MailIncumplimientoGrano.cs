using NHibernate;
using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace ResourceServer.Models.Email
{
    public class MailIncumplimientoGrano
    {
        public int Grano { get; set; }
        public IList<ICupo> ListDetalleIncumplimiento{ get; set; }

        public MailIncumplimientoGrano(int grano)
        {
            this.Grano = grano;
        }

        /// <summary>
        /// Total de cupos perdidos por grano.
        /// Es la suma de la columna perdidos de Detalle de incumplimiento para este grano.
        /// </summary>
        /// <returns></returns>
        public int TotalCuposPerdidos()
        {
            int total = 0;
            foreach (DetalleDeIncumplimientoCupo detalleDiario in ListDetalleIncumplimiento) 
            {
                total+= detalleDiario.Perdidos;
            }
            return total;
        }

        public int TotalCuposOtorgados()
        {
            int total = 0;
            foreach (DetalleDeIncumplimientoCupo detalleDiario in ListDetalleIncumplimiento)
            {
                total += detalleDiario.Otorgados;
            }
            return total;
        }

        /// <summary>
        /// Esta func retorna un string que contiene una tabla (html) de detalles de incumplimiento para el grano.
        /// indica primero su nombre y luego la tabla
        /// </summary>
        /// <returns></returns>
        public string ConverToString(ISession session)
        {
            IGranoStore granoStore = new GranoStore();
            //Grano grano = granoStore.FindById(this.Grano.ToString());
            Grano grano = granoStore.FindById(this.Grano.ToString(), session);
            var detalleDeIncumplimientoConFormato = this.ListDetalleIncumplimiento
                                                .Select(z =>
                                                new 
                                                {
                                                    Vendcta = z.Vendcta,
                                                    Grano = z.Grano,
                                                    Fecha = z.Fecha.ToString("dd/MM/yyyy"),
                                                    Otorgados = z.Otorgados,
                                                    Cumplidos = z.Cumplidos,
                                                    Incumplimiento = z.Perdidos,
                                                    PorcentajeDeIncumplimiento = Math.Truncate(z.PorcentajeDePerdida) + " %"
                                                })
                                                .ToList();

            string texto = "<br/><b>" + grano.Nombre + "</b><br/>" + System.Environment.NewLine;
            ListToHtmlTable conversor = new ListToHtmlTable("center", "center", calcularTotales());
            texto += conversor.GetMyTable(detalleDeIncumplimientoConFormato, x => x.Fecha, x => x.Otorgados, x => x.Cumplidos, x => x.Incumplimiento, x => x.PorcentajeDeIncumplimiento);
            return texto;
        }

        /// <summary>
        /// creamos la lista de valores para el footer. Esta lista tiene la misma cantidad de elementos
        /// que columnas la tabla
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        protected List<string> calcularTotales()
        {
            List<string> totales = new List<string>();
            totales.Add("Totales");
            int otorg = this.ListDetalleIncumplimiento.Sum(x => x.Otorgados);
            totales.Add(otorg.ToString());
            totales.Add(this.ListDetalleIncumplimiento.Sum(x => x.Cumplidos).ToString());
            int perd = this.ListDetalleIncumplimiento.Sum(x => x.Perdidos);
            totales.Add(perd.ToString());
            totales.Add((perd*100/otorg).ToString()+" %");
            return totales;
        }
    }
}