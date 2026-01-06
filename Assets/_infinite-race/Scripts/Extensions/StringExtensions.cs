using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Southbyte
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmptyOrWhiteSpace(this string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }
        
        /// <summary>
        /// Converts a number into a display string in the form of 123, 12.3k, 12.3M etc. Values are rounded down.
        /// </summary>
        public static string To1kString(this int number)
        {
            var abs = Mathf.Abs(number);
            
            if (abs < 1000)
                return number.ToString();
            
            if (abs < 1000000)
                return ToFloorWithOneDecimalPoint(number, 1000, "k");
            
            if (abs < 1000000000)
                return ToFloorWithOneDecimalPoint(number, 1000000, "M");
            
            return ToFloorWithOneDecimalPoint(number, 1000000000, "B");
        }
        
        public static bool ContainsIgnoreCase(this IEnumerable<string> collection, string element)
        {
            if(collection.IsNullOrEmpty())
                return false;
            
            foreach (var arrayElement in collection)
            {
                if(string.Equals(arrayElement, element, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            
            return false;
        }
        
        
        private static string ToFloorWithOneDecimalPoint(int number, int divisor, string suffix)
        {
            return $"{(number / divisor).ToString()}.{(Mathf.Abs(number / (divisor / 10)) % 10).ToString()}{suffix}";
        }
    }
}