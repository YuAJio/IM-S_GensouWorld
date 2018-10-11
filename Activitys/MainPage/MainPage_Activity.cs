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
using Android.Content.PM;
using IdoMaster_GensouWorld.Services;
using Android.Text;
using Java.Lang;
using System.Threading.Tasks;
using IMAS.Tips.Logic.LocalDBManager;
using Android.Views.InputMethods;
using Android.Support.V4.Content;
using System.Security.Cryptography;
using IMAS.Utils.Cryptographic;
using IMAS.Utils.Sp;
using IdoMaster_GensouWorld.Film_Activitys;
using Android.Hardware.Fingerprints;
using IdoMaster_GensouWorld.Listeners;
using Android;
using IdoMaster_GensouWorld.Utils;
using Com.Andremion.Floatingnavigationview;
using IdoMaster_GensouWorld.Activitys.Test;
using In.ShadowFax;
using Hardware.Print;
using ZXing.QrCode;
using System.IO;
using Android.Hardware;
using Android.Graphics;

namespace IdoMaster_GensouWorld.Activitys.MainPage
{
    [Activity(Label = "MainPage_Activity", Theme = "@style/Theme.PublicThemePlus")]
    public class MainPage_Activity : BaseActivity, ITextWatcher
    {
        private Button bt_new_game;
        private Button bt_lode_game;
        private Button bt_quite;
        private TextView tv_version;

        private AzPermissionManager permissionManager;
        private List<string> list_Permission;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_main_main;
        }

        public override void B_BeforeInitView()
        {

#if !DEBUG
            
            var intent = new Intent(this, typeof(BackGroundMusicPlayer));
            intent.PutExtra(BackGroundMusicPlayer.MusicSelectKey, (int)BGM_Enumeration.Main_BGM);
            ApplicationContext.StartService(intent);
             
#endif
        }

        public override void C_InitView()
        {
            bt_new_game = FindViewById<Button>(Resource.Id.bt_new_game);
            bt_lode_game = FindViewById<Button>(Resource.Id.bt_lode_game);
            bt_quite = FindViewById<Button>(Resource.Id.bt_quite_game);
            tv_version = FindViewById<TextView>(Resource.Id.tv_version_name);
        }

        public override void D_BindEvent()
        {
            bt_new_game.Click += OnClickListener;
            bt_lode_game.Click += OnClickListener;
            bt_quite.Click += OnClickListener;
            FindViewById<ImageView>(Resource.Id.iv_film_into).Click += OnClickListener; ;
        }

        public override void E_InitData()
        {

            tv_version.Text = GetVersionName();

            list_Permission = new List<string>()
            {
                Manifest.Permission.Camera,
                Manifest.Permission.ReadExternalStorage,
                Manifest.Permission.WriteExternalStorage,
            };
            permissionManager = new AzPermissionManager(list_Permission);

            permissionManager.CheckAndRequestPermissions(this);
        }

        private const string HASH_Eins = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
        private const string HASH_Zwei = "9d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
        private const string HASH_Drei = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
        private const string HASH_Vier = "9d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
        private const string HASH_Fuenf = "sa89e4hgy132k13bv2b1ws9d87fwsey21h9fg84waser1cxvnbmhg87ii8w322e8";

