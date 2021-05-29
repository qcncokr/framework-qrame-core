using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qrame.CoreFX.ExtensionMethod
{
    public static class DayOfWeekExtensions
    {
        public static DateTime FindNthWeekDayOfMonth(this DayOfWeek @this, int year, int month, int n)
        {
            if (n < 1 || n > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            var y = 0;

            var daysOfMonth = DateTimeExtensions.DaysOfMonth(year, month);
            
            var totalInstances = @this.TotalInstancesInMonth(year, month);
            if (n == 5 && n > totalInstances)
                n = 4;

            var foundDate = daysOfMonth
                .Where(date => @this.Equals(date.DayOfWeek))
                .OrderBy(date => date)
                .Select(x => new { n = ++y, date = x })
                .Where(x => x.n.Equals(n)).Select(x => x.date).First();

            return foundDate;
        }
        
        public static int TotalInstancesInMonth(this DayOfWeek @this, int year, int month)
        {
            return DateTimeExtensions.DaysOfMonth(year, month).Count(date => @this.Equals(date.DayOfWeek));
        }
        
        public static int TotalInstancesInMonth(this DayOfWeek @this, DateTime dateTime)
        {
            return @this.TotalInstancesInMonth(dateTime.Year, dateTime.Month);
        }
    }
}
