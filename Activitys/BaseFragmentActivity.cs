using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using Android.Content;
using IdoMaster_GensouWorld.Adapters;
using Android.Support.V4.App;

namespace IdoMaster_GensouWorld
{
    public abstract class BaseFragmentActivity : FragmentActivity
    {
        protected int activityCloseEnterAnimation;
        protected int activityCloseExitAnimaiton;
        /// <summary>
        /// 等待框
        /// </summary>
        protected ProgressDialog _WaitDialog;
        protected Dialog _ConfigDialog;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            #region 动画效果
            var activityStyle = Theme.ObtainStyledAttributes(new int[] { Android.Resource.Attribute.WindowAnimationStyle });
            var windowAnimationStyleResid = activityStyle.GetResourceId(0, 0);
            activityStyle.Recycle();
            activityStyle = Theme.ObtainStyledAttributes(windowAnimationStyleResid, new int[] { Android.Resource.Attribute.ActivityCloseEnterAnimation, Android.Resource.Attribute.ActivityCloseExitAnimation });
            activityCloseEnterAnimation = activityStyle.GetResourceId(0, 0);
            activityCloseExitAnimaiton = activityStyle.GetResourceId(1, 0);
            activityStyle.Recycle();
            OverridePendingTransition(activityCloseEnterAnimation, activityCloseExitAnimaiton);
            #endregion
            ////去掉标题栏
            //RequestWindowFeature(WindowFeatures.NoTitle);
            ////强制设置竖屏
            //RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            //添加开启的Activity进入缓存列表
            IMAS_Application.Sington?.OpenActivityList?.Add(this);
            SetContentView(A_GetContentViewId());
            B_BeforeInitView();
            C_InitView();
            D_BindEvent();
            E_InitData();

        }

        #region 封装方法
        /// <summary>
        /// 绑定点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClickListener(object sender, EventArgs e)
        {
            var v = sender as View;
            F_OnClickListener(v, e);
        }
        protected void OnAdapterItemClickListener(object sender, AdapterView.ItemClickEventArgs e)
        {
            // var v = sender as View;
            G_OnAdapterItemClickListener(e);
        }

