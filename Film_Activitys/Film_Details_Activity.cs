using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using IdoMaster_GensouWorld.Film_Adapters;
using IMAS.OkHttp.Bases;
using IMAS.Tips.Logic.HttpRemoteManager;
using Com.Ksyun.Media.Player;
using IdoMaster_GensouWorld.Threads;
using Android.Views.Animations;
using IdoMaster_GensouWorld.Adapters;
using IMAS.LocalDBManager.Models;
using static Android.Media.MediaPlayer;
using Android.Media;
using IdoMaster_GensouWorld.Listeners;
using Android.Content.PM;
using IdoMaster_GensouWorld.Utils;

namespace IdoMaster_GensouWorld.Film_Activitys
{
    /// <summary>
    /// 播放视频页面
    /// </summary>
    [Activity(Label = "Film_Details_Activity", Theme = "@style/Theme.PublicTheme", ConfigurationChanges =
        Android.Content.PM.ConfigChanges.Orientation |
        Android.Content.PM.ConfigChanges.ScreenSize |
        Android.Content.PM.ConfigChanges.Keyboard |
        Android.Content.PM.ConfigChanges.KeyboardHidden
        )]
    public class Film_Details_Activity : BaseActivity
    {
        #region UI控件
        /// <summary>
        /// 返回
        /// </summary>
        private ImageView iv_back;
        /// <summary>
        /// 播放/暂停
        /// </summary>
        private ImageView iv_play_or_stop;
        /// <summary>
        /// 全屏
        /// </summary>
        private ImageView iv_full_screen;
        /// <summary>
        /// 等待圆圈
        /// </summary>
        private ImageView iv_wait;
        /// <summary>
        /// 准备播放
        /// </summary>
        private ImageView iv_prepera;
        /// <summary>
        /// 播放进度(文字)
        /// </summary>
        private TextView tv_seek;
        /// <summary>
        /// 视频标题
        /// </summary>
        private TextView tv_title;
        /// <summary>
        /// 展开简介
        /// </summary>
        private TextView tv_open_intro;
        /// <summary>
        /// 视频简介
        /// </summary>
        private TextView tv_intro;
        /// <summary>
        /// 重试
        /// </summary>
        private TextView tv_retry;
        /// <summary>
        /// 主要演员
        /// </summary>
        private TextView tv_art;
        /// <summary>
        /// 进度条
        /// </summary>
        private SeekBar sb_seek;
        /// <summary>
        /// 视频播放容器控件
        /// </summary>
        private RelativeLayout rl_video;
        /// <summary>
        /// 菜单控件容器
        /// </summary>
        private RelativeLayout rl_menu;
        /// <summary>
        /// 等待控件容器
        /// </summary>
        private RelativeLayout rl_wait;
        /// <summary>
        /// 全屏时隐藏其他信息布局
        /// </summary>
        private LinearLayout ll_info;
        /// <summary>
        /// 错误控件容器
        /// </summary>
        private LinearLayout ll_error;
        /// <summary>
        /// 集数列表
        /// </summary>
        private RecyclerView rc_episodes;

        /// <summary>
        /// 两位父亲
        /// </summary>
        private RelativeLayout rl_fahter;
        private ScrollView sv_fahter;
        /// <summary>
        /// 儿子布局
        /// </summary>
        private RelativeLayout rl_child;
        #endregion
        #region 杂项
        /// <summary>
        /// 集数适配器
        /// </summary>
        private Film_FilmEpisodes_Adapter adapter_es;
        /// <summary>
        /// 进度条回调
        /// </summary>
        private YsSeekChangeListener onSeekChangeListener;
        #endregion
        #region Handler&Runnable
        /// <summary>
        /// Handler对象
        /// </summary>
        private YurishBaseiHandler mHandler;
        /// <summary>
        /// 线程1,专门检查是否正在播放
        /// </summary>
        private NormalRunnable mRunnable_Alpha;
        /// <summary>
        /// 线程2,专门隐藏播放界面
        /// </summary>
        private NormalRunnable mRunnable_Beta;
        /// <summary>
        /// 隐藏菜单消息
        /// 代号:林
        /// </summary>
        private const int MSG_COVER_MENU = 0x0010;
        /// <summary>
        /// 重新连接播放器
        /// 代码:火
        /// </summary>
        private const int MSG_RETRY_PLAYER = 0x0020;
        /// <summary>
        /// 检查视频是否正在播放
        /// 代号:风
        /// </summary>
        private const int MSG_CHECK_VIDEO_IS_PLAY = 0x0030;
        /// <summary>
        /// 更新进度条
        /// 代号:水
        /// </summary>
        private const int MSG_UPDATE_SEEK_PROGRESS = 0x0040;
        #endregion


