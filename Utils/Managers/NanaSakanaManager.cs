using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Qiyukf.Unicorn.Api;
using IdoMaster_GensouWorld.Listeners;

namespace IdoMaster_GensouWorld.Utils.Managers
{
    /// <summary>
    /// 七鱼管理页面
    /// </summary>
    public class NanaSakanaManager
    {
        /// <summary>
        /// 管理单例
        /// </summary>
        public static NanaSakanaManager GetInstance()
        {
            if (Instance == null)
                Instance = new NanaSakanaManager();
            return Instance;
        }

        private static NanaSakanaManager Instance;

        #region 初始化相关
        private const string APPID = "9c761972343819f684df4efd0a010995";

        /// <summary>
        /// 初始化七鱼
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool InitNanaSakana(Context context)
        {
            var option = new YSFOptions
            {
                StatusBarNotificationConfig = new StatusBarNotificationConfig(),
                SavePowerConfig = new SavePowerConfig()
            };
            option.StatusBarNotificationConfig.Vibrate = false;
            option.StatusBarNotificationConfig.NotificationSmallIconId = Resource.Mipmap.icon_fuku_on;

            //初始化
            return Unicorn.Init(context, APPID, option, new UnicornImageLoader());
        }

        #endregion

        /// <summary>
        /// 打开客服页面
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="title">左上角标题</param>
        /// <param name="sourceUrl">来源页面的Url</param>
        /// <param name="sourceTitle">来源页面标题</param>
        /// <param name="cis">无用</param>
        /// <returns><see langword="false"/>打开时失败</returns>
        public bool OpenCsPage(Context context, string title = "客服咨询", string sourceUrl = "", string sourceTitle = "", string cis = null)
        {
            if (Unicorn.IsServiceAvailable)
            {
                var source = new ConsultSource(sourceUrl, sourceTitle, cis);
                Unicorn.OpenServiceActivity(context, title, source);
                return true;
            }
            else
                return false;
        }



    }
}