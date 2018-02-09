using System;
using System.Collections.Generic;
using System.Linq;

namespace IMAS.CupCake.Extensions
{  /// <summary>
   /// 集合扩展方法
   /// </summary>
    public static class CollectionExtensions
    {

        /// <summary>
        /// 集合是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool NullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }
        /// <summary>
        /// 集合非空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool NotNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection != null && collection.Count > 0;
        }
        /// <summary>
        /// 集合是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool NullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }
        /// <summary>
        /// 集合非空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool NotNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<long> Range(long start, int count)
        {
            var retList = new List<long>();
            for (int i = 0; i < count; i++)
            {
                retList[i] = start + i;
            }
            return retList;
        }

        /// <summary>
        /// 遍历集合执行，List`T 实例有该方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null || action == null)
            {
                return;
            }

            foreach (var item in source)
            {
                action(item);
            }
        }
    }

}