using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace IdoMaster_GensouWorld.Film_Adapters
{
    public class Z_GridLayoutManager : GridLayoutManager
    {
        #region 系统方法
        public Z_GridLayoutManager(Context context, int spanCount) : base(context, spanCount)
        {
        }

        public Z_GridLayoutManager(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        public Z_GridLayoutManager(Context context, int spanCount, int orientation, bool reverseLayout) : base(context, spanCount, orientation, reverseLayout)
        {
        }

        protected Z_GridLayoutManager(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
        #endregion

    }
}