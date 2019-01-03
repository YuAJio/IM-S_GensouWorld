using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using IdoMaster_GensouWorld.Adapters;
using IdoMaster_GensouWorld.CustomControl;
using IdoMaster_GensouWorld.Film_Adapters;
using IdoMaster_GensouWorld.Utils;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Com.Nostra13.Universalimageloader.Core;
using Android.Util;
using System.Timers;

namespace IdoMaster_GensouWorld.Activitys.Test.OtherToooooools.FakeDesktable
{
    /// <summary>
    /// U称市场页面
    /// </summary>
    [Activity(Label = "Acty_UChengMarket", Theme = "@style/Theme.PublicThemePlus")]
    public class Acty_UChengMarket : Ys.BeLazy.YsBaseActivity
    {
        #region UI控件
        private HightFoucesBackImageView iv_Back;
        private EditText et_Search;
        private RecyclerView rv_MarketApp;
        /// <summary>
        ///  遮罩层
        /// </summary>
        private View v_Mask;
        #endregion

        #region 变量
        /// <summary>
        /// 点击的List的Index
        /// </summary>
        private int clickPosition;
        #endregion
        /// <summary>
        /// 市场适配器
        /// </summary>
        private MarkerAppApdater adapter_market;




        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_ucheng_market;
        }

        public override void B_BeforeInitView()
        {
            adapter_market = new MarkerAppApdater(this);
        }

