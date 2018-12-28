using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace IdoMaster_GensouWorld.Activitys.Test.OtherToooooools.SettingsActivitys
{
    /// <summary>
    /// 屏幕显示相关
    /// </summary>
    public class Frg_Screen : Ys.BeLazy.YsBaseFragment
    {
        public override int A_GetFragmentContentViewId()
        {
            return Resource.Layout.frg_ucheng_account;
        }

        public override void B_BeforeInitFragmentView()
        {

        }

        public override void C_InitFragmentView(View view)
        {
            this.FindViewById<TextView>(Resource.Id.tv_title).Text = "显示";
        }

        public override void D_InitFragmentData()
        {

        }

        public override void E_SetOnFragmentClick(View view, EventArgs e)
        {

        }


    }
}