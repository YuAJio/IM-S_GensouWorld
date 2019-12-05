using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace IdoMaster_GensouWorld.Activitys.Test
{
    /// <summary>
    /// 左滑关闭页面测试
    /// </summary>
    [Activity(Label = "LeftDiftClosActivity", Theme = "@style/JK.SwipeBack.Transparent.Theme")]
    public class LeftDiftClosActivity : Ys.BeLazy.AdvanceWithTheTimes.BaseSwipeBackActivity
    {
        public override int A_GetContentViewId()
        {
            return -1;
        }

        public override void B_BeforeInitView()
        {
            this.SetContentView(ProcessLayout());
        }

        public override void C_InitView()
        {
            var rl = this.FindViewById<RelativeLayout>(0x12);

            var bt = this.FindViewById<Button>(0x123);

            var random = new Java.Util.Random();
            var red = random.NextInt(255);
            var green = random.NextInt(255);
            var blue = random.NextInt(255);

            rl.SetBackgroundColor(Color.Argb(255, red, green, blue));

            bt.Click -= OnClickListner;
            bt.Click += OnClickListner;
        }

        public override void D_BindEvent()
        {
        }

        public override void E_InitData()
        {
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {

        }


        private void OnClickListner
            (object sender, EventArgs e)
        {
            StartActivity(new Intent(this, typeof(LeftDiftClosActivity)));

        }

        private View ProcessLayout()
        {
            var v = new RelativeLayout(this) { LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent) };

            var rl_rela = new RelativeLayout(this) { Id = 0x12, LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent) };
            var rlp = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            rlp.AddRule(LayoutRules.CenterInParent);

            var bt = new Button(this) { Id = 0x123, LayoutParameters = rlp, Text = "つづく" };
            rl_rela.AddView(bt);

            v.AddView(rl_rela);
            return v;
        }
    }
}