        #region 金山云播放相关
        /// <summary>
        /// 金山云播放器对象
        /// </summary>
        private KSYTextureView mVideoView = null;
        /// <summary>
        /// 错误Catch
        /// </summary>
        private KsyAzEventCallBack azEventCallBack;
        //private IOnPreparedListener onPreparedListener;
        //private IOnCompletionListener onCompletionListener;
        //private IOnSeekCompleteListener onSeekCompleteListener;
        #endregion

        #region 变量
        /// <summary>
        /// 简介是否是展开状态
        /// </summary>
        private bool isOpenIntro = false;
        ///// <summary>
        ///// 目前正在播放的集数
        ///// </summary>
        //private int PlayingIndex;
        /// <summary>
        /// 视频唯一Id
        /// </summary>
        private string videoHref;
        /// <summary>
        /// 视频播放地址
        /// </summary>
        private string rPath;
        /// <summary>
        /// 是否第一次进入
        /// </summary>
        private bool isFirstComein = true;
        /// <summary>
        /// 距离上次点击的时间
        /// </summary>
        private long mClickTime = 0;
        /// <summary>
        /// 重试连接次数
        /// </summary>
        private int retryCount = 0;
        #endregion

        public override int A_GetContentViewId()
        {
            return Resource.Layout.film_activity_film_details;
        }

        public override void B_BeforeInitView()
        {
            videoHref = Intent.GetStringExtra("value");
            //初始化线程
            mHandler = new YurishBaseiHandler();
            mRunnable_Alpha = new NormalRunnable(mHandler, MSG_CHECK_VIDEO_IS_PLAY);
            mRunnable_Beta = new NormalRunnable(mHandler, MSG_COVER_MENU);
            mHandler.handlerAction -= RunHandlerAction;
            mHandler.handlerAction += RunHandlerAction;
        }

        public override void C_InitView()
        {
            iv_back = FindViewById<ImageView>(Resource.Id.iv_back);
            iv_play_or_stop = FindViewById<ImageView>(Resource.Id.iv_play);
            iv_full_screen = FindViewById<ImageView>(Resource.Id.iv_full_screen);
            iv_wait = FindViewById<ImageView>(Resource.Id.iv_wait);
            iv_prepera = FindViewById<ImageView>(Resource.Id.iv_prepare);
            tv_seek = FindViewById<TextView>(Resource.Id.tv_progress);
            tv_title = FindViewById<TextView>(Resource.Id.tv_title);
            tv_intro = FindViewById<TextView>(Resource.Id.tv_intro);
            tv_art = FindViewById<TextView>(Resource.Id.tv_art);
            tv_retry = FindViewById<TextView>(Resource.Id.tv_retry);
            tv_open_intro = FindViewById<TextView>(Resource.Id.tv_open_intro);
            sb_seek = FindViewById<SeekBar>(Resource.Id.sb_progress);
            rl_video = FindViewById<RelativeLayout>(Resource.Id.rl_video);
            rl_menu = FindViewById<RelativeLayout>(Resource.Id.rl_menu);
            rl_wait = FindViewById<RelativeLayout>(Resource.Id.rl_wait);
            ll_info = FindViewById<LinearLayout>(Resource.Id.ll_info);
            ll_error = FindViewById<LinearLayout>(Resource.Id.ll_error);
            rc_episodes = FindViewById<RecyclerView>(Resource.Id.rc_episodes);

            rl_fahter = FindViewById<RelativeLayout>(Resource.Id.rl_father);
            sv_fahter = FindViewById<ScrollView>(Resource.Id.sv_father);

            rl_child = FindViewById<RelativeLayout>(Resource.Id.rl_child);

            rc_episodes.SetLayoutManager(new StaggeredGridLayoutManager(4, StaggeredGridLayoutManager.Horizontal));
            rc_episodes.AddItemDecoration(new Z_RecyclerViewAsStaggered_Spacing(0, 5, 10, 0));
            tv_retry.Paint.Flags = Android.Graphics.PaintFlags.UnderlineText;

        }

