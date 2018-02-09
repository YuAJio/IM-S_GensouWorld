using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util.Regex;


namespace IdoMaster_GensouWorld.Utils
{
    public class GalTextUtils
    {
        private const string TAG = "GalTextUtils";

        public static List<string> GetContentList(string content)
        {
            var list = new List<string>();
            string s = "";
            string n = "";
            int sn = 0;
            content += "";
            int len = content.Length;

            for (int i = 0; i < len; i++)
            {
                //  char c =content.Ch
                var c = char.Parse(content.Substring(i, 1));
                var c_1 = ' ';
                if (i >= 1)
                {
                    c_1 = char.Parse(content.Substring(i - 1, 1));
                }
                if (IsEnglish(c + "") || (IsEnglish(c_1 + "") && (c + "").Equals("`")))
                {
                    s += c;
                    sn = 1;
                }
                else if (IsNumber(c + "") || (IsNumber(c_1 + "") && (c + "").Equals(".")))
                {
                    n += c;
                    sn = 2;
                }
                else
                {
                    if (!s.Equals("") && !n.Equals("") & sn == 1)
                    {
                        list.Add(n + s);
                    }
                    else if (!s.Equals("") && !n.Equals("") & sn == 2)
                    {
                        list.Add(s + n);
                    }
                    else if (!s.Equals("") && n.Equals(""))
                    {
                        list.Add(s);
                    }
                    else if (!n.Equals("") && s.Equals(""))
                    {
                        list.Add(n);
                    }
                    list.Add(c + "");
                    sn = 0;
                    n = "";
                    s = "";
                }
            }
            return list;
        }
        /// <summary>
        /// 是不是英文
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsEnglish(string s)
        {
            var p = Java.Util.Regex.Pattern.Compile("^[a-zA-Z]*$");
            var m = p.Matcher(s);
            if (m.Matches())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是不是数字
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumber(string s)
        {
            var p = Java.Util.Regex.Pattern.Compile("^[0-9]*$");
            var m = p.Matcher(s);
            if (m.Matches())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 是不是中文字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsChinese(string s)
        {
            var p = Java.Util.Regex.Pattern.Compile("^[\\u4E00-\\u9FA5\\uF900-\\uFA2D]*$");
            var m = p.Matcher(s);
            if (m.Matches())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}