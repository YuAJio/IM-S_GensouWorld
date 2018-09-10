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

        }

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
    }
}