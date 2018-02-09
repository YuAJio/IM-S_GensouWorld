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
using Android.Views.Animations;

namespace IdoMaster_GensouWorld.Utils
{
    /// <summary>
    /// 创建各种动画效果
    /// </summary>
    public class AnimationHelper
    {
        /// <summary>
        /// 渐现动画效果
        /// </summary>
        /// <returns></returns>
        public static Animation Fade_InAlphaAnimation(int duration)
        {
            var mShowAnimation = new AlphaAnimation(0.0f, 1.0f);
            mShowAnimation.Duration = duration;
            mShowAnimation.FillAfter = true;
            return mShowAnimation;

        }

        /// <summary>
        /// 淡出动画效果
        /// </summary>
        /// <returns></returns>
        public static Animation Fade_OutAlphaAnimation(int duration)
        {
            var mShowAnimation = new AlphaAnimation(1.0f, 0.0f);
            mShowAnimation.Duration = duration;
            mShowAnimation.FillAfter = true;
            return mShowAnimation;

        }
    }
}