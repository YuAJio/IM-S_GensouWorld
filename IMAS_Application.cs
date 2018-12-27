using System;
using System.Collections.Generic;

using Android.App;
using Android.Runtime;
using IMAS.Utils.Sp;
using IMAS.Utils.Files;
using IMAS.Tips.Logic.LocalDBManager;
using IMAS.Utils.Logs;
using Com.Nostra13.Universalimageloader.Cache.Disc.Impl;
using Com.Nostra13.Universalimageloader.Core;
using IMAS.BaiduAI.Vocal_Compound;
using IMAS.HelpfulUtils.Zatsu;
using IMAS.CupCake.Extensions;
using Android.Graphics;
using Android.Views;

namespace IdoMaster_GensouWorld
{
    [Android.App.Application]
    public class IMAS_Application : Application
    {
        public static IMAS_Application Sington;
        /// <summary>
        /// 打开的activity集合
        /// </summary>
        public List<Activity> OpenActivityList = null;

        #region 点击消息事件相关
        ///// <summary>
        ///// 点击消息进入某个页面
        ///// </summary>
        //private StatusBarNotificationConfig mStatusBarNotificationConfig;

        ///// <summary>
        ///// 设置消息进入页面
        ///// </summary>
        ///// <param name="activity"></param>
        //public void SetServiceEntranceActivity(Activity activity)
        //{
        //    mStatusBarNotificationConfig.NotificationEntrance = activity.Class;
        //}
        #endregion

        /// <summary>
        /// 一定要写这个
        /// </summary>
        /// <param name="javaReference"></param>
        /// <param name="transfer"></param>
        public IMAS_Application(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        public override void OnCreate()
        {
            base.OnCreate();
            Sington = this;
            OpenActivityList = new List<Activity>();

            //mStatusBarNotificationConfig = new StatusBarNotificationConfig
            //{
            //    NotificationEntrance = Java.Lang.Class.FromType(typeof(Activitys.MainPage.MainPage_Activity))
            //};

            #region 初始化文件管理器
            FilePathManager.GetInstance().Init(ApplicationContext);
            #endregion

            #region 初始化ImageLoader
            InitImageLoader();
            #endregion

            #region 初始化Sp
            AndroidPreferenceProvider.GetInstance().Init(this.ApplicationContext, IMAS_Constants.SpFileNameKey);
            #endregion

            #region 初始化Sqlite
            InitDb();
            #endregion

            #region 初始化Log管理器
            FileLogManager.GetInstance().Init("IMAS_Log");
            #endregion

            #region 初始化百度语音合成
            //BaiduVocalManager.GetKagemusha().Init(ApplicationContext, null);
            #endregion

            #region 初始化CrashHandler
            InitCrashHandler();
            #endregion

            #region 系统Task异常接收
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException -= TaskScheduler_UnobservedTaskException;
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            #endregion

            #region Android异常接收
            AndroidEnvironment.UnhandledExceptionRaiser -= AndroidEnvironment_UnhandledExceptionRaiser;
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            #endregion

            //#region 初始化网易七鱼
            //Unicorn.Init(this, ysfAppId(), ySFOptions(), new Listeners.UnicornImageLoader());
            ////Utils.Managers.NanaSakanaManager.GetInstance().InitNanaSakana(this);
            //#endregion
        }

        #region FlowView相关
        private WindowManagerLayoutParams wmParams = new WindowManagerLayoutParams();
        public WindowManagerLayoutParams GetWindowManagerLayoutParams()
        {
            return wmParams;
        }
        #endregion
        //#region 七鱼初始化相关
        //private string ysfAppId()
        //{
        //    return "9c761972343819f684df4efd0a010996";
        //}

        //private YSFOptions ySFOptions()
        //{
        //    var options = new YSFOptions();
        //    options.StatusBarNotificationConfig = new StatusBarNotificationConfig();
        //    options.StatusBarNotificationConfig.NotificationSmallIconId = Resource.Drawable.notification_template_icon_bg;
        //    return options;
        //}
        //#endregion


        /// <summary>
        /// 初始化图片加载器
        /// </summary>
        private void InitImageLoader()
        {
            var cachePath = $"{FilePathManager.GetInstance().GetPrivateRootDirPath()}/Cache";
            FilePathManager.GetInstance().CreateDir(cachePath);
            var k = new TotalSizeLimitedDiscCache(new Java.IO.File(cachePath), 100 * 1024 * 1024);
            var imageconfig = new ImageLoaderConfiguration.Builder(this.ApplicationContext)
                  .DiscCache(k)
                  .DiscCacheSize(100 * 1024 * 1024)//磁盘100M
                  .MemoryCacheSize(32 * 1024 * 1024)//内存5M
                  .ThreadPoolSize(2)//线程2
#if _IsOwnCert_
                      .ImageDownloader(new AuthImageDownloader(this.ApplicationContext))
#endif

#if DEBUG
                  .WriteDebugLogs()
#endif
                .Build();
            ImageLoader.Instance.Init(imageconfig);//只用初始化一个
        }

        /// <summary>
        /// 初始化数据库SQLite
        /// </summary>
        private void InitDb()
        {
            var path = $"{FilePathManager.GetInstance().GetPrivateRootDirPath()}/{IMAS_Constants.DBDir}";
            if (!FilePathManager.GetInstance().CheckDirExist(path))
            {
                var b = FilePathManager.GetInstance().CreateDir(path);
                //Console.WriteLine("b = " + b);
            }
            var dbPath = $"{path}/imasprodb.db";
            IMAS_ProAppDBManager.GetInstance().Init(this.ApplicationContext, dbPath);//初始化数据库管理器
        }

        /// <summary>
        /// 创建异常捕获
        /// </summary>
        private void InitCrashHandler()
        {
            CrashHandler.GetInstance().Init(ApplicationContext);

            CrashHandler.GetInstance().ThrowableCallback -= ThrowableCallback;
            CrashHandler.GetInstance().ThrowableCallback += ThrowableCallback;
        }

        #region 事件接收者
        /// <summary>
        /// 异常回调处理
        /// </summary>
        /// <param name="ex"></param>
        private void ThrowableCallback(Java.Lang.Throwable ex)
        {
#if DEBUG
            Console.WriteLine("异常：" + ex.Message);
            //Com.Chteam.Agent.BuglyAgentHelper.PostCatchedException(ex);
            //Com.Chteam.Agent.BuglyAgentHelper.TestJavaCrash();
#else
            Com.Chteam.Agent.BuglyAgentHelper.PostCatchedException(ex);
#endif

            FileLogManager.GetInstance().Log(LogLevel.Error, "Handler异常", ex.ToJson(true));
        }
        /// <summary>
        /// task处理异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskScheduler_UnobservedTaskException(object sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
        {
            FileLogManager.GetInstance().Log(LogLevel.Error, "task异常", e.Exception.ToJson(true));
        }
        /// <summary>
        /// Android 运行时异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            FileLogManager.GetInstance().Log(LogLevel.Error, "Android异常", e.Exception.ToJson(true));
        }
        #endregion

    }
}