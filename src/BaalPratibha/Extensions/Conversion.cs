using System;
using System.Globalization;
using System.Threading;

namespace ResumeMaker.Common.Extensions
{
    public static class Conversions
    {
        /// <summary>
        /// Safely Converts a value to integer
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this object value)
        {
            var retVal = 0;

            if (value != null && value != DBNull.Value)
            {
                if (value is bool)
                {
                    if (Convert.ToBoolean(value, CultureInfo.InvariantCulture))
                    {
                        return 1;
                    }
                }
                string numberToParse = value.ToString();

                if (int.TryParse(numberToParse, out retVal))
                {
                    return retVal;
                }
            }

            return retVal;
        }

        public static double ToDouble(this object value)
        {
            double retVal = 0;

            if (value == null || value == DBNull.Value) return retVal;
            //string numberToParse = RemoveGroupping(value.ToString());
            string numberToParse = value.ToString();

            if (double.TryParse(numberToParse, out retVal))
            {
                return retVal;
            }

            return retVal;
        }

        public static short ToShort(this object value)
        {
            short retVal = 0;

            if (value != null && value != DBNull.Value)
            {
                string numberToParse = value.ToString();

                if (short.TryParse(numberToParse, out retVal))
                {
                    return retVal;
                }
            }

            return retVal;
        }

        public static long ToLong(this object value)
        {
            long retVal = 0;

            if (value != null && value != DBNull.Value)
            {
                //string numberToParse = RemoveGroupping(value.ToString());
                string numberToParse = value.ToString();

                if (long.TryParse(numberToParse, out retVal))
                {
                    return retVal;
                }
            }

            return retVal;
        }

        public static bool ToBool(this object value)
        {
            bool retVal = false;

            if (value != null && value != DBNull.Value)
            {
                if (value is string)
                {
                    if (value.ToString().ToLower().Equals("yes"))
                    {
                        return true;
                    }

                    if (value.ToString().ToLower().Equals("true"))
                    {
                        return true;
                    }
                }

                if (bool.TryParse(value.ToString(), out retVal))
                {
                    return retVal;
                }
            }

            return retVal;
        }

        public static decimal ToDecimal(this object value)
        {
            decimal retVal = 0;

            if (value == null || value == DBNull.Value) return retVal;
            var numberToParse = value.ToString();
            return decimal.TryParse(numberToParse, out retVal) ? retVal : retVal;
        }

        public static string ToUpperFirstLetterString(this object value)
        {
            if (value != null && value != DBNull.Value)
            {
                var str = value.ToString();
                if (!string.IsNullOrWhiteSpace(str))
                {
                    char[] letters = str.ToCharArray();
                    letters[0] = char.ToUpper(letters[0]);
                    return new string(letters);
                }
                return str;
            }
            return null;
        }

        public static DateTime ToDateTime(this object value)
        {
            try
            {
                if (value == DBNull.Value)
                {
                    return DateTime.MinValue;
                }

                return Convert.ToDateTime(value);
            }
            catch (FormatException)
            {
                //swallow the exception
            }
            catch (InvalidCastException)
            {
                //swallow the exception
            }

            return DateTime.MinValue;
        }

        public static string ToText(this object value)
        {
            try
            {
                if (value != null && value != DBNull.Value)
                {
                    if (value is bool)
                    {
                        if (Convert.ToBoolean(value, CultureInfo.InvariantCulture))
                        {
                            return "true";
                        }

                        return "false";
                    }

                    if (value == DBNull.Value)
                    {
                        return string.Empty;
                    }

                    string retVal = value.ToString();
                    return retVal;
                }

                return string.Empty;
            }
            catch (FormatException)
            {
                //swallow the exception
            }
            catch (InvalidCastException)
            {
                //swallow the exception
            }

            return string.Empty;
        }

        public static T ToEnum<T>(this object value)
        {
            if (!string.IsNullOrWhiteSpace(value?.ToString()))
            {
                var str = value.ToString();
                return (T)Enum.Parse(typeof(T), str, true);
            }
            return default(T);
        }


    }
}
