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
using Java.Lang;
using CustomControl;
using Com.Nostra13.Universalimageloader.Core;
using IdoMaster_GensouWorld.Utils;

namespace IdoMaster_GensouWorld.Adapters
{
    #region 实体类
    public class AD_MyGame
    {
        /// <summary>
        /// 游戏ID
        /// </summary>
        public long G_UID { get; set; }
        /// <summary>
        /// 游戏图片网址
        /// </summary>
        public string G_PATH { get; set; }
        /// <summary>
        /// 游戏名字
        /// </summary>
        public string G_NAME { get; set; }
        /// <summary>
        /// 游戏生产商
        /// </summary>
        public string G_MANUFACTURERS { get; set; }

    }
    #endregion

    public class Test_SlidMenuAdapter : AsakuraBaseAdapter<AD_MyGame>, DragView.IDragStateListenner
    {
        private Context context;
        private DragView tempView;

        public Action<int> DeletAct;
        public Action<int> ConfigAct;
        public Test_SlidMenuAdapter(Context context)
        {
            this.context = context;
        }
        public void SetDataList(List<AD_MyGame> list)
        {
            this.SetContainerList(list);
        }
        public override AD_MyGame this[int position]
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
                convertView = View.Inflate(context, Resource.Layout.test_item_slid_menu, null);
                viewHolder.iv_path = convertView.FindViewById<ImageView>(Resource.Id.iv_item);
                viewHolder.tv_name = convertView.FindViewById<TextView>(Resource.Id.tv_item_name);
                viewHolder.tv_maker = convertView.FindViewById<TextView>(Resource.Id.tv_item_maker);
                viewHolder.dragView = convertView.FindViewById<DragView>(Resource.Id.dv_item);
                viewHolder.dragView.SetOnDragStateListener(null);
                viewHolder.dragView.SetOnDragStateListener(this);
                convertView.Tag = viewHolder;
            }
            else
            {
                viewHolder = (ViewHolder)convertView.Tag;
            }
            var data = list_data[position];
            viewHolder.dragView.Tag = position;
            viewHolder.tv_name.Text = data.G_NAME;
            viewHolder.tv_maker.Text = data.G_MANUFACTURERS;
            if (data.G_PATH != null)
            {
                ImageLoader.Instance.DisplayImage(data.G_PATH, viewHolder.iv_path, ImageLoaderHelper.GeneralImageOption());
            }
            return convertView;
        }

        public void OnOpened(DragView dragView)
        {
            if (tempView == null)
            {
                tempView = dragView;
            }
            if (tempView.Tag != dragView.Tag)
            {
                tempView.Close();
                tempView = dragView;
            }
        }

        public void OnClosed(DragView dragView)
        {

        }

        public void OnForegroundViewClick(DragView dragView, View v)
        {

        }

        public void OnBackgroundViewClick(DragView dragView, View v)
        {
            var position = (int)dragView.Tag;
            switch (v.Id)
            {
                case Resource.Id.bt_delete:
                    DeletAct?.Invoke(position);
                    break;
                case Resource.Id.bt_change:
                    ConfigAct?.Invoke(position);
                    break;
            }
        }

        private class ViewHolder : Java.Lang.Object
        {
            public DragView dragView;
            public TextView tv_name;
            public TextView tv_maker;
            public ImageView iv_path;
        }
    }
}