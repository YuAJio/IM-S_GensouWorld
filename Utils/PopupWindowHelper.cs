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
using Android.Util;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Content.Res;
using Android.Views.InputMethods;

namespace IdoMaster_GensouWorld.Utils
{
    public class PopupWindowHelper
    {
        private View popupView;
        private MyPopupWindow mPopupWindow;
        private static int TYPE_WRAP_CONTENT = 0, TYPE_MATCH_PARENT = 1;

        public PopupWindowHelper(View view)
        {
            popupView = view;
        }

        public void ShowAsDropDown(View anchor)
        {
            //  mPopupWindow.ShowAsDropDown(anchor);
            ShowAsDropDown(anchor, 0, 0);
        }

        public void ShowAsDropDown(View anchor, int xoff, int yoff)
        {
            InitPopupWindow(TYPE_MATCH_PARENT);
            mPopupWindow.ShowAsDropDown(anchor, xoff, yoff);
        }

        public void ShowAtLocation(View parent, GravityFlags gravity, int x, int y)
        {
            InitPopupWindow(TYPE_WRAP_CONTENT);
            mPopupWindow.ShowAtLocation(parent, gravity, x, y);
        }

        public void Dismiss()
        {
            if (mPopupWindow.IsShowing)
            {
                mPopupWindow.Dismiss();
            }
            //InputMethodManager inputMethodManager = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);
            //if (inputMethodManager != null)//软键盘管理是否为空
            //{
            //    if (inputMethodManager.IsActive)//软键盘是否启动
            //    {
            //        inputMethodManager.ToggleSoftInput(0, HideSoftInputFlags.None);
            //    }
            //}
            InputMethodManager inputMethodManager = (InputMethodManager)Application.Context.GetSystemService(Context.InputMethodService);
            if (inputMethodManager.IsActive)
            {
                inputMethodManager.ToggleSoftInput(0, HideSoftInputFlags.NotAlways);
            }

        }
        public bool IsShowing()
        {
            return mPopupWindow.IsShowing ? true : false;
        }
        public void ShowAsPopUp(View anchor)
        {
            ShowAsPopUp(anchor, 0, 0);
        }

        public void ShowAsPopUp(View anchor, int xoff, int yoff)
        {
            InitPopupWindow(TYPE_WRAP_CONTENT);
            mPopupWindow.AnimationStyle = (Resource.Style.AnimationUpPopup);
            popupView.Measure(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            int height = popupView.MeasuredHeight;
            int[] location = new int[2];
            anchor.GetLocationInWindow(location);
            mPopupWindow.ShowAtLocation(anchor, GravityFlags.Left | GravityFlags.Top, location[0] + xoff, location[1] - height + yoff);
        }

        public void ShowFromBottom(View anchor)
        {
            InitPopupWindow(TYPE_MATCH_PARENT);
            mPopupWindow.AnimationStyle = Resource.Style.AnimationFromButtom;
            mPopupWindow.ShowAtLocation(anchor, GravityFlags.Left | GravityFlags.Bottom, 0, 0);
        }

        public void ShowFromTop(View anchor)
        {
            InitPopupWindow(TYPE_MATCH_PARENT);
            mPopupWindow.AnimationStyle = Resource.Style.AnimationFromTop;
            mPopupWindow.ShowAtLocation(anchor, GravityFlags.Left | GravityFlags.Top, 0, GetStatusBarHeight());
        }

        /**
         * touch outside dismiss the popupwindow, default is ture
         * @param isCancelable
         */
        public void SetCancelable(bool isCancelable)
        {
            if (isCancelable)
            {
                mPopupWindow.OutsideTouchable = true;
                mPopupWindow.Focusable = true;
            }
            else
            {
                mPopupWindow.OutsideTouchable = false;
                mPopupWindow.Focusable = false;
            }
        }

        public void InitPopupWindow(int type)
        {
            if (type == TYPE_WRAP_CONTENT)
            {
                mPopupWindow = new MyPopupWindow(popupView, ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            }
            else if (type == TYPE_MATCH_PARENT)
            {
                mPopupWindow = new MyPopupWindow(popupView, ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            }
            mPopupWindow.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            SetCancelable(true);
        }

        private int GetStatusBarHeight()
        {
            return (int)Math.Round(25 * Resources.System.DisplayMetrics.Density);
        }


    }
    public class MyPopupWindow : PopupWindow
    {
        public MyPopupWindow() { }
        public MyPopupWindow(Context context) : base(context) { }
        public MyPopupWindow(View contentView) : base(contentView) { }
        public MyPopupWindow(Context context, IAttributeSet attrs) : base(context, attrs) { }
        public MyPopupWindow(int width, int height) : base(width, height) { }
        public MyPopupWindow(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }
        public MyPopupWindow(View contentView, int width, int height) : base(contentView, width, height) { }
        public MyPopupWindow(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleAttr) { }
        public MyPopupWindow(View contentView, int width, int height, bool focusable) : base(contentView, width, height, focusable) { }
        public MyPopupWindow(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        public override void ShowAsDropDown(View anchor, int xoff, int yoff)
        {
            //if (Build.VERSION.SdkInt == BuildVersionCodes.Cupcake)
            //{
            //    Rect rect = new Rect();
            //    anchor.GetGlobalVisibleRect(rect);
            //    int h = anchor.Resources.DisplayMetrics.HeightPixels - rect.Bottom;
            //    Height = h;
            //}
            base.ShowAsDropDown(anchor, xoff, yoff);
        }
    }

}