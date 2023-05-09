using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.APP.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString("dd") + "/" + dateTime.ToString("MM") + "/" + dateTime.ToString("yyyy");
        }

        public static DateTime ToDateTime(this string originalString)
        {
            var splitString = originalString.Split('/');
            return new DateTime(int.Parse(splitString[2]), int.Parse(splitString[1]), int.Parse(splitString[0]));
        }
    }
}
