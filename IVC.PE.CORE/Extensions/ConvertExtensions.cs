using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using TimeZoneConverter;

namespace IVC.PE.CORE.Helpers
{
    public static class ConvertExtensions
    {
        public static DateTime ToUtcDateTime(this string dateString)
        {
            return DateTime.ParseExact(dateString, ConstantHelpers.Format.DATE, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime();
        }

        public static DateTime ToDefaultTimeZone(this DateTime dateTime)
        {
            var timeZoneID = ConstantHelpers.TimeZone.DEFAULT_ID;
            return TimeZoneInfo.ConvertTimeFromUtc(dateTime, TZConvert.GetTimeZoneInfo(timeZoneID));
        }

        public static string ToDateFormat(this DateTime dateTime)
        {
            return dateTime.ToString(ConstantHelpers.Format.DATE, CultureInfo.InvariantCulture);
        }

        public static string ToDateTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToString(ConstantHelpers.Format.DATETIME, CultureInfo.InvariantCulture);
        }

        public static string ToTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToString(ConstantHelpers.Format.TIME, CultureInfo.InvariantCulture);
        }

        public static string ToLocalDateFormat(this DateTime dateTime)
        {
            return dateTime.ToDefaultTimeZone().ToString(ConstantHelpers.Format.DATE, CultureInfo.InvariantCulture);
        }

        public static string ToLocalDateTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToDefaultTimeZone().ToString(ConstantHelpers.Format.DATETIME, CultureInfo.InvariantCulture);
        }

        public static string ToLocalTimeFormat(this DateTime dateTime)
        {
            return dateTime.ToDefaultTimeZone().ToString(ConstantHelpers.Format.TIME, CultureInfo.InvariantCulture);
        }

        public static string RemoveAccentMarks(this string originalString)
        {
            return Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(originalString));
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("dd") + "/" + dateTime.ToString("MM") + "/" + dateTime.ToString("yyyy");
        }

        public static string ToTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("HH") + ":" + dateTime.ToString("mm");
        }

        public static string ToDateTimeString(this DateTime dateTime)
        {
            return $"{dateTime.ToDateString()} {dateTime.ToTimeString()}";
        }

        public static DateTime ToDateTime(this string originalString)
        {
            var splitString = originalString.Split("/");
            return new DateTime(int.Parse(splitString[2]), int.Parse(splitString[1]), int.Parse(splitString[0]));
        }

        public static int ToMonth(this string originalString)
        {
            var splitString = originalString.Split("/");
            return int.Parse(splitString[1]);
        }

        public static int ToYear(this string originalString)
        {
            var splitString = originalString.Split("/");
            return int.Parse(splitString[2]);
        }

        public static double ToDoubleString(this string originalString)
        {
            if (originalString == null || originalString == "")
                return 0.0;
            else if (originalString.Contains("."))
            {
                var splitString = originalString.Split(".");
                var entero = double.Parse(splitString[0]);
                var decimals = double.Parse(splitString[1]);
                var negativo = false;
                if (entero < 0)
                    negativo = true;

                var result = entero + (decimals / 100);
                if (negativo == false)
                {
                    if (splitString[1].StartsWith("00") && decimals < 10)
                        result = entero + (decimals / 1000);

                    if (!splitString[1].StartsWith("0") && decimals < 10)
                        result = entero + (decimals / 10);

                    if (splitString[1].StartsWith("0") && decimals > 10)
                        result = entero + (decimals / 1000);

                    if (decimals > 99)
                        result = entero + (decimals / 1000);
                } else
                {
                    result = entero - (decimals / 100);
                    if (splitString[1].StartsWith("00") && decimals < 10)
                        result = entero - (decimals / 1000);

                    if (!splitString[1].StartsWith("0") && decimals < 10)
                        result = entero - (decimals / 10);

                    if (splitString[1].StartsWith("0") && decimals > 10)
                        result = entero - (decimals / 1000);

                    if (decimals > 99)
                        result = entero - (decimals / 1000);
                }

                return result;
            }else if (originalString.Contains(","))
            {
                var splitString = originalString.Split(",");
                var entero = double.Parse(splitString[0]);
                var decimals = double.Parse(splitString[1]);
                var negativo = false;
                if (entero < 0)
                    negativo = true;

                var result = entero + (decimals / 100);
                if (negativo == false)
                {
                    if (splitString[1].StartsWith("00") && decimals < 10)
                        result = entero + (decimals / 1000);

                    if (!splitString[1].StartsWith("0") && decimals < 10)
                        result = entero + (decimals / 10);

                    if (splitString[1].StartsWith("0") && decimals > 10)
                        result = entero + (decimals / 1000);

                    if (decimals > 99)
                        result = entero + (decimals / 1000);
                } else
                {
                    if (splitString[1].StartsWith("00") && decimals < 10)
                        result = entero - (decimals / 1000);

                    if (!splitString[1].StartsWith("0") && decimals < 10)
                        result = entero - (decimals / 10);

                    if (splitString[1].StartsWith("0") && decimals > 10)
                        result = entero - (decimals / 1000);

                    if (decimals > 99)
                        result = entero - (decimals / 1000);
                }

                return result;
            }
            else
            {
                var result = double.Parse(originalString);
                return result;
            }
        }

        public static string ToFileNameString(this string fileName)
        {
            var splitString = fileName.Split(".");
            return splitString[0];
        }
        public static string ToStringExcel(this int value)
        {
            string Num2Text = "";
            if (value < 0) return "";

            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + (value - 10).ToStringExcel();
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + (value - 20).ToStringExcel();
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100)
            {
                int u = value % 10;
                Num2Text = string.Format("{0} y {1}", ((value / 10) * 10).ToStringExcel(), (u == 1 ? "UN" : (value % 10).ToStringExcel()));
            }
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + (value - 100).ToStringExcel();
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800))
                Num2Text = ((value / 100)).ToStringExcel() + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = string.Format("{0} {1}", ((value / 100) * 100).ToStringExcel(), (value % 100).ToStringExcel());
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + (value % 1000).ToStringExcel();
            else if (value < 1000000)
            {
                Num2Text = ((value / 1000)).ToStringExcel() + " MIL";
                if ((value % 1000) > 0) Num2Text += " " + (value % 1000).ToStringExcel();
            }
            else if (value == 1000000) Num2Text = "UN MILLÓN";
            else if (value < 2000000) Num2Text = "UN MILLÓN " + (value % 1000000).ToStringExcel();
            else if (value < int.MaxValue)
            {
                Num2Text = ((value / 1000000)).ToStringExcel() + " MILLONES        ";
                if ((value - (value / 1000000) * 1000000) > 0) Num2Text += " " + (value - (value / 1000000) * 1000000).ToStringExcel();
            }
            return Num2Text;
        }
    }
}
