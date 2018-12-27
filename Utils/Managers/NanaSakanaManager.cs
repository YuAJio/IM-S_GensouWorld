using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IdoMaster_GensouWorld.Listeners;
using IMAS.CupCake.Extensions;

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

        private static Md_UserInfo userInfo;

        #region 初始化相关
        //private const string APPID = "9c761972343819f684df4efd0a010995";

        ///// <summary>
        ///// 初始化七鱼
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public bool InitNanaSakana(Context context)
        //{
        //    var option = new YSFOptions
        //    {
        //        StatusBarNotificationConfig = new StatusBarNotificationConfig(),
        //        SavePowerConfig = new SavePowerConfig()
        //    };
        //    option.StatusBarNotificationConfig.Vibrate = false;
        //    option.StatusBarNotificationConfig.NotificationSmallIconId = Resource.Mipmap.icon_fuku_on;

        //    //初始化
        //    return Unicorn.Init(context, APPID, option, new UnicornImageLoader());
        //}

        ///// <summary>
        ///// 退出登录
        ///// 在关闭程序的时候或是退出的时候 结束与七鱼的联系
        ///// </summary>
        ///// <returns></returns>
        //public void QuitUserLogin()
        //{
        //    Unicorn.Logout();
        //}
        #endregion

        #region 客服会话相关
        public void InitUserInfo(
        string userId = "",
        string name = "",
        string phone = "",
        string email = "",
        string avatar = "")
        {
            if (userInfo == null)
            {
                userInfo = new Md_UserInfo()
                {
                    UserId = userId,
                    Avatar = avatar,
                    Email = email,
                    Name = name,
                    Phone = phone,
                };
            }
            else
            {
                userInfo.UserId = string.IsNullOrEmpty(userId) ? userInfo.UserId : userId;
                userInfo.Name = string.IsNullOrEmpty(name) ? userInfo.UserId : name;
                userInfo.Phone = string.IsNullOrEmpty(phone) ? userInfo.UserId : phone;
                userInfo.Email = string.IsNullOrEmpty(email) ? userInfo.UserId : email;
                userInfo.Avatar = string.IsNullOrEmpty(avatar) ? userInfo.UserId : avatar;
            }

        }

        /// <summary>
        /// 打开客服页面
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="title">左上角标题</param>
        /// <param name="sourceUrl">来源页面的Url</param>
        /// <param name="sourceTitle">来源页面标题</param>
        /// <param name="cis">无用</param>
        /// <returns><see langword="false"/>打开时失败</returns>
        public bool OpenCsPage(Context context,
            string title = "客服咨询",
            string sourceUrl = "",
            string sourceTitle = "",
            string cis = null)
        {
            //if (Unicorn.IsServiceAvailable)
            //{
            //    var source = new ConsultSource(sourceUrl, sourceTitle, cis);
            //    Unicorn.OpenServiceActivity(context, title, source);
            //    return true;
            //}
            //else
            //    return false;

            Intent intent = new Intent(Intent.ActionMain, null);
            intent.AddCategory(Intent.CategoryLauncher);
            var mApps = (context as Activity).PackageManager.QueryIntentActivities(intent, PackageInfoFlags.Activities);
            var listMaaps = mApps.ToList();
            var fileMgrInfo = mApps.Where(t => t.ActivityInfo.PackageName.Equals(KEFU_PACKAGE_NAME)).FirstOrDefault();
            if (fileMgrInfo != null)
            {
                var pkgName = fileMgrInfo.ActivityInfo.PackageName;
                //应用的主Activity
                ComponentName componentName = new ComponentName(pkgName, fileMgrInfo.ActivityInfo.Name);

                Intent intent_ni = new Intent();
                #region 包装Bundler
                var bundler = new Bundle();
                bundler.PutString("title", title);
                bundler.PutString("sourceUrl", sourceUrl);
                bundler.PutString("sourceTitle", sourceTitle);
                bundler.PutString("userInfo", userInfo.ToJson());
                #endregion
                //#region 注册用户
                //var ysfuserInfo = new YSFUserInfo();
                //ysfuserInfo.UserId = userInfo.UserId;
                //ysfuserInfo.AuthToken = "auth-token-from-user-server";
                //ysfuserInfo.Data = ProcessUserInfoJson(userInfo);
                //Unicorn.SetUserInfo(ysfuserInfo);
                //#endregion
                intent_ni.PutExtra("data", bundler);
                intent_ni.SetComponent(componentName);
                intent_ni.SetFlags(ActivityFlags.NewTask);
                (context as Activity).StartActivityForResult(intent_ni, 0x765);
                return true;
            }
            else
                return false;
        }

        private string ProcessUserInfoJson(Md_UserInfo userInfo)
        {
            var jk = new List<Md_YSFUserInfo>()
            {
                ProcessUserInfoSingle("real_name",userInfo.Name,false,-1,null,null),
                ProcessUserInfoSingle("mobile_phone",userInfo.Phone,false,-1,null,null),
                ProcessUserInfoSingle("email",userInfo.Email,false,-1,null,null),
                ProcessUserInfoSingle("avatar",userInfo.Avatar,false,-1,null,null),
                ProcessUserInfoSingle("real_name_auth",userInfo.Name,false,0,"实名认证",null),
                ProcessUserInfoSingle("bound_bank_card",userInfo.Name,false,1,"绑定银行卡",null),
                ProcessUserInfoSingle("recent_order",userInfo.Name,false,2,"最近订单",null),
                //new Md_YSFUserInfo(){key="real_name",value=userInfo.Name,hidden=false,index=-1,label=string.IsNullOrEmpty(label)?null:label,href=string.IsNullOrEmpty(href)?null:href },
                //new Md_YSFUserInfo(){key="mobile_phone",value=userInfo.Phone,hidden=false,index=-1,label=string.IsNullOrEmpty(label)?null:label,href=string.IsNullOrEmpty(href)?null:href },
                //new Md_YSFUserInfo(){key="email",value=userInfo.Email,hidden=false,index=-1,label=string.IsNullOrEmpty(label)?null:label,href=string.IsNullOrEmpty(href)?null:href },
                //new Md_YSFUserInfo(){key="avatar",value=userInfo.Avatar,hidden=false,index=-1,label=string.IsNullOrEmpty(label)?null:label,href=string.IsNullOrEmpty(href)?null:href },
                //new Md_YSFUserInfo(){key="real_name_auth",value="",hidden=false,index=0,label=null,href=null },
                //new Md_YSFUserInfo(){key="bound_bank_card",value="",hidden=false,index=1,label=null,href=null },
                //new Md_YSFUserInfo(){key="recent_order",value="",hidden=false,index=2,label=null,href=null },
            };
            return jk.ToJson();
        }

        private Md_YSFUserInfo ProcessUserInfoSingle(string key, object value, bool hidden, int index, string label, string href)
        {
            var obj = new Md_YSFUserInfo
            {
                key = key,
                value = value
            };
            if (hidden)
                obj.hidden = true;
            if (index >= 0)
                obj.index = index;
            if (string.IsNullOrEmpty(label))
                obj.label = label;
            if (string.IsNullOrEmpty(href))
                obj.href = href;

            return obj;
        }

        /// <summary>
        /// 客服程序包名
        /// </summary>
        private const string KEFU_PACKAGE_NAME = "nskf.mysafe.com.nanasakanakf";


        private class Md_UserInfo
        {
            public string UserId { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string Avatar { get; set; }
        }

        private class Md_YSFUserInfo
        {
            public string key { get; set; }
            public object value { get; set; }
            public bool hidden { get; set; }
            public int index { get; set; }
            public string label { get; set; }
            public string href { get; set; }
        }

        #endregion

        #region 工单系统相关


        #endregion

    }

}