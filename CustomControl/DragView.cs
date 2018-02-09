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
using Android.Support.V4.Widget;
using Android.Util;

namespace CustomControl
{
    public class DragView : RelativeLayout, View.IOnClickListener, DragView.AZ_ViewDragHelpderCallBack.IOnCallBack
    {
        private View fgView, bgView;
        private ViewDragHelper mDrager;
        private IDragStateListenner mDragStateListener;
        private const int DRAG_LEFT = -1, DRAG_RIGHT = 1;
        private int dragMode = DRAG_LEFT;
        private float minX, maxX;

        public DragView(Context context) : base(context) { }

        public DragView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            var dragH = new AZ_ViewDragHelpderCallBack();
            dragH.SetListener(this);
            mDrager = ViewDragHelper.Create(this, 5f, dragH);
        }



        public interface IDragStateListenner
        {
            void OnOpened(DragView dragView);
            void OnClosed(DragView dragView);
            void OnForegroundViewClick(DragView dragView, View v);
            void OnBackgroundViewClick(DragView dragView, View v);
        }
        public void OnClick(View v)
        {
            if (mDragStateListener != null)
            {
                if (v == fgView)
                {
                    if (IsOpen())
                    {
                        CloseAnim();
                        return;
                    }
                    mDragStateListener.OnForegroundViewClick(this, v);
                }
                else
                {
                    mDragStateListener.OnBackgroundViewClick(this, v);
                }
            }
        }

        #region ViewDragHelper接口回调
        public bool TryAct(View child, int pointerId)
        {
            return child == fgView;
        }

        public int GetViewHorizontalAct(View child)
        {
            return bgView.MeasuredWidth;
        }

        public int ClampViewHorizontalAct(View child, int left, int dx)
        {
            return GetPositionX(left);
        }

        public int ClampViewVerticalAct(View child, int top, int dy)
        {
            return 0;
        }

        public void OnViewReleasedAct(View releasedChild, float xvel, float yvel)
        {
            if (Math.Abs(fgView.Left) != 0 || Math.Abs(fgView.Left) != bgView.MeasuredWidth)
            {
                var x = fgView.Left + 0.1f * xvel;
                mDrager.SmoothSlideViewTo(fgView, Math.Abs(GetPositionX(x)) > bgView.MeasuredWidth / 2 ? bgView.MeasuredWidth * dragMode : 0, 0);
                PostInvalidate();
            }
        }

        public void OnViewPosiitonChangedAct(View changedView, int left, int top, int dx, int dy)
        {
            if (changedView == fgView)
            {
                Parent.RequestDisallowInterceptTouchEvent(fgView.Left != 0 ? true : false);
                if (mDragStateListener != null)
                {
                    if (left == 0)
                    {
                        mDragStateListener.OnClosed(this);
                    }
                    else if (Math.Abs(left) == bgView.MeasuredWidth)
                    {
                        mDragStateListener.OnOpened(this);
                    }
                }
            }
        }
        #endregion

        #region 外部调用方法组
        public View GetForegroundView()
        {
            return fgView;
        }
        public View GetBackGroundView()
        {
            return bgView;
        }
        public void Open()
        {
            fgView.OffsetLeftAndRight(dragMode * (bgView.MeasuredWidth - fgView.Left));
        }

