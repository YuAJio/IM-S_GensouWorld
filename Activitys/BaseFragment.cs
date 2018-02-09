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
using Android.Support.V4.App;

namespace IdoMaster_GensouWorld.Activitys
{

    public abstract class BaseFragment : Android.Support.V4.App.Fragment
    {
        private View rootView;// 缓存Fragment view  
        protected ProgressDialog _loading;
        public Dialog NowDialog;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            mContext = Activity;
            if (rootView == null)
            {
                rootView = inflater.Inflate(GetFragmentContentViewId(), null);
                InitFragmentView(rootView);
                InitFragmentData();
            }
            // 缓存的rootView需要判断是否已经被加过parent，如果有parent需要从parent删除，要不然会发生这个rootview已经有parent的错误。  
            ViewGroup parent = (ViewGroup)rootView.Parent;
            if (parent != null)
            {
                parent.RemoveView(rootView);
            }
            return rootView;
        }
        #region 初始化工具
        private void InitBaseTools()
        {
            _loading = new ProgressDialog(mContext, ProgressDialog.ThemeHoloLight);
        }
        #endregion
        #region 封装类
        public abstract int GetFragmentContentViewId();
        public abstract void InitFragmentView(View view);
        public abstract void InitFragmentData();
        public abstract void SetOnFragmentClick(View v, EventArgs e);
        #endregion
        #region 自定义工具方法
        /// <summary>
        /// 上下文
        /// </summary>
        protected Activity mContext { get; set; }
        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnFragmentClickListener(object sender, EventArgs e)
        {
            var v = (View)sender;
            SetOnFragmentClick(v, e);
        }
        /// <summary>
        /// 省去view.FInd....懒惰才是最好的! 吊·不懒惰!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        protected T FindViewById<T>(int id) where T : View
        {
            return rootView?.FindViewById<T>(id);
        }
        /// <summary>
        /// 显示自定义Toast
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowDiyToastShort(string msg)
        {
            View v = View.Inflate(mContext, Resource.Layout.utils_diy_toast, null);
            var textView = v.FindViewById<TextView>(Resource.Id.tv_toast);
            textView.Text = msg;
            var mToast = new Toast(mContext);
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
            View v = View.Inflate(mContext, Resource.Layout.utils_diy_toast, null);
            var textView = v.FindViewById<TextView>(Resource.Id.tv_toast);
            textView.Text = msg;
            var mToast = new Toast(mContext);
            mToast.Duration = ToastLength.Long;
            mToast.View = v;
            mToast.Show();
        }
        /// <summary>
        /// 显示短消息
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowMsgShort(string msg)
        {
            Toast.MakeText(mContext, msg, ToastLength.Short).Show();
        }
        /// <summary>
        /// 显示长消息
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowMsgLong(string msg)
        {
            Toast.MakeText(mContext, msg, ToastLength.Long).Show();
        }
        /// <summary>
        /// 显示等待进度框
        /// </summary>
        /// <param name="textMsg">文字提示</param>
        /// <param name="canCancelLoading">是否能点击取消</param>
        protected void ShowWaitDiaLog(string textMsg, bool canCancelLoading = true)
        {
            if (_loading == null)
            {
                InitBaseTools();
            }
            _loading?.SetMessage(textMsg);
            _loading?.SetCancelable(canCancelLoading);
            _loading?.Show();
        }
        /// <summary>
        /// 隐藏等待进度框
        /// </summary>
        protected void HideWaitDiaLog()
        {
            _loading?.Dismiss();
        }
        /// <summary>
        /// 弹出确认框
        /// </summary>
        /// <param name="title">标题栏文字</param>
        /// <param name="msg">内容提示</param>
        /// <param name="e">用户点击"确定"按钮后执行</param>
        /// <param name="cancelBtn"></param>
        public void ShowConfim(string title, string msg, EventHandler<DialogClickEventArgs> e, string yesBtnName = "确定", bool cancelBtn = false)
        {
            var window = new AlertDialog.Builder(mContext, AlertDialog.ThemeHoloLight);
            window = window.SetTitle(title).SetMessage(msg).SetCancelable(cancelBtn).SetPositiveButton(yesBtnName, e);
            NowDialog = window.Show();
        }
        #endregion

    }
}