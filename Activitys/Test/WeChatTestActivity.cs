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
//using Com.Tencent.MM.Opensdk.Openapi;
//using Wechatsdk.Yurishi.Com.Wechatandroidsdk;
//using YsWeChatSDK;

//namespace IdoMaster_GensouWorld.Activitys.Test
//{
//    [Activity(Label = "WeChatTestActivity", Theme = "@style/Theme.PublicThemePlus")]
//    public class WeChatTestActivity : BaseActivity
//    {
//        public override int A_GetContentViewId()
//        {
//            return 0;
//        }

//        public override void B_BeforeInitView()
//        {

//        }

//        public override void C_InitView()
//        {

//        }

//        public override void D_BindEvent()
//        {

//        }

//        public override void E_InitData()
//        {
//            OpenWeChatMiniProgram();
//        }

//        public override void F_OnClickListener(View v, EventArgs e)
//        {

//        }

//        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
//        {

//        }

//        //private WeChatMiniProManager weChatClient;

//        private void OpenWeChatMiniProgram()
//        {
//            var appId = "wx06976f6bb8b69ff3"/*"wxd8453ba255ed33c0"*/;
//            var api = WXAPIFactory.CreateWXAPI(this, appId);
//            weChatClient = new WeChatMiniProManager(OnRespAction);

//            try
//            {
//                var jks = weChatClient.YsStartMiniProgram(this, appId, "gh_f77a6c86ae48", "", 0);
//            }
//            catch (Exception e)
//            {

//            }
//            //var req = new Com.Tencent.MM.Opensdk.Utils.()
//            //{
//            //    userName = "gh_15a49211ffae",
//            //    path = "",
//            //    miniprogramType = Req.MINIPTOGRAM_TYPE_RELEASE,
//            //};
//            //try
//            //{
//            //    api.SendReq(req);
//            //}
//            //catch (Exception e)
//            //{
//            //}

//        }

//        private void OnRespAction(BaseResp resp)
//        {

//        }

//        //private class YsReq : Wechatsdk.Yurishi.Com.Wechatandroidsdk.YsWeChatManager
//        //{
//        //    private const string TAG = "MicroMsg.SDK.WXLaunchMiniProgram.Req";
//        //    public const int MINIPTOGRAM_TYPE_RELEASE = 0;
//        //    public const int MINIPROGRAM_TYPE_TEST = 1;
//        //    public const int MINIPROGRAM_TYPE_PREVIEW = 2;
//        //    /// <summary>
//        //    /// 填小程序原始id，
//        //    /// </summary>
//        //    public string userName;
//        //    /// <summary>
//        //    /// 拉起小程序页面的可带参路径，不填默认拉起小程序首页
//        //    /// </summary>
//        //    public string path = "";
//        //    /// <summary>
//        //    /// 可选打开 开发版，体验版和正式版
//        //    /// </summary>
//        //    public int miniprogramType = 0;


//        //    public YsReq() { }

//        //    public override int Type { get { return 19; } }

//        //    public override bool CheckArgs()
//        //    {
//        //        if (string.IsNullOrEmpty(userName))
//        //            return false;
//        //        else if (this.miniprogramType >= 0 && this.miniprogramType <= 2)
//        //            return true;
//        //        else
//        //            return false;
//        //    }

//        //    public void ToBundle(Bundle var1)
//        //    {
//        //        base.ToBundle(var1);
//        //        var1.PutString("_launch_wxminiprogram_username", this.userName);
//        //        var1.PutString("_launch_wxminiprogram_path", this.path);
//        //        var1.PutInt("_launch_wxminiprogram_type", this.miniprogramType);
//        //    }

//        //}
//    }
//}