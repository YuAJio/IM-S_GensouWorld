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
    public class RunHorseLanternTextView : TextView
    {
        public RunHorseLanternTextView(Context context) : base(context)
        {
        }

        public RunHorseLanternTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public RunHorseLanternTextView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public RunHorseLanternTextView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected RunHorseLanternTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override bool IsFocused => true;
    }
}