using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace IMAS.CupCake.Extensions
{   /// <summary>
    /// 通用对象的方法扩展
    /// 序列化，反序列化
    /// </summary>
    public static class ObjectExtenssions
    {
        /// <summary>
        /// 序列化反序列化配置，忽略默认值，忽略空值，减小持久化大小
        /// </summary>
        public static readonly JsonSerializerSettings SerializeSetting;

        /// <summary>
        /// 序列化反序列化配置，保留默认值，忽略空值，减小持久化大小
        /// </summary>
        public static readonly JsonSerializerSettings SerializeSetting2;

        /// <summary>
        /// 
        /// </summary>
        public static readonly JsonSerializer JsonSerializer;


        static ObjectExtenssions()
        {
            SerializeSetting = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                ContractResolver = new IPAddressConverterContractResolver()
            };
            SerializeSetting.Error += (sender, args) =>
            {
                Console.WriteLine(args.ErrorContext.Error.FullMessage());
            };
            SerializeSetting.DateFormatString = "yyyy-MM-dd H:mm:ss";
            JsonSerializer = JsonSerializer.Create(SerializeSetting);

            SerializeSetting2 = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new IPAddressConverterContractResolver()
            };
            SerializeSetting2.Error += (sender, args) =>
            {
                Console.WriteLine(args.ErrorContext.Error.FullMessage());
            };
            SerializeSetting2.DateFormatString = "yyyy-MM-dd H:mm:ss";
        }

        /// <summary>
        /// 深复制，简单采用Json序列化再反序列化的方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeepClone<T>(this T data)
        {
            try
            {
                var str = data.ToJson();
                return str.ToObject<T>();
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 深复制，简单采用Json序列化再反序列化的方式,TSource不能是列表类型
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TTarget">目标类型</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget ConvertTo<TSource, TTarget>(this TSource source)
        {
            try
            {
                var sourceJObject = JObject.FromObject(source);
                return sourceJObject.ToObject<TTarget>();
            }
            catch (Exception)
            {
                return default(TTarget);
            }
        }

        /// <summary>
        /// 获取对象的Json序列化文本
        /// </summary>
        /// <param name="data">需要序列化的对象</param>
        /// <param name="pretty">是否美化格式</param>
        /// <param name="config">默认值配置</param>
        /// <returns></returns>
        public static string ToJson(this object data, bool pretty = false, IgnoreDefaultConfig config = IgnoreDefaultConfig.IgnoreDefaultValue)
        {
            if (data == null)
                return string.Empty;
            if (pretty)
            {
                return JsonConvert.SerializeObject(data, Formatting.Indented, config == IgnoreDefaultConfig.IgnoreDefaultValue ? SerializeSetting : SerializeSetting2);
            }
            return JsonConvert.SerializeObject(data, Formatting.None, config == IgnoreDefaultConfig.IgnoreDefaultValue ? SerializeSetting : SerializeSetting2);
        }

        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">取字符串</param>
        /// <returns></returns>
        public static T ToObject<T>(this object data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.ToString()))
                return default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(data.ToString(), SerializeSetting);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToObject(this object data, Type type)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.ToString()))
                return data;
            return JsonConvert.DeserializeObject(data.ToString(), type, SerializeSetting);
        }

        #region Object转换

        public static string ToStr(this object data)
        {
            if (data == null)
                return string.Empty;
            return data.ToString();
        }

        /// <summary>
        /// Convert string to int32.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int ToInt32(this object source, int defaultValue = 0)
        {
            if (source == null)
                return defaultValue;

            int value;

            if (!int.TryParse(source.ToString(), out value))
                value = defaultValue;

            return value;
        }
        /// <summary>
        /// Convert string to long.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static long ToLong(this object source, long defaultValue = 0)
        {
            if (source == null)
                return defaultValue;

            long value;

            if (!long.TryParse(source.ToString(), out value))
                value = defaultValue;

            return value;
        }
        /// <summary>
        /// Convert string to long.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static long? ToNullLong(this object source)
        {
            if (source == null)
                return null;

            long value;

            if (!long.TryParse(source.ToString(), out value))
                return null;

            return value;
        }


        /// <summary>
        /// Convert string to short.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static short ToShort(this object source, short defaultValue = 0)
        {
            if (source == null)
                return defaultValue;

            short value;

            if (!short.TryParse(source.ToString(), out value))
                value = defaultValue;

            return value;
        }

        /// <summary>
        /// Convert string to decimal.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object source, decimal defaultValue = 0)
        {
            if (source == null)
                return defaultValue;

            decimal value;

            if (!decimal.TryParse(source.ToString(), out value))
                value = defaultValue;

            return value;
        }

        /// <summary>
        /// Convert string to double.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static double ToDouble(this object source, double defaultValue = 0)
        {
            if (source == null)
                return defaultValue;

            double value;

            if (!double.TryParse(source.ToString(), out value))
                value = defaultValue;

            return value;
        }

        /// <summary>
        /// Convert string to date time.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object source)
        {
            return source.ToDateTime(DateTime.MinValue);
        }

        /// <summary>
        /// Convert string to date time.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object source, DateTime defaultValue)
        {
            if (source == null)
                return defaultValue;

            DateTime value;

            if (!DateTime.TryParse(source.ToString(), out value))
                value = defaultValue;

            return value;
        }

        /// <summary>
        /// Convert string tp boolean.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public static bool ToBoolean(this object source, bool defaultValue = false)
        {
            if (source == null)
                return defaultValue;

            bool value;

            if (!bool.TryParse(source.ToString(), out value))
                value = defaultValue;

            return value;
        }
        #endregion
    }

    /// <summary>
    /// 默认值忽略和包含的配置项
    /// </summary>
    public enum IgnoreDefaultConfig
    {
        /// <summary>
        /// 忽略默认值
        /// </summary>
        IgnoreDefaultValue = 1,
        /// <summary>
        /// 包含默认值
        /// </summary>
        IncludeDefaultValue = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public class IPAddressConverterContractResolver : DefaultContractResolver
    {
        public static readonly IPAddressConverterContractResolver Instance = new IPAddressConverterContractResolver();

        protected override JsonContract CreateContract(Type objectType)
        {
            JsonContract contract = base.CreateContract(objectType);

            // this will only be called once and then cached
            if (objectType == typeof(IPAddress))
            {
                contract.Converter = new IpAddressJsonConverter();
            }

            return contract;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class IpAddressJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var ip = value as IPAddress;
            if (ip != null)
            {
                writer.WriteValue(ip.ToString());
            }
            else
            {
                writer.WriteNull();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            IPAddress ip;
            IPAddress.TryParse(reader.Value.ToString(), out ip);
            //IPAddress.TryParse(reader.ReadAsString(), out ip);
            return ip;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IPAddress);
        }
    }
}
