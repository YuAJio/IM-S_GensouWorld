using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using IMAS.LocalDBManager.Models;

namespace IdoMaster_GensouWorld.Adapters
{
    /// <summary>
    /// 视频集数适配器
    /// </summary>
    public class Film_FilmEpisodes_Adapter : AsakuraBaseRvAdapter<VideoResources>
    {
        private Context context;
        private int playingIndex = 0;
        /// <summary>
        /// 设置适配内容
        /// </summary>
        /// <param name="dataList">数据列表</param>
        /// <param name="playingIndex">正在播放的集数</param>
        public void SetDataList(List<VideoResources> dataList, int playingIndex)
        {
            this.playingIndex = playingIndex;
            SetContainerList(dataList);
        }
        /// <summary>
        /// 设置目前播放的位置
        /// </summary>
        /// <param name="playingIndex"></param>
        public void SetPlayerIndex(int playingIndex)
        {
            this.playingIndex = playingIndex;
            NotifThisAdapterData();
        }

        public Film_FilmEpisodes_Adapter(Context context)
        {
            this.context = context;
        }
        public override VideoResources this[int position]
        {
            get { return list_data[position]; }
        }

        protected override void AbOnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as ViewHolder;
            if (viewHolder == null)
                return;
            if (onItemClickAct != null)
                ItemOnClick(viewHolder, position);

            var data = list_data[position];
            viewHolder.tv_Name.Text = data.Name;
            viewHolder.rl_Item.Selected = playingIndex == position;
        }

        protected override RecyclerView.ViewHolder AbOnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var v = View.Inflate(context, Resource.Layout.film_item_film_episode, null);
            return new ViewHolder(v);
        }

        private class ViewHolder : RecyclerView.ViewHolder
        {
            public TextView tv_Name;
            public RelativeLayout rl_Item;

            public ViewHolder(View itemView) : base(itemView)
            {
                tv_Name = itemView.FindViewById<TextView>(Resource.Id.tv_item);
                rl_Item = itemView.FindViewById<RelativeLayout>(Resource.Id.rl_item);
            }
        }
    }
}