        private class JsoushiKousei
        {
            public string Name { get; set; }
            public decimal Liang { get; set; }
            public string Person { get; set; }
            public string Canci { get; set; }
            public DateTime DateTime { get; set; }
        }
        public override void F_OnClickListener(View v, EventArgs e)
        {
            var intent = new Intent();
            switch (v.Id)
            {
                case Resource.Id.bt_new_game:
                    ShowEditProducerInfomationPop();
                    //StartActivity(new Intent(this, typeof(Film_HomePage)));
                    break;
                case Resource.Id.bt_lode_game:
                    intent.SetClass(this, typeof(LoadGame_Activity));
                    StartActivity(intent);
                    break;
                case Resource.Id.bt_quite_game:
#if DEBUG
                    {
                        StartActivity(new Intent(this, typeof(WeekSleepControlActivity)));
                        //YsDialogManager.BuildIosSingleChoose(new List<string>() { "", "", "", "" }, new YsMyItemDialogListener((j, k) => { })).Show();
                        //var jk = YsDialogManager.BuildMdAlert("这是标题", "这个是啥", new YsMyDialogListener(() =>
                        //{
                        //    ShowMsgLong("这个是第一个 第一个!");
                        //}, () =>
                        //{
                        //    ShowMsgLong("这个是第二个 第二个!");
                        //}, () =>
                        //{
                        //    ShowMsgLong("这个是第三个 第三个!");
                        //}, () =>
                        //{
                        //    ShowMsgLong("这个是取消取消 取消取消!");
                        //})).SetBtnText("我第一", "我第二", "我第三").Show();
                    }
#else
 
                    //结束所有Activity
                    IMAS_Application.Sington?.OpenActivityList?.Clear();
                    //  APPManager.GetInstance().KillAllActivity();
                    CleanAllActivity(IMAS_Application.Sington.OpenActivityList.Count);
                    //关闭服务
                    ApplicationContext.StopService(new Intent(this, typeof(BackGroundMusicPlayer)));
                    this.Finish();
#endif
                    break;
                    //case Resource.Id.iv_film_into:
                    //    {
                    //        var jk = IsFingerCanUse();
                    //        if (jk.IsSuccess)
                    //        {
                    //            StartListening();
                    //            ShowSureConfim(null, "指紋を入力してください", (j, k) =>
                    //            {
                    //                mCancellationSignal.Cancel();
                    //            }, "キャンセル", false);
                    //        }
                    //        else
                    //        {
                    //            ShowMsgShort(jk.Message);
                    //            IntoFilmActivity();
                    //        }
                    //    }
                    //    break;
            }
        }

        private void OnFirsetDialogClick()
        {
            ShowDiyToastLong("好呀好呀");
        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }
        /// <summary>
        /// 进入电影院
        /// </summary>
        private void IntoFilmActivity()
        {
            DismissConfim();
            StartActivity(new Intent(this, typeof(Film_HomePage)));
        }

        //#region 指纹读取相关
        //private CancellationSignal mCancellationSignal;
        //private FingerprintManager fpManager;
        //private KeyguardManager kgManager;
        //private FingerAuthenticationCallback mSelfCancelled;


        //private const string TAG = "finger_log";

        ///// <summary>
        ///// 初始化指纹识别管理器
        ///// </summary>
        //private void InitFingerPrint()
        //{
        //    mCancellationSignal = new CancellationSignal();
        //    fpManager = (FingerprintManager)GetSystemService(FingerprintService);
        //    kgManager = (KeyguardManager)GetSystemService(KeyguardService);
        //    if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        //        InitFingerCallBack();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //private Results IsFingerCanUse()
        //{
        //    var result = new Results();
        //    if (fpManager != null)
        //    {
        //        if (!fpManager.IsHardwareDetected)
        //            return result.Error(message: "没有指纹识别模块");
        //        if (!kgManager.IsKeyguardSecure)
        //            return result.Error(message: "没有开启锁屏密码");
        //        if (!fpManager.HasEnrolledFingerprints)
        //            return result.Error(message: "没有录入指纹");
        //    }
        //    return result.Success();
        //}

        ///// <summary>
        ///// 开始监听
        ///// </summary>
        //private void StartListening()
        //{
        //    //android studio 上，没有这个会报错
        //    if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, Manifest.Permission.UseFingerprint) != Android.Content.PM.Permission.Granted)
        //    {
        //        ShowMsgLong("没有指纹识别权限");
        //        return;
        //    }
        //    if (mCancellationSignal != null)
        //        if (mCancellationSignal.IsCanceled)
        //            mCancellationSignal = new CancellationSignal();

        //    fpManager.Authenticate(null, mCancellationSignal, FingerprintAuthenticationFlags.None, mSelfCancelled, null);
        //}

