using NHibernate;
using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace ResourceServer.Models.Email
{
    public class MailIncumplimientoVendedor: IMailInformation
    {
        public Int64 Vendcta { get; set; }
        public IList<MailIncumplimientoGrano> ListIncumplimientoPorGrano { get; set; }
        public ISession SessionDePrueba { get; set; }

        public MailIncumplimientoVendedor(Int64 vendcta)
        {
            this.Vendcta = vendcta;
            this.ListIncumplimientoPorGrano = new List<MailIncumplimientoGrano>();
        }

        /// <summary>
        /// Total de cupos perdidos por cuenta
        /// Es la suma de la columna perdidos de Detalle de incumplimiento para todos los granos juntos
        /// </summary>
        /// <returns></returns>
        public int TotalCuposPerdidos() 
        {
            int total = 0;
            foreach (MailIncumplimientoGrano detalleDiario in ListIncumplimientoPorGrano)
            {
                total += detalleDiario.TotalCuposPerdidos();
            }
            return total;
        }

        public int TotalCuposOtorgados()
        {
            int total = 0;
            foreach (MailIncumplimientoGrano detalleDiario in ListIncumplimientoPorGrano)
            {
                total += detalleDiario.TotalCuposOtorgados();
            }
            return total;
        }

        public decimal PorcentajeDeEfectividad()
        {
            decimal co = Convert.ToDecimal(this.TotalCuposOtorgados());
            decimal cp = Convert.ToDecimal(this.TotalCuposPerdidos());
            decimal total = decimal.Round((((co - cp) * 100) / co), 2, MidpointRounding.AwayFromZero);
            return total;
        }
        /// <summary>
        /// Esta func retorna un string que contiene una tabla (html) de detalles de incumplimiento por Grano.
        /// indica primero su nombre y luego la tabla
        /// </summary>
        /// <returns></returns>
        public override string ToString() 
        {
            string retorno = "";
            foreach (MailIncumplimientoGrano c in this.ListIncumplimientoPorGrano) 
            {
                retorno += c.ToString() + "<Br/><Br/>";
            }
            return retorno;
        }

        /// <summary>
        /// Esta func retorna un string que contiene una tabla (html) de detalles de incumplimiento por Grano. Seguido de un grafico
        /// mostrando la misma informacion
        /// </summary>
        /// <param name="bodyCorriente"></param>
        /// <returns></returns>
        public AlternateView ToStringWithGraph(ISession session, string bodyCorriente = "", string pieDeCorreo ="") 
        {
            string retorno = bodyCorriente;
            int numeroDeTabla = 1;
            foreach (MailIncumplimientoGrano c in this.ListIncumplimientoPorGrano)
            {
                string aux = "\"$CONTENTID" + numeroDeTabla + "$\"";
                //retorno += c.ToString() + @"<img src="+aux+"/>" + "<Br/><Br/>";
                retorno += @"<img src=" + aux + "/><Br/>" + c.ConverToString(session) + "<Br/><Br/>";
                numeroDeTabla++;
            }
            retorno += "<br/><br/>" + pieDeCorreo + "<br/>";
            AlternateView resultBody = this.completarConGraficos(retorno, session);
            return resultBody;
        }

        /// <summary>
        /// reemplaza los ids generados en ToStringWithGraph con los graficos para cada grano
        /// </summary>
        /// <param name="bodyCorriente"></param>
        /// <returns>el body con los graficos incluidos</returns>
        protected AlternateView completarConGraficos(string bodyCorriente, ISession session) 
        {
            string bodyDelMail = bodyCorriente;
            int numeroDeTabla = 1;
            List<string> listaDeGui = new List<string>();
            foreach (MailIncumplimientoGrano c in this.ListIncumplimientoPorGrano) 
            {
                var CONTENTID1 = Guid.NewGuid().ToString().Replace("-", "");
                string aux = "$CONTENTID" + numeroDeTabla + "$";
                bodyDelMail = bodyDelMail.Replace(aux, "cid:" + CONTENTID1);
                listaDeGui.Add(CONTENTID1);
                numeroDeTabla++;
            }
            numeroDeTabla = 1;
            var htmlView = AlternateView.CreateAlternateViewFromString(bodyDelMail, null, "text/html");
            foreach (MailIncumplimientoGrano c in this.ListIncumplimientoPorGrano)
            {
                var stream = new MemoryStream();
                //stream = this.ToGraph(c); 
                stream = this.GraficoMultipleOtorgadosCumplimientoIncumplimiento(c, session);
                stream.Position = 0;
                LinkedResource imagelink1 = new LinkedResource(stream, "image/png");
                imagelink1.ContentId = listaDeGui[numeroDeTabla - 1];
                imagelink1.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                htmlView.LinkedResources.Add(imagelink1);
                numeroDeTabla++;
            }
            return htmlView;
        }

        /// <summary>
        /// calcula el ancho del grafico. para que no se superponga la informacion
        /// </summary>
        /// <returns></returns>
        public int CalculaAnchoDeGraficoSegunPeriodoIngresado(int cantidadDeElementos = 0) 
        {
            int width = 600;
            int elementos = (cantidadDeElementos == 0) ? this.ListIncumplimientoPorGrano[0].ListDetalleIncumplimiento.Count() : cantidadDeElementos;    
            if (elementos >= 10 && elementos < 20) width = 1000;
            if (elementos >= 20 && elementos < 30) width = 1400;
            if (elementos >= 30 && elementos < 40) width = 1800;
            if (elementos >= 40) width = 2000;
            return width;
        }

        /// <summary>
        /// genera un grafico para visualizar cupos incumplidos para la lista de 
        /// incumplimiento pasado como parametro(esta es para un grano determinado)
        /// </summary>
        /// <param name="DetalleIncumplimiento"></param>
        /// <returns></returns>
        protected MemoryStream GraficoSimpleIncumplimiento(MailIncumplimientoGrano DetalleIncumplimiento) 
        {
            var stream = new MemoryStream();
            IGranoStore granoStore = new GranoStore();
            IList<ICupo> listaDeElementosGrano = new List<ICupo>();
            IList<DominioEImagenForMultiLine> listaDeDominioEImagenPorGrano = new List<DominioEImagenForMultiLine>();
            List<System.Drawing.Color> listaDeColoresParaLosGraficos = new List<System.Drawing.Color>();
            IList<DominioEImagen> listaDominioEImagenDelGrano = new List<DominioEImagen>();
            EmailChart constructorGraph = new EmailChart(300, CalculaAnchoDeGraficoSegunPeriodoIngresado(DetalleIncumplimiento.ListDetalleIncumplimiento.Count()), "Porcentaje de perdida de granos");

            DominioEImagenForMultiLine incumplimientoDominioEImagenForMultiLine = new DominioEImagenForMultiLine();            
            incumplimientoDominioEImagenForMultiLine.nombre = "Incumplidos";
            incumplimientoDominioEImagenForMultiLine.codigoNombre = DetalleIncumplimiento.Grano;

            /*incumplidos*/
            listaDeElementosGrano = DetalleIncumplimiento.ListDetalleIncumplimiento;
            listaDominioEImagenDelGrano = listaDeElementosGrano
                .Select(y =>
                                new DominioEImagen
                                {
                                    Dominio = y.Fecha.ToString("dd/MM/yy"),
                                    Imagen = y.Perdidos.ToString()
                                }
                    )
                    .ToList();
            incumplimientoDominioEImagenForMultiLine.ListOfDominioEImagen = listaDominioEImagenDelGrano;
            listaDeDominioEImagenPorGrano.Add(incumplimientoDominioEImagenForMultiLine);
            listaDeColoresParaLosGraficos.Add(System.Drawing.Color.Red);

            stream = constructorGraph.AddChart(listaDeDominioEImagenPorGrano, listaDeColoresParaLosGraficos, System.Web.UI.DataVisualization.Charting.SeriesChartType.Column, granoStore.FindById(DetalleIncumplimiento.Grano.ToString()).Nombre);

            return stream;
        }

        /// <summary>
        /// genera un grafico para visualizar cupos otorgados, cumplidos e incumplidos para la lista de 
        /// incumplimiento pasado como parametro(esta es para un grano determinado)
        /// </summary>
        /// <param name="DetalleIncumplimiento"></param>
        /// <returns></returns>
        public MemoryStream GraficoMultipleOtorgadosCumplimientoIncumplimiento(MailIncumplimientoGrano DetalleIncumplimiento, ISession session) 
        {
            var stream = new MemoryStream();
            IGranoStore granoStore = new GranoStore();
            IList<ICupo> listaDeElementosGrano = new List<ICupo>();
            IList<DominioEImagenForMultiLine> listaDeDominioEImagenPorGrano = new List<DominioEImagenForMultiLine>();
            List<System.Drawing.Color> listaDeColoresParaLosGraficos = new List<System.Drawing.Color>();
            IList<DominioEImagen> listaDominioEImagenDelGrano = new List<DominioEImagen>();
            EmailChart constructorGraph = new EmailChart(300, 1100, "Porcentaje de perdida de granos");

                            /*genera un grafico para el incumplimiento pasado como parametro*/
            DominioEImagenForMultiLine incumplimientoDominioEImagenForMultiLine = new DominioEImagenForMultiLine();
            DominioEImagenForMultiLine cumplimientoDominioEImagenForMultiLine = new DominioEImagenForMultiLine();
            DominioEImagenForMultiLine otorgadoDominioEImagenForMultiLine = new DominioEImagenForMultiLine();
                
            incumplimientoDominioEImagenForMultiLine.nombre = "Incumplidos";
            cumplimientoDominioEImagenForMultiLine.nombre = "Cumplidos";
            otorgadoDominioEImagenForMultiLine.nombre = "Otorgados";
            incumplimientoDominioEImagenForMultiLine.codigoNombre = DetalleIncumplimiento.Grano;

                            /*incumplidos*/
            listaDeElementosGrano = DetalleIncumplimiento.ListDetalleIncumplimiento;
            listaDominioEImagenDelGrano = listaDeElementosGrano
                .Select(y =>
                                new DominioEImagen
                                {
                                    Dominio = y.Fecha.ToString("dd/MM"),
                                    Imagen = y.Perdidos.ToString()
                                }
                    )
                    .ToList();
            incumplimientoDominioEImagenForMultiLine.ListOfDominioEImagen = listaDominioEImagenDelGrano;
                
                                /*cumplidos*/
            listaDominioEImagenDelGrano = listaDeElementosGrano
                    .Select(y =>
                                new DominioEImagen
                                {
                                    Dominio = y.Fecha.ToString("dd/MM"),
                                    Imagen = y.Cumplidos.ToString()
                                }
                    )
                    .ToList();
            cumplimientoDominioEImagenForMultiLine.ListOfDominioEImagen = listaDominioEImagenDelGrano;
                
                                /*otorgados*/
            listaDominioEImagenDelGrano = listaDeElementosGrano
                    .Select(y =>
                                new DominioEImagen
                                {
                                    Dominio = y.Fecha.ToString("dd/MM"),
                                    Imagen = y.Otorgados.ToString()
                                }
                    )
                    .ToList();
            otorgadoDominioEImagenForMultiLine.ListOfDominioEImagen = listaDominioEImagenDelGrano;

            listaDeDominioEImagenPorGrano.Add(otorgadoDominioEImagenForMultiLine);
            listaDeColoresParaLosGraficos.Add(System.Drawing.Color.Blue);
            listaDeDominioEImagenPorGrano.Add(cumplimientoDominioEImagenForMultiLine);
            listaDeColoresParaLosGraficos.Add(System.Drawing.Color.Green);
            listaDeDominioEImagenPorGrano.Add(incumplimientoDominioEImagenForMultiLine);
            listaDeColoresParaLosGraficos.Add(System.Drawing.Color.Red);       

            stream = constructorGraph.AddChart(listaDeDominioEImagenPorGrano, listaDeColoresParaLosGraficos, System.Web.UI.DataVisualization.Charting.SeriesChartType.Spline, granoStore.FindById(DetalleIncumplimiento.Grano.ToString(),session).Nombre);
            
            return stream;
        }

        /// <summary>
        /// genera un grafico con todos los incumplimientos (para cada grano) en un solo grafico
        /// </summary>
        /// <returns></returns>
        protected MemoryStream GraficoMultipleIncumplimientosPorGrano() 
        {
            var stream = new MemoryStream();
            IGranoStore granoStore = new GranoStore();
            IList<ICupo> listaDeElementosGrano = new List<ICupo>();
            IList<DominioEImagenForMultiLine> listaDeDominioEImagenPorGrano = new List<DominioEImagenForMultiLine>();
            List<System.Drawing.Color> listaDeColores = new List<System.Drawing.Color>();
            IList<DominioEImagen> listaDominioEImagenDelGrano = new List<DominioEImagen>();
            EmailChart constructorGraph = new EmailChart(300, CalculaAnchoDeGraficoSegunPeriodoIngresado(), "Porcentaje de perdida de granos");

            /*genera un grafico con todos los incumplimientos (para cada grano) en un solo grafico*/
            listaDeColores.Add(System.Drawing.Color.Blue);

            foreach (MailIncumplimientoGrano c in this.ListIncumplimientoPorGrano)
            {
                DominioEImagenForMultiLine DeDominioEImagenForMultiple = new DominioEImagenForMultiLine();
                DeDominioEImagenForMultiple.nombre = granoStore.FindById(c.Grano.ToString()).Nombre;
                DeDominioEImagenForMultiple.codigoNombre = c.Grano;
                listaDeElementosGrano = c.ListDetalleIncumplimiento;
                listaDominioEImagenDelGrano = listaDeElementosGrano
                        .Select(y =>
                                    new DominioEImagen
                                    {
                                        Dominio = y.Fecha.ToString("dd/MM/yyyy"),
                                        Imagen = y.PorcentajeDePerdida.ToString()
                                    }
                        )
                        .ToList();
                DeDominioEImagenForMultiple.ListOfDominioEImagen = listaDominioEImagenDelGrano;
                listaDeDominioEImagenPorGrano.Add(DeDominioEImagenForMultiple);
            }
            stream = constructorGraph.AddChart(listaDeDominioEImagenPorGrano, listaDeColores, System.Web.UI.DataVisualization.Charting.SeriesChartType.Spline);
            return stream;
        }

        public Cupos Consignacion
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Pdf
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IList<Cupos> CuposAInformar
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}