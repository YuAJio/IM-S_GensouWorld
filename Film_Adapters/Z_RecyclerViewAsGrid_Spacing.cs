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
    /// Recycler专用代替Grid设置间距
    /// </summary>
    public class Z_RecyclerViewAsGrid_Spacing : RecyclerView.ItemDecoration
    {
        private int spanCount;
        private int spacing;
        private bool includeEdge;

        public Z_RecyclerViewAsGrid_Spacing(int spanCount, int spacing, bool includeEdge)
        {
            this.spanCount = spanCount;
            this.spacing = spacing;
            this.includeEdge = includeEdge;
        }
        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            int position = parent.GetChildAdapterPosition(view);
            int column = position % spanCount;

            if (includeEdge)
            {
                outRect.Left = spacing - column * spacing / spanCount;
                outRect.Right = (column + 1) * spacing / spanCount;
                if (position < spanCount)
                    outRect.Top = spacing;
                outRect.Bottom = spacing;

            }
            else
            {
                outRect.Left = column * spacing / spanCount;
                outRect.Right = spacing - (column + 1) * spacing / spanCount;
                if (position >= spanCount)
                    outRect.Top = spanCount;
            }
        }

    }
}