        //private void ShowAuthenticationScreen()
        //{
        //    var intent = kgManager.CreateConfirmDeviceCredentialIntent("finger", "测试指纹识别");
        //    isCanBeCallBack = false;
        //    if (intent != null)
        //        StartActivityForResult(intent, IMAS_Constants.OnDeviceFingerCheckKey);
        //}

        ///// <summary>
        ///// 实现指纹验证回调
        ///// </summary>
        //private void InitFingerCallBack()
        //{
        //    mSelfCancelled = new FingerAuthenticationCallback((val) => { OnAuthenticationSucceeded(val); }, () => { OnAuthenticationFailed(); }, (j, k) => { OnAuthenticationError(j, k); }, (j, k) => { OnAuthenticationHelp(j, k); });
        //}

        //#region 接口回调
        //private bool isCanBeCallBack = true;
        //private void OnAuthenticationSucceeded(FingerprintManager.AuthenticationResult result)
        //{
        //    ShowMsgLong("指纹验证成功");

        //    IntoFilmActivity();
        //}
        //private void OnAuthenticationFailed()
        //{
        //    ShowMsgLong("指纹识别失败");
        //}
        //private void OnAuthenticationError(FingerprintState errorCode, ICharSequence errString)
        //{
        //    if (!isCanBeCallBack)
        //        return;
        //    //但多次指纹密码验证错误后，进入此方法；并且，不能短时间内调用指纹验证
        //    ShowMsgLong("指纹验证多次失败");
        //    ShowAuthenticationScreen();
        //}
        //public void OnAuthenticationHelp(FingerprintState helpCode, ICharSequence helpString)
        //{
        //    ShowMsgLong(helpString.ToString());
        //}
        //#endregion
        ////#endregion

        #region 开始新游戏弹出制作人信息弹出框
        private EditText et_input;
        /// <summary>
        /// 显示弹出框
        /// </summary>
        private void ShowEditProducerInfomationPop()
        {
            var jk = new AlertDialog.Builder(this);
            var view = View.Inflate(this, Resource.Layout.pop_producer_status_input, null);
            et_input = view.FindViewById<EditText>(Resource.Id.et_input);
            view.FindViewById<Button>(Resource.Id.bt_next).Click -= OnPopWindowClickListener;
            view.FindViewById<Button>(Resource.Id.bt_next).Click += OnPopWindowClickListener;
            et_input.RemoveTextChangedListener(this);
            et_input.AddTextChangedListener(this);
            jk.SetView(view);
            _ConfigDialog = jk.Show();
            _ConfigDialog.Window.ClearFlags(WindowManagerFlags.DimBehind);
            _ConfigDialog.Window.SetWindowAnimations(Resource.Style.DialogFadeAnimation);
            //  _ConfigDialog.Window.SetSoftInputMode(SoftInput.StateHidden);

        }

        /// <summary>
        /// 对话框点击监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPopWindowClickListener(object sender, EventArgs e)
        {
            var imm = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
            imm.ToggleSoftInput(0, HideSoftInputFlags.NotAlways);
            var v = sender as View;
            switch (v.Id)
            {
                case Resource.Id.bt_next:
                    var str_input = et_input.Text.Trim();
                    if (string.IsNullOrEmpty(str_input))
                    {
                        et_input.SetBackgroundResource(Resource.Drawable.bg_edittext_login_error);
                        et_input.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.white));
                    }
                    else
                    {
                        SQLiteQProducerInfo(str_input);
                    }

