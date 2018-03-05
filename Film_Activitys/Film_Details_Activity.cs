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

namespace IdoMaster_GensouWorld.Film_Activitys
{
    [Activity(Label = "Film_Details_Activity", Theme = "@style/Theme.PublicTheme")]
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
        /// 菜单键盘
        /// </summary>
        private RelativeLayout rl_menu;
        /// <summary>
        /// 集数列表
        /// </summary>
        private RecyclerView rc_episodes;
        #endregion
        #region 杂项
        /// <summary>
        /// 简介是否是展开状态
        /// </summary>
        private bool isOpenIntro;
        #endregion
        #region 金山云播放相关
        private string rPath;
        #endregion
        /// <summary>
        /// 视频唯一Id
        /// </summary>
        private string Path;
        public override int A_GetContentViewId()
        {
            return Resource.Layout.film_activity_film_details;
        }

        public override void B_BeforeInitView()
        {
            Path = Intent.GetStringExtra("value");
        }

        public override void C_InitView()
        {
            iv_back = FindViewById<ImageView>(Resource.Id.iv_back);
            iv_play_or_stop = FindViewById<ImageView>(Resource.Id.iv_play);
            iv_full_screen = FindViewById<ImageView>(Resource.Id.iv_full_screen);
            tv_seek = FindViewById<TextView>(Resource.Id.tv_progress);
            tv_title = FindViewById<TextView>(Resource.Id.tv_title);
            tv_intro = FindViewById<TextView>(Resource.Id.tv_intro);
            tv_art = FindViewById<TextView>(Resource.Id.tv_art);
            tv_open_intro = FindViewById<TextView>(Resource.Id.tv_open_intro);
            rl_video = FindViewById<RelativeLayout>(Resource.Id.rl_video);
            rl_menu = FindViewById<RelativeLayout>(Resource.Id.rl_menu);
            rc_episodes = FindViewById<RecyclerView>(Resource.Id.rc_episodes);

            rc_episodes.SetLayoutManager(new GridLayoutManager(this, 5));
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
            HttpGetSearchResult(Path);
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

                        }
                        else
                        {
                            //播放

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

        /// <summary>
        /// 退出此页面
        /// </summary>
        private void QuitThePage()
        {
            this.Finish();
        }

        #region Http相关
        private void HttpGetSearchResult(string href)
        {
            ShowWaitDiaLog("请稍等//...");
            Task.Run(async () =>
            {
                var result = await FilmApiHttpProxys.GetInstance().GetVideoInfo(href);
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
    }
}