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

namespace IdoMaster_GensouWorld.Activitys.MainPage
{
    [Activity(Label = "MainPage_Activity", Theme = "@style/Theme.PublicTheme")]
    public class MainPage_Activity : BaseActivity, ITextWatcher
    {
        private Button bt_new_game;
        private Button bt_lode_game;
        private Button bt_quite;
        private TextView tv_version;
        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_main_main;
        }

        public override void B_BeforeInitView()
        {
            var intent = new Intent(this, typeof(BackGroundMusicPlayer));
            intent.PutExtra(BackGroundMusicPlayer.MusicSelectKey, (int)BGM_Enumeration.Main_BGM);
            ApplicationContext.StartService(intent);
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
        }

        public override void E_InitData()
        {
            tv_version.Text = GetVersionName();
        }

        private const string HASH_Eins = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
        private const string HASH_Zwei = "9d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
        private const string HASH_Drei = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
        private const string HASH_Vier = "9d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
        private const string HASH_Fuenf = "sa89e4hgy132k13bv2b1ws9d87fwsey21h9fg84waser1cxvnbmhg87ii8w322e8";
        public override void F_OnClickListener(View v, EventArgs e)
        {
            var intent = new Intent();
            switch (v.Id)
            {
                case Resource.Id.bt_new_game:
                    ShowEditProducerInfomationPop();
                    break;
                case Resource.Id.bt_lode_game:
                    intent.SetClass(this, typeof(LoadGame_Activity));
                    StartActivity(intent);
                    break;
                case Resource.Id.bt_quite_game:
                    //结束所有Activity
                    IMAS_Application.Sington?.OpenActivityList?.Clear();
                    //  APPManager.GetInstance().KillAllActivity();
                    CleanAllActivity(IMAS_Application.Sington.OpenActivityList.Count);
                    //关闭服务
                    ApplicationContext.StopService(new Intent(this, typeof(BackGroundMusicPlayer)));
                    this.Finish();
                    break;
            }
        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }
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
    }
}