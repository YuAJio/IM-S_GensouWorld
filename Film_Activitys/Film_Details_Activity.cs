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
    public class Film_Details_Activity : BaseActivity, IOnPreparedListener, IOnCompletionListener, IOnSeekCompleteListener
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
        /// 集数列表
        /// </summary>
        private RecyclerView rc_episodes;
        #endregion
        #region 杂项
        /// <summary>
        /// 简介是否是展开状态
        /// </summary>
        private bool isOpenIntro = false;
        /// <summary>
        /// 目前正在播放的集数
        /// </summary>
        private int PlayingIndex;
        /// <summary>
        /// 集数适配器
        /// </summary>
        private Film_FilmEpisodes_Adapter adapter_es;
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
        #endregion


        #region 金山云播放相关
        /// <summary>
        /// 金山云播放器对象
        /// </summary>
        private KSYTextureView mVideoView = null;
        /// <summary>
        /// 错误Catch
        /// </summary>
        private MyOnErrorListenerImp errorListennerImp;
        //private IOnPreparedListener onPreparedListener;
        //private IOnCompletionListener onCompletionListener;
        //private IOnSeekCompleteListener onSeekCompleteListener;
        #endregion
        #region 变量
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
            mHandler.handlerAction -= RunHandlerAction;
            mHandler.handlerAction += RunHandlerAction;
        }

        public override void C_InitView()
        {
            iv_back = FindViewById<ImageView>(Resource.Id.iv_back);
            iv_play_or_stop = FindViewById<ImageView>(Resource.Id.iv_play);
            iv_full_screen = FindViewById<ImageView>(Resource.Id.iv_full_screen);
            iv_wait = FindViewById<ImageView>(Resource.Id.iv_wait);
            tv_seek = FindViewById<TextView>(Resource.Id.tv_progress);
            tv_title = FindViewById<TextView>(Resource.Id.tv_title);
            tv_intro = FindViewById<TextView>(Resource.Id.tv_intro);
            tv_art = FindViewById<TextView>(Resource.Id.tv_art);
            tv_open_intro = FindViewById<TextView>(Resource.Id.tv_open_intro);
            sb_seek = FindViewById<SeekBar>(Resource.Id.sb_progress);
            rl_video = FindViewById<RelativeLayout>(Resource.Id.rl_video);
            rl_menu = FindViewById<RelativeLayout>(Resource.Id.rl_menu);
            rl_wait = FindViewById<RelativeLayout>(Resource.Id.rl_wait);
            rc_episodes = FindViewById<RecyclerView>(Resource.Id.rc_episodes);

            rc_episodes.SetLayoutManager(new GridLayoutManager(this, 4));
            rc_episodes.AddItemDecoration(new Z_RecyclerViewAsGrid_Spacing(3, 10, true));
        }

        public override void D_BindEvent()
        {
            iv_back.Click += OnClickListener;
            iv_play_or_stop.Click += OnClickListener;
            iv_full_screen.Click += OnClickListener;
            tv_open_intro.Click += OnClickListener;
        }

        public override void E_InitData()
        {
            //初始化适配器
            adapter_es = new Film_FilmEpisodes_Adapter(this);
            adapter_es.onItemClickAct -= OnRVItemClickListener;
            adapter_es.onItemClickAct += OnRVItemClickListener;
            rc_episodes.SetAdapter(adapter_es);

            //初始化错误回调
            errorListennerImp = new MyOnErrorListenerImp();
            errorListennerImp.ErrorAction -= OnError;
            errorListennerImp.ErrorAction += OnError;


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
                        QuitThePage();
                    }
                    break;
                case Resource.Id.iv_play:///播放/暂停
                    {
                        var b = iv_back.Selected;
                        if (b)
                        {
                            //暂停
                            if (IsPlaying())
                                mVideoView.Stop();
                        }
                        else
                        {
                            if (isFirstComein)
                            {//如果是第一次进入
                                ShowWaitAnime();
                                InitKSYPlayer();
                            }
                            //播放
                            if (IsPlaying())
                                mVideoView.Start();
                        }
                        iv_back.Selected = !b;
                    }
                    break;
                case Resource.Id.iv_full_screen:///全屏
                    {


                    }
                    break;
                case Resource.Id.tv_open_intro:///展开&关闭简介
                    {
                        if (isOpenIntro)
                            //展开简介
                            CoverUIControl(CoverFlag.Visible, tv_intro);
                        else
                            //关闭简介
                            CoverUIControl(CoverFlag.Gone, tv_intro);
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
                        CleanTheKSYMedia();
                        if (rl_wait.Visibility == ViewStates.Gone)
                        {
                            ShowWaitAnime();
                        }
                        InitKSYPlayer();
                    }
                    break;
                case MSG_COVER_MENU:
                    {
                        CoverUIControl(CoverFlag.Gone, rl_menu);
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
            }
        }

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
            KSYTextureViewManager.Instance.SetOnErrorListener(mVideoView, errorListennerImp);
            mVideoView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
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
        private void SetProgreesInfo()
        {
            var progrees = mVideoView.Duration;
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
                HideWaitDiaLog();
                if (t.Exception != null)
                {
                    Console.WriteLine("线程异常");
                    return;
                }
                if (t.Result.IsSuccess)
                {
                    var jk = t.Result.Data;
                    tv_art.Text = jk.Starring;
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
        private void HttpGetVideoPlayUrl(string href, string name)
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
                    var jk = t.Result.Data;
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

        #region 金山云错误回调查看
        private class MyOnErrorListenerImp : Java.Lang.Object, IMyOnErrorListener
        {
            /// <summary>
            /// 错误回调,第一个参数
            /// </summary>
            public Action<int, int> ErrorAction;
            public bool OnError(int what, int extro)
            {
                ErrorAction?.Invoke(what, extro);
                return false;
            }
        }
        private void OnError(int what, int extro)
        {
            CleanTheKSYMedia();
            SendRetryPlayerMsg();
        }
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
        #endregion

        #region 生命周期
        protected override void OnResume()
        {
            base.OnResume();
            //每五秒发送一条检测视频是否正常播放的消息
            mHandler.PostDelayed(mRunnable_Alpha, 5000);
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

        #region 播放器各种接口回调

        public void OnPrepared(MediaPlayer mp)
        {
             
        }

        public void OnCompletion(MediaPlayer mp)
        {
             
        }

        public void OnSeekComplete(MediaPlayer mp)
        {
             
        }

        #endregion


        #endregion
    }
}