                    break;
            }
        }
        #endregion

        #region SQLite操作
        private void SQLiteQProducerInfo(string name)
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QProducerInfo(name);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    ShowDiyToastLong("この名前わ既に存在します");
                    et_input.SetBackgroundResource(Resource.Drawable.bg_edittext_login_error);
                    et_input.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.white));
                }
                else
                {
                    et_input.SetBackgroundResource(Resource.Drawable.bg_edittext_login);
                    SQLiteInsertProducerInfo(name);

                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        /// <summary>
        /// 插入制作人信息
        /// </summary>
        /// <param name="name"></param>
        private void SQLiteInsertProducerInfo(string name)
        {
            Task.Run(async () =>
           {
               var result = await IMAS_ProAppDBManager.GetInstance().InsertProducerInfo(name, DateTime.Now);
               return result;
           }).ContinueWith(t =>
           {
               if (t.Result.IsSuccess)
               {
                   AndroidPreferenceProvider.GetInstance().PutString(IMAS_Constants.SpProducerInfoNameKey, name);

                   IMAS_Constants.Producer_Name = $"{name}";
                   StartActivity(new Intent(this, typeof(NewGame_Activity)));
                   StopService(new Intent(this, typeof(BackGroundMusicPlayer)));
                   this.Finish();
               }
               else
               {
                   ShowDiyToastLong(t.Result.Message);
               }
           }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

        #region 监听EditText
        public void AfterTextChanged(IEditable s)
        {

        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {

        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            if (et_input == null)
            {
                return;
            }
            var str_input = et_input?.Text.Trim();
            if (!string.IsNullOrEmpty(str_input))
            {
                et_input.SetBackgroundResource(Resource.Drawable.bg_edittext_login);
                et_input.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.darkgray));
            }
        }
        #endregion

        #region 显示版本号
        private int EasterEggCount;
        /// <summary>
        /// 获取APP版本号 实例="1.1.1"
        /// </summary>
        /// <returns></returns>
        private string GetVersionName()
        {
            var packageManager = this.PackageManager;
            PackageInfo packageInfo;
            var packageName = "";

            try
            {
                packageInfo = packageManager.GetPackageInfo(this.PackageName, 0);
                packageName = packageInfo.VersionName + "";
            }
            catch (System.Exception)
            {

            }
            return packageName;
        }
        #endregion

        #region 监听返回键做APP退出
        private void CleanAllActivity(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var activity = IMAS_Application.Sington.OpenActivityList[IMAS_Application.Sington.OpenActivityList.Count];
                IMAS_Application.Sington.OpenActivityList.Remove(activity);
                activity.Finish();
            }
            GC.Collect();
        }

        long waitTime = 2000;// 如果两次按下时间在2000毫秒以内，则退出
        long touchTime = 0;// 记录上一次按下的时间

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (e.Action == KeyEventActions.Down && Keycode.Back == keyCode)
            {
                long currentTime = Java.Lang.JavaSystem.CurrentTimeMillis();
                if ((currentTime - touchTime) >= waitTime)
                {
                    Toast.MakeText(this, "再びクリックお終了します", ToastLength.Short).Show();
                    touchTime = currentTime;
                }
                else
                {
                    //结束所有Activity
                    IMAS_Application.Sington?.OpenActivityList?.Clear();
                    //  APPManager.GetInstance().KillAllActivity();
                    CleanAllActivity(IMAS_Application.Sington.OpenActivityList.Count);
                    //关闭服务
                    ApplicationContext.StopService(new Intent(this, typeof(BackGroundMusicPlayer)));
                    this.Finish();
                }
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }

        #endregion


        private Bitmap CreatErWeiMa(string content)
        {
            var options = new QrCodeEncodingOptions();
            options.CharacterSet = "UTF-8";
            options.DisableECI = true;
            options.ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.H;
            options.Width = 165;
            options.Height = 165;
            options.Margin = 1;

            var writer = new ZXing.BarcodeWriter();
            writer.Format = ZXing.BarcodeFormat.QR_CODE;
            writer.Options = options;


            var bmp = writer.Write("http://www.acfun.cn/");
            return bmp;

            //using (var bmp = writer.Write("http://www.acfun.cn/")) // Write 具备生成、写入两个功能
            //{
            //    return bmp;
            //}
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            permissionManager.CheckResult(requestCode, permissions, grantResults);

            var granted = permissionManager.Status[0].Granted;
            var denied = permissionManager.Status[0].Denied;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == IMAS_Constants.OnDeviceFingerCheckKey)
            {
                //isCanBeCallBack = true;
                if (resultCode == Result.Ok)
                {
                    ShowMsgLong("识别成功于ActivityResult");
                    IntoFilmActivity();
                }
                else
                    ShowMsgLong("识别失败了于ActivityResult");

            }

        }

    }
}