using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResourceServer.Models
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            int diff = (7 + (endOfWeek - dt.DayOfWeek)) % 7;
            return dt.AddDays(diff).Date;
        }

        public static IList<DateTime> DatesBetween(DateTime Start, DateTime End)
        {
            IList<DateTime> dates = new List<DateTime>();

            for (var dt = Start; dt <= End; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            return dates;
        }
    }
}