using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public static class DateUtils
    {
        public static int n_date(DateTime? fecha)
        {
            if (fecha == null)
            {
                return 0;
            }
            else
            {
                DateTime tmp = fecha.Value;
                DateTime old = new DateTime(1800, 12, 28);
                return (tmp - old).Days;
            }
        }
        public static DateTime date_c(int fecha)
        {
            DateTime nuevaFecha = new DateTime(1800, 12, 28);
            if (fecha == 0 || fecha == null)
            {
                return nuevaFecha;
            }
            else
            {
                return nuevaFecha.AddDays(fecha);
            }
        }
        public static DateTime hora_c(long hora)
        {
            hora = hora - 1;
            var centecimas = hora % 100;
            hora = (Int64)Math.Truncate(Convert.ToDouble(hora) / 100.00); //centecimas
            var segundos = hora % 60;
            hora = (Int64)Math.Truncate(Convert.ToDouble(hora) / 60.00); //segundos
            var minutos = hora % 60;
            hora = (Int64)Math.Truncate(Convert.ToDouble(hora) / 60.00); //minutos
            var horas = hora % 24;
            DateTime nuevaHora = new DateTime(1, 1, 1, (Int32)horas, (Int32)minutos, 0);
            return nuevaHora;
        }
    }
}