        public void Close()
        {
            fgView.OffsetLeftAndRight(-fgView.Left);
        }
        public void OpenAnim()
        {
            mDrager.SmoothSlideViewTo(fgView, bgView.MeasuredWidth * dragMode, 0);
            PostInvalidate();
        }
        public void CloseAnim()
        {
            mDrager.SmoothSlideViewTo(fgView, 0, 0);
            PostInvalidate();
        }
        public bool IsOpen()
        {
            return Math.Abs(fgView.Left) == bgView.MeasuredWidth;
        }
        public void SetOnDragStateListener(IDragStateListenner listener)
        {
            mDragStateListener = listener;
        }
        #endregion
        #region 重写方法
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }
        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            if (dragMode == DRAG_LEFT)
            {
                minX = -bgView.MeasuredWidth;
                maxX = 0;
            }
            else
            {
                minX = 0;
                maxX = bgView.MeasuredWidth;
            }
        }
        protected override void OnFinishInflate()
        {
            base.OnFinishInflate();
            if (ChildCount != 2)
            {
                throw new Java.Lang.IllegalArgumentException("must contain only two child view(只能有两个子控件)");
            }
            fgView = GetChildAt(1);
            bgView = GetChildAt(0);
            if (!(fgView is ViewGroup && bgView is ViewGroup))
            {
                throw new Java.Lang.IllegalArgumentException("ForegroundView and BackgoundView must be a subClass of ViewGroup(前视图和BackgoundView必须是ViewGroup的子类)");
            }
            var param = (RelativeLayout.LayoutParams)bgView.LayoutParameters;
            param.AddRule(dragMode == DRAG_LEFT ? LayoutRules.AlignParentRight : LayoutRules.AlignParentLeft);
            param.Width = LayoutParams.WrapContent;
            fgView.SetOnClickListener(this);
            var bgViewCount = ((ViewGroup)bgView).ChildCount;
            for (int i = 0; i < bgViewCount; i++)
            {
                var child = ((ViewGroup)bgView).GetChildAt(i);
                if (child.Clickable)
                {
                    child.SetOnClickListener(this);
                }
            }
        }
        public override void ComputeScroll()
        {
            if (mDrager.ContinueSettling(true))
            {
                PostInvalidate();
            }
        }
        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return mDrager.ShouldInterceptTouchEvent(ev);
        }
        public override bool OnTouchEvent(MotionEvent e)
        {
            mDrager.ProcessTouchEvent(e);
            return true;
        }
        #endregion
        #region 方法组
        private int GetPositionX(float x)
        {
            if (x < minX) x = minX;
            if (x > maxX) x = maxX;
            return (int)x;
        }
        #endregion
        #region ViewDragHelper重写
        public class AZ_ViewDragHelpderCallBack : ViewDragHelper.Callback
        {

            private IOnCallBack callBack;

            public void SetListener(IOnCallBack callBack)
            {
                this.callBack = callBack;
            }

            public override bool TryCaptureView(View child, int pointerId)
            {
                if (callBack != null)
                {
                    return callBack.TryAct(child, pointerId);
                }
                return false;

            }
            public override int GetViewHorizontalDragRange(View child)
            {
                if (callBack != null)
                {
                    return callBack.GetViewHorizontalAct(child);
                }
                return 0;
            }
            public override int ClampViewPositionHorizontal(View child, int left, int dx)
            {
                if (callBack != null)
                {
                    return callBack.ClampViewHorizontalAct(child, left, dx);
                }
                return 0;
            }
            public override int ClampViewPositionVertical(View child, int top, int dy)
            {
                if (callBack != null)
                {
                    return callBack.ClampViewVerticalAct(child, top, dy);
                }
                return 0;
            }
            public override void OnViewReleased(View releasedChild, float xvel, float yvel)
            {
                base.OnViewReleased(releasedChild, xvel, yvel);
                if (callBack != null)
                {
                    callBack.OnViewReleasedAct(releasedChild, xvel, yvel);
                }
            }
            public override void OnViewPositionChanged(View changedView, int left, int top, int dx, int dy)
            {
                base.OnViewPositionChanged(changedView, left, top, dx, dy);
                if (callBack != null)
                {
                    callBack.OnViewPosiitonChangedAct(changedView, left, top, dx, dy);
                }
            }

            public interface IOnCallBack
            {
                bool TryAct(View child, int pointerId);
                int GetViewHorizontalAct(View child);
                int ClampViewHorizontalAct(View child, int left, int dx);
                int ClampViewVerticalAct(View child, int top, int dy);
                void OnViewReleasedAct(View releasedChild, float xvel, float yvel);
                void OnViewPosiitonChangedAct(View changedView, int left, int top, int dx, int dy);
            }
        }
        #endregion

    }
}