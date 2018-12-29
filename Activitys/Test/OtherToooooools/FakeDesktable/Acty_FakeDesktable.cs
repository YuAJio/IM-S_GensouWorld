using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using IdoMaster_GensouWorld.CustomControl;
using IdoMaster_GensouWorld.Film_Adapters;
using IdoMaster_GensouWorld.Utils;
using IMAS.CupCake.Extensions;

namespace IdoMaster_GensouWorld.Activitys.Test.OtherToooooools.FakeDesktable
{
    /// <summary>
    /// 假的系统桌面
    /// </summary>
    [Activity(Label = "Acty_FakeDesktable", Theme = "@style/Theme.PublicThemePlus")]
    public class Acty_FakeDesktable : Ys.BeLazy.YsBaseActivity, DefautItemTouchHelpCallback.IOnItemTouchCallbackEvent
    {
        #region UI控件
        private HightFoucesBackImageView iv_Back;
        private RecyclerView rv_app;
        #endregion

        /// <summary>
        /// App适配器
        /// </summary>
        private AppAdapter adapter_App;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_fake_desktable;
        }

        public override void B_BeforeInitView()
        {
            adapter_App = new AppAdapter(this);
        }

        public override void C_InitView()
        {
            iv_Back = FindViewById<HightFoucesBackImageView>(Resource.Id.iv_back);
            rv_app = FindViewById<RecyclerView>(Resource.Id.rv_app);

            rv_app.SetLayoutManager(new GridLayoutManager(this, 4));
            rv_app.AddItemDecoration(new Z_RecyclerViewAsGrid_Spacing(4, 10, true));
            rv_app.SetAdapter(adapter_App);

            #region 滑动拖拽事件绑定
            //var itemTouchHelper = new DefaultItemTouchHelper(this);
            //itemTouchHelper.AttachToRecyclerView(rv_app);
            //itemTouchHelper.SetDragEnable(true);
            //itemTouchHelper.SetSwipEnable(true);
            #endregion
        }

        public override void D_BindEvent()
        {
            iv_Back.Click += OnClickListener;
        }

        public override void E_InitData()
        {
            adapter_App.onItemClickAct -= OnItemClick;
            adapter_App.onItemClickAct += OnItemClick;

            adapter_App.longClickAciton -= OnItemLongClick;
            adapter_App.longClickAciton += OnItemLongClick;

            GetSystemAppList();
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
            switch (v.Id)
            {
                case Resource.Id.iv_back:
                    {
                        this.Finish();
                    }
                    break;
            }
        }

        public override void G_SomethingElse()
        {

        }

        private void OnItemLongClick(string packNage, View view)
        {
            var clickItem = adapter_App[GetIndexOfPackageName(packNage)];
            popingAppPackageName = clickItem.PackageName;
            ShowPopAsLocation(view);
        }


        private void OnItemClick(View view, int position)
        {
            ShowMsgShort(position + "");
        }


        /// <summary>
        /// 获取系统的APP列表
        /// </summary>
        private void GetSystemAppList()
        {
            var packages = PackageManager.GetInstalledPackages(Android.Content.PM.PackageInfoFlags.Activities);
            var adapterData = packages.Where(
                x =>
                !x.ApplicationInfo.Flags.HasFlag(Android.Content.PM.ApplicationInfoFlags.System) &&
                !x.ApplicationInfo.Flags.HasFlag(Android.Content.PM.ApplicationInfoFlags.Debuggable) &&
                !x.PackageName.StartsWith("Mono.Android"))
                .Select(x => new Md_Applicaiotn()
                {
                    Name = x.ApplicationInfo.LoadLabel(PackageManager).ToString(),
                    PackageName = x.PackageName,
                    IconResource = x.ApplicationInfo.LoadIcon(PackageManager)
                }).ToList();

            //var intent = new Intent(Intent.ActionMain, null);
            //intent.AddCategory(Intent.CategoryLauncher);
            //var mApps = this.PackageManager.QueryIntentActivities(intent, Android.Content.PM.PackageInfoFlags.Activities);
            //var listMaaps = mApps.ToList();
            //var adapterData = listMaaps.Select(x => new Md_Applicaiotn()
            //{
            //    Name = x.ActivityInfo.Name,
            //    IconResource = x.ActivityInfo.IconResource,
            //    PackageName = x.ActivityInfo.PackageName
            //}).ToList();

            adapter_App.SetDataList(adapterData);
        }

        /// <summary>
        /// 获取某个包名在列表中的哪里
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        private int GetIndexOfPackageName(string packageName)
        {
            var jk = adapter_App.DataList.Where(x => x.PackageName == packageName).FirstOrDefault();
            return jk == null ? 0 : adapter_App.DataList.IndexOf(jk);
        }

        #region 控件拖动接口回调
        private string popingAppPackageName;
        public void OnSwiped(int adapterPosition)
        {
            adapter_App.DataList.RemoveAt(adapterPosition);
            adapter_App.NotifyItemRemoved(adapterPosition);
        }

