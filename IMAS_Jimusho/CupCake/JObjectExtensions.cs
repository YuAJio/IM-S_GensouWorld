using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IMAS.CupCake.Extensions
{  /// <summary>
   /// NewtonJson JObject
   /// </summary>
    public static class JObjectExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static JObject FromObject(object data)
        {
            if (data == null)
            {
                return null;
            }
            try
            {
                return JObject.FromObject(data, ObjectExtenssions.JsonSerializer);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static JToken GetValue(this JObject source, string propertyName)
        {
            if (source == null)
            {
                return null;
            }
            JToken value;

            if (source.TryGetValue(propertyName, out value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetString(this JObject source, string propertyName)
        {
            if (source == null)
            {
                return string.Empty;
            }
            JToken value;

            if (!source.TryGetValue(propertyName, out value))
            {
                return string.Empty;
            }

            return value.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetInt32(this JObject source, string propertyName, int defaultValue = 0)
        {
            if (source == null)
            {
                return defaultValue;
            }
            JToken value;

            if (!source.TryGetValue(propertyName, out value))
            {
                return defaultValue;
            }

            return (int)value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long GetLong(this JObject source, string propertyName, long defaultValue = 0)
        {
            if (source == null)
            {
                return defaultValue;
            }
            JToken value;

            if (!source.TryGetValue(propertyName, out value))
            {
                return defaultValue;
            }

            return (long)value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static short GetShort(this JObject source, string propertyName, short defaultValue = 0)
        {
            if (source == null)
            {
                return defaultValue;
            }
            JToken value;

            if (!source.TryGetValue(propertyName, out value))
            {
                return defaultValue;
            }

            return (short)value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal GetDecimal(this JObject source, string propertyName, decimal defaultValue = 0)
        {
            if (source == null)
            {
                return defaultValue;
            }
            JToken value;

            if (!source.TryGetValue(propertyName, out value))
            {
                return defaultValue;
            }

            return (decimal)value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double GetDouble(this JObject source, string propertyName, double defaultValue = 0)
        {
            if (source == null)
            {
                return defaultValue;
            }
            JToken value;

            if (!source.TryGetValue(propertyName, out value))
            {
                return defaultValue;
            }

            return (double)value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(this JObject source, string propertyName)
        {
            return source.GetDateTime(propertyName, DateTime.MinValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(this JObject source, string propertyName, DateTime defaultValue)
        {
            if (source == null)
            {
                return defaultValue;
            }
            JToken value;

            if (!source.TryGetValue(propertyName, out value))
            {
                return defaultValue;
            }

            return (DateTime)value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetBoolean(this JObject source, string propertyName, bool defaultValue = false)
        {
            if (source == null)
            {
                return defaultValue;
            }
            JToken value;

            if (!source.TryGetValue(propertyName, out value))
            {
                return defaultValue;
            }

            return (bool)value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="propertyName"></param>
        /// <param name="jsonSerializer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetObject<T>(this JObject source, string propertyName, JsonSerializer jsonSerializer = null)
        {
            if (source == null)
                return default(T);
            JToken value;

            if (!source.TryGetValue(propertyName, out value))
            {
                return default(T);
            }

            return value.ToObject<T>(jsonSerializer ?? ObjectExtenssions.JsonSerializer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="jsonSerializer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToObjectEx<T>(this JObject source, JsonSerializer jsonSerializer = null)
        {
            if (source == null)
                return default(T);

            return source.ToObject<T>(jsonSerializer ?? ObjectExtenssions.JsonSerializer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="jsonSerializer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T ToObjectEx<T>(this JToken source, JsonSerializer jsonSerializer = null)
        {
            if (source == null)
                return default(T);

            return source.ToObject<T>(jsonSerializer ?? ObjectExtenssions.JsonSerializer);
        }

    }
}