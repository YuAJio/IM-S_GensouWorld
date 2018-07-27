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
using CustomControl;
using Java.Lang;
using Android.Support.V4.Content;
using Android.Views.Animations;
using IdoMaster_GensouWorld.Activitys.Opening;
using Java.Util;
using System.Threading.Tasks;
using Android.Media;
using IdoMaster_GensouWorld.Utils;

namespace IdoMaster_GensouWorld.Activitys.MainPage
{
    /// <summary>
    /// 开始游戏开场白
    /// </summary>
    [Activity(Label = "NewGame_Activity", Theme = "@style/Theme.PublicThemePlus")]
    public class NewGame_Activity : BaseActivity
    {
        private RelativeLayout rl_tv_father;
        private TextView tv_Twinkle;

        private string[] movie_Titils = { "アイドル", "それわすべての女の子の憧れの存在", "今 十三人の少女が トープに目指せ", "ステージに立ち向かう！" };
        #region Handler
        /// <summary>
        /// 发送改变电影式文字的标识
        /// </summary>
        private const int SEND_CHANGE_MOVIE_TITILE_WHAT = 0x124;
        /// <summary>
        /// 发送跳转到另一个Activity的标识
        /// </summary>
        private const int SEND_INTO_NEXT_ACTIVITY_WHAT = 0x954;

        private MyHandler mHandler;
        private class MyHandler : Handler
        {
            public Action<Message> handlerAction;
            public override void HandleMessage(Message msg)
            {
                handlerAction?.Invoke(msg);
            }
        }
        private void RunHandlerAction(Message msg)
        {
            switch (msg.What)
            {
                case SEND_CHANGE_MOVIE_TITILE_WHAT:
                    rl_tv_father.RemoveAllViews();
                    if (movie_Titils.Length <= textIndex)
                    {
                        rl_tv_father.AddView(InitTextView());
                        mHandler.RemoveCallbacks(runable_Alpha);
                        canGoToTheNextStep = true;
                        SetTextViewTwinkle();
                    }
                    else
                    {
                        rl_tv_father.AddView(InitTextView());
                        textIndex++;
                        mHandler.PostDelayed(runable_Alpha, 3 * 1000);
                    }
                    break;

                case SEND_INTO_NEXT_ACTIVITY_WHAT:
                    mHandler.RemoveCallbacks(runabla_Beta);
                    GoToNextStep();
                    break;
            }
        }


        private MyRunable runable_Alpha;
        private MyRunable runabla_Beta;

        /// <summary>
        /// 子线程Runnable
        /// </summary>
        public class MyRunable : Java.Lang.Object, IRunnable
        {
            private Handler mHandwel;
            private int mWhat;

            public MyRunable(Handler mHandwel, int what)
            {
                this.mHandwel = mHandwel;
                this.mWhat = what;
            }
            public void Run()
            {
                Message msg = new Message();
                msg.What = mWhat;
                mHandwel.SendMessage(msg);
            }
        }
        #endregion
        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_main_newgame;
        }

        public override void B_BeforeInitView()
        {
            mHandler = new MyHandler();
            mHandler.handlerAction += RunHandlerAction;

            runable_Alpha = new MyRunable(mHandler, SEND_CHANGE_MOVIE_TITILE_WHAT);
            runabla_Beta = new MyRunable(mHandler, SEND_INTO_NEXT_ACTIVITY_WHAT);
        }

        public override void C_InitView()
        {
            rl_tv_father = FindViewById<RelativeLayout>(Resource.Id.rl_tv_father);
            tv_Twinkle = FindViewById<TextView>(Resource.Id.tv_twinkle);
        }

        public override void D_BindEvent()
        {
            rl_tv_father.ChildViewAdded -= RelatviLayoutAddViewAction;
            rl_tv_father.ChildViewRemoved -= RelatviLayoutRemoveViewAction;

            rl_tv_father.ChildViewAdded += RelatviLayoutAddViewAction;
            rl_tv_father.ChildViewRemoved += RelatviLayoutRemoveViewAction;
        }

