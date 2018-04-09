using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using Com.Andremion.Floatingnavigationview;

namespace IdoMaster_GensouWorld.Activitys.Test
{
    /// <summary>
    /// 侧滑测试
    /// </summary>
    [Activity(Label = "SlidActivity", Theme = "@style/Theme.PublicThemePlus")]
    public class SlidActivity : BaseActivity
    {
        private FloatingNavigationView mFloatingNavigationView;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.test_activity_slid;
        }

        public override void B_BeforeInitView()
        {

        }

        public override void C_InitView()
        {
            mFloatingNavigationView = FindViewById<FloatingNavigationView>(Resource.Id.floating_navigation_view);

        }

        public override void D_BindEvent()
        {
            mFloatingNavigationView.Click += OnClickListener;

        }

        public override void E_InitData()
        {
            var jk = new MyInterface();
            mFloatingNavigationView.SetNavigationItemSelectedListener(jk);
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
            mFloatingNavigationView.Open();
        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }

        public override void OnBackPressed()
        {
            if (mFloatingNavigationView.IsOpened)
                mFloatingNavigationView.Close();
            base.OnBackPressed();
        }

        public class MyInterface : Java.Lang.Object, NavigationView.IOnNavigationItemSelectedListener
        {
            public bool OnNavigationItemSelected(IMenuItem menuItem)
            {
             
                return true;
            }
        }
    }
}