        #region 隐藏显示控件
        protected enum CoverFlag
        {
            /// <summary>
            /// 隐藏
            /// </summary>
            Gone = 0,
            /// <summary>
            /// 显示
            /// </summary>
            Visible = 1,
            /// <summary>
            /// 隐藏并占位
            /// </summary>
            Invisible = 2

        }
        /// <summary>
        ///显示隐藏控件
        /// </summary>
        /// <param name="flag">0.隐藏    1.显示   2.隐藏并占位</param>
        /// <param name="v">View们</param>
        protected void CoverUIControl(CoverFlag flag, params View[] v)
        {
            foreach (var item in v)
            {
                switch (flag)
                {
                    case CoverFlag.Gone:
                        item.Visibility = ViewStates.Gone;
                        break;
                    case CoverFlag.Visible:
                        item.Visibility = ViewStates.Visible;
                        break;
                    case CoverFlag.Invisible:
                        item.Visibility = ViewStates.Invisible;
                        break;
                }
            }
        }
        #endregion
        #region 等待框
        /// <summary>
        /// 显示等待进度框
        /// </summary>
        /// <param name="textMsg">文字提示</param>
        /// <param name="canCancelLoading">是否能点击取消</param>
        protected void ShowWaitDiaLog(string textMsg, bool canCancelLoading = true)
        {
            if (_WaitDialog == null)
            {
                _WaitDialog = new ProgressDialog(this, ProgressDialog.ThemeDeviceDefaultLight);
            }
            _WaitDialog?.SetMessage(textMsg);
            _WaitDialog?.SetCancelable(canCancelLoading);
            _WaitDialog?.Show();
        }
        /// <summary>
        /// 隐藏等待进度框
        /// </summary>
        protected void HideWaitDiaLog()
        {
            _WaitDialog?.Dismiss();
        }
        #endregion
        #region Toast提示框
        /// <summary>
        /// 显示自定义Toast
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowDiyToastShort(string msg)
        {
            View v = View.Inflate(this, Resource.Layout.utils_diy_toast, null);
            var textView = v.FindViewById<TextView>(Resource.Id.tv_toast);
            textView.Text = msg;
            var mToast = new Toast(this);
            mToast.Duration = ToastLength.Short;
            mToast.View = v;
            mToast.Show();
        }
        /// <summary>
        /// 显示自定义Toast
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowDiyToastLong(string msg)
        {
            View v = View.Inflate(this, Resource.Layout.utils_diy_toast, null);
            var textView = v.FindViewById<TextView>(Resource.Id.tv_toast);
            textView.Text = msg;
            var mToast = new Toast(this);
            mToast.Duration = ToastLength.Long;
            mToast.View = v;
            mToast.Show();
        }
        protected void ShowMsgShort(string msg)
        {
            Toast.MakeText(this, msg, ToastLength.Short).Show();

        }
        protected void ShowMsgLong(string msg)
        {
            Toast.MakeText(this, msg, ToastLength.Long).Show();
        }
        #endregion
        #region 弹出确认框
        /// <summary>
        /// 弹出确认取消框
        /// </summary>
        /// <param name="title">标题文字</param>
        /// <param name="msg">提示消息</param>
        /// <param name="yesE">点击确定的事件</param>
        /// <param name="cancelE">点击取消的事件</param>
        /// <param name="yesBtnName">确定文字@默认为"确定"</param>
        /// <param name="cancelBtnName">取消文字@默认为"取消"</param>
        /// <param name="cancelble">是否可以取消</param>
        protected void ShowConfim(string title, string msg, EventHandler<DialogClickEventArgs> yesE, EventHandler<DialogClickEventArgs> cancelE, string yesBtnName = "はい", string cancelBtnName = "いいえ", bool cancelble = true)
        {
            var window = new AlertDialog.Builder(this, AlertDialog.ThemeDeviceDefaultLight);
            window = window.SetTitle(title).SetMessage(msg).SetCancelable(cancelble).SetPositiveButton(yesBtnName, yesE).SetNegativeButton(cancelBtnName, cancelE);
            _ConfigDialog = window.Show();
        }
        /// <summary>
        /// 弹出确认框
        /// </summary>
        /// <param name="title">标题文字</param>
        /// <param name="msg">提示信息</param>
        /// <param name="yesE">点击确定触发的事件</param>
        /// <param name="yesBtnName">确定文字</param>
        /// <param name="cancelble">是否可以取消</param>
        protected void ShowSureConfim(string title, string msg, EventHandler<DialogClickEventArgs> yesE, string yesBtnName = "はい", bool cancelble = false)
        {
            var window = new AlertDialog.Builder(this, AlertDialog.ThemeDeviceDefaultLight);
            window = window.SetTitle(title).SetMessage(msg).SetCancelable(cancelble).SetPositiveButton(yesBtnName, yesE);
            _ConfigDialog = window.Show();
        }
        protected void DismissConfim()
        {
            if (_ConfigDialog != null)
            {
                _ConfigDialog.Dismiss();
                _ConfigDialog = null;
            }
        }
        //private void YesEventHandler(object sender, DialogClickEventArgs e)
        //{

        //}
        //private void NoEventHandler(object sender, DialogClickEventArgs e)
        //{

        //}
        #endregion
        #endregion
        #region 抽象方法
        /// <summary>
        /// Activity界面给予
        /// </summary>
        /// <returns></returns>
        public abstract int A_GetContentViewId();
        /// <summary>
        /// 在初始化控件前所做操作
        /// </summary>
        public abstract void B_BeforeInitView();
        /// <summary>
        /// 初始化控件操作
        /// </summary>
        public abstract void C_InitView();
        /// <summary>
        /// 绑定控件时间操作
        /// </summary>
        public abstract void D_BindEvent();
        /// <summary>
        /// 初始化数据操作
        /// </summary>
        public abstract void E_InitData();
        /// <summary>
        /// 设置点击事件操作
        /// </summary>
        /// <param name="v"></param>
        /// <param name="e"></param>
        public abstract void F_OnClickListener(View v, EventArgs e);
        /// <summary>
        /// 适配器点击事件操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void G_OnAdapterItemClickListener(AdapterView.ItemClickEventArgs e);
        #endregion
        #region 生命周期
        protected override void OnDestroy()
        {
            base.OnDestroy();
            IMAS_Application.Sington?.OpenActivityList?.Remove(this);
        }

        public override void Finish()
        {
            base.Finish();
            OverridePendingTransition(activityCloseEnterAnimation, activityCloseExitAnimaiton);
        }
        #endregion

    }
}

