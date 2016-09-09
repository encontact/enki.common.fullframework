using System;

namespace Enki.Common
{
    public class DateUtils
    {
        private static DateTime _utcEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static DateTime _localEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
        private static DateTime _unspecifiedEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

        /// <summary>
        /// Converte a data atual (UTC/GMT) para UNIX Timestamp
        /// </summary>
        /// <returns>Valor numério representando os segundos transcorridos desde 01/01/1970 até a data informada.</returns>
        [Obsolete("Change to use the function GetGMTUnixEpochNow().")]
        public static long GetUnixEpochNow()
        {
            return GetGMTUnixEpochNow();
        }

        /// <summary>
        /// Converte a data atual (UTC/GMT) para UNIX Timestamp
        /// </summary>
        /// <returns>Valor numério representando os segundos transcorridos desde 01/01/1970 até a data informada.</returns>
        public static long GetGMTUnixEpochNow()
        {
            return Convert.ToInt64((DateTime.UtcNow - _utcEpoch).TotalSeconds);
        }

        /// <summary>
        /// Converte a data atual para UNIX Timestamp
        /// </summary>
        /// <returns>Valor numério representando os segundos transcorridos desde 01/01/1970 até a data informada.</returns>
        public static long GetLocalUnixEpochNow()
        {
            return Convert.ToInt64((DateTime.Now - _localEpoch).TotalSeconds);
        }

        /// <summary>
        /// Recupera a Data GMT atualizada com o tempo informado.
        /// </summary>
        /// <param name="milliseconds">Tempo em milissegundos a serem adicionados ao EPOCH</param>
        /// <returns>Data resultante</returns>
        public static DateTime FromUnixEpoch(double milliseconds)
        {
            return _utcEpoch.AddMilliseconds(milliseconds);
        }

        /// <summary>
        /// Recupera um System.DateTime para UNIX Timestamp
        /// </summary>
        /// <param name="value">Data a ser convertida</param>
        /// <returns>Valor numério representando os segundos transcorridos desde 01/01/1970 até a data informada.</returns>
        public static long GetUnixEpoch(DateTime value, DateTimeKind kind)
        {
            switch (kind)
            {
                case DateTimeKind.Local:
                    return Convert.ToInt64((value - _utcEpoch).TotalSeconds);
                case DateTimeKind.Utc:
                    return Convert.ToInt64((value - _localEpoch).TotalSeconds);
                case DateTimeKind.Unspecified:
                    return Convert.ToInt64((value - _unspecifiedEpoch).TotalSeconds);
                default:
                    throw new Exception("Not implemented date kind.");
            }
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