        public bool OnMove(int srcPosition, int targetPosition)
        {
            var dataList = adapter_App.DataList.ToList();
            var cliItem = dataList[srcPosition];
            dataList.Remove(cliItem);
            dataList.Insert(targetPosition, cliItem);
            adapter_App.NotifyItemMoved(srcPosition, targetPosition);
            return true;
        }
        #endregion

        #region PopWindow
        private PopupWindowHelper popHelper;
        private void ShowPopAsLocation(View view)
        {
            var popView = LayoutInflater.From(this).Inflate(Resource.Layout.popwin_item_illustrate, null);
            #region 绑定点击事件
            var tv_Uninstall = popView.FindViewById<TextView>(Resource.Id.tv_Uninstall);
            var tv_AddQuick = popView.FindViewById<TextView>(Resource.Id.tv_AddQuick);
            tv_Uninstall.Click -= OnPopItemClickLisnter;
            tv_Uninstall.Click += OnPopItemClickLisnter;

            tv_AddQuick.Click -= OnPopItemClickLisnter;
            tv_AddQuick.Click += OnPopItemClickLisnter;
            #endregion
            popHelper = new PopupWindowHelper(popView);
            popHelper.ShowAsDropDown(view, 0, -10);
        }

        private void OnPopItemClickLisnter(object sender, EventArgs eventArgs)
        {
            var v = sender as View;
            switch (v.Id)
            {
                case Resource.Id.tv_Uninstall:
                    {
                        ShowMsgLong("卸载");
                    }
                    break;
                case Resource.Id.tv_AddQuick:
                    {
                        ShowMsgLong("添加快捷方式");
                    }
                    break;
            }
        }

        private class PopOnItemClick : Java.Lang.Object, AdapterView.IOnItemClickListener
        {
            private Action<int> onItemClick;
            public PopOnItemClick(Action<int> action)
            {
                this.onItemClick = action;
            }

            public void OnItemClick(AdapterView parent, View view, int position, long id)
            {
                onItemClick?.Invoke(position);
            }
        }

        #endregion

        #region 适配器
        /// <summary>
        /// APP列表适配器
        /// </summary>
        private class AppAdapter : Ys.BeLazy.YsBaseRvAdapter<Md_Applicaiotn>
        {
            public Action<string, View> longClickAciton;

            public AppAdapter(Context context)
            {
                this.context = context;
            }

            public override Md_Applicaiotn this[int position] { get { return list_data[position]; } }

            protected override void AbOnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
            {
                var holder = viewHolder as ViewHolder;
                var data = list_data[position];

                holder.tv_name.Text = data.Name;
#pragma warning disable CS0618 // 类型或成员已过时
                holder.iv_icon.SetBackgroundDrawable(data.IconResource);
#pragma warning restore CS0618 // 类型或成员已过时

                #region 设置长按监听
                holder.ItemView.Tag = data.PackageName;
                holder.ItemView.LongClick += ItemLongClickListner;
                #endregion
            }

            private void ItemLongClickListner(object sender, View.LongClickEventArgs e)
            {
                var v = sender as View;
                try
                {
                    longClickAciton?.Invoke(v.Tag.ToString(), v);
                }
                catch (Exception)
                {
                }
            }

            protected override RecyclerView.ViewHolder AbOnCreateViewHolder(ViewGroup parent, int viewType)
            {
                var jk = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_fake_desktable_app, parent, false);
                return new ViewHolder(jk);
            }


            private class ViewHolder : RecyclerView.ViewHolder
            {
                public ImageView iv_icon;
                public TextView tv_name;

                public ViewHolder(View itemView) : base(itemView)
                {
                    iv_icon = itemView.FindViewById<ImageView>(Resource.Id.iv_icon);
                    tv_name = itemView.FindViewById<TextView>(Resource.Id.tv_name);
                }


            }

        }

        //private class PopUpAdapter : Ys.BeLazy.YsBaseApdater<string>
        //{
        //    private readonly Context context;
        //    public PopUpAdapter(Context context)
        //    {
        //        this.context = context;
        //    }
        //    public void SetDataList(List<string> dataList)
        //    {
        //        this.SetContainerList(dataList);
        //        this.NotifThisAdapterData();
        //    }

        //    public override string this[int position] { get { return list_data[position]; } }

        //    protected override View GetContentView(int position, View convertView, ViewGroup parent)
        //    {
        //        TextView tv;
        //        if (convertView == null || convertView.Tag == null)
        //        {
        //            convertView = View.Inflate(context, Resource.Layout.item_fakedesktable_app_click_menu, null);
        //            tv = convertView.FindViewById<TextView>(Resource.Id.tv_name);
        //            convertView.Tag = tv;
        //        }
        //        else
        //            tv = (TextView)convertView.Tag;

        //        tv.Text = list_data[position];

        //        return convertView;
        //    }


        //}

        #endregion

        #region 实体类
        private class Md_Applicaiotn
        {
            /// <summary>
            /// APP名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// APP包名
            /// </summary>
            public string PackageName { get; set; }
            /// <summary>
            /// 图标资源
            /// </summary>
            public Drawable IconResource { get; set; }
        }
        #endregion

    }
}