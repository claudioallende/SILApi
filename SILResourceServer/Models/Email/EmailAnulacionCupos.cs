using NHibernate;
using ResourceServer.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ResourceServer.Models.Email
{
    public class EmailAnulacionCupos : ServiceEmail
    {
        private IList<Cupos> ListaCupos { get; set; }
        protected long CuentaComprador { get; set; }
        protected long CuentaVendedor { get; set; }
        protected int CodigoGrano { get; set; }
        protected long CuentaPuerto { get; set; }
        protected Consignacion Consignacion { get; set; }
        protected DateTime InicioSemana { get; set; }
        protected DateTime FinSemana { get; set; }
        private CuposStore Store { get; set; }

        private string Puerto { get; set; }
        private string Grano { get; set; }
        private string NroPlanta { get; set; }

        protected ISession Session { get; set; }

        public EmailAnulacionCupos(IList<Cupos> ListaCupos, ISession Session)
        {
            this.ListaCupos = ListaCupos;

            DateTime FechaMasChica = ListaCupos.OrderBy(x => x.Fecha).FirstOrDefault().Fecha;
            DateTime FechaMasGrande = ListaCupos.OrderByDescending(x => x.Fecha).FirstOrDefault().Fecha;
            //Utiliza la Clase DateTimeExtensions
            this.InicioSemana = FechaMasChica.StartOfWeek(DayOfWeek.Monday);
            this.FinSemana = FechaMasGrande.EndOfWeek(DayOfWeek.Sunday);

            this.CuentaComprador = ListaCupos.ElementAt(0).Compcta;
            this.CuentaVendedor = ListaCupos.ElementAt(0).Vendcta;
            this.CodigoGrano = ListaCupos.ElementAt(0).Grano;
            this.CuentaPuerto = ListaCupos.ElementAt(0).Puerto;
            this.Consignacion = ListaCupos.ElementAt(0).GetConsignacion();

            this.Session = Session;
        }
        public EmailAnulacionCupos(IList<Cupos> ListaCupos, long CuentaComprador, long CuentaVendedor, int CodigoGrano, long CuentaPuerto, Consignacion Consignacion, ISession Session)
        {
            this.ListaCupos = ListaCupos;

            DateTime FechaMasChica = ListaCupos.OrderBy(x => x.Fecha).FirstOrDefault().Fecha;
            DateTime FechaMasGrande = ListaCupos.OrderByDescending(x => x.Fecha).FirstOrDefault().Fecha;
            //Utiliza la Clase DateTimeExtensions
            this.InicioSemana = FechaMasChica.StartOfWeek(DayOfWeek.Monday);
            this.FinSemana = FechaMasGrande.EndOfWeek(DayOfWeek.Sunday);

            this.CuentaComprador = CuentaComprador;
            this.CuentaVendedor = CuentaVendedor;
            this.CodigoGrano = CodigoGrano;
            this.CuentaPuerto = CuentaPuerto;
            this.Consignacion = Consignacion;

            this.Session = Session;
        }

        protected override System.Net.Mail.MailMessage GetMessage(System.Net.Mail.MailMessage msg)
        {
            msg.Subject = GetAsunto();
            msg.IsBodyHtml = true;
            msg.Body = GetCuerpo();
            return msg;
        }

        protected string GetAsunto()
        {
            return string.Format("Anulación turnos {0} para la semana del {1} a {2} producto {3} planta {4}", ListaCupos.Count, this.InicioSemana.ToString("dd/MM/yyyy"), this.FinSemana.ToString("dd/MM/yyyy"), GetGrano(this.Session), GetPuerto(this.Session));
            //return ;
        }

        protected string GetCuerpo()
        {
            StringBuilder Cuerpo = new StringBuilder("Estimados: <br/><br/>");
            StringBuilder Detalle = new StringBuilder("");
            StringBuilder PrevInformado = new StringBuilder("");
            StringBuilder Informado = new StringBuilder("");
            StringBuilder Totales = new StringBuilder("");
            ServicioCupo Servicio = new ServicioCupo();

            Cuerpo.Append(string.Format(@"Se han cancelado {0} turnos de entrega de {1} en la planta {2} para la semana comprendida entre el {3} y el {4}.<br>",
                ListaCupos.Count, GetGrano(Session), GetPuerto(Session), this.InicioSemana.ToString("dd/MM/yyyy"), this.FinSemana.ToString("dd/MM/yyyy")));
            Cuerpo.Append("A continuaci&oacute;n, existe una tabla, en donde informamos los turnos que fueron Anulados. Esta grilla tendr&aacute; informaci&oacute;n de turnos previamente informados y las Anulaciones que se han generado.<br/>");
            Cuerpo.Append("Los totales ser&aacute;n los turnos que para esta consignaci&oacute;n, tienen otorgados y pueden ser consumidos con sus cargas.<br><br>");

            if (Store == null) Store = new CuposStore();
            IList<Cupos> CuposInformados = Store.FindByCompradorAndVendedorAndGranoAndPuertoAndConsignacionAndInformadoAndDistribuidoBetweenDates
                (this.CuentaComprador, this.CuentaVendedor, this.CodigoGrano, this.CuentaPuerto, this.Consignacion, this.InicioSemana, this.FinSemana, this.Session);
            Cuerpo.Append(@"<div style='width:100%;text-align:center;'><table style='border-collapse: collapse;border-spacing: 0;'>");
            Cuerpo.Append(string.Format("<tr><td></td><td style='{0}' colspan='{1}'><b>Logística Semanal desde el {2} al {3}</b></td></tr>", EstilosEmail.TdCentradoNegrita + EstilosEmail.TdColorTitulo, (DateTimeExtensions.DatesBetween(this.InicioSemana, this.FinSemana).Count), this.InicioSemana.ToString("dd/MM"), this.FinSemana.ToString("dd/MM")));
            foreach (DateTime Fecha in DateTimeExtensions.DatesBetween(this.InicioSemana, this.FinSemana))
            {
                IEnumerable<Cupos> CuposAInformarDeFecha = ListaCupos.Where(x => x.Fecha.Date == Fecha.Date);
                IEnumerable<Cupos> CuposInformadosDeFecha = CuposInformados.Where(x => x.Fecha.Date == Fecha.Date);
                Detalle.Append(string.Format("<td style='{0}'><b>{1}</b></td>", EstilosEmail.TdCentradoNegrita + EstilosEmail.TdColorSubTitulo, Fecha.ToString("dd/MM/yyyy")));
                PrevInformado.Append(string.Format("<td style='{0}'>{1}</td>", EstilosEmail.TdCentrado, CuposInformadosDeFecha.Count() + CuposAInformarDeFecha.Count())); //Cupos que se están informando en esta linea ya estan anulados, por lo que no aparece como cupo informado, para ello le sumo el que estoy anulando.
                Informado.Append(string.Format("<td style='{0}'>{1}</td>", EstilosEmail.TdCentrado, CuposAInformarDeFecha.Count()));
                Totales.Append(string.Format("<td style='{0}'>{1}</td>", EstilosEmail.TdCentrado, (CuposInformadosDeFecha.Count())));
            }
            Cuerpo.Append(string.Format("<tr><td style='{0}'><b>Turnos</b></td>{1}</tr>", EstilosEmail.TdNegrita + EstilosEmail.TdColorSubTitulo, Detalle));
            Cuerpo.Append(string.Format("<tr><td style='{0}'><b>Informados</b></td>{1}</tr>", EstilosEmail.TdNegrita, PrevInformado));
            Cuerpo.Append(string.Format("<tr><td style='{0}'><b>Anulaciones Informadas</b></td>{1}</tr>", EstilosEmail.TdNegrita, Informado));
            Cuerpo.Append(string.Format("<tr><td style='{0}'><b>Totales</b></td>{1}</tr>", EstilosEmail.TdNegrita, Totales));
            Cuerpo.Append("</table></div><br>");

            Cuerpo.Append("Turnos Alfanum&eacute;ricos anulados por d&iacute;a.<br/><br/>");
            Cuerpo.Append(GetCuerpoAlfas(this.ListaCupos));

            Cuerpo.Append(@"Si existe alguna duda, rogamos comunicarse con los operadores log&iacute;sticos correspondientes a sus centros.<br>
                Saludos cordiales.");

            return Cuerpo.ToString();
        }

        private string GetCuerpoAlfas(IList<Cupos> TotalCupos)
        {
            StringBuilder builder = new StringBuilder();
            var AlfasPorFecha = TotalCupos.GroupBy(x => x.Fecha)
                .Select(x => new { Fecha = x.Key, Alfas = TotalCupos.Where(y => y.Fecha == x.Key).Select(y => y.Nrocupo).ToList() });

            foreach (var AlfasFecha in AlfasPorFecha)
            {
                builder.Append(GetCuerpoAlfasPorDia(AlfasFecha.Fecha, AlfasFecha.Alfas));
            }

            return builder.ToString();
        }

        private string GetCuerpoAlfasPorDia(DateTime Fecha, IList<string> AlfasFecha)
        {
            StringBuilder AlfasPorDia = new StringBuilder(string.Format("<b>Turnos {0}</b><br>", Fecha.ToString("dd/MM/yyyy")));
            //IList<Cupos> CuposDia = TotalCupos.Where(x => x.Fecha.Date == Fecha.Date).ToList();
            foreach (string Alfa in AlfasFecha)
            {
                AlfasPorDia.Append(string.Format("{0}<br>", Alfa));
            }
            AlfasPorDia.Append("<br>");
            return AlfasPorDia.ToString();
        }

        private string GetGrano(ISession Session)
        {
            if (string.IsNullOrEmpty(Grano))
            {
                IGranoStore GranoStore = new GranoStore();
                Grano = GranoStore.FindById(CodigoGrano.ToString(), Session).Nombre;
            }
            return Grano;
        }

        private string GetPuerto(ISession Session)
        {
            if (string.IsNullOrEmpty(Puerto))
            {
                IPuertoStore PuertoStore = new PuertoStore();
                Puerto = PuertoStore.FindById(CuentaPuerto, Session).Nombre;
            }
            return Puerto;
        }

        private string GetNroPlanta(ISession Session)
        {
            if (string.IsNullOrEmpty(NroPlanta))
            {
                IPuertoStore PuertoStore = new PuertoStore();
                NroPlanta = PuertoStore.FindNroPlantaByCuentaPuerto(CuentaPuerto, Session);
            }
            return NroPlanta;
        }
    }
}