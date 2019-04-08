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
using IdoMaster_GensouWorld.Activitys;

namespace IdoMaster_GensouWorld.Services
{
    /// <summary>
    /// 插播任务管理服务
    /// </summary>
    [Service(Name = "com.yurishi.imas_protable.Sevr_Inter_CutControl")]
    public class Sevr_Inter_CutControl : Service
    {

        private AlarmManager alarmManager;


        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            alarmManager = (AlarmManager)GetSystemService(AlarmService);



            return base.OnStartCommand(intent, flags, startId);
        }

        #region AlarmManager相关
        /// <summary>
        /// 发送延迟单次广播
        /// </summary>
        /// <param name="delayTime">延迟时间(默认不延迟)(毫秒)</param>
        private void SendDelayOneTimeAlarm(long delayTime = 0)
        {
            var triggerAtTIme = SystemClock.ElapsedRealtime() + delayTime;
            var intent2 = new Intent(this, typeof(Rece_Inter_CutControl));
            var pi = PendingIntent.GetBroadcast(this, 0, intent2, 0);
            alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, triggerAtTIme, pi);

        }

        ///// <summary>
        ///// 在指定的时间开始
        ///// 在指定的时间结束
        ///// </summary>
        ///// <param name="startTime">开始时间</param>
        ///// <param name="EndTime">结束时间</param>
        //private void SendTimingOneTimeAlarm(DateTime? startTime, DateTime? EndTime)
        //{
        //    DateTime rStartTime = DateTime.Now;
        //    DateTime rEndTime = DateTime.Now.AddMinutes(5d);
        //    rStartTime = startTime ?? rStartTime;
        //    rEndTime = EndTime ?? rEndTime;

        //    var myCall = Android.Icu.Util.Calendar.Instance;
        //    myCall.Set(rStartTime.Year, rStartTime.Month, rStartTime.Day, rStartTime.Hour, rStartTime.Minute, rStartTime.Second);
        //    var myCall2 = Android.Icu.Util.Calendar.Instance;
        //    myCall2.Set(rEndTime.Year, rEndTime.Month, rEndTime.Day, rEndTime.Hour, rEndTime.Minute, rEndTime.Second);

        //    var notTime = myCall.TimeInMillis;
        //    var shutDownTime = myCall2.TimeInMillis;

        //    var intent2 = new Intent(this, typeof(Rece_Inter_CutControl));
        //    var pi = PendingIntent.GetBroadcast(this, 0, intent2, 0);
        //    var triggerAtTime = SystemClock.ElapsedRealtime() + shutDownTime - notTime;
        //    alarmManager.Set(AlarmType.ElapsedRealtimeWakeup, triggerAtTime, pi);
        //}


        /// <summary>
        /// 延迟发送重复广播
        /// </summary>
        /// <param name="repeatTime">重复间隔时间(毫秒)</param>
        /// <param name="delayTime">延迟时间(毫秒)</param>
        private void SendDelayRepeatAlarm(long repeatTime, long delayTime = 0)
        {
            var triggerAtTIme = SystemClock.ElapsedRealtime() + delayTime;
            var intent2 = new Intent(this, typeof(Rece_Inter_CutControl));
            var pi = PendingIntent.GetBroadcast(this, 0, intent2, 0);
            alarmManager.SetRepeating(AlarmType.ElapsedRealtimeWakeup, triggerAtTIme, repeatTime, pi);
        }

        /// <summary>
        /// 设置每天的几时几分发送广播
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        private void SendEveryDayAlarm(int hour, int minute)
        {
            //获取系统当前时间
            var firstTIme = SystemClock.ElapsedRealtime();
            var systemTime = Java.Lang.JavaSystem.CurrentTimeMillis();//返回从 UTC 1970 年 1 月 1 日午夜开始经过的毫秒数
            var calendar = Android.Icu.Util.Calendar.Instance;
            calendar.TimeInMillis = systemTime;
            calendar.TimeZone = Android.Icu.Util.TimeZone.GetTimeZone("GMT+8");
            calendar.Set(Android.Icu.Util.CalendarField.Minute, minute);
            calendar.Set(Android.Icu.Util.CalendarField.HourOfDay, hour);
            calendar.Set(Android.Icu.Util.CalendarField.Second, 0);
            calendar.Set(Android.Icu.Util.CalendarField.Millisecond, 0);

            //计算出设定的时间
            var selectTime = calendar.TimeInMillis;

            if (systemTime > selectTime)
                //如果当前时间大于设置的时间,那么就从第二天的设定时间开始
                selectTime = calendar.TimeInMillis;

            //计算当前时间到设定时间的时间差
            var time = selectTime - systemTime;
            //系统 当前的时间+时间差
            var myTime = firstTIme + time;

            var intent2 = new Intent(this, typeof(Rece_Inter_CutControl));
            var pi = PendingIntent.GetBroadcast(this, 0, intent2, 0);
            alarmManager.SetRepeating(AlarmType.ElapsedRealtimeWakeup, myTime, AlarmManager.IntervalDay, pi);
        }

        #endregion

        #region Quartz相关
        ///// <summary>
        ///// 代理Quartz类-代号.沙鹰
        ///// </summary>
        //private class AgencyQuartzClas_DustEgle : Ys.QuartzStuff.QS_JobBase
        //{
        //    private readonly Action runAct;
        //    public AgencyQuartzClas_DustEgle() { }
        //    public AgencyQuartzClas_DustEgle(Action runAct)
        //    {
        //        this.runAct = runAct;
        //    }
        //    public override void Run()
        //    {
        //        runAct?.Invoke();
        //    }
        //}
        ///// <summary>
        ///// 代理Quartz类-代号.海洋法律
        ///// </summary>
        //private class AgencyQuartzClas_OceanLaw : Ys.QuartzStuff.QS_JobBase
        //{
        //    private readonly Action runAct;
        //    public AgencyQuartzClas_OceanLaw() { }
        //    public AgencyQuartzClas_OceanLaw(Action runAct)
        //    {
        //        this.runAct = runAct;
        //    }
        //    public override void Run()
        //    {
        //        runAct?.Invoke();
        //    }
        //}
        #endregion

        #region 方法池

        #endregion

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        #region 广播接收者
        [BroadcastReceiver(Name = "com.yurishi.Inter_CutControl_Receiver")]
        public class Rece_Inter_CutControl : BroadcastReceiver
        {

            public Rece_Inter_CutControl()
            {
            }

            public override void OnReceive(Context context, Intent intent)
            {
                ActivateCut_InActivity(context);
            }

            /// <summary>
            /// 启动插播页面
            /// </summary>
            public void ActivateCut_InActivity(Context context)
            {
                //if (!dearSon.IsAlive)
                //{
                var activity = IMAS_Application.Sington.OpenActivityList.Where(x => x.ComponentName.ShortClassName.EndsWith(".Inter_CutSimActivity")).FirstOrDefault();
                if (activity == null)
                {
                    var intent = new Intent(context, typeof(Inter_CutSimActivity));
                    intent.PutExtra(Inter_CutSimActivity.IntentFlag_Cut_In_Type, (int)Cut_In_ShowType.Image);
                    intent.AddFlags(ActivityFlags.NewTask);
                    context.StartActivity(intent);
                }
                else
                {
                    activity.Finish();
                    ActivateCut_InActivity(context);
                }
            }

            /// <summary>
            /// 结束插播页面
            /// </summary>
            /// <param name="hardFinish">是否强制结束</param>
            public void FinishCut_InActivity(Context context, bool hardFinish = false)
            {
                //if (dearSon.IsAlive)
                //    dearSon.Finish();
                //else
                //    if (hardFinish)
                //    dearSon.Finish();
            }

        }
        #endregion
    }

}