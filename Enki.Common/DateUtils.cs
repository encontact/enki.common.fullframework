using System;

namespace Enki.Common
{
    public class DateUtils
    {
        private static DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converte a data atual para UNIX Timestamp
        /// </summary>
        /// <returns>Valor numério representando os segundos transcorridos desde 01/01/1970 até a data informada.</returns>
        public static long GetUnixEpochNow()
        {
            return Convert.ToInt64((DateTime.UtcNow - _epoch).TotalSeconds);
        }

        public static DateTime FromUnixEpoch(double milliseconds)
        {
            return _epoch.AddMilliseconds(milliseconds);
        }

        /// <summary>
        /// Recupera um System.DateTime para UNIX Timestamp
        /// </summary>
        /// <param name="value">Data a ser convertida</param>
        /// <returns>Valor numério representando os segundos transcorridos desde 01/01/1970 até a data informada.</returns>
        public static long GetUnixEpoch(DateTime value)
        {
            return Convert.ToInt64((value - _epoch).TotalSeconds);
        }

        public static string GetTotalHours(TimeSpan time)
        {
            var hours = time.TotalHours;
            var hoursString = "";
            if (hours < 10) hoursString = "0" + time.Hours;
            else hoursString = ((time.Days * 24) + time.Hours).ToString();
            return hoursString + ":" + time.Minutes.ToString("00") + ":" + time.Seconds.ToString("00");
        }
    }
}