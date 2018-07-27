﻿using System;
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

namespace IdoMaster_GensouWorld.Adapters
{
    public abstract class AsakuraBaseRvAdapter<Model> : RecyclerView.Adapter
    {
        public Action<View, int> onItemClickAct;
        protected Context context; 

        /// <summary>
        /// 适配器数据列表
        /// </summary>
        protected IList<Model> list_data;

        /// <summary>
        /// 设置适配器的数据
        /// </summary>
        /// <param name="list_data"></param>
        protected void SetContainerList(IList<Model> list_data)
        {
            this.list_data = list_data;
            NotifThisAdapterData();
        }

        /// <summary>
        /// 刷新适配器
        /// </summary>
        protected void NotifThisAdapterData()
        {
            this.NotifyDataSetChanged();
        }

        public override int ItemCount
        {
            get
            {
                return list_data == null ? 0 : list_data.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            AbOnBindViewHolder(holder, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var jk = AbOnCreateViewHolder(parent, viewType);

            return jk;
        }


        /// <summary>
        /// 适配内容
        /// </summary>
        /// <param name="holder">RvHolder</param>
        /// <param name="parent">数据坐标</param>
        /// <returns></returns>
        protected abstract void AbOnBindViewHolder(RecyclerView.ViewHolder holder, int position);

        /// <summary>
        /// 创建ViewHolder
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="viewType"></param>
        /// <returns></returns>
        protected abstract RecyclerView.ViewHolder AbOnCreateViewHolder(ViewGroup parent, int viewType);


        /// <summary>
        /// 设置控件点击
        /// </summary>
        /// <param name="holder"></param>
        /// <param name="position"></param>
        protected void ItemOnClick(RecyclerView.ViewHolder holder, int position)
        {
            holder.ItemView.Tag = position;
            holder.ItemView.Click -= OnItemClickListener;
            holder.ItemView.Click += OnItemClickListener;
        }

        /// <summary>
        /// 设置控件点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemClickListener(Object sender, EventArgs e)
        {
            var v = sender as View;
            onItemClickAct?.Invoke(v, (int)v.Tag);
        }

        /// <summary>
        /// ItemClick方法获取的数据
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public abstract Model this[int position] { get; }

        /// <summary>
        /// 数据列表
        /// </summary>
        public IList<Model> DataList
        {
            get
            {
                return this.list_data == null ? new List<Model>() : list_data;
            }
        }
    }
}