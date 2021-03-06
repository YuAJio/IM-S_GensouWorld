﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using IdoMaster_GensouWorld.Adapters;
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
        /// <summary>
        /// 应用市场包名
        /// </summary>
        private const string MarketPackageName = "Not.In.Town";
        #region UI控件
        private HightFoucesBackImageView iv_Back;
        private EditText et_Search;
        private RecyclerView rv_app;
        #endregion

        /// <summary>
        /// App适配器
        /// </summary>
        private AppAdapter adapter_App;
        /// <summary>
        /// APK列表
        /// </summary>
        private List<Md_Applicaiotn> list_Apk;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_fake_desktable;
        }

        public override void B_BeforeInitView()
        {
            adapter_App = new AppAdapter(this);
            list_Apk = new List<Md_Applicaiotn>() { };
        }

        public override void C_InitView()
        {
            iv_Back = FindViewById<HightFoucesBackImageView>(Resource.Id.iv_back);
            rv_app = FindViewById<RecyclerView>(Resource.Id.rv_app);
            et_Search = FindViewById<AppCompatEditText>(Resource.Id.et_search);

            rv_app.SetLayoutManager(new GridLayoutManager(this, 6));
            rv_app.AddItemDecoration(new Z_RecyclerViewAsGrid_Spacing(6, 16, true));
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
            et_Search.TextChanged -= OnSeaechTextChange;
            et_Search.TextChanged += OnSeaechTextChange;
            et_Search.EditorAction -= OnSearchAction;
            et_Search.EditorAction += OnSearchAction;


        }

        public override void E_InitData()
        {
            adapter_App.clickAciton -= OnItemClick;
            adapter_App.clickAciton += OnItemClick;

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


        private void OnItemLongClick(string packNage, View view)
        {
            var clickItem = adapter_App[GetIndexOfPackageName(packNage)];
            popingAppPackageName = clickItem.PackageName;
            ShowPopAsLocation(view);
        }


        private void OnItemClick(string packNage, View view)
        {
            switch (packNage)
            {
                case MarketPackageName:
                    {//应用市场
                        var apkPackageNames = list_Apk.Select(x => x.PackageName).ToJson();
                        var intent = new Intent(this, typeof(Acty_UChengMarket));
                        intent.PutExtra("packagenames", apkPackageNames);
                        StartActivityForResult(intent, 0x114);
                    }
                    break;
                default:
                    {//其他应用
                        if (!string.IsNullOrEmpty(packNage))
                            OpenApk(packNage);
                    }
                    break;
            }
        }
        #region 方法池

        /// <summary>
        /// 打开APK
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="name"></param>
        private void OpenApk(string packageName)
        {
            var intent = PackageManager.GetLaunchIntentForPackage(packageName);
            if (intent != null)
            {
                Task.Run(() =>
            {
                System.Threading.Thread.Sleep(200);
            }).ContinueWith(x =>
            {
                intent.SetFlags(ActivityFlags.NewTask);
                this.StartActivity(intent);
            }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }      /// <summary>
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
                    NameForOpen = x.ApplicationInfo.Name,
                    IconResource = x.ApplicationInfo.LoadIcon(PackageManager)
                }).ToList();

            //var intent = new Intent(Intent.ActionMain, null);
            //intent.AddCategory(Intent.CategoryLauncher);
            //var mApps = this.PackageManager.QueryIntentActivities(intent, Android.Content.PM.PackageInfoFlags.Activities);
            //var listMaaps = mApps.ToList();
            //var adapterDatass = listMaaps.Select(x => new Md_Applicaiotn()
            //{
            //    Name = x.ActivityInfo.Name,
            //    NameForOpen=x.ActivityInfo.Name,
            //    PackageName = x.ActivityInfo.PackageName
            //}).ToList();

            list_Apk.Clear();
            list_Apk.Add(GetApkMarketMoudle());
            list_Apk.InsertRange(0, adapterData);
            adapter_App.SetDataList(list_Apk);
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

        /// <summary>
        /// 获取应用市场APP
        /// </summary>
        /// <returns></returns>
        private Md_Applicaiotn GetApkMarketMoudle()
        {
            var md = new Md_Applicaiotn() { };
            md.IconResource = ContextCompat.GetDrawable(this, Resource.Mipmap.ico_appmark);
            md.PackageName = MarketPackageName;
            md.Name = "U称商城";
            return md;
        }

        #endregion

        #region EventHanlder
        /// <summary>
        /// 搜索栏文字变动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSeaechTextChange(object sender, Android.Text.TextChangedEventArgs e)
        {
            var txt = e.Text.ToString();
            if (string.IsNullOrEmpty(txt))
                if (adapter_App.ItemCount != list_Apk.Count)
                    adapter_App.SetDataList(list_Apk);
            var searchContent = adapter_App.DataList.Where(x => x.Name.Contains(txt));
            if (searchContent == null || !searchContent.Any())
            {//没有搜索的内容
                adapter_App.SetDataList(null);
            }
            else
            {//有搜索的内容
                adapter_App.SetDataList(searchContent.ToList());
            }
        }

        /// <summary>
        /// 点击软键盘搜索键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSearchAction(object sender, TextView.EditorActionEventArgs e)
        {
            HideTheSoftKeybow(et_Search);

        }
        #endregion

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
            var popView = LayoutInflater.From(this).Inflate(Resource.Layout.popwin_item_fake_appmenu, null);
            #region 绑定点击事件
            var tv_Uninstall = popView.FindViewById<TextView>(Resource.Id.tv_Uninstall);
            var tv_AddQuick = popView.FindViewById<TextView>(Resource.Id.tv_AddQuick);
            tv_Uninstall.Click -= OnPopItemClickLisnter;
            tv_Uninstall.Click += OnPopItemClickLisnter;

            tv_AddQuick.Click -= OnPopItemClickLisnter;
            tv_AddQuick.Click += OnPopItemClickLisnter;
            #endregion
            //判断是否是应用市场长按
            if (popingAppPackageName == MarketPackageName)
            {
                tv_Uninstall.Enabled = false;
                tv_Uninstall.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.gray));
            }

            popHelper = new PopupWindowHelper(popView);
            var jk = new int[2];
            view.GetLocationOnScreen(jk);
            var father = FindViewById<Android.Support.Constraints.ConstraintLayout>(Resource.Id.cl_father);
            popHelper.ShowAtLocation(father, GravityFlags.NoGravity, jk[0], jk[1]);
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
        private class AppAdapter : AsakuraBaseRvAdapter<Md_Applicaiotn>
        {
            public Action<string, View> longClickAciton;
            public Action<string, View> clickAciton;

            public AppAdapter(Context context)
            {
                this.context = context;
            }
            /// <summary>
            /// 适配数据
            /// </summary>
            /// <param name="list"></param>
            public void SetDataList(List<Md_Applicaiotn> list)
            {
                SetContainerList(list);
            }

            public override Md_Applicaiotn this[int position] { get { return list_data[position]; } }

            protected override void AbOnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
            {
                var holder = viewHolder as ViewHolder;
                if (viewHolder == null)
                    return;

                var data = list_data[position];

                holder.tv_name.Text = data.Name;
#pragma warning disable CS0618 // 类型或成员已过时
                holder.iv_icon.SetBackgroundDrawable(data.IconResource);
#pragma warning restore CS0618 // 类型或成员已过时

                #region 设置长按监听和点击监听
                holder.ItemView.Tag = data.PackageName;
                holder.ItemView.LongClick -= ItemLongClickListner;
                holder.ItemView.LongClick += ItemLongClickListner;
                holder.ItemView.Click -= ItemClickListner;
                holder.ItemView.Click += ItemClickListner;
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
            private void ItemClickListner(object sender, EventArgs e)
            {
                var v = sender as View;
                try
                {
                    clickAciton?.Invoke(v.Tag.ToString(), v);
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
            /// Class名(打开所需)
            /// </summary>
            public string NameForOpen { get; set; }
            /// <summary>
            /// 图标资源
            /// </summary>
            public Drawable IconResource { get; set; }
        }
        #endregion

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 0x114)
                if (resultCode == Result.Ok)
                {//市场访问后
                    GetSystemAppList();
                }

        }

    }
}