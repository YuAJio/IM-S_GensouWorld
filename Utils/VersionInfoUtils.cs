using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace IdoMaster_GensouWorld.Utils
{
    /// <summary>
    /// 查看APP版本信息
    /// </summary>
    public class VersionInfoUtils
    {
        /// <summary>
        /// 获得APP版本Code 实例="11"
        /// </summary>
        /// <returns></returns>
        public static int GetVersionCode()
        {
            var packageManager = IMAS_Application.Context.PackageManager;
            PackageInfo packageInfo;
            var versionCode = 0;
            try
            {
                packageInfo = packageManager.GetPackageInfo(IMAS_Application.Context.PackageName, 0);
                versionCode = packageInfo.VersionCode;
            }
            catch (Exception e)
            {

            }
            return versionCode;
        }

        /// <summary>
        /// 获取APP版本号 实例="1.1.1"
        /// </summary>
        /// <returns></returns>
        public static string GetVersionName()
        {
            var packageManager = IMAS_Application.Context.PackageManager;
            PackageInfo packageInfo;
            var packageName = "";

            try
            {
                packageInfo = packageManager.GetPackageInfo(IMAS_Application.Context.PackageName, 0);
                packageName = packageInfo.VersionName + "";
            }
            catch (Exception)
            {

            }
            return packageName;
        }
        /// <summary>
        /// 获取项目包名 实例="com.yurishi.imas"
        /// </summary>
        /// <returns></returns>
        public static string GetPackageName()
        {
            var packageManager = IMAS_Application.Context.PackageManager;
            PackageInfo packageInfo;
            var versionName = "";

            try
            {
                packageInfo = packageManager.GetPackageInfo(IMAS_Application.Context.PackageName, 0);
                versionName = packageInfo.PackageName + "";
            }
            catch (Exception)
            {

            }
            return versionName;
        }
    }
}