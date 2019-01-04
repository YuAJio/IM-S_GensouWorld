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
using IMAS.Utils.Files;
using IMAS.OkHttp.Download;
using System.Threading.Tasks;
using IMAS.Tips.Logic.LocalDBManager;
using IdoMaster_GensouWorld.Threads;
using Android;
using Android.Support.V4.App;

namespace IdoMaster_GensouWorld.Activitys.Test.OtherToooooools.FakeDesktable
{
    /// <summary>
    /// U称市场页面
    /// </summary>
    [Activity(Label = "Acty_UChengMarket", Theme = "@style/Theme.PublicThemePlus")]
    public class Acty_UChengMarket : Ys.BeLazy.YsBaseActivity, IProgressListener
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
            mHandler = new YurishBaseiHandler();
            mHandler.handlerAction -= DownloadHandMessageAction;
            mHandler.handlerAction += DownloadHandMessageAction;
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
                        SaveApkDownLoadFile();
                        popWindow.Dismiss();
                    }
                    break;
            }
        }

        public override void G_SomethingElse()
        {

        }

        #region 计时器相关
        private A_Timer ysTimer;
        private void StartTimeCount()
        {
            var view = rv_MarketApp.GetChildAt(clickPosition);
            if (view != null)
            {
                var pb_Pro = view.FindViewById<ProgressBar>(Resource.Id.pb_download);
                if (pb_Pro != null)
                {
                    ysTimer = new A_Timer(this, pb_Pro, 100);
                    ysTimer.StartCount();
                }
            }

        }

        private class A_Timer
        {
            private Activity context;
            private ProgressBar pb_Progress;

            public double DownloadProgress { private get; set; }
            /// <summary>
            /// 总大小
            /// </summary>
            private double totalFileSize;

            private Timer ys_Timer;

            public A_Timer(Activity context, ProgressBar progressBar, double totalFileSize)
            {
                this.pb_Progress = progressBar;
                this.totalFileSize = totalFileSize;
                this.context = context;

                ys_Timer = new Timer()
                {
                    Interval = 1 * 1000,
                    Enabled = true,
                    AutoReset = true,
                };

                ys_Timer.Elapsed -= ItIsTimeToDoSomeThing;
                ys_Timer.Elapsed += ItIsTimeToDoSomeThing;

                ys_Timer.Disposed -= ItIsCloseTime;
                ys_Timer.Disposed += ItIsCloseTime;

            }

            #region 外部调用方法
            /// <summary>
            /// 开始计时
            /// </summary>
            public void StartCount()
            {
                this.ys_Timer.Start();
            }
            /// <summary>
            /// 取消计时
            /// </summary>
            public void Dispost()
            {
                this.ys_Timer.Close();
                this.ys_Timer.Dispose();
            }
            #endregion


            /// <summary>
            /// 达到时间间隔做的事
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void ItIsTimeToDoSomeThing(object sender, ElapsedEventArgs e)
            {
                var rd = new Random();
                DownloadProgress += rd.NextDouble();

                context.RunOnUiThread(() =>
                {
                    if (pb_Progress != null)
                    {
                        pb_Progress.Visibility = ViewStates.Visible;
                        pb_Progress.Progress = Convert.ToInt32(DownloadProgress);
                    }
                });

                //if (DownloadProgress >= totalFileSize)
                //{//下载完成
                //    ys_Timer.Dispose();
                //    ys_Timer.Close();
                //}

            }

            private void ItIsCloseTime(object sender, EventArgs e)
            {//时间结束
                pb_Progress.Visibility = ViewStates.Invisible;
                pb_Progress.Progress = 0;
            }

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
                    Name = "侍魂：胧月传说",
                    Maker = "腾讯科技（上海）有限公司",
                    Intro = "《侍魂：胧月传说》是一款日式和风3D动作手游（ARPG）。游戏中流畅的战斗闯关玩法，带来激爽的刷图快感，更有经典深渊副本挑战，掉落极品史诗套装。剑士、巫女、忍者、游侠4大日式经典流派各具特色，8种转职技能组合丰富，更有觉醒后的终极奥义，全屏炫酷大招，释放武者之魂。首创十三键位双层技能盘，挑战连招极限，组合自己的独门必杀。更有多人组队玩法，挑战高难度团本。",
                    PicPath = "https://android-artworks.25pp.com/fs08/2018/12/03/9/109_df56b97594127c6a73b2a613f53b7369_con_130x130.png",
                    DownLoadPath="https://www.wandoujia.com/apps/com.tencent.shihun.android/download/dot?ch=detail_normal_dl"
                },
                new Md_MarketApk()
                {
                    Name = "手快",
                    Maker = "广东人文化传媒",
                    Intro = "我也是个广东人,所以我们是老乡",
                    PicPath = "https://img.moegirl.org/common/thumb/6/6b/Ef376e17406d8d612ae0003742137a4cfd7aaa7f.jpg/250px-Ef376e17406d8d612ae0003742137a4cfd7aaa7f.jpg"
                    ,DownLoadPath="http://ccapkg.cancanan.cn/EScales/1.0.12.12.apk"
                },
                new Md_MarketApk()
                {
                    Name = "昨日头条",
                    Maker = "木吉Kazuya株式会社",
                    Intro = "这个世界太乱,昨日头条今日看",
                    PicPath = "https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=3584973490,413986855&fm=27&gp=0.jpg",
                    DownLoadPath="http://ccapkg.cancanan.cn/2.0.8.38.apk",
                }
            };
            #endregion
            adapter_market.SetDataList(list);
        }
        #endregion

        #region 下载器相关
        /// <summary>
        /// 通知管理器
        /// </summary>
        private static ProgressDownloader progressDownloader;
        /// <summary>
        /// Handler
        /// </summary>
        private YurishBaseiHandler mHandler;
        #region 数据库操作
        /// <summary>
        /// 将下载记录存入本地
        /// </summary>
        private void SaveApkDownLoadFile()
        {
            var apkInfo = adapter_market[clickPosition];
            ShowWaitDialog_Normal("准备下载中..");
            Task.Run(async () =>
            {
                var jk = apkInfo.DownLoadPath.Substring(apkInfo.DownLoadPath.LastIndexOf('/') + 1);
                var filePath = $"{FilePathManager.GetInstance().GetPrivateRootDirPath()}/DownLoadApk/{jk}";
                var tempFilepath = $"{filePath}.tmp";
                var tag = $"{apkInfo.Maker}=>{apkInfo.Name}";
                var result = await IMAS_ProAppDBManager.GetInstance().InsertDownLoadFileInfo(apkInfo.DownLoadPath, tempFilepath, filePath, tag);
                return result;
            }).ContinueWith(t =>
            {
                HideWaitDiaLog();
                if (t.Exception != null)
                {
                    return;
                }

                if (t.Result.IsSuccess)
                    GetApkDownLoadData(apkInfo.DownLoadPath);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// 获取apk的下载信息并继续下载
        /// </summary>
        /// <param name="url"></param>
        private void GetApkDownLoadData(string url)
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().GetDownLoadFileInfo(url);

                return result;

            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    return;
                }

                if (t.Result.IsSuccess && t.Result.Data != null)
                {
                    var info = t.Result.Data;

                    if (t.Result.Data.DownLoadState == IMAS.OkHttp.Models.DownLoadState.Finished)
                    {
                        //if (isCleanLastPackage)
                        //{
                        //    info.DownLoadState = OkHttpShared.Models.DownLoadState.None;
                        //    info.Now = 0;
                        //    System.IO.File.Delete(info.LocalPath);
                        //}
                        //else
                        IMAS_Application.Sington.YsInstallApk(t.Result.Data.LocalPath);
                    }
                    var downloader = new ProgressDownloader(info.FileUrl, info.DownLoadTag, info.TempLocalPath, this);
                    downloader.Download(info.Now);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

        ///// <summary>
        ///// 开始下载
        ///// </summary>
        ///// <param name="url"></param>
        //private void StartDownloadApk(string url)
        //{
        //    var kk = url.Substring(url.LastIndexOf('/') + 1);
        //    var filePath = $"{FilePathManager.GetInstance().GetPrivateRootDirPath()}/DownLoadApk/{kk}";
        //    var tempFilepath = $"{filePath}.tmp";
        //    new ProgressDownloader(url, tempFilepath, this).Download(0);
        //}

        public void OnPreExecute(ProgressDownloader sender, long contentLength)
        {
            sender.TotalLen = contentLength + sender.StartPoint;
        }

        public async void Update(ProgressDownloader sender, long totalBytes, bool done)
        {
            if (done)
            {//表示下载完成
                //停止下载
                sender.Pause();

                await IMAS_ProAppDBManager.GetInstance().UpdateDownLoadFileInfoState(sender.Url, IMAS.OkHttp.Models.DownLoadState.Finished);

                var result = await IMAS_ProAppDBManager.GetInstance().GetDownLoadFileInfo(sender.Url);
                if (result.IsSuccess && result.Data != null)
                {
                    var ok = ChangeDoneFileName(result.Data.TempLocalPath, result.Data.LocalPath);
                    if (ok)
                    {
                        var msg = new Message();
                        Bundle b = new Bundle();
                        b.PutString("LocalPath", result.Data.LocalPath);
                        b.PutString("tag", result.Data.DownLoadTag);
                        msg.Data = b;
                        msg.What = 10;
                        msg.Arg1 = sender.Id;
                        mHandler.SendMessage(msg);
                    }
                }
                return;
            }
            else
            {
                sender.DTotalBytes = totalBytes + sender.StartPoint;
                sender.SaveFlag++;

                var b = new Bundle();
                b.PutString("tag", sender.Tag);
                mHandler.SendMessage(new Message() { Data = b, What = 0, Arg1 = sender.Id });
                progressDownloader = sender;
                if (sender.SaveFlag >= 50)
                {
                    sender.SaveFlag = 0;
                    var result = await IMAS_ProAppDBManager.GetInstance().UpdateDownLoadFileInfo(sender.Url, sender.DTotalBytes);
                }
            }
        }

        public void UpdateError(ProgressDownloader sender, string errorMsg)
        {
            sender?.Pause();
            sender = null;
            //if (!string.IsNullOrWhiteSpace(mDownloadUrl))
            //{//如果下载地址不是空
            //    GetData(mDownloadUrl);
            //}
            //else
            //如果下载地址是空的,则发送消息
            ShowMsgLong("网络问题,下载APK失败,请重试");
            //}
        }

        /// <summary>
        /// 修改文件后缀名
        /// </summary>
        /// <returns></returns>
        private bool ChangeDoneFileName(string tempFilePath, string filePath)
        {
            try
            {
                var srcDir = new Java.IO.File(tempFilePath);
                bool isOk = srcDir.RenameTo(new Java.IO.File(filePath));
                return isOk;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///下载Handler
        /// </summary>
        private void DownloadHandMessageAction(Message msg)
        {
            if (msg.What == 10)
            {
                var downloadApkPath = msg.Data.GetString("LocalPath");
                //if (isCleanLastPackage)
                //    System.IO.File.Delete(downloadApkPath);
                #region 设置进度条进度满
                var tag = msg.Data.GetString("tag");
                var adapterData = adapter_market.DataList.Where(x => ($"{x.Maker}=>{x.Name}") == tag).FirstOrDefault();
                if (adapterData != null)
                {
                    var index = adapter_market.DataList.IndexOf(adapterData);
                    var pb_progress = rv_MarketApp.GetChildAt(index).FindViewById<ProgressBar>(Resource.Id.pb_download);
                    pb_progress.Visibility = ViewStates.Visible;
                    pb_progress.Progress = 100;
                }
                #endregion

                if (IsPermissionGranted(Manifest.Permission.RequestInstallPackages))
                    IMAS_Application.Sington.YsInstallApk(downloadApkPath/*msg.Data.GetString("LocalPath")*/);
                else
                    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.RequestInstallPackages }, 10040);
            }
            else
            {
                if (progressDownloader != null)
                {
                    var currentPro = (int)(progressDownloader.DTotalBytes) / 1024;
                    var jk = ((progressDownloader.DTotalBytes) * 1.0f / progressDownloader.TotalLen);
                    var tag = msg.Data.GetString("tag");
                    var adapterData = adapter_market.DataList.Where(x => ($"{x.Maker}=>{x.Name}") == tag).FirstOrDefault();
                    if (adapterData != null)
                    {
                        var index = adapter_market.DataList.IndexOf(adapterData);
                        var pb_progress = rv_MarketApp.GetChildAt(index).FindViewById<ProgressBar>(Resource.Id.pb_download);
                        pb_progress.Visibility = ViewStates.Visible;
                        pb_progress.Progress = Convert.ToInt32(jk * 100);
                    }
                }

            }
        }
        #endregion

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
                if (e.Action == KeyEventActions.Up)
                    if (popWindow != null)
                        if (popWindow.IsShowing)
                        {
                            popWindow.Dismiss();
                            return true;
                        }
            return base.OnKeyUp(keyCode, e);
        }

    }
}