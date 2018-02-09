using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using IMAS.CupCake.Extensions;

namespace IMAS.CupCake.Configuration
{/// <summary>
 /// Json配置基类
 /// </summary>
    public abstract class JsonConfigBase
    {
        //private IJsonConfigDao _jsonConfigDao;
        /// <summary>
        /// 配置缓存
        /// </summary>
        protected IList<JObject> _configs = new List<JObject>();
        /// <summary>
        /// 本地缓存
        /// </summary>
        protected readonly Dictionary<string, JToken> _caches = new Dictionary<string, JToken>();

        // protected JsonConfigBase(IJsonConfigDao jsonConfigDao)
        //{
        //     _jsonConfigDao = jsonConfigDao;
        // }

        /// <summary>
        /// 获取配置的原始字节
        /// </summary>
        /// <param name="key">文件名（配置键）</param>
        /// <param name="base64"></param>
        /// <returns></returns>
        public byte[] GetConfigBytes(string key, bool base64 = true)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            var config = GetConfig(key);
            if (config == default(JToken))
            {
                return null;
            }

            if (base64)
            {
                return Convert.FromBase64String((string)config);
            }
            return Encoding.UTF8.GetBytes((string)config);
        }

        /// <summary>
        /// 获取配置的原始字节
        /// </summary>
        /// <param name="key">文件名（配置键）</param>
        /// <returns></returns>
        public string GetConfigString(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            var config = GetConfig(key);
            if (config == default(JToken))
            {
                return string.Empty;
            }
            return (string)config;
        }

        /// <summary>
        /// 根据Key获取配置对象
        /// </summary>
        /// <typeparam name="T">配置对象类型</typeparam>
        /// <param name="key">配置键</param>
        /// <returns></returns>
        public T GetConfigObject<T>(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            var config = GetConfig(key);
            if (config == default(JToken))
            {
                return default(T);
            }
            return config.ToObjectEx<T>();
        }

        /// <summary>
        /// 根据key获取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JToken GetConfig(string key)
        {
            if (_caches.ContainsKey(key))
            {
                return _caches[key];
            }

            foreach (var jObject in _configs)
            {
                var config = jObject.SelectToken(key);
                if (config != default(JToken))
                {
                    _caches[key] = config;
                    return config;
                }
            }

            return default(JToken);
        }

    }
}