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
using Com.Qiyukf.Unicorn.Api;

namespace IdoMaster_GensouWorld.Activitys.Test
{
    /// <summary>
    /// 七鱼测试
    /// </summary>
    [Activity(Label = "SevenFishPigActivity", Theme = "@style/Theme.PublicThemePlus")]
    public class SevenFishPigActivity : BaseActivity
    {
        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_sevenfish_pigfactory_test;
        }

        public override void B_BeforeInitView()
        {

        }

        public override void C_InitView()
        {

        }

        public override void D_BindEvent()
        {

        }

        public override void E_InitData()
        {
            var title = "東北きりたん";
            var source = new ConsultSource(null, null, "custom infomation string");
            if (Unicorn.IsServiceAvailable)
            {
                Unicorn.OpenServiceActivity(this, title, source);
            }
            else
            {
                ShowMsgLong("打开客服失败..");
            }
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }



    }
}