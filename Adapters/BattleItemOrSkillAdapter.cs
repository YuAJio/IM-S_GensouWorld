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
    /// 战斗物品和战技Adapter
    /// </summary>
    public class BattleItemAdapter : AsakuraBaseAdapter<Model_Items>
    {
        private Context context;
        public BattleItemAdapter(Context context)
        {
            this.context = context;
        }
        public void SetDataList(List<Model_Items> dataList)
        {
            this.SetContainerList(dataList);
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
            SkillOrItemViewHolder viewHolder;
            if (convertView == null || convertView.Tag == null)
            {
                viewHolder = new SkillOrItemViewHolder();
                convertView = View.Inflate(context, Resource.Layout.item_battle_skill_or_item, null);
                viewHolder.Tv_Name = convertView.FindViewById<TextView>(Resource.Id.tv_name);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (SkillOrItemViewHolder)convertView.Tag;
            }
            var data = list_data[position];
            viewHolder.Tv_Name.Text = data.Name;

            return convertView;
        }
    }
    public class BattleSkillAdapter : AsakuraBaseAdapter<Model_Skills>
    {
        private Context context;

        public BattleSkillAdapter(Context context)
        {
            this.context = context;
        }
        public void SetDataList(List<Model_Skills> dataList)
        {
            this.SetContainerList(dataList);
        }
        public override Model_Skills this[int position]
        {
            get
            {
                return list_data[position];
            }
        }

        protected override View GetContentView(int position, View convertView, ViewGroup parent)
        {
            SkillOrItemViewHolder viewHolder;
            if (convertView == null || convertView.Tag == null)
            {
                viewHolder = new SkillOrItemViewHolder();
                convertView = View.Inflate(context, Resource.Layout.item_battle_skill_or_item, null);
                viewHolder.Tv_Name = convertView.FindViewById<TextView>(Resource.Id.tv_name);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (SkillOrItemViewHolder)convertView.Tag;
            }
            var data = list_data[position];
            viewHolder.Tv_Name.Text = data.Name;

            return convertView;
        }
    }

    public class SkillOrItemViewHolder : Java.Lang.Object
    {
        /// <summary>
        /// 名字
        /// </summary>
        public TextView Tv_Name { get; set; }
    }

}