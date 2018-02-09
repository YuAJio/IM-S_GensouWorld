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
    public class ShopItemAdapter : AsakuraBaseAdapter<Model_Items>
    {
        private Context context;

        public ShopItemAdapter(Context context)
        {
            this.context = context;
        }
        public void SetDataList(List<Model_Items> list)
        {
            SetContainerList(list);
        }

        public override Model_Items this[int position]
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
                convertView = View.Inflate(context, Resource.Layout.item_shop_goods, null);
                viewHolder.tv_Name = convertView.FindViewById<TextView>(Resource.Id.tv_goods_name);
                viewHolder.tv_Price = convertView.FindViewById<TextView>(Resource.Id.tv_goods_price);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (ViewHolder)convertView.Tag;
            }
            var data = list_data[position];
            viewHolder.tv_Name.Text = data.Name;
            viewHolder.tv_Price.Text = $"{data.ShopPrice}円";

            return convertView;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public TextView tv_Name;
            public TextView tv_Price;
        }
    }
}