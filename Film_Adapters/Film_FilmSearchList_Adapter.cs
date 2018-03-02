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
using Com.Nostra13.Universalimageloader.Core;
using IdoMaster_GensouWorld.Utils;
using IMAS.LocalDBManager.Models;

namespace IdoMaster_GensouWorld.Adapters
{
    ///// <summary>
    ///// 电影搜索
    ///// </summary>
    public class Film_FilmSearchList_Adapter : AsakuraBaseRvAdapter<Model_VideoCover>
    {
        private Context context;

        public void SetDataList(List<Model_VideoCover> dataList)
        {
            SetContainerList(dataList);
        }

        public Film_FilmSearchList_Adapter(Context context)
        {
            this.context = context;
        }
        public override Model_VideoCover this[int position]
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
            viewHolder.tv_Title.Text = data.Title;
            ImageLoader.Instance.DisplayImage($"http:{data.Img}", viewHolder.iv_Poster, ImageLoaderHelper.CircleImageOptions());
        }

        protected override RecyclerView.ViewHolder AbOnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var v = View.Inflate(context, Resource.Layout.film_item_film_list, null);

            return new ViewHolder(v);
        }


        private class ViewHolder : RecyclerView.ViewHolder
        {
            public TextView tv_Title;
            public TextView tv_Name;
            public ImageView iv_Poster;

            public ViewHolder(View itemView) : base(itemView)
            {
                tv_Title = itemView.FindViewById<TextView>(Resource.Id.tv_up_title);
                tv_Name = itemView.FindViewById<TextView>(Resource.Id.tv_title);
                iv_Poster = itemView.FindViewById<ImageView>(Resource.Id.iv_img);
            }
        }
    }

    ///// <summary>
    ///// 电影搜索
    ///// </summary>
    //public class Film_FilmSearchList_Adapter : RecyclerView.Adapter
    //{
    //    public Action<View, int> onItemClickAct;

    //    private List<Model_VideoSearchs> dataList;
    //    private Context context;



    //    public Film_FilmSearchList_Adapter(Context context)
    //    {
    //        this.context = context;
    //    }

    //    public void SetDataList(List<Model_VideoSearchs> datalist)
    //    {
    //        this.dataList = datalist;
    //        this.NotifyDataSetChanged();
    //    }

    //    public override int ItemCount
    //    {
    //        get { return dataList == null ? 0 : dataList.Count; }
    //    }

    //    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    //    {
    //        var v = View.Inflate(context, Resource.Layout.film_item_film_list, null);
    //        return new ViewHolder(v);
    //    }

    //    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    //    {
    //        var viewHolder = holder as ViewHolder;
    //        if (viewHolder == null)
    //            return;
    //        if (onItemClickAct != null)
    //        {
    //            ItemOnClick(viewHolder, position);
    //        }
    //        var data = dataList[position];
    //        viewHolder.tv_Name.Text = data.Name;
    //        viewHolder.tv_Title.Text = data.Title;
    //        ImageLoader.Instance.DisplayImage($"http:{data.Img}", viewHolder.iv_Poster, ImageLoaderHelper.CircleImageOptions());

    //    }

    //    private void ItemOnClick(RecyclerView.ViewHolder holder, int position)
    //    {
    //        holder.ItemView.Tag = position;
    //        holder.ItemView.Click -= OnItemClickListener;
    //        holder.ItemView.Click += OnItemClickListener;
    //    }

    //    private class ViewHolder : RecyclerView.ViewHolder
    //    {
    //        public TextView tv_Title;
    //        public TextView tv_Name;
    //        public ImageView iv_Poster;

    //        public ViewHolder(View itemView) : base(itemView)
    //        {
    //            tv_Title = itemView.FindViewById<TextView>(Resource.Id.tv_up_title);
    //            tv_Name = itemView.FindViewById<TextView>(Resource.Id.tv_title);
    //            iv_Poster = itemView.FindViewById<ImageView>(Resource.Id.iv_img);
    //        }
    //    }

    //    private void OnItemClickListener(Object sender, EventArgs e)
    //    {
    //        var v = sender as View;
    //        onItemClickAct?.Invoke(v, (int)v.Tag);
    //    }

    //}

    ///// <summary>
    ///// 电影搜索
    ///// </summary>
    //public class Film_FIlmSearchList_Adapter : AsakuraBaseAdapter<Model_VideoSearchs>
    //{
    //    private Context context;

    //    public Film_FIlmSearchList_Adapter(Context context)
    //    {
    //        this.context = context;
    //    }

    //    public void SetDataList(List<Model_VideoSearchs> data)
    //    {
    //        SetContainerList(data);
    //    }
    //    public override Model_VideoSearchs this[int position]
    //    {

    //        get
    //        {
    //            return list_data[position];
    //        }
    //    }

    //    protected override View GetContentView(int position, View convertView, ViewGroup parent)
    //    {
    //        ViewHolder viewHolder;
    //        if (convertView == null || convertView.Tag == null)
    //        {
    //            viewHolder = new ViewHolder();
    //            convertView = View.Inflate(context, Resource.Layout.film_item_film_list, null);
    //            viewHolder.tv_title = convertView.FindViewById<TextView>(Resource.Id.tv_title);
    //            viewHolder.tv_up_title = convertView.FindViewById<TextView>(Resource.Id.tv_up_title);
    //            viewHolder.iv_img = convertView.FindViewById<ImageView>(Resource.Id.iv_img);
    //            convertView.Tag = viewHolder;
    //        }
    //        else
    //        {
    //            viewHolder = (ViewHolder)convertView.Tag;
    //        }
    //        var data = list_data[position];
    //        viewHolder.tv_title.Text = data.Name;
    //        viewHolder.tv_up_title.Text = data.Title;
    //        ImageLoader.Instance.DisplayImage($"http:{data.Img}", viewHolder.iv_img, ImageLoaderHelper.CircleImageOptions());
    //        return convertView;
    //    }

    //    private class ViewHolder : Java.Lang.Object
    //    {
    //        public TextView tv_title;
    //        public ImageView iv_img;
    //        public TextView tv_up_title;
    //    }
    //}
}