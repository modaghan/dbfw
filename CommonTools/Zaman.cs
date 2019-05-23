using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class Zaman
    {
        public static DateTime Simdi
        {
            get
            {
                var info = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time");
                DateTimeOffset localServerTime = DateTimeOffset.Now;
                return TimeZoneInfo.ConvertTime(localServerTime, info).DateTime;
                //return DateTime.Now;
            }
        }
        public static bool isToday(this DateTime date)
        {
            DateTime simdi = Zaman.Simdi;
            return simdi.Year == date.Year && simdi.Month == date.Month && simdi.Day == date.Day;
        }
        public static bool isLaterOrToday(DateTime date)
        {
            DateTime pr = new DateTime(date.Year, date.Month, date.Day);
            DateTime nx = new DateTime(Simdi.Year, Simdi.Month, Simdi.Day);
            return pr.Ticks >= nx.Ticks;
        }

        public static bool isLaterOrNow(TimeSpan time)
        {
            DateTime timeDate = new DateTime(time.Ticks);
            DateTime pr = new DateTime(Simdi.Year, Simdi.Month, Simdi.Day, timeDate.Hour, timeDate.Minute, timeDate.Second);
            DateTime nx = new DateTime(Simdi.Year, Simdi.Month, Simdi.Day, Simdi.Hour, Simdi.Minute, Simdi.Second);
            return pr.Ticks >= nx.Ticks;
        }
        public static bool isLaterOrNow(DateTime timeDate)
        {
            DateTime pr = new DateTime(Simdi.Year, Simdi.Month, Simdi.Day, timeDate.Hour, timeDate.Minute, timeDate.Second);
            DateTime nx = new DateTime(Simdi.Year, Simdi.Month, Simdi.Day, Simdi.Hour, Simdi.Minute, Simdi.Second);
            return pr.Ticks >= nx.Ticks;
        }
        public static bool isSameMonth(DateTime date1, DateTime date2)
        {
            return date2.Year == date1.Year && date2.Month == date1.Month;
        }
        public static bool isSameDay(DateTime date1, DateTime date2)
        {
            return date2.Year == date1.Year && date2.Month == date1.Month && date2.Day == date1.Day;
        }

        /// <summary>
        /// Tarih ve Saati birleştirir.
        /// </summary>
        /// <param name="date">Tarih değeri</param>
        /// <param name="time">Saat değeri</param>
        /// <returns></returns>
        public static DateTime CombineDates(DateTime date, DateTime time)
        {
            if (date ==null || time == null)
                return Zaman.Simdi;
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }
    }
}
