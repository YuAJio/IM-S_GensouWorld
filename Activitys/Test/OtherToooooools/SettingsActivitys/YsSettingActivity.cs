using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using IdoMaster_GensouWorld.CustomControl;
using IdoMaster_GensouWorld.Utils;

namespace IdoMaster_GensouWorld.Activitys.Test.OtherToooooools.SettingsActivitys
{
    /// <summary>
    /// 设置页面
    /// </summary>
    [Activity(Label = "YsSettingActivity", Theme = "@style/Theme.PublicThemePlus")]
    public class YsSettingActivity : Ys.BeLazy.YsBaseFragmentActivity
    {
        #region UI控件
        private ImageView iv_Back;
        private LinearLayout ll_Menu;
        private ConstraintLayout cl_Account;

        /// <summary>
        /// U称用户名
        /// </summary>
        private RunHorseLanternTextView tv_UcName;
        /// <summary>
        /// U称头像
        /// </summary>
        private ImageView iv_UcAvatar;
        #endregion

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_ys_setting;
        }

        public override void B_BeforeInitView()
        {
        }

        public override void C_InitView()
        {
            iv_Back = FindViewById<ImageView>(Resource.Id.iv_back);
            ll_Menu = FindViewById<LinearLayout>(Resource.Id.ll_menu);
            cl_Account = FindViewById<ConstraintLayout>(Resource.Id.cl_account);
            tv_UcName = FindViewById<RunHorseLanternTextView>(Resource.Id.tv_name);
            iv_UcAvatar = FindViewById<ImageView>(Resource.Id.iv_avatar);
        }

        public override void D_BindEvent()
        {
            iv_Back.Click += OnClickListener;
        }

        public override void E_InitData()
        {
            ll_Menu.AddView(CreatMenuView(0x101, true, title: "WIFI设置"));
            ll_Menu.AddView(CreatMenuView(0x102, true, title: "显示设置"));
            ll_Menu.AddView(CreatMenuView(0x103, true));
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
            switch (v.Id)
            {
                case Resource.Id.iv_back:
                    {
                        this.Finish();
                    }
                    break;
            }
        }

        private void OnMenuClickListner(object sender, EventArgs eventArgse)
        {
            var v = sender as View;
            switch (v.Id)
            {
                case 0x101:
                    {//wifi
                        if (Frg_UChengAccount == null)
                            Frg_UChengAccount = new Frg_UChengAccount();
                        ChangeFragment(Frg_UChengAccount);
                    }
                    break;
                case 0x102:
                    {
                        if (Frg_Screen == null)
                            Frg_Screen = new Frg_Screen();
                        ChangeFragment(Frg_Screen);
                    }
                    break;
                case 0x103:
                    {
                        if (Frg_UChengAccount == null)
                            Frg_UChengAccount = new Frg_UChengAccount();
                        ChangeFragment(Frg_UChengAccount);
                    }
                    break;
            }
        }

        #region Fragment事务相关 
        private Frg_UChengAccount Frg_UChengAccount;
        private Frg_Screen Frg_Screen;

        /// <summary>
        /// 提交Fragment,切换
        /// </summary>
        /// <param name="fragment"></param>
        private void ChangeFragment(Android.Support.V4.App.Fragment fragment)
        {
            SupportFragmentManager.BeginTransaction().
                Replace(GetFragmentCarrier(), fragment).
                Commit();
        }

        /// <summary>
        /// 获取Fragment载体
        /// </summary>
        /// <returns></returns>
        private int GetFragmentCarrier()
        {
            return Resource.Id.fl_fragment;
        }
        #endregion



        /// <summary>
        /// 创建菜单视图
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="isAddDiver"></param>
        /// <param name="clickAction"></param>
        /// <param name="title"></param>
        /// <param name="textSize"></param>
        /// <param name="textColor"></param>
        /// <param name="divColor"></param>
        /// <returns></returns>
        private YsSettingMenuView CreatMenuView(int id, bool isAddDiver, string title = "", int textSize = 0, int textColor = 0, int divColor = 0)
        {
            var mView = new YsSettingMenuView(this, id, isAddDiver);
            mView.tv_Menu.Text = string.IsNullOrEmpty(title) ? "其他选项" : title;
            mView.tv_Menu.TextSize = textSize <= 0 ? DPChangeUtils.Px2dip(22) : textSize;
            mView.tv_Menu.SetTextColor(ContextCompat.GetColorStateList(this, textColor <= 0 ? Resource.Color.black : textColor));
            mView.v_diviver.SetBackgroundResource(divColor <= 0 ? Resource.Color.gainsboro : divColor);

            mView.Click -= OnMenuClickListner;
            mView.Click += OnMenuClickListner;
            return mView;
        }



    }
}