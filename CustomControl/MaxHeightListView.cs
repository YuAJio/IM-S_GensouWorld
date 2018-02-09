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
using IdoMaster_GensouWorld;

namespace CustomControl
{
    /// <summary>
    /// 设置最大高度ListView
    /// </summary>
    public class MaxHeightListView : ListView
    {
        private int maxHeight;

        public MaxHeightListView(Context context) : base(context) { }
        public MaxHeightListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            var a = context.ObtainStyledAttributes(attrs, Resource.Styleable.MaxHeightListView);
            maxHeight = a.GetInt(Resource.Styleable.MaxHeightListView_maxHeight, -1);
        }
        public MaxHeightListView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            var a = context.ObtainStyledAttributes(attrs, Resource.Styleable.MaxHeightListView);
            maxHeight = a.GetInt(Resource.Styleable.MaxHeightListView_maxHeight, -1);
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);
            SetViewHeightBasedOnChildre();
        }

        private void SetViewHeightBasedOnChildre()
        {
            var listAdapter = this.Adapter;
            if (listAdapter == null)
            {
                return;
            }

            int sumHeight = 0;
            int size = listAdapter.Count;

            for (int i = 0; i < size; i++)
            {
                var v = listAdapter.GetView(i, null, null);
                v.Measure(0, 0);
                sumHeight += v.MeasuredHeight;
            }
            if (sumHeight > maxHeight)
            {
                sumHeight = maxHeight;
            }
            var layoutParams = this.LayoutParameters;
            layoutParams.Height = sumHeight;
            this.LayoutParameters = layoutParams;
        }
    }
}