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
    /// 所持装备列表
    /// </summary>
    public class ChoseSoubiListAdapter : AsakuraBaseAdapter<Model_Items>
    {
        private Context context;
        public ChoseSoubiListAdapter(Context context)
        {
            this.context = context;
        }

        public void SetDataList(List<Model_Items> data_list)
        {
            SetContainerList(data_list);
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
                convertView = View.Inflate(context, Resource.Layout.item_chose_soubi_item, null);
                viewHolder.tv_name = convertView.FindViewById<TextView>(Resource.Id.tv_name);
                viewHolder.tv_att = convertView.FindViewById<TextView>(Resource.Id.tv_att);
                viewHolder.tv_def = convertView.FindViewById<TextView>(Resource.Id.tv_def);
                viewHolder.tv_health = convertView.FindViewById<TextView>(Resource.Id.tv_healingPoint);
                viewHolder.rl_weapon = convertView.FindViewById<RelativeLayout>(Resource.Id.rl_weapon);
                viewHolder.rl_health = convertView.FindViewById<RelativeLayout>(Resource.Id.rl_health);
                viewHolder.rl_fahter = convertView.FindViewById<RelativeLayout>(Resource.Id.rl_father);

                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (ViewHolder)convertView.Tag;
            }
            var data = list_data[position];
            viewHolder.rl_fahter.Selected = data.IsSelect;
            viewHolder.tv_name.Text = data.Name;
            viewHolder.tv_att.Text = $"攻撃力：{data.ATTPromote}";
            viewHolder.tv_def.Text = $"防御力：{data.DEFPromote}";
            #region 检测回复值
            var sb = new StringBuilder();
            var healbo = false;
            if (data.HealingHealthPoint > 0)
            {
                sb.Append($"生命回復:{data.HealingHealthPoint}/t");
                healbo = true;
            }
            if (data.HealingManaPoint > 0)
            {
                sb.Append($"魔法回復:{data.HealingHealthPoint}/t");
                healbo = true;
            }
            if (data.HealingStaminaPoint > 0)
            {
                sb.Append($"体力回復:{data.HealingHealthPoint}/t");
                healbo = true;
            }
            #endregion
            if (healbo)
            {
                viewHolder.rl_health.Visibility = ViewStates.Visible;
                viewHolder.tv_health.Text = $"{sb.ToString()}";
            }
            else
            {
                viewHolder.rl_health.Visibility = ViewStates.Gone;
            }
            switch (data.ItemType)
            {
                case IMAS.Tips.Enums.ItemEnumeration.HealingItems:
                    break;
                case IMAS.Tips.Enums.ItemEnumeration.EquipmentItems:
                    viewHolder.rl_weapon.Visibility = ViewStates.Visible;
                    break;
                case IMAS.Tips.Enums.ItemEnumeration.WeaponItems:
                    viewHolder.rl_weapon.Visibility = ViewStates.Visible;
                    break;
                case IMAS.Tips.Enums.ItemEnumeration.OtherItems:
                    viewHolder.rl_weapon.Visibility = ViewStates.Visible;
                    break;
            }

            return convertView;
        }

        private class ViewHolder : Java.Lang.Object
        {
            public RelativeLayout rl_weapon;
            public RelativeLayout rl_health;
            public RelativeLayout rl_fahter;
            public TextView tv_name;
            public TextView tv_att;
            public TextView tv_def;
            public TextView tv_health;
        }
    }
}