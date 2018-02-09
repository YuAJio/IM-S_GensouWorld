using System;

namespace IMAS.CupCake.Extensions
{  /// <summary>
   /// 
   /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 一周的开始时间
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DateTime WeekStartTime(this DateTime data)
        {
            var dayOfWeek = (int)data.DayOfWeek;
            var ajust = (dayOfWeek == 0) ? 6 : dayOfWeek - 1;
            return data.Date.AddDays(-ajust);
        }

        /// <summary>
        /// 一周的周五时间
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DateTime WeekFridayTime(this DateTime data)
        {

            return data.WeekStartTime().AddDays(4);
        }

        /// <summary>
        /// 一月的开始时间
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DateTime MonthStartTime(this DateTime data)
        {
            return new DateTime(data.Year, data.Month, 1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToLocalTimeStr(this DateTime dateTime)
        {
            return dateTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 比较时间大小
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="dtTarget"></param>
        /// <returns></returns>
        public static bool Gt(this DateTime dtSource, DateTime dtTarget)
        {
            return dtSource.ToLocalTime() > dtTarget.ToLocalTime();
        }

        /// <summary>
        /// 计算年龄
        /// </summary>
        /// <param name="birthDay"></param>
        /// <returns></returns>
        public static int CalAge(this DateTime birthDay)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - birthDay.Year;
            if (birthDay > now.AddYears(-age))
                age--;
            return age;
        }

    }
}