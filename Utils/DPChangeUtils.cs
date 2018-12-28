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

namespace IdoMaster_GensouWorld.Utils
{
    /// <summary>
    /// 控件dp px dip转换工具
    /// </summary>
    public class DPChangeUtils
    {
        /// <summary>
        /// dp转px
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static int Dip2px(int dp)
        {
            var density = Application.Context.Resources.DisplayMetrics.Density;
            return (int)(dp * density + 0.5);
        }

        /// <summary>
        /// px转换dip
        /// </summary>
        /// <param name="px"></param>
        /// <returns></returns>
        public static int Px2dip(int px)
        {
            var scale = Application.Context.Resources.DisplayMetrics.Density;
            return (int)(px / scale + 0.5f);
        }

        /// <summary>
        /// px转换sp
        /// </summary>
        /// <param name="pxValue"></param>
        /// <returns></returns>
        public static int Px2sp(int pxValue)
        {
            var fontScale = Application.Context.Resources.DisplayMetrics.ScaledDensity;
            return (int)(pxValue / fontScale + 0.5f);
        }

        /// <summary>
        /// sp转换px
        /// </summary>
        /// <param name="spValue"></param>
        /// <returns></returns>
        public static int Sp2px(int spValue)
        {
            var fontScale = Application.Context.Resources.DisplayMetrics.ScaledDensity;
            return (int)(spValue * fontScale + 0.5f);
        }
    }
}