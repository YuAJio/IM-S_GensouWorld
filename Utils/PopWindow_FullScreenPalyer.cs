//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Util;
//using Android.Views;
//using Android.Widget;
//using Com.Ksyun.Media.Player;

//namespace IdoMaster_GensouWorld.Utils
//{
//    class PopWindow_FullScreenPalyer : PopupWindow
//    {
//        #region UI控件
//        /// <summary>
//        /// 返回
//        /// </summary>
//        private ImageView iv_back;
//        /// <summary>
//        /// 播放/暂停
//        /// </summary>
//        private ImageView iv_play_or_stop;
//        /// <summary>
//        /// 全屏
//        /// </summary>
//        private ImageView iv_full_screen;
//        /// <summary>
//        /// 等待圆圈
//        /// </summary>
//        private ImageView iv_wait;
//        /// <summary>
//        /// 播放进度(文字)
//        /// </summary>
//        private TextView tv_seek;
//        /// <summary>
//        /// 重试
//        /// </summary>
//        private TextView tv_retry;
//        /// <summary>
//        /// 进度条
//        /// </summary>
//        private SeekBar sb_seek;
//        /// <summary>
//        /// 视频播放容器控件
//        /// </summary>
//        private RelativeLayout rl_video;
//        /// <summary>
//        /// 菜单控件容器
//        /// </summary>
//        private RelativeLayout rl_menu;
//        /// <summary>
//        /// 等待控件容器
//        /// </summary>
//        private RelativeLayout rl_wait;
//        /// <summary>
//        /// 错误控件容器
//        /// </summary>
//        private LinearLayout ll_error;
//        #endregion

//        private KSYTextureView mVideoPalyer;

//        public PopWindow_FullScreenPalyer() { }
//        public PopWindow_FullScreenPalyer(Activity context, Dictionary<string, Object> value)
//        {
//            this.mVideoPalyer = (KSYTextureView)value["video"];
//            InitView(context, value);
//        }

//        private void InitView(Activity context, Dictionary<string, Object> value)
//        {
//            var view = View.Inflate(context, Resource.Layout.film_video_fullscreen, null);
//            var dic = GetHeightAndWidth(context);
//            this.ContentView = view;
//            this.Width = dic[1];
//            this.Height = dic[2];
//            this.Focusable = false;
//            this.OutsideTouchable = false;

//            #region InitView
//            iv_back = (ImageView)value["iv_back"];
//            iv_play_or_stop = (ImageView)value["iv_play_or_stop"];
//            iv_full_screen = (ImageView)value["iv_full_screen"];
//            iv_wait = (ImageView)value["iv_wait"];
//            tv_seek = (TextView)value["tv_seek"];
//            tv_retry = (TextView)value["tv_retry"];
//            sb_seek = (SeekBar)value["sb_seek"];
//            rl_video = (RelativeLayout)value["rl_video"];
//            rl_menu = (RelativeLayout)value["rl_menu"];
//            rl_wait = (RelativeLayout)value["rl_wait"];
//            ll_error = (LinearLayout)value["ll_error"];

//            //iv_back = view.FindViewById<ImageView>(Resource.Id.iv_back);
//            //iv_play_or_stop = view.FindViewById<ImageView>(Resource.Id.iv_play);
//            //iv_full_screen = view.FindViewById<ImageView>(Resource.Id.iv_full_screen);
//            //iv_wait = view.FindViewById<ImageView>(Resource.Id.iv_wait);
//            //tv_seek = view.FindViewById<TextView>(Resource.Id.tv_progress);
//            //tv_retry = view.FindViewById<TextView>(Resource.Id.tv_retry);
//            //sb_seek = view.FindViewById<SeekBar>(Resource.Id.sb_progress);
//            //rl_video = view.FindViewById<RelativeLayout>(Resource.Id.rl_video);
//            //rl_menu = view.FindViewById<RelativeLayout>(Resource.Id.rl_menu);
//            //rl_wait = view.FindViewById<RelativeLayout>(Resource.Id.rl_wait);
//            //ll_error = view.FindViewById<LinearLayout>(Resource.Id.ll_error);
//            #endregion

//            if (mVideoPalyer != null)
//            {
//                if (!mVideoPalyer.IsPlaying)
//                    mVideoPalyer.Start();
//            }
//        }

//        #region 显示隐藏POPWINDOW
//        public void ShowPopWindow(View view)
//        {
//            if (!IsShowing)
//            {
//                //  this.showAsDropDown(view);//可以显示在指定view的指定位置
//                this.ShowAtLocation(view, GravityFlags.Start, 0, 0);
//            }
//        }
//        public void DismissPop()
//        {
//            if (IsShowing)
//            {
//                //if (callBack != null)
//                //{
//                //    callBack.NotifCallBack();
//                //}
//                this.Dismiss();
//            }
//        }
//        #endregion

//        /// <summary>
//        /// 获取宽高
//        /// </summary>
//        /// <param name="context"></param>
//        /// <returns> 1:宽 2:高</returns>
//        private Dictionary<int, int> GetHeightAndWidth(Activity context)
//        {
//            var metrics = new DisplayMetrics();
//            context.WindowManager.DefaultDisplay.GetMetrics(metrics);
//            var diction = new Dictionary<int, int>
//            {
//                { 1, metrics.WidthPixels },
//                { 2, metrics.HeightPixels }
//            };
//            return diction;
//            //if (flag == 0)
//            //    return metrics.WidthPixels;
//            //else
//            //    return metrics.HeightPixels;
//            //width = metrics.WidthPixels;
//            //height = metrics.HeightPixels;
//        }
//    }
//}