using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace IdoMaster_GensouWorld.Film_Adapters
{
    /// <summary>
    /// 瀑布流布局间距
    /// </summary>
    public class Z_RecyclerViewAsStaggered_Spacing : RecyclerView.ItemDecoration
    {
        private int spaceL;
        private int spaceR;
        private int spaceT;
        private int spaceB;

        public Z_RecyclerViewAsStaggered_Spacing(int spaceL, int spaceR, int spaceT, int spaceB)
        {
            this.spaceL = spaceL;
            this.spaceR = spaceR;
            this.spaceT = spaceT;
            this.spaceB = spaceB;
        }

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            outRect.Left = spaceL;
            outRect.Right = spaceR;
            outRect.Bottom = spaceT;
            outRect.Top = spaceB;
        }

    }
}