        public override void D_BindEvent()
        {
            iv_back.Click += OnClickListener;
            iv_play_or_stop.Click += OnClickListener;
            iv_full_screen.Click += OnClickListener;
            iv_prepera.Click += OnClickListener;
            tv_open_intro.Click += OnClickListener;
            rl_video.Click += OnClickListener;
            tv_retry.Click += OnClickListener;
            InitOnSeekChangeListener();

        }

        public override void E_InitData()
        {
            //初始化适配器
            adapter_es = new Film_FilmEpisodes_Adapter(this);
            adapter_es.onItemClickAct -= OnRVItemClickListener;
            adapter_es.onItemClickAct += OnRVItemClickListener;
            rc_episodes.SetAdapter(adapter_es);

            ///初始化播放器回调
            InitPlayerCallBack();

            HttpGetVideoInfo();
            //rPath = "http://yingshi.yazyzw.com/20171010/A2sCXesp/index.m3u8";
            //InitKSYPlayer();
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
            switch (v.Id)
            {
                case Resource.Id.iv_back:///返回
                    {
                        if (isFullScreenOrNot)
                            ChangeTheScreenFull();
                        else
                            QuitThePage();
                    }
                    break;
                case Resource.Id.tv_retry:///重试
                    {
                        HideErrorLayout();
                        ShowWaitAnime();
                        SendRetryPlayerMsg();
                    }
                    break;
                case Resource.Id.iv_play:///播放/暂停
                    {
                        var b = iv_play_or_stop.Selected;
                        if (b)
                        {
                            //暂停
                            if (IsPlaying())
                                StopPlayer();
                        }
                        else
                        {
                            //播放
                            if (mVideoView != null)
                                StartPlayer();
                        }
                        iv_play_or_stop.Selected = !b;
                    }
                    break;
                case Resource.Id.iv_prepare:
                    {
                        //开始准备视频
                        ShowWaitAnime();
                        CoverUIControl(CoverFlag.Gone, iv_prepera);
                        //HideMenu(CoverFlag.Gone);
                        InitKSYPlayer();
                        iv_play_or_stop.Selected = true;
                    }
                    break;
                case Resource.Id.iv_full_screen:///全屏
                    {
                        //if (IsPlaying())
                        //{
                        //    mVideoView.Pause();
                        //}
                        //var bundler = new Bundle();
                        //var dicition = new Dictionary<string, Object>()
                        //{
                        //    {"video",mVideoView },
                        //    {"iv_back",iv_back },
                        //    {"iv_wait",iv_wait },
                        //    {"iv_play_or_stop",iv_play_or_stop },
                        //    {"iv_full_screen",iv_full_screen },
                        //    {"tv_seek",tv_seek },
                        //    {"tv_retry",tv_retry },
                        //    {"sb_seek",sb_seek },
                        //    {"rl_video",rl_video },
                        //    {"rl_menu",rl_menu },
                        //    {"rl_wait",rl_wait },
                        //    {"ll_error",ll_error }
                        //};
                        //var pop_FullScreen = new PopWindow_FullScreenPalyer(this, dicition);
                        //pop_FullScreen.ShowPopWindow(iv_back);

                        try
                        {
                            ChangeTheScreenFull();
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    break;
                case Resource.Id.tv_open_intro:///展开&关闭简介
                    {
                        if (!isOpenIntro)
                        {
                            //展开简介
                            tv_open_intro.Text = "关闭展开";
                            CoverUIControl(CoverFlag.Visible, tv_intro);
                        }
                        else
                        {
                            //关闭简介
                            tv_open_intro.Text = "展开简介";
                            CoverUIControl(CoverFlag.Gone, tv_intro);
                        }
                        isOpenIntro = !isOpenIntro;
                    }
                    break;

                case Resource.Id.rl_video:///点击品目
                    {
                        if (Java.Lang.JavaSystem.CurrentTimeMillis() - mClickTime < 200)
                        {
                            if (!IsPlaying())
                                return;
                            //双击操作
                            ChangeTheScreenFull();

                        }
                        else
                        {
                            //单击操作
                            mClickTime = Java.Lang.JavaSystem.CurrentTimeMillis();
                            if (rl_menu.Visibility == ViewStates.Invisible)
                            {
                                StartHideTimer();
                                HideMenu(CoverFlag.Visible);
                            }
                            else
                            {
                                HideMenu(CoverFlag.Invisible);
                            }

                        }

                    }
                    break;
            }

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }

        public void OnRVItemClickListener(View sender, int position)
        {
            var clickItem = adapter_es[position];
            adapter_es.SetPlayerIndex(position);
            HttpGetVideoPlayUrl(clickItem.Href, clickItem.Name, true);

        }

        /// <summary>
        /// Handler消息接收处理 
        /// </summary>
        private void RunHandlerAction(Message msg)
        {
            switch (msg.What)
            {
                case MSG_RETRY_PLAYER:
                    {
                        if (retryCount >= 8)
                        {
                            ShowErrorLayout();
                            retryCount = 0;
                            return;
                        }
                        CleanTheKSYMedia();
                        if (rl_wait.Visibility == ViewStates.Gone)
                        {
                            ShowWaitAnime();
                        }
                        InitKSYPlayer();
                        retryCount++;
                    }
                    break;
                case MSG_COVER_MENU:
                    {
                        CoverUIControl(CoverFlag.Invisible, rl_menu);
                    }
                    break;
                case MSG_CHECK_VIDEO_IS_PLAY:
                    {
                        if (mVideoView != null)
                        {
                            if (mVideoView.IsPlaying)
                            {
                                HideWaitAnime();
                                mHandler.RemoveMessages(MSG_RETRY_PLAYER);
                            }
                        }
                        mHandler.PostDelayed(mRunnable_Alpha, 3000);
                    }
                    break;
                case MSG_UPDATE_SEEK_PROGRESS:
                    {
                        mHandler.SendMessageDelayed(mHandler.ObtainMessage(MSG_UPDATE_SEEK_PROGRESS), 1 * 1000);
                        UpdateProgreedInfo();
                    }
                    break;
            }
        }

        #region 全屏操作
        /// <summary>
        /// 是否是全屏
        /// </summary>
        private bool isFullScreenOrNot = false;
        /// <summary>
        /// 是否开启了重力检测
        /// </summary>
        private bool isGravityOpen = false;
        /// <summary>
        /// 切换全屏与半屏幕
        /// </summary>
        private void ChangeTheScreenFull()
        {
            if (isFullScreenOrNot)
                //取消全屏
                SetPortraitAndLandscape(false);
            else
                //切换全屏
                SetPortraitAndLandscape(true);
        }
        /// 根据输入的布尔值来切换全屏播放或是小屏播放
        /// </summary>
        /// <param name="bo">true:切换横屏,false:切换竖屏</param>
        private void SetPortraitAndLandscape(bool bo)
        {
            //rl_video.RemoveView(mVideoView);
            if (bo)
            {
                ///屏幕横屏相关设置
                if (isGravityOpen)
                    RequestedOrientation = ScreenOrientation.FullSensor;
                else
                    RequestedOrientation = ScreenOrientation.Landscape;
                //HideOtherUiWidget(true);
                rl_video.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                CoverUIControl(CoverFlag.Gone, ll_info, sv_fahter);
                CoverUIControl(CoverFlag.Visible, rl_fahter);
                sv_fahter.RemoveView(rl_child);
                rl_fahter.AddView(rl_child);
                isFullScreenOrNot = true;
            }
            else
            {
                ///屏幕竖屏相关设置
                if (isGravityOpen)
                    RequestedOrientation = ScreenOrientation.FullSensor;
                else
                    RequestedOrientation = ScreenOrientation.Portrait;
                //LinearLayout.LayoutParams lp = rl_video.LayoutParameters;
                //HideOtherUiWidget(false);
                rl_video.LayoutParameters = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, (int)Resources.GetDimension(Resource.Dimension.media_player_height));
                CoverUIControl(CoverFlag.Visible, ll_info, sv_fahter);
                CoverUIControl(CoverFlag.Gone, rl_fahter);
                rl_fahter.RemoveView(rl_child);
                sv_fahter.AddView(rl_child);
                isFullScreenOrNot = false;
            }
            //rl_video.AddView(mVideoView);
        }

        #endregion

        #region 隐藏操作页面

        /// <summary>
        /// 隐藏操作菜单
        /// </summary>
        /// <param name="bo">true:隐藏  false:显示</param>
        private void HideMenu(CoverFlag flag)
        {
            CoverUIControl(flag, rl_menu, iv_play_or_stop);
        }

        /// <summary>
        /// 开始计时,三秒后执行runable
        /// </summary>
        private void StartHideTimer()
        {
            mHandler.RemoveMessages(MSG_COVER_MENU);
            if (rl_menu.Visibility == ViewStates.Invisible)
            {
                HideMenu(CoverFlag.Visible);
            }
            mHandler.SendMessageDelayed(mHandler.ObtainMessage(MSG_COVER_MENU), 5000);
        }

        /// <summary>
        /// 移除Msg,不在计时
        /// </summary>
        private void EndHideTimer()
        {
            mHandler.RemoveMessages(MSG_COVER_MENU);
        }

        /// <summary>
        /// 重置计时
        /// </summary>
        private void ResetHideTimer()
        {
            mHandler.RemoveMessages(MSG_COVER_MENU);
            mHandler.SendMessageDelayed(mHandler.ObtainMessage(MSG_COVER_MENU), 5000);
        }

        #endregion

        /// <summary>
        /// 退出此页面
        /// </summary>
        private void QuitThePage()
        {
            this.Finish();
        }

        /// <summary>
        /// 初始化金山云播放器
        /// </summary>
        private void InitKSYPlayer()
        {
            if (mVideoView != null)
                rl_video.RemoveView(mVideoView);
            mVideoView = new KSYTextureView(this)
            {
                //Id = IMAS_Constants.KsyTextureViewId,
                KeepScreenOn = true,
                BufferTimeMax = 2,
                DataSource = rPath,
            };
            mVideoView.SetBufferSize(15);
            mVideoView.SetTimeout(15, 30);
            mVideoView.SetRotateDegree(90);
            mVideoView.SetVideoScalingMode(0);
            mVideoView.ShouldAutoPlay(false);//关闭自动开播功能
            mVideoView.PrepareAsync();
            KSYTextureViewManager.Instance.SetHardWareDecodeMode(mVideoView);
            //KSYTextureViewManager.Instance.SetOnErrorListener(mVideoView, errorListennerImp);
            KSYTextureViewManager.Instance.SetAzEventListener(mVideoView, azEventCallBack);
            mVideoView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            //SetFilmTouchEvent();
            rl_video.AddView(mVideoView, 0);
            //mVideoView.SeekTo(5000);

        }

        /// <summary>
        /// 检测影片是否在播放
        /// </summary>
        /// <returns></returns>
        private bool IsPlaying()
        {
            var bo = false;
            if (mVideoView != null)
            {
                if (mVideoView.IsPlaying)
                {
                    bo = true;
                }
            }
            return bo;
        }

        /// <summary>
        /// 设置点击播放
        /// </summary>
        private void SetFilmTouchEvent()
        {
            mVideoView.Click -= OnClickListener;
            mVideoView.Click += OnClickListener;
        }
        /// <summary>
        /// 清理播放器
        /// </summary>
        private void CleanTheKSYMedia()
        {
            if (mVideoView != null)
            {
                mVideoView.Release();
                mVideoView.Dispose();
            }
            mVideoView = null;
            GC.Collect();
        }

        #region 播放暂停控制
        /// <summary>
        /// 开始视频播放
        /// </summary>
        private void StartPlayer()
        {
            mHandler.SendMessageDelayed(mHandler.ObtainMessage(MSG_UPDATE_SEEK_PROGRESS), 1 * 1000);
            if (mVideoView != null)
                mVideoView.Start();
        }
        /// <summary>
        /// 暂停视频播放
        /// </summary>
        private void StopPlayer()
        {
            mHandler.RemoveMessages(MSG_UPDATE_SEEK_PROGRESS);
            if (mVideoView != null)
                mVideoView.Pause();
        }
        #endregion

        #region 播放等待动画控制
        /// <summary>
        /// 显示等待动画
        /// </summary>
        private void ShowWaitAnime()
        {
            CoverUIControl(CoverFlag.Visible, rl_wait);
            var anime = AnimationUtils.LoadAnimation(this, Resource.Animation.anime_video_wait);
            var lin = new LinearInterpolator();
            anime.Interpolator = lin;
            if (anime != null)
            {
                iv_wait.StartAnimation(anime);
            }
        }
        /// <summary>
        /// 隐藏等待动画
        /// </summary>
        private void HideWaitAnime()
        {
            CoverUIControl(CoverFlag.Gone, rl_wait);
            iv_wait.ClearAnimation();
        }
        /// <summary>
        /// 显示错误布局
        /// </summary>
        private void ShowErrorLayout()
        {
            CoverUIControl(CoverFlag.Visible, ll_error);
        }
        /// <summary>
        /// 显示错误布局
        /// </summary>
        private void HideErrorLayout()
        {
            CoverUIControl(CoverFlag.Gone, ll_error);
        }
        #endregion

        #region Handler消息发送
        /// <summary>
        /// 发送隐藏菜单消息
        /// </summary>
        /// <param name="flag">true:停止发送,false:开始发送</param>
        private void SendCoverMenuMsg(bool flag)
        {
            if (flag)
            {
                mHandler.RemoveMessages(MSG_COVER_MENU);
            }
            else
            {
                mHandler.SendMessageDelayed(mHandler.ObtainMessage(MSG_COVER_MENU), 5000);

            }
        }
        /// <summary>
        /// 发送重连播放器消息
        /// </summary>
        private void SendRetryPlayerMsg()
        {
            mHandler.SendMessageDelayed(mHandler.ObtainMessage(MSG_RETRY_PLAYER), 3000);
        }

        #endregion

        #region 播放进度控制
        /// <summary>
        /// 初始化进度
        /// </summary>
        private void InitOnSeekChangeListener()
        {
            onSeekChangeListener = new YsSeekChangeListener();
            onSeekChangeListener.Act_OnProgressChanged -= OnProgressChanged;
            onSeekChangeListener.Act_OnStartTrackingTouch -= OnStartTrackingTouch;
            onSeekChangeListener.Act_OnStopTrackingTouch -= OnStopTrackingTouch;

            onSeekChangeListener.Act_OnProgressChanged += OnProgressChanged;
            onSeekChangeListener.Act_OnStartTrackingTouch += OnStartTrackingTouch;
            onSeekChangeListener.Act_OnStopTrackingTouch += OnStopTrackingTouch;

            sb_seek.SetOnSeekBarChangeListener(onSeekChangeListener);
        }
        /// <summary>
        /// 总时长
        /// </summary>
        private long totalTime;

        /// <summary>
        /// 设置初始进度信息
        /// </summary>
        private void SetProgreesInfo()
        {
            var progrees = mVideoView.Duration;//获取视频总时长毫秒
            totalTime = progrees;
            var jk = $"{00} : {00}/{FormatLongToTimeStr(progrees)}";
            tv_seek.Text = jk;
            sb_seek.Max = Convert.ToInt32(progrees);
        }

        /// <summary>
        /// 更新视频信息
        /// </summary>
        private void UpdateProgreedInfo()
        {
            sb_seek.Progress = Convert.ToInt32(mVideoView.CurrentPosition);
            var positionTime = mVideoView.CurrentPosition;
            var jk = $"{FormatLongToTimeStr(positionTime)}/{FormatLongToTimeStr(totalTime)}";
            tv_seek.Text = jk;
        }

        private string FormatLongToTimeStr(long duration)
        {
            var ts = new TimeSpan(0, 0, Convert.ToInt32(duration / 1000));
            var str = "";
            var s = ts.Seconds;
            var m = ts.Minutes;
            if (ts.Hours > 0)
            {
                var jk = ts.Hours * 60;
                if (s < 10)
                    str = $"{m * jk} : 0{s}";
                if (m < 10)
                    str = $"0{m * jk} : {s}";
                if (s < 10 && m < 10)
                    str = $"0{m * jk} : 0{s}";
            }
            else
            {
                if (s < 10)
                    str = $"{m } : 0{s}";
                if (m < 10)
                    str = $"0{m} : {s}";
                if (s < 10 && m < 10)
                    str = $"0{m} : 0{s}";
                //str = $"{ts.Minutes} : {ts.Seconds}";
            }
            return str;
        }

        private void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            Console.Write($"SeekBar____Changed{progress}=={fromUser}");
        }

