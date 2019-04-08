using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace IdoMaster_GensouWorld.CustomControl
{
    public class FakeDamakuView : TextView
    {
        public FakeDamakuView(Context context) : base(context)
        {
            Init();
        }

        public FakeDamakuView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        public FakeDamakuView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init();
        }

        public FakeDamakuView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init();
        }

        protected FakeDamakuView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }


        private void Init()
        {
            this.SetSingleLine(true);
            this.SetMarqueeRepeatLimit(-1);
            this.Ellipsize = TextUtils.TruncateAt.Marquee;

        }

        public override bool IsFocused { get { return true; } }

    }
}