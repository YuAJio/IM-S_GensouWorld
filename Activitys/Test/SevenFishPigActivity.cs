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
using IdoMaster_GensouWorld.CustomControl;

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
            Utils.Managers.NanaSakanaManager.GetInstance().InitUserInfo(
                userId: "GGTX",
                avatar: "http://img0.imgtn.bdimg.com/it/u=1037853487,1298829527&fm=26&gp=0.jpg",
                phone: "15111845151",
                email: "yuajio@yahoo.com",
               name: "イザヨイ　サクヤ"
                );
        }

        public override void C_InitView()
        {

            var context = this;
            Utils.Managers.NanaSakanaManager.GetInstance().OpenCsPage(
                context,
                title: "关于アイゲンソウ",
                sourceTitle: "A岛匿名版",
                sourceUrl: "http://adnmb1.com/Forum/timeline/page/3.html");
            context.Finish();
        }

        public override void D_BindEvent()
        {

        }

        public override void E_InitData()
        {
            ShowView();
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }


        #region 悬浮窗所需
        private WindowManagerLayoutParams @params;
        private FloatView mLayout;

        private void ShowView()
        {
            mLayout = new FloatView(ApplicationContext);
            mLayout.SetWindowManager(WindowManager);
            mLayout.SetBackgroundResource(Resource.Mipmap.icon_ac_video_error);

            //var jc = WindowManager;
            ////获取WindowManager
            //var js = GetSystemService(WindowService);
            //var jk = ApplicationContext.GetSystemService(WindowService);
            //windowManager = js as IWindowManager;
            @params = (Application as IMAS_Application).GetWindowManagerLayoutParams();
            @params.Type = WindowManagerTypes.SystemAlert;
            @params.Flags = WindowManagerFlags.NotFocusable | WindowManagerFlags.WatchOutsideTouch /*| WindowManagerFlags.LayoutNoLimits;*/ ;
            @params.Alpha = 1.0f;
            @params.Format = Format.Rgba8888;
            //调整悬浮窗口为左上角
            @params.Gravity = GravityFlags.Top | GravityFlags.Start;
            //以屏幕上角为圆点设置x,y的值
            @params.X = 1600;
            @params.Y = 800;

            //设置悬浮窗口长宽数据
            @params.Width = 100;
            @params.Height = 100;
            mLayout.Click += delegate
            {
                ShowMsgLong("点啦!!");
            };

            //显示FloatView图像
            WindowManager.AddView(mLayout, @params);

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            WindowManager.RemoveView(mLayout);
        }

        private int screenRight;
        private int screenBottom;
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);

            var rect = new Rect();
            Window.DecorView.GetWindowVisibleDisplayFrame(rect);
            this.mLayout.Top = rect.Top;
            //this.mLayout.SetX(/*rect.Right - 100*/100);
            //this.mLayout.SetY(/*rect.Bottom - 100*/100);
        }
        #endregion


    }
}