using System;

namespace Light.Extensions
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Use this for check different minutes from Now to DateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="mins"></param>
        /// <returns></returns>
        public static bool IsNearlyInMinutes(this DateTime dateTime, int mins)
        {
            var diff = dateTime - DateTime.Now;
            return diff.TotalMinutes <= mins;
        }

        /// <summary>
        /// Use this for check different seconds from Now to DateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static bool IsNearlyInSeconds(this DateTime dateTime, int seconds)
        {
            var diff = dateTime - DateTime.Now;
            return diff.TotalSeconds <= seconds;
        }

        /// <summary>
        /// Convert DateTime to seconds Unix Timestamp
        /// </summary>
        public static long ToUnixTimeSeconds(this DateTime value)
        {
            var dateTimeOffset = new DateTimeOffset(value);
            return dateTimeOffset.ToUnixTimeSeconds();
        }

        /// <summary>
        /// Convert DateTime to milliseconds Unix Timestamp
        /// </summary>
        public static long ToUnixTimeMilliseconds(this DateTime value)
        {
            var dateTimeOffset = new DateTimeOffset(value);
            return dateTimeOffset.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// Convert seconds Unix Timestamp to DateTime 
        /// </summary>
        public static DateTime GetDateTimeFromSeconds(long value)
        {
            return new DateTime(1970, 1, 1).AddSeconds(value);
        }

        /// <summary>
        /// Convert milliseconds Unix Timestamp to DateTime 
        /// </summary>
        public static DateTime GetDateTimeFromMilliseconds(long value)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(value);
        }
    }
}
