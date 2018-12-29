using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace IdoMaster_GensouWorld.CustomControl
{
    public class HightFoucesBackImageView : Android.Support.V7.Widget.AppCompatImageView
    {
        public HightFoucesBackImageView(Context context) : base(context)
        {
        }

        public HightFoucesBackImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public HightFoucesBackImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected HightFoucesBackImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override bool IsFocused => true;
    }
}