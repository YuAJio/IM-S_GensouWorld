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
using IMAS.LocalDBManager.Models;

namespace IdoMaster_GensouWorld.Adapters
{
    /// <summary>
    /// 输入历史记录适配器
    /// </summary>
    public class SimpleHistorySearchAdapter : AsakuraBaseAdapter<Model_SearchHistory>
    {
        private Context context;
        public Action<int, View> clickAction;

        public SimpleHistorySearchAdapter(Context context)
        {
            this.context = context;
        }

        public void SetDataList(List<Model_SearchHistory> datalist)
        {
            SetContainerList(datalist);
        }

        public override Model_SearchHistory this[int position]
        {
            get
            {
                return list_data[position];
            }
        }

        protected override View GetContentView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder viewHolder;
            if (convertView == null || convertView.Tag == null)
            {
                viewHolder = new ViewHolder();
                convertView = View.Inflate(context, Resource.Layout.item_search_history, null);
                viewHolder.tv_message = convertView.FindViewById<TextView>(Resource.Id.tv_item);
                viewHolder.iv_delete = convertView.FindViewById<ImageView>(Resource.Id.iv_delete);

                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (ViewHolder)convertView.Tag;
            }
            var data = list_data[position];

            viewHolder.tv_message.Text = $"{data.Message} P"; 

            viewHolder.iv_delete.Tag = position;
            viewHolder.iv_delete.Click -= OnAdapterClickListener;
            viewHolder.iv_delete.Click += OnAdapterClickListener;

            return convertView;
        }

        private void OnAdapterClickListener(object sender, EventArgs e)
        {
            var v = sender as View;
            var position = (int)v.Tag;
            clickAction?.Invoke(position, v);
        }

        private class ViewHolder : Java.Lang.Object
        {
            /// <summary>
            /// 历史记录
            /// </summary>
            public TextView tv_message { get; set; }

            /// <summary>
            /// 删除图标
            /// </summary>
            public ImageView iv_delete { get; set; }
        }

    }
}