        public override void E_InitData()
        {
            mHandler.PostDelayed(runable_Alpha, 1200);
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }
        /// <summary>
        /// 跳转到序章页面
        /// </summary>
        private void GoToNextStep()
        {
            StartActivity(new Intent(this, typeof(Prologue_Activity)));
        }

        #region 关于点击屏幕进行下一步
        private bool canGoToTheNextStep = false;
        private bool change = false;
        private void SetTextViewTwinkle()
        {
            CoverUIControl(CoverFlag.Visible, tv_Twinkle);
            TimeTasker task = new TimeTasker();
            Timer timer = new Timer();
            task.taskerAction -= OnTimeTaskActin;
            task.taskerAction += OnTimeTaskActin;
            timer.Schedule(task, 1, 800);  //参数分别是delay（多长时间后执行），duration（执行间隔）
        }

        private void OnTimeTaskActin()
        {
            RunOnUiThread(delegate ()
            {
                if (change)
                {
                    change = false;
                    tv_Twinkle.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.kuuhaku));
                }
                else
                {
                    change = true;
                    tv_Twinkle.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.snow));

                }
            });
        }
        protected class TimeTasker : TimerTask
        {
            public Action taskerAction;
            public override void Run()
            {
                taskerAction?.Invoke();
            }
        }
        public override bool OnTouchEvent(MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    //   PlayClickMusic();
                    if (canGoToTheNextStep)
                    {
                        mHandler.RemoveMessages(SEND_CHANGE_MOVIE_TITILE_WHAT);
                        mHandler.RemoveMessages(SEND_INTO_NEXT_ACTIVITY_WHAT);

                        GoToNextStep();
                        canGoToTheNextStep = false;
                        return false;
                    }
                    break;
            }
            return base.OnTouchEvent(e);
        }

        #endregion
        #region 字幕淡入淡出

        /// <summary>
        /// View动画渐隐效果
        /// </summary>
        /// <param name="view"></param>
        /// <param name="duration"></param>
        private void SetHideAnimation(View view, int duration)
        {
            if (null == view || duration < 0)
            {
                return;
            }
            //if (mHideAnimation != null)
            //{
            //    mHideAnimation.Cancel();
            //}
            //监听动画结束的操作
            view.StartAnimation(AnimationHelper.Fade_OutAlphaAnimation(duration));
        }
        /// <summary>
        /// View动画渐现效果
        /// </summary>
        private void SetShowAnimation(View view, int duration)
        {
            if (view == null || duration < 0)
            {
                return;
            }
            //if (mShowAnimation != null)
            //{
            //    mShowAnimation.Cancel();
            //}
            view.StartAnimation(AnimationHelper.Fade_InAlphaAnimation(duration));
        }


        private void RelatviLayoutAddViewAction(object sender, ViewGroup.ChildViewAddedEventArgs e)
        {
            SetShowAnimation(e.Child, 500);
        }

        private void RelatviLayoutRemoveViewAction(object sender, ViewGroup.ChildViewRemovedEventArgs e)
        {
            SetHideAnimation(e.Child, 500);
        }
        #endregion
        #region 设置滚动字幕
        private int textIndex = 0;
        private StartMovieTextView InitTextView()
        {
            var textView = new StartMovieTextView(this);
            textView.TextSize = 18;
            textView.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.ghostwhite));
            try
            {
                textView.Text = movie_Titils[textIndex];
            }
            catch (System.Exception)
            {
                textView.Text = movie_Titils[movie_Titils.Length - 1];
            }
            return textView;

        }
        #endregion
        #region 点击特效相关
        private MediaPlayer sound_Player;
        private void PlayClickMusic(int musicId)
        {
            sound_Player = MediaPlayer.Create(this, musicId);
            sound_Player.Start();
        }
        #endregion
    }
}