using IMAS.LocalDBManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.Tips.Logic.Accountings
{
    /// <summary>
    /// 事件触发器(权重控制)
    /// </summary>
    public class ACC_EventWeight
    {
        /// <summary>
        /// 随机触发事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<T> GetRandomList<T>(List<T> list, int count) where T : Model_Weight
        {
            if (list == null || list.Count <= count || count <= 0)
            {
                return list;
            }
            var totalWeights = 0;
            for (int i = 0; i < list.Count; i++)
            {
                totalWeights += list[i].PickWeight + 1;
            }
            //随机种子,防止快速频繁调用导致随机一样的问题
            var ran = new Random(GetRandomSeed());
            //第一个int 为list下标索引.第一个int为权重排序值
            var wList = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < list.Count; i++)
            {
                // （权重+1） + 从0到（总权重-1）的随机数
                var w = (list[i].PickWeight + 1) + ran.Next(0, totalWeights);
                wList.Add(new KeyValuePair<int, int>(i, w));
            }

            //排序
            wList.Sort(delegate (KeyValuePair<int, int> kvp1, KeyValuePair<int, int> kvp2)
            {
                return kvp2.Value - kvp1.Value;
            });

            //根据实际情况取排在最前面的几个
            var newList = new List<T>();
            for (int i = 0; i < count; i++)
            {
                T entiy = list[wList[i].Key];
                newList.Add(entiy);
            }
            //随机法则
            return newList;
        }

        /// <summary>
        /// 随机种子值
        /// </summary>
        /// <returns></returns>
        private static int GetRandomSeed()
        {
            var bytes = new byte[4];
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