        public override void C_InitView()
        {
            iv_Back = FindViewById<HightFoucesBackImageView>(Resource.Id.iv_back);
            rv_MarketApp = FindViewById<RecyclerView>(Resource.Id.rv_app);
            et_Search = FindViewById<AppCompatEditText>(Resource.Id.et_search);
            v_Mask = FindViewById<View>(Resource.Id.v_mask);

            rv_MarketApp.SetLayoutManager(new GridLayoutManager(this, 2));
            rv_MarketApp.AddItemDecoration(new Z_RecyclerViewAsGrid_Spacing(2, 16, true));
            (rv_MarketApp.GetItemAnimator() as SimpleItemAnimator).SupportsChangeAnimations = false;
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
            rv_MarketApp.SetAdapter(adapter_market);
            adapter_market.onItemClickAct -= OnItemClickLisnter;
            adapter_market.onItemClickAct += OnItemClickLisnter;
            HttpGetMarketApplicaiton();
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
                case Resource.Id.iv_Download:
                    {//下载app
                        StartTimeCount();
                        popWindow.Dismiss();
                    }
                    break;
            }
        }

        public override void G_SomethingElse()
        {

        }

        #region 计时器相关
        private Timer ysTimer;
        private double downloadProgress;
        private void StartTimeCount()
        {
            if (ysTimer == null)
            {
                ysTimer = new Timer
                {
                    Interval = 1 * 1000
                };
                ysTimer.Elapsed -= ItIsTimeToDoSomeThing;
                ysTimer.Elapsed += ItIsTimeToDoSomeThing;
            }
            ysTimer.Start();
        }

        private void ItIsTimeToDoSomeThing(object sender, ElapsedEventArgs e)
        {
            var rd = new Random();
            downloadProgress += rd.NextDouble();
            adapter_market[clickPosition].DownLoadProgress = downloadProgress;
            RunOnUiThread(() =>
            {
                adapter_market.NotifyItemChanged(clickPosition);
            });

        }
        #endregion


        #region EventHandler
        /// <summary>
        /// 适配器点击事件
        /// </summary>
        /// <param name="view"></param>
        /// <param name="position"></param>
        private void OnItemClickLisnter(View view, int position)
        {
            clickPosition = position;
            ShowPopWinodw(position);
        }
        /// <summary>
        /// 搜索栏文字变动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSeaechTextChange(object sender, Android.Text.TextChangedEventArgs e)
        {

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

        #region PopWindow
        private MyPopWindow popWindow;
        /// <summary>
        /// 显示PopWindow
        /// </summary>
        private void ShowPopWinodw(int position)
        {
            var data = adapter_market[position];

            var popView = LayoutInflater.From(this).Inflate(Resource.Layout.popwin_market_application_info, null);
            popView.FindViewById<AppCompatTextView>(Resource.Id.tv_Name).Text = data.Name;
            popView.FindViewById<AppCompatTextView>(Resource.Id.tv_Maker).Text = data.Maker;
            popView.FindViewById<AppCompatImageView>(Resource.Id.iv_Download).Click += OnClickListener;
            var iv = popView.FindViewById<AppCompatImageView>(Resource.Id.iv_Icon);

            ImageLoader.Instance.DisplayImage(data.PicPath, iv, ImageLoaderHelper.GeneralImageOption());
            v_Mask.Visibility = ViewStates.Visible;
            var metrics = Resources.DisplayMetrics;
            int width = metrics.WidthPixels;
            int height = metrics.HeightPixels;
            popWindow = new MyPopWindow(popView, Convert.ToInt32(width * 0.6), Convert.ToInt32(height * 0.8));
            popWindow.dismissCallBack -= OnPopDismiss;
            popWindow.dismissCallBack += OnPopDismiss;
            popWindow.OutsideTouchable = true;
            popWindow.ShowAtLocation(rv_MarketApp, GravityFlags.Center, 0, 0);
        }

        private void OnPopDismiss()
        {
            if (v_Mask != null)
                v_Mask.Visibility = ViewStates.Gone;
        }

        private class MyPopWindow : PopupWindow
        {
            public Action dismissCallBack;

            public MyPopWindow()
            {
            }

            public MyPopWindow(Context context) : base(context)
            {
            }

            public MyPopWindow(View contentView) : base(contentView)
            {
            }

            public MyPopWindow(Context context, IAttributeSet attrs) : base(context, attrs)
            {
            }

            public MyPopWindow(int width, int height) : base(width, height)
            {
            }

            public MyPopWindow(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
            {
            }

            public MyPopWindow(View contentView, int width, int height) : base(contentView, width, height)
            {
            }

            public MyPopWindow(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
            {
            }

            public MyPopWindow(View contentView, int width, int height, bool focusable) : base(contentView, width, height, focusable)
            {
            }

            protected MyPopWindow(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
            {
            }

            public override void Dismiss()
            {
                dismissCallBack?.Invoke();
                base.Dismiss();
            }
        }
        #endregion

        #region 适配器
        /// <summary>
        /// 应用市场适配器
        /// </summary>
        private class MarkerAppApdater : Ys.BeLazy.YsBaseRvAdapter<Md_MarketApk>
        {
            public MarkerAppApdater(Context context)
            {
                this.context = context;
            }

            public override Md_MarketApk this[int position] { get { return list_data[position]; } }

            protected override void AbOnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
            {
                var holder = viewHolder as ViewHolder;
                var data = list_data[position];

                holder.tv_Name.Text = data.Name;
                holder.tv_Intro.Text = data.Intro;
                holder.tv_Maker.Text = data.Maker;

                ImageLoader.Instance.DisplayImage(data.PicPath, holder.iv_Icon, ImageLoaderHelper.GeneralImageOption());

                if (data.DownLoadProgress > 0 && data.DownLoadProgress <= 100)
                {
                    holder.pb_Progress.Visibility = ViewStates.Visible;
                    holder.pb_Progress.Progress = Convert.ToInt32(data.DownLoadProgress);
                }
                else
                {
                    holder.pb_Progress.Visibility = ViewStates.Invisible;
                    holder.pb_Progress.Progress = 0;
                }

            }

            protected override RecyclerView.ViewHolder AbOnCreateViewHolder(ViewGroup parent, int viewType)
            {
                var jk = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_market_application, parent, false);
                return new ViewHolder(jk);
            }

            private class ViewHolder : RecyclerView.ViewHolder
            {
                public AppCompatTextView tv_Name { get; set; }
                public AppCompatTextView tv_Maker { get; set; }
                public AppCompatTextView tv_Intro { get; set; }
                public AppCompatImageView iv_Icon { get; set; }
                public ProgressBar pb_Progress { get; set; }
                public ViewHolder(View itemView) : base(itemView)
                {
                    tv_Name = ItemView.FindViewById<AppCompatTextView>(Resource.Id.tv_Name);
                    tv_Maker = ItemView.FindViewById<AppCompatTextView>(Resource.Id.tv_Maker);
                    tv_Intro = ItemView.FindViewById<AppCompatTextView>(Resource.Id.tv_Intro);
                    iv_Icon = ItemView.FindViewById<AppCompatImageView>(Resource.Id.iv_Icon);
                    pb_Progress = ItemView.FindViewById<ProgressBar>(Resource.Id.pb_download);
                }
            }

        }
        #endregion

        #region 实体类
        private class Md_MarketApk
        {
            /// <summary>
            /// APP名字
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// APP图标地址
            /// </summary>
            public string PicPath { get; set; }
            /// <summary>
            /// APP介绍
            /// </summary>
            public string Intro { get; set; }
            /// <summary>
            /// APP制作商
            /// </summary>
            public string Maker { get; set; }
            /// <summary>
            /// 下载地址
            /// </summary>
            public string DownLoadPath { get; set; }
            /// <summary>
            /// 下载进度
            /// </summary>
            public double DownLoadProgress { get; set; }
        }

        #endregion

        #region Http相关
        /// <summary>
        /// 获取市场APP
        /// </summary>
        private void HttpGetMarketApplicaiton()
        {
            #region 初始化List
            var list = new List<Md_MarketApk>
            {
                new Md_MarketApk()
                {
                    Name = "抖阴",
                    Maker = "美利坚科技无限公司",
                    Intro = "Boy♂Next♂Door",
                    PicPath = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=1812511178,794527672&fm=27&gp=0.jpg"
                },
                new Md_MarketApk()
                {
                    Name = "手快",
                    Maker = "广东人文化传媒",
                    Intro = "我也是个广东人,所以我们是老乡",
                    PicPath = "https://img.moegirl.org/common/thumb/6/6b/Ef376e17406d8d612ae0003742137a4cfd7aaa7f.jpg/250px-Ef376e17406d8d612ae0003742137a4cfd7aaa7f.jpg"
                },
                new Md_MarketApk()
                {
                    Name = "昨日头条",
                    Maker = "木吉Kazuya株式会社",
                    Intro = "这个世界太乱,昨日头条今日看",
                    PicPath = "https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=3584973490,413986855&fm=27&gp=0.jpg"
                }
            };
            #endregion
            adapter_market.SetDataList(list);
        }
        #endregion


        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
                if (e.Action == KeyEventActions.Up)
                    if (popWindow != null)
                        if (popWindow.IsShowing)
                            popWindow.Dismiss();

            return base.OnKeyUp(keyCode, e);
        }
    }
}