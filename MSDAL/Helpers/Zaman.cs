using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS.BLL
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

        public static string GetMonthByName(int i)
        {
            i = i == 0 ? 1 : i;
            i = i == 13 ? 12 : i;
            return new DateTime(1, i, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("tr"));
        }
        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay, params DateTime[] bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }
    }
}
