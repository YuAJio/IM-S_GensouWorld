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

namespace IdoMaster_GensouWorld.Activitys.Test
{
    [Activity(Label = "WeekSleepControl", Theme = "@style/Theme.PublicThemePlus")]
    public class WeekSleepControlActivity : BaseActivity
    {
        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_week_sleep_control;
        }

        public override void B_BeforeInitView()
        {

        }

        public override void C_InitView()
        {
            var tv_lock = FindViewById<TextView>(Resource.Id.tv_lock);
            tv_lock.Click -= OnLockClickListner;
            tv_lock.Click += OnLockClickListner;
        }

        public override void D_BindEvent()
        {

        }

        public override void E_InitData()
        {

        }

        public override void F_OnClickListener(View v, EventArgs e)
        {

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }
        private void OnLockClickListner(object sender, System.EventArgs e)
        {
            var countDown = new MyCountDownTimer(5 * 1000, 1000);
            countDown.onFinishAct -= LockScreen;
            countDown.onFinishAct -= WeekScreen;
            countDown.onFinishAct += LockScreen;
            countDown.Start();
        }


        #region 休眠实现
        private PowerManager pm;
        private void LockScreen()
        {
            //pm = pm ?? (PowerManager)GetSystemService(PowerService);
            //pm.GoToSleep(SystemClock.UptimeMillis());
            var countDown = new MyCountDownTimer(3 * 1000, 1000);
            Intent intent = new Intent("android.intent.action.JSHSDZ.POWER");
            intent.PutExtra("command", "sleep");
            this.SendBroadcast(intent);
            countDown.onFinishAct -= LockScreen;
            countDown.onFinishAct -= WeekScreen;
            countDown.onFinishAct += WeekScreen;

            countDown.Start();
        }
        private void WeekScreen()
        {
            Intent intent = new Intent("android.intent.action.JSHSDZ.POWER");
            intent.PutExtra("command", "wakeup");
            this.SendBroadcast(intent);
        }

        #endregion

        private class MyCountDownTimer : CountDownTimer
        {
            public Action onFinishAct;
            public Action onTickAct;
            public MyCountDownTimer(long millisInFuture, long countDownInterval) : base(millisInFuture, countDownInterval)
            {
            }

            public override void OnFinish()
            {
                onFinishAct?.Invoke();
            }

            public override void OnTick(long millisUntilFinished)
            {
                onTickAct?.Invoke();
            }
        }
    }
}