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
using Android.Graphics;
using Android.Util;

namespace CustomControl
{
    public class StartMovieTextView : TextView
    {
        public StartMovieTextView(Context context) : base(context)
        {
            Init(context);
        }
        public StartMovieTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }
        /** 
         * 初始化字体 
         * @param context 
         */
        private void Init(Context context)
        {
            //设置字体样式  
            SetTypeface(FontCustom.SetFont(context), TypefaceStyle.Normal);
        }
    }

    public class FontCustom
    {

        // fongUrl是自定义字体分类的名称  
        private static string fongUrl = "nihonn_sild.ttf";
        //Typeface是字体，这里我们创建一个对象  
        private static Typeface tf;

        /** 
         * 设置字体 
         */
        public static Typeface SetFont(Context context)
        {
            if (tf == null)
            {
                //给它设置你传入的自定义字体文件，再返回回来  
                tf = Typeface.CreateFromAsset(context.Assets, fongUrl);
            }
            return tf;
        }
    }
}