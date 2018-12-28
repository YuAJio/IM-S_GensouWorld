using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;

namespace IdoMaster_GensouWorld.CustomControl
{
    public class YsSettingMenuView : LinearLayout
    {
        /// <summary>
        /// 是否添加下划线
        /// </summary>
        private bool isAddDiv;
        /// <summary>
        /// 标题
        /// </summary>
        public TextView tv_Menu { get; set; }
        /// <summary>
        /// 下划线
        /// </summary>
        public View v_diviver { get; set; }


        /// <summary>
        /// 自定义菜单控件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="isAddDiv">是否添加下划线</param>
        public YsSettingMenuView(Context context, int id, bool isAddDiv) : base(context)
        {
            this.isAddDiv = isAddDiv;
            this.Id = id;
            Init();
        }




        private void Init()
        {
            this.Orientation = Orientation.Vertical;
            this.SetBackgroundResource(Resource.Drawable.bg_textview_setting_menu);
            this.SetMinimumHeight(PX2DP(56));

            InitTextView();
            InitDivider();

            this.AddView(tv_Menu);
            this.AddView(v_diviver);
            if (!isAddDiv)
                v_diviver.Visibility = ViewStates.Invisible;
        }

        private void InitTextView()
        {
            if (tv_Menu == null)
            {
                var lp = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent)
                {
                    MarginStart = PX2DP(12)
                };
                tv_Menu = new TextView(Context)
                {
                    LayoutParameters = lp,
                    Gravity = GravityFlags.Start | GravityFlags.CenterVertical,
                };

                //tv_Menu.SetTextColor(ContextCompat.GetColorStateList(Context, Text_MenuColor/* <= 0 ? Resource.Color.black : Text_MenuColor*/));
                tv_Menu.SetPadding(0, PX2DP(18), 0, PX2DP(18));
            }
        }

        private void InitDivider()
        {
            if (v_diviver == null)
            {
                var lp = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, PX2DP(1))
                {
                    MarginStart = PX2DP(4),
                    MarginEnd = PX2DP(4),
                    TopMargin = PX2DP(6),
                    //BottomMargin = PX2DP(6)
                };

                v_diviver = new View(Context)
                {
                    LayoutParameters = lp
                };

            }

        }

        private int PX2DP(float pxValue)
        {
            var scale = Context.Resources.DisplayMetrics.Density;
            return (int)(pxValue / scale + 0.5f);
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();


        }

    }
}