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
    /// 简单字符串适配器
    /// </summary>
    public class CharacterSkillsAdapter : AsakuraBaseAdapter<Model_Skills>
    {
        private Context context;

        public CharacterSkillsAdapter(Context context)
        {
            this.context = context;
        }

        public void SetDataList(List<Model_Skills> list)
        {
            SetContainerList(list);
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
            ViewHolder viewHolder;
            if (convertView == null || convertView.Tag == null)
            {
                viewHolder = new ViewHolder();
                convertView = View.Inflate(context, Resource.Layout.item_simplestring, null);
                viewHolder.rl_father = convertView.FindViewById<RelativeLayout>(Resource.Id.rl_father);
                viewHolder.tv_Name = convertView.FindViewById<TextView>(Resource.Id.tv_item);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (ViewHolder)convertView.Tag;
            }
            var data = list_data[position];
            viewHolder.rl_father.Selected = data.IsSelect;
            viewHolder.tv_Name.Text = data.Name;
            return convertView;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public RelativeLayout rl_father;
            public TextView tv_Name;
        }
    }
}