        private void OnStartTrackingTouch(SeekBar seekBar)
        {

        }

        private void OnStopTrackingTouch(SeekBar seekBar)
        {

        }

        #endregion

        #region Http相关
        /// <summary>
        /// 获取视频详情
        /// </summary>
        /// <param name="href"></param>
        private void HttpGetVideoInfo()
        {
            ShowWaitDiaLog("请稍等//...");
            Task.Run(async () =>
            {
                var result = await FilmApiHttpProxys.GetInstance().GetVideoInfo(videoHref);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    Console.WriteLine("线程异常");
                    return;
                }
                if (t.Result.IsSuccess)
                {
                    var jk = t.Result.Data;
                    tv_art.Text = $"主演:{jk.Starring}";
                    tv_intro.Text = jk.Intro;
                    tv_title.Text = jk.Name;


                    if (jk.PlayList.Any())
                        CoverUIControl(CoverFlag.Visible, rc_episodes);

                    var dataList = jk.PlayList;
                    dataList.Reverse();
                    adapter_es.SetDataList(dataList, 0);
                    var data = jk.PlayList[0];

                    HttpGetVideoPlayUrl(data.Href, data.Name);
                }
                else
                {
                    if (t.Result.Code == OkHttpBase.LocalExceptionState)
                    {
                        ShowMsgShort("网络异常，请检查网络");
                    }
                    else if (t.Result.Code == OkHttpBase.WebExceptionState)
                    {
                        ShowMsgShort("服务器连接异常");
                    }
                    else
                    {
                        ShowMsgShort($"{t.Result.Message}");
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        /// <summary>
        /// 获取播放地址
        /// </summary>
        private void HttpGetVideoPlayUrl(string href, string name, bool isChange = false)
        {
            Task.Run(async () =>
            {
                var result = await FilmApiHttpProxys.GetInstance().GetPlayResources(href, name);
                return result;
            }).ContinueWith(t =>
            {
                HideWaitDiaLog();
                if (t.Exception != null)
                {
                    Console.WriteLine("线程异常");
                    return;
                }
                if (t.Result.IsSuccess)
                {
                    rPath = t.Result.Data;
                    if (isChange)
                    {
                        CleanTheKSYMedia();
                        InitKSYPlayer();
                    }

                }
                else
                {
                    if (t.Result.Code == OkHttpBase.LocalExceptionState)
                    {
                        ShowMsgShort("网络异常，请检查网络");
                    }
                    else if (t.Result.Code == OkHttpBase.WebExceptionState)
                    {
                        ShowMsgShort("服务器连接异常");
                    }
                    else
                    {
                        ShowMsgShort($"{t.Result.Message}");
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        #region 生命周期
        protected override void OnResume()
        {
            base.OnResume();
            //每五秒发送一条检测视频是否正常播放的消息
            mHandler.PostDelayed(mRunnable_Alpha, 5000);
            isGravityOpen = Android.Provider.Settings.System.GetInt(this.ContentResolver, Android.Provider.Settings.System.AccelerometerRotation) == 1 ? true : false;
        }
        protected override void OnPause()
        {
            base.OnPause();
            if (mVideoView != null && mVideoView.IsPlaying)
            {
                mVideoView.Pause();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            try
            {
                mHandler.RemoveCallbacksAndMessages(null);
                if (mVideoView != null)
                {
                    rl_video.RemoveView(mVideoView);
                    Task.Run(() =>
                    {
                        try
                        {
                            mVideoView?.Release();
                            mVideoView?.Stop();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("销毁异常: " + ex.Message);
                        }
                    });
                    mVideoView = null;
                }
            }
            catch (Exception e)
            {

            }
        }
        #endregion

        #region 播放器回调
        /// <summary>
        /// 初始化播放器回调
        /// </summary>
        private void InitPlayerCallBack()
        {
            azEventCallBack = new KsyAzEventCallBack(OnError, OnInfo);

            azEventCallBack.Act_OnBufferingUpdate -= OnBufferingUpdate;
            azEventCallBack.Act_OnCompletion -= OnCompletion;
            azEventCallBack.Act_OnPrepared -= OnPrepared;
            azEventCallBack.Act_OnSeekComplete -= OnSeekComplete;
            azEventCallBack.Act_OnVideoSizeChanged -= OnVideoSizeChanged;

            azEventCallBack.Act_OnBufferingUpdate += OnBufferingUpdate;
            azEventCallBack.Act_OnCompletion += OnCompletion;
            azEventCallBack.Act_OnPrepared += OnPrepared;
            azEventCallBack.Act_OnSeekComplete += OnSeekComplete;
            azEventCallBack.Act_OnVideoSizeChanged += OnVideoSizeChanged;
        }

        private void OnBufferingUpdate(int p0)
        {

        }

        private void OnCompletion()
        {

        }

        private bool OnError(int p0, int p1)
        {
            CleanTheKSYMedia();
            SendRetryPlayerMsg();
            return false;
        }

        private bool OnInfo(int p0, int p1)
        {
            if (p0 == 40020)
            {
                if (mVideoView != null)
                    mVideoView.Reload(rPath, false);
            }
            return false;
        }

        private void OnPrepared()
        {
            SetProgreesInfo();
            StartPlayer();
            HideWaitAnime();
            HideMenu(CoverFlag.Visible);
        }

        private void OnSeekComplete()
        {

        }

        private void OnVideoSizeChanged(int width, int height, int sarNum, int sarDen)
        {

        }

        #endregion

    }
}
