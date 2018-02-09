using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IMAS.CupCake.Extensions
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum | AttributeTargets.Class)]
    public class EnumDescription : Attribute
    {
        private readonly string _enumDisplayText;
        private readonly int _enumRank;
        private FieldInfo _fieldIno;

        /// <summary>
        /// 描述枚举值
        /// </summary>
        /// <param name="enumDisplayText">描述内容</param>
        /// <param name="enumRank">排列顺序</param>
        public EnumDescription(string enumDisplayText, int enumRank)
        {
            _enumDisplayText = enumDisplayText;
            _enumRank = enumRank;
        }

        /// <summary>
        /// 描述枚举值，默认排序为5
        /// </summary>
        /// <param name="enumDisplayText">描述内容</param>
        public EnumDescription(string enumDisplayText)
            : this(enumDisplayText, 5) { }

        public string EnumDisplayText
        {
            get { return _enumDisplayText; }
        }

        public int EnumRank
        {
            get { return _enumRank; }
        }

        public int EnumValue
        {
            get { return (int)_fieldIno.GetValue(null); }
        }

        public string FieldName
        {
            get { return _fieldIno.Name; }
        }

        #region  =========================================对枚举描述属性的解释相关函数

        /// <summary>
        /// 排序类型
        /// </summary>
        public enum SortType
        {
            /// <summary>
            ///按枚举顺序默认排序
            /// </summary>
            Default,
            /// <summary>
            /// 按描述值排序
            /// </summary>
            DisplayText,
            /// <summary>
            /// 按排序熵
            /// </summary>
            Rank
        }
        //silverlight不支持hashtable，这里改为字典
        private static readonly Dictionary<string, EnumDescription[]> CachedEnum = new Dictionary<string, EnumDescription[]>();

        /// <summary>
        /// 得到对枚举的描述文本
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// 
        /// <returns></returns>
        public static string GetEnumText(Type enumType)
        {
            var eds = (EnumDescription[])enumType.GetCustomAttributes(typeof(EnumDescription), false);
            return eds.Length != 1 ? string.Empty : eds[0].EnumDisplayText;
        }

        /// <summary>
        /// 获得指定枚举类型中，指定值的描述文本。
        /// </summary>
        /// <param name="enumValue">枚举值，不要作任何类型转换</param>
        /// <returns>描述字符串</returns>
        public static string GetFieldText(object enumValue)
        {
            if (enumValue == null)
            {
                return null;
            }
            var descriptions = GetFieldTexts(enumValue.GetType(), SortType.Default);
            foreach (var ed in descriptions.Where(ed => ed._fieldIno.Name == enumValue.ToString()))
            {
                return ed.EnumDisplayText;
            }
            return string.Empty;
        }


        /// <summary>
        /// 得到枚举类型定义的所有文本，按定义的顺序返回
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        /// <param name="enumType">枚举类型</param>
        /// <returns>所有定义的文本</returns>
        public static EnumDescription[] GetFieldTexts(Type enumType)
        {
            return GetFieldTexts(enumType, SortType.Default);
        }

        /// <summary>
        /// 得到枚举类型定义的所有文本
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        /// <param name="enumType">枚举类型</param>
        /// <param name="sortType">指定排序类型</param>
        /// <returns>所有定义的文本</returns>
        public static EnumDescription[] GetFieldTexts(Type enumType, SortType sortType)
        {
            EnumDescription[] descriptions = null;
            //缓存中没有找到，通过反射获得字段的描述信息
            if (enumType.FullName != null && CachedEnum.ContainsKey(enumType.FullName) == false)
            {
                var fields = enumType.GetFields();
                //ArrayList在silverlight没有该类，私有改用列表
                var edAl = new List<EnumDescription>();
                foreach (FieldInfo fi in fields)
                {
                    var eds = fi.GetCustomAttributes(typeof(EnumDescription), false);
                    if (eds.Length != 1) continue;
                    var description = eds[0] as EnumDescription;
                    if (description != null) description._fieldIno = fi;
                    edAl.Add(eds[0] as EnumDescription);
                }
                if (enumType.FullName != null) CachedEnum.Add(enumType.FullName, edAl.ToArray());
            }
            if (enumType.FullName != null) descriptions = CachedEnum[enumType.FullName];
            if (descriptions == null || descriptions.Length <= 0)
                throw new NotSupportedException("枚举类型[" + enumType.Name + "]未定义属性EnumValueDescription");

            //按指定的属性冒泡排序
            for (int m = 0; m < descriptions.Length; m++)
            {
                //默认就不排序了
                if (sortType == SortType.Default) break;

                for (int n = m; n < descriptions.Length; n++)
                {
                    var swap = false;
                    switch (sortType)
                    {
                        case SortType.Default:
                            break;
                        case SortType.DisplayText:
                            if (String.CompareOrdinal(descriptions[m].EnumDisplayText, descriptions[n].EnumDisplayText) > 0) swap = true;
                            break;
                        case SortType.Rank:
                            if (descriptions[m].EnumRank > descriptions[n].EnumRank) swap = true;
                            break;
                    }
                    if (swap)
                    {
                        var temp = descriptions[m];
                        descriptions[m] = descriptions[n];
                        descriptions[n] = temp;
                    }
                }
            }

            return descriptions;
        }

        #endregion
    }
}
