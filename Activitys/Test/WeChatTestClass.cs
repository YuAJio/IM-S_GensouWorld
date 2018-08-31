//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Com.Tencent.MM.Opensdk.Modelbase;
//using Wechatsdk.Yurishi.Com.Wechatandroidsdk;

//namespace IdoMaster_GensouWorld.Activitys.Test
//{
//    public class WeChatTestClass : YsWeChatManager
//    {
//        public WeChatTestClass(Action<BaseResp> action)
//        {
//            this.onResp = action;
//        }

//        /// <summary>
//        /// 打开小程序
//        /// </summary>
//        /// <param name="context">上下文</param>
//        /// <param name="appId">小程序ID</param>
//        /// <param name="userName">小程序原始ID</param>
//        /// <param name="path">拉起小程序页面的可带参路径，不填默认拉起小程序首页</param>
//        /// <param name="miniprogramType">可选打开 开发版，体验版和正式版  0:Release 1:Test 2:PreView</param>
//        /// <returns></returns>
//        public bool YsStartMiniProgram(Context context, string appId, string userName, string path, int miniprogramType)
//        {
//            return this.StartMiniProgram(context, appId, userName, path, miniprogramType);
//        }
//        public Action<BaseResp> onResp;

//        public override void OnResp(BaseResp p0)
//        {
//            onResp?.Invoke(p0);
//        }
//    }
//}