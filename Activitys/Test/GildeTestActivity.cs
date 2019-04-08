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
using Bumptech.Glide;

namespace IdoMaster_GensouWorld.Activitys.Test
{
    [Activity(Label = "GildeTestActivity", Theme = "@style/Theme.PublicThemePlus")]
    public class GildeTestActivity : Ys.BeLazy.YsBaseActivity
    {
        private ImageView iv_1;
        private ImageView iv_2;
        private ImageView iv_3;
        private ImageView iv_4;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_gilde_test;
        }

        public override void B_BeforeInitView()
        {
        }

        public override void C_InitView()
        {
            iv_1 = FindViewById<ImageView>(Resource.Id.iv_1);
            iv_2 = FindViewById<ImageView>(Resource.Id.iv_2);
            iv_3 = FindViewById<ImageView>(Resource.Id.iv_3);
            iv_4 = FindViewById<ImageView>(Resource.Id.iv_4);
        }

        public override void D_BindEvent()
        {
        }

        public override void E_InitData()
        {
            var url1 = "https://cancanan-1257056207.cos.ap-chengdu.myqcloud.com/misimage/2bbaf06d-60d7-4b6e-a422-4d37b5daf830.gif";
            var url2 = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=3087766220,2961674138&fm=26&gp=0.jpg";
            var url3 = "https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=1494247418,482341077&fm=26&gp=0.jpg";
            var url4 = "https://cancanan-1257056207.cos.ap-chengdu.myqcloud.com/misimage/9cdca1f5-4a0e-4559-a559-527acce4819b.gif";

            Glide.With(this).Load(url1).Into(iv_1); ;
            Glide.With(this).Load(url2).Into(iv_2); ;
            Glide.With(this).Load(url3).Into(iv_3); ;
            Glide.With(this).Load(url4).Into(iv_4); ;


        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
        }
    }
}