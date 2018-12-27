using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V7.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using IdoMaster_GensouWorld.Utils;
using Android.Support.V4.Content;
using Android.Net.Wifi;

namespace IdoMaster_GensouWorld.CustomControl.Dialogs
{
    public class WifiLinkDialog : AlertDialog
    {
        private string wifiName;
        private string capabilities;

        private Context context;

        public Action onCancelClickAct;
        public Action<TextInputEditText> onLinkClickAct;

        private WifiDialogType dialogType;
        public enum WifiDialogType
        {
            /// <summary>
            /// 密码输入
            /// </summary>
            PassWordInput,
            /// <summary>
            /// 查看已连接WIFI状态
            /// </summary>
            ChechOutWifiInfo
        }

        public WifiLinkDialog(Context context, string wifiName, string capabilities) : base(context)
        {
            this.wifiName = wifiName;
            this.capabilities = capabilities;
            this.context = context;
            dialogType = WifiDialogType.PassWordInput;
        }

        public WifiLinkDialog(Context context, WifiInfo wifiInfo) : base(context)
        {
            this.context = context;
            this.connectingWifiInfo = wifiInfo;
            dialogType = WifiDialogType.ChechOutWifiInfo;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            switch (dialogType)
            {
                case WifiDialogType.PassWordInput:
                    InitPassWordInputDialog();
                    break;
                case WifiDialogType.ChechOutWifiInfo:
                    InitWifiInfoDialog();
                    break;
            }

        }

        #region 输入密码Dialog
        private string inputPassWord;
        private TextView tv_Link;
        private TextInputEditText et_PassWord;
        private TextInputLayout til_PassWord;

        private void InitPassWordInputDialog()
        {
            Window.ClearFlags(WindowManagerFlags.AltFocusableIm);

            var view = View.Inflate(context, Resource.Layout.utils_diy_dialog_wifi_password_input, null);
            SetContentView(view);

            view.FindViewById<TextView>(Resource.Id.tv_Name).Text = this.wifiName;
            til_PassWord = view.FindViewById<TextInputLayout>(Resource.Id.til_PassWord);
            et_PassWord = view.FindViewById<TextInputEditText>(Resource.Id.et_PassWord);
            et_PassWord.TextChanged -= OnPassWordInputingListener;
            et_PassWord.TextChanged += OnPassWordInputingListener;

            var tv_C = view.FindViewById<TextView>(Resource.Id.tv_Cancel);
            tv_Link = view.FindViewById<TextView>(Resource.Id.tv_Link);
            tv_C.Click -= CancelClick;
            tv_C.Click += CancelClick;

            tv_Link.Click -= LinkClick;
            tv_Link.Click += LinkClick;
        }

        /// <summary>
        /// 连接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkClick(object sender, EventArgs e)
        {
            var tempConfig = WifiSupport.IsExsits(wifiName, context);
            if (tempConfig == null)
            {
                var wifiConfiguration = WifiSupport.CreateWifiConfig(wifiName, inputPassWord, WifiSupport.GetWifiCipher(capabilities));
                if (!WifiSupport.AddNetWork(wifiConfiguration, context))
                {
                    til_PassWord.ErrorEnabled = true;
                    til_PassWord.Error = "密码错误";
                }
                else
                {
                    til_PassWord.ErrorEnabled = false;
                    this.Dismiss();
                }
            }
            else
              if (WifiSupport.AddNetWork(tempConfig, context))
            {
                Toast.MakeText(context, "连接成功", ToastLength.Short);
                this.Dismiss();
            }
            else
            {
                Toast.MakeText(context, "连接失败", ToastLength.Short);
            }

            //onLinkClickAct?.Invoke(et_PassWord);
        }

        /// <summary>
        ///  取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelClick(object sender, EventArgs e)
        {
            this.Dismiss();
            //onCancelClickAct?.Invoke();
        }

        private void OnPassWordInputingListener(object sender, Android.Text.TextChangedEventArgs e)
        {
            inputPassWord = e.Text.ToString();

            //输入的字符小于八位,连接按钮不打开
            tv_Link.Enabled = inputPassWord.Length >= 8;
        }
        #endregion

        #region 查看WIFI连接状态Dialog
        private readonly WifiInfo connectingWifiInfo;

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitWifiInfoDialog()
        {
            var view = View.Inflate(context, Resource.Layout.utils_diy_dialog_wifi_info_check, null);
            SetContentView(view);
            view.FindViewById<TextView>(Resource.Id.tv_Name).Text = this.connectingWifiInfo.SSID;
            var ll_info = view.FindViewById<LinearLayout>(Resource.Id.ll_info);
            #region 添加信息
            ll_info.AddView(GetInfoTextView("メッセージだよ　　レロレロ"));
            #endregion

            var tv_Cancle = view.FindViewById<TextView>(Resource.Id.tv_Cancel);
            var tv_Cut = view.FindViewById<TextView>(Resource.Id.tv_Cut);
            tv_Cancle.Click -= CancelClick;
            tv_Cancle.Click += CancelClick;

            tv_Cut.Click -= CutClick;
            tv_Cut.Click += CutClick;
        }

        /// <summary>
        /// 创建info展示用TextView
        /// </summary>
        /// <returns></returns>
        private TextView GetInfoTextView(string str = "")
        {
            var tv = new TextView(context)
            {
                TextSize = 14,
                Text = str
            };
            tv.SetTextColor(ContextCompat.GetColorStateList(context, Resource.Color.gray));
            return tv;
        }

        /// <summary>
        /// 断开WIFI连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CutClick(object sender, EventArgs e)
        {
            if (WifiSupport.CutTheConnect(context, connectingWifiInfo?.NetworkId))
                this.Dismiss();
            else
                Toast.MakeText(context, "断开失败", ToastLength.Short);
        }
        #endregion


    }
}