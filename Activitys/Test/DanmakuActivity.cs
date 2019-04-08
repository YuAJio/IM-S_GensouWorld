using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Support.V4.Content;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Com.Nostra13.Universalimageloader.Core;
using IdoMaster_GensouWorld.CustomControl;
using IdoMaster_GensouWorld.Services;
using IdoMaster_GensouWorld.Utils;

namespace IdoMaster_GensouWorld.Activitys.Test
{
    [Activity(Label = "DanmakuActivity", Theme = "@style/Theme.PublicThemePlus")]
    public class DanmakuActivity : BaseActivity
    {
        private ImageView iv_bg1;
        private ImageView iv_bg2;
        private ImageView iv_bg3;
        private ImageView iv_bg4;

        private RelativeLayout rl_fahter;


        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_danmaku;
        }

        public override void B_BeforeInitView()
        {

        }

        public override void C_InitView()
        {
            iv_bg1 = FindViewById<ImageView>(Resource.Id.iv_1);
            iv_bg2 = FindViewById<ImageView>(Resource.Id.iv_2);
            iv_bg3 = FindViewById<ImageView>(Resource.Id.iv_3);
            iv_bg4 = FindViewById<ImageView>(Resource.Id.iv_4);
            rl_fahter = FindViewById<RelativeLayout>(Resource.Id.rl_father);
        }

        public override void D_BindEvent()
        {

        }

        public override void E_InitData()
        {
            var path1 = "http://img3.imgtn.bdimg.com/it/u=639829592,8024413&fm=26&gp=0.jpg";
            var path2 = "https://ss3.bdstatic.com/70cFv8Sh_Q1YnxGkpoWK1HF6hhy/it/u=991999372,4138273839&fm=26&gp=0.jpg";
            var path3 = "https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=233502162,2756285051&fm=26&gp=0.jpg";
            var path4 = "https://ss3.bdstatic.com/70cFv8Sh_Q1YnxGkpoWK1HF6hhy/it/u=2716787542,3114362757&fm=26&gp=0.jpg";
            ImageLoader.Instance.DisplayImage(path1, iv_bg1, ImageLoaderHelper.GeneralImageOption());
            ImageLoader.Instance.DisplayImage(path2, iv_bg2, ImageLoaderHelper.GeneralImageOption());
            ImageLoader.Instance.DisplayImage(path3, iv_bg3, ImageLoaderHelper.GeneralImageOption());
            ImageLoader.Instance.DisplayImage(path4, iv_bg4, ImageLoaderHelper.GeneralImageOption());

            iv_bg1.Click += OnClickListener;
            iv_bg2.Click += OnClickListener;
            iv_bg3.Click += OnClickListener;
            iv_bg4.Click += OnClickListener;
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
            switch (v.Id)
            {
                case Resource.Id.iv_1:
                case Resource.Id.iv_4:
                    {
                        StartCountDown();
                    }
                    break;
                case Resource.Id.iv_2:
                    {
                        var intent = new Intent(this, typeof(Inter_CutSimActivity));
                        intent.PutExtra(Inter_CutSimActivity.IntentFlag_Cut_In_Type, (int)Cut_In_ShowType.Video);
                        intent.AddFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                    }
                    break;
                case Resource.Id.iv_3:
                    {
                        var intent = new Intent(this, typeof(Sevr_Inter_CutControl));
                        ApplicationContext.StartService(intent);
                        //var intent = new Intent(this, typeof(Inter_CutSimActivity));
                        //intent.PutExtra(Inter_CutSimActivity.IntentFlag_Cut_In_Type, (int)Cut_In_ShowType.Image);
                        //StartActivity(intent);
                    }
                    break;
            }

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }



        private void StartCountDown()
        {
            lock (
            Task.Run(() =>
            {
                Thread.Sleep(2 * 1000);
            }).ContinueWith(x =>
            {
                if (x.Exception != null)
                    return;

                var layoutParamas = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

                var danmaku = new FakeDamakuView(this)
                {
                    Text = "公告:玩家<爷丶点草全服>因为言语过激,已被禁言,禁言时间30天.请各位玩家控制好情绪,理智游戏.                                公告:玩家<爷丶点草全服>因为言语过激,已被禁言,禁言时间30天.请各位玩家控制好情绪,理智游戏.",
                    TextSize = 20,
                    LayoutParameters = layoutParamas,
                };

                danmaku.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.white));

                rl_fahter.AddView(danmaku);
                rl_fahter.StartAnimation(GetShowActionAnimation());
                rl_fahter.Visibility = ViewStates.Visible;

                StartTimeCountDown();

            }, TaskScheduler.FromCurrentSynchronizationContext())) ;

        }

        private void StartTimeCountDown()
        {
            var countDown = new YsCountDownTimer(10 * 1000, 1000);
            countDown.OnFinishAct -= TimeCountDownFinshi;
            countDown.OnFinishAct += TimeCountDownFinshi;
            countDown.Start();
        }
        private void TimeCountDownFinshi()
        {
            rl_fahter.RemoveAllViews();
            rl_fahter.StartAnimation(GetDismissActionAnimation());
            rl_fahter.Visibility = ViewStates.Gone;
        }

        #region 显示隐藏动画
        private TranslateAnimation GetShowActionAnimation()
        {
            var anime = new TranslateAnimation(
                Dimension.RelativeToSelf, 0.0f,
                Dimension.RelativeToSelf, 0.0f,
                Dimension.RelativeToSelf, 1.0f,
                Dimension.RelativeToSelf, 0.0f)
            {
                Duration = 500
            };
            return anime;
        }
        private TranslateAnimation GetDismissActionAnimation()
        {
            var anime = new TranslateAnimation(
                Dimension.RelativeToSelf, 0.0f,
                Dimension.RelativeToSelf, 0.0f,
                Dimension.RelativeToSelf, 0.0f,
                Dimension.RelativeToSelf, 1.0f)
            {
                Duration = 500
            };
            return anime;
        }
        #endregion


        private class YsCountDownTimer : CountDownTimer
        {
            public YsCountDownTimer(long millisInFuture, long countDownInterval) : base(millisInFuture, countDownInterval)
            {
            }

            public Action OnFinishAct { get; set; }
            public Action<long> OnOnTickAct { get; set; }

            public override void OnFinish()
            {
                OnFinishAct?.Invoke();
            }

            public override void OnTick(long millisUntilFinished)
            {
                OnOnTickAct?.Invoke(millisUntilFinished);
            }


        }
    }
}