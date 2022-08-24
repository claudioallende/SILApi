using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;

namespace ResourceServer.Models.Email
{
    public class EmailChart
    {
        protected int width {get;set;}
        protected int height {get;set;}
        protected string Titulo {get;set;}


        public EmailChart(int xalto, int xancho, string xtitulo) 
        {
            this.width = xancho;
            this.height = xalto;
            this.Titulo = xtitulo;
        }

        public MemoryStream AddChart(IList<DominioEImagenForMultiLine> ListDominioEImagen, IList<System.Drawing.Color> ListColorForGraph, SeriesChartType tipoDeGrafico, string tituloDeGrafico = "")
        {
            var stream = new MemoryStream();
            var chart = new System.Web.UI.DataVisualization.Charting.Chart();
            chart.Width = this.width;
            chart.Height = this.height;
            chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BackSecondaryColor = Color.White;
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.FromArgb(26, 59, 105);
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            if (tituloDeGrafico != null)
            {
                chart.Titles.Add(CreateTitle(tituloDeGrafico));
            }
            else {
                chart.Titles.Add(CreateTitle(this.Titulo));
            }
            chart.Legends.Add(CreateLegend());
            if (ListDominioEImagen.Count == 1) 
            {
                chart.Series.Add(CreateSeries(tipoDeGrafico, ListDominioEImagen,"Porcentaje de Turnos no Usados"));
            }
            if (ListDominioEImagen.Count > 1)
            {
                IList<Series> listOfSeries = CreateSeriesForMultiLine(tipoDeGrafico, ListDominioEImagen, ListColorForGraph);
                foreach (Series c in listOfSeries) 
                {
                    chart.Series.Add(c);
                }
            }
            chart.ChartAreas.Add(CreateChartArea());
            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms, ChartImageFormat.Png);
            return ms;
            //return File(ms.GetBuffer(), @"image/png");
        }

        private Title CreateTitle(string texto) 
        {
            Title titulo = new Title
            {
                Text = texto,
                ShadowColor = Color.FromArgb(32, 0, 0, 0),
                Font = new Font("Trebuchet MS", 14F, FontStyle.Bold),
                ShadowOffset = 3,
                ForeColor = Color.FromArgb(26, 59, 105)
            };
            return titulo;
        }

        /// <summary>
        /// Genera un listado de Series con el tipo de grafico indicado.
        /// Esto para permitir graficar varios graficos sobre los mismos ejes
        /// </summary>
        /// <param name="chartType"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public IList<Series> CreateSeriesForMultiLine(SeriesChartType chartType, IList<DominioEImagenForMultiLine> list, IList<System.Drawing.Color> ListColorForGraph)
        {
            IList<Series> listaDeSeriesResult = new List<Series>();
            List<string> nombres = list.Select(x => x.nombre).ToList();
            int element = 0;
            foreach (string name in nombres)
            {                
                List<DominioEImagenForMultiLine> elemento = list.Where(y => y.nombre == name).ToList();
                /*sabemos que tenemos un elemento por grano*/
                string[] Xs = elemento[0].ListOfDominioEImagen
                                                            .Select(x => x.Dominio)
                                                            .ToArray();
                double[] Ys = elemento[0].ListOfDominioEImagen
                                                            .Select(x => double.Parse(x.Imagen))
                                                            .ToArray();
                //Add Series to the Chart.
                Series serie = new Series();
                serie.Name = name;
                serie.IsValueShownAsLabel = false;
                serie.BorderWidth = 2;
                //serie.Color = Color.FromArgb(198, 99, 99);
                serie.Color = ListColorForGraph[element];
                serie.ChartType = chartType;
                serie.Points.DataBindXY(Xs, Ys);
                listaDeSeriesResult.Add(serie);
                element++;
            }
            return listaDeSeriesResult;
        }

        /// <summary>
        ///grafica la funcion para una sola entrada
        /// </summary>
        /// <param name="chartType"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public Series CreateSeries(SeriesChartType chartType, IList<DominioEImagenForMultiLine> list, string nombreDeSerie)
        {
            var series = new Series
            {
                Name = nombreDeSerie,
                IsValueShownAsLabel = true,
                Color = Color.FromArgb(198, 99, 99),
                ChartType = chartType,
                BorderWidth = 4
            };
            var granoName = list[0].nombre;
            foreach (var item in list[0].ListOfDominioEImagen)
            {
                var point = new DataPoint
                {
                    AxisLabel = item.Dominio,
                    YValues = new double[] { double.Parse(item.Imagen) }
                };
                series.Points.Add(point);
            }
            return series;
        }

        public ChartArea CreateChartArea()
        {
            var chartArea = new ChartArea();
            chartArea.Name = "GDP Current Prices in Billion($)";
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font = new Font("Verdana,Arial,Helvetica,sans-serif", 8F, FontStyle.Regular);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Interval = 1;
            return chartArea;
        }

        public Legend CreateLegend()
        {
            var legend = new Legend
            {
                Name = "GDP Current Prices in Billion($)",
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                BackColor = Color.Transparent,
                Font = new Font(new FontFamily("Trebuchet MS"), 9),
                LegendStyle = LegendStyle.Row
            };
            return legend;
        }
    }
}