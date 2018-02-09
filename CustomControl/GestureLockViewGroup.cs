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
using Android.Graphics;
using Android.Util;
using Android.Content.Res;
using IdoMaster_GensouWorld;

namespace CustomControl
{
    public class GestureLockViewGroup : RelativeLayout
    {
        private const string TAG = "GestureLockViewGroup";

        /// <summary>
        /// 保存所有的GestureLockView 
        /// </summary>
        private GestureLockView[] mGestureLockViews;
        /// <summary>
        /// 每个边上的GestureLockView的个数 
        /// </summary>
        private int mCount = 3;
        /// <summary>
        /// 存储答案
        /// </summary>
        private int[] mAnswer = { 0, 1, 2, 5, 8 };
        /// <summary>
        /// 保存用户选中的GestureLockView的id 
        /// </summary>
        private List<int> mChoose = new List<int>();

        private Paint mPaint;
        /// <summary>
        /// 每个GestureLockView中间的间距 设置为：mGestureLockViewWidth * 25% 
        /// </summary>
        private int mMarginBetweenLockView = 35;
        /// <summary>
        /// GestureLockView的边长 4 * mWidth / ( 5 * mCount + 1 ) 
        /// </summary>
        private int mGestureLockViewWidth = 75;

        /// <summary>
        /// GestureLockView无手指触摸的状态下内圆的颜色 
        /// </summary>
        private int mNoFingerInnerCircleColor = unchecked((int)0xFF939090);
        /// <summary>
        ///  GestureLockView无手指触摸的状态下外圆的颜色 
        /// </summary>
        private int mNoFingerOuterCircleColor = unchecked((int)0xFFE0DBDB);
        /// <summary>
        /// GestureLockView手指触摸的状态下内圆和外圆的颜色 
        /// </summary>
        private int mFingerOnColor = unchecked((int)0xFF378FC9);
        /// <summary>
        /// GestureLockView手指抬起的状态下内圆和外圆的颜色 
        /// </summary>
        private int mFingerUpColor = unchecked((int)0xFFFF0000);

        /// <summary>
        ///  宽度 
        /// </summary>
        private int mWidth;
        /// <summary>
        /// 高度 
        /// </summary>
        private int mHeight;

        private Path mPath;
        /// <summary>
        /// 指引线的开始位置x 
        /// </summary>
        private int mLastPathX;
        /// <summary>
        /// 指引线的开始位置y 
        /// </summary>
        private int mLastPathY;
        /// <summary>
        /// 指引下的结束位置 
        /// </summary>
        private Point mTmpTarget = new Point();

        /// <summary>
        /// 最大尝试次数 
        /// </summary>
        private int mTryTimes = 4;

        /// <summary>
        /// 回调接口
        /// </summary>
        private IOnGestureLockViewListener mOnGestureLockViewListener;

        public GestureLockViewGroup(Context context, IAttributeSet attrs) : base(context, attrs, 0)
        {
            ///获得所有自定义的参数的值 
            TypedArray a = context.Theme.ObtainStyledAttributes(attrs,
                    Resource.Styleable.GestureLockViewGroup, 1600, 0);
            int n = a.IndexCount;

            for (int i = 0; i < n; i++)
            {
                int attr = a.GetIndex(i);
                switch (attr)
                {
                    case Resource.Styleable.GestureLockViewGroup_color_no_finger_inner_circle:
                        mNoFingerInnerCircleColor = a.GetColor(attr,
                                mNoFingerInnerCircleColor);
                        break;
                    case Resource.Styleable.GestureLockViewGroup_color_no_finger_outer_circle:
                        mNoFingerOuterCircleColor = a.GetColor(attr,
                                mNoFingerOuterCircleColor);
                        break;
                    case Resource.Styleable.GestureLockViewGroup_color_finger_on:
                        mFingerOnColor = a.GetColor(attr, mFingerOnColor);
                        break;
                    case Resource.Styleable.GestureLockViewGroup_color_finger_up:
                        mFingerUpColor = a.GetColor(attr, mFingerUpColor);
                        break;
                    case Resource.Styleable.GestureLockViewGroup_count:
                        mCount = a.GetInt(attr, 3);
                        break;
                    case Resource.Styleable.GestureLockViewGroup_tryTimes:
                        mTryTimes = a.GetInt(attr, 5);
                        break;
                }
            }

            a.Recycle();

            // 初始化画笔  
            mPaint = new Paint(PaintFlags.AntiAlias);
            mPaint.SetStyle(Paint.Style.Stroke);
            // mPaint.setStrokeWidth(20);  
            mPaint.StrokeCap = Paint.Cap.Round;
            mPaint.StrokeJoin = Paint.Join.Round;
            // mPaint.setColor(Color.parseColor("#aaffffff"));  
            mPath = new Path();

        }

        public GestureLockViewGroup(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            ///获得所有自定义的参数的值 
            TypedArray a = context.Theme.ObtainStyledAttributes(attrs,
                    Resource.Styleable.GestureLockViewGroup, defStyle, 0);
            int n = a.IndexCount;

            for (int i = 0; i < n; i++)
            {
                int attr = a.GetIndex(i);
                switch (attr)
                {
                    case Resource.Styleable.GestureLockViewGroup_color_no_finger_inner_circle:
                        mNoFingerInnerCircleColor = a.GetColor(attr,
                                mNoFingerInnerCircleColor);
                        break;
                    case Resource.Styleable.GestureLockViewGroup_color_no_finger_outer_circle:
                        mNoFingerOuterCircleColor = a.GetColor(attr,
                                mNoFingerOuterCircleColor);
                        break;
                    case Resource.Styleable.GestureLockViewGroup_color_finger_on:
                        mFingerOnColor = a.GetColor(attr, mFingerOnColor);
                        break;
                    case Resource.Styleable.GestureLockViewGroup_color_finger_up:
                        mFingerUpColor = a.GetColor(attr, mFingerUpColor);
                        break;
                    case Resource.Styleable.GestureLockViewGroup_count:
                        mCount = a.GetInt(attr, 3);
                        break;
                    case Resource.Styleable.GestureLockViewGroup_tryTimes:
                        mTryTimes = a.GetInt(attr, 5);
                        break;
                }
            }

            a.Recycle();

            // 初始化画笔  
            mPaint = new Paint(PaintFlags.AntiAlias);
            mPaint.SetStyle(Paint.Style.Stroke);
            // mPaint.setStrokeWidth(20);  
            mPaint.StrokeCap = Paint.Cap.Round;
            mPaint.StrokeJoin = Paint.Join.Round;
            // mPaint.setColor(Color.parseColor("#aaffffff"));  
            mPath = new Path();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            mWidth = MeasureSpec.GetSize(widthMeasureSpec);
            mHeight = MeasureSpec.GetSize(heightMeasureSpec);

            // Log.e(TAG, mWidth + "");  
            // Log.e(TAG, mHeight + "");  

            mHeight = mWidth = mWidth < mHeight ? mWidth : mHeight;

            // setMeasuredDimension(mWidth, mHeight);  

            // 初始化mGestureLockViews  
            if (mGestureLockViews == null)
            {
                var dins = Resources.DisplayMetrics;
                mGestureLockViewWidth = (int)(dins.ScaledDensity * mGestureLockViewWidth);
                mMarginBetweenLockView = (int)(dins.ScaledDensity * mMarginBetweenLockView);

                mGestureLockViews = new GestureLockView[mCount * mCount];
                // 计算每个GestureLockView的宽度  
                mGestureLockViewWidth = (int)(4 * mWidth * 1.0f / (5 * mCount + 1));
                //计算每个GestureLockView的间距  
                mMarginBetweenLockView = (int)(mGestureLockViewWidth * 0.25);
                // 设置画笔的宽度为GestureLockView的内圆直径稍微小点（不喜欢的话，随便设）  
                mPaint.StrokeWidth = (mGestureLockViewWidth * 0.29f);

                for (int i = 0; i < mGestureLockViews.Length; i++)
                {
                    //初始化每个GestureLockView  
                    mGestureLockViews[i] = new GestureLockView(Context,
                            mNoFingerInnerCircleColor, mNoFingerOuterCircleColor,
                            mFingerOnColor, mFingerUpColor);
                    mGestureLockViews[i].Id = (i + 1);
                    //设置参数，主要是定位GestureLockView间的位置  
                    RelativeLayout.LayoutParams lockerParams = new RelativeLayout.LayoutParams(
                            mGestureLockViewWidth, mGestureLockViewWidth);

                    // 不是每行的第一个，则设置位置为前一个的右边  
                    if (i % mCount != 0)
                    {
                        lockerParams.AddRule(LayoutRules.RightOf,
                                mGestureLockViews[i - 1].Id);
                    }
                    // 从第二行开始，设置为上一行同一位置View的下面  
                    if (i > mCount - 1)
                    {
                        lockerParams.AddRule(LayoutRules.Below,
                                mGestureLockViews[i - mCount].Id);
                    }
                    //设置右下左上的边距  
                    int rightMargin = mMarginBetweenLockView;
                    int bottomMargin = mMarginBetweenLockView;
                    int leftMagin = 0;
                    int topMargin = 0;

                    /** 
                     * 每个View都有右外边距和底外边距 第一行的有上外边距 第一列的有左外边距 
                     */
                    if (i >= 0 && i < mCount)// 第一行  
                    {
                        topMargin = mMarginBetweenLockView;
                    }
                    if (i % mCount == 0)// 第一列  
                    {
                        leftMagin = mMarginBetweenLockView;
                    }

                    lockerParams.SetMargins(leftMagin, topMargin , rightMargin,
                            bottomMargin);
                    mGestureLockViews[i].SetMode(GestureLockView.Mode.STATUS_NO_FINGER);
                    AddView(mGestureLockViews[i], lockerParams);
                }
            }
        }
        public override bool OnTouchEvent(MotionEvent e)
        {
            //if (mTryTimes <= 0)
            //{
            //    return false;
            //}

            var action = e.Action;
            int x = (int)e.RawX;
            int y = (int)e.RawY;

            switch (action)
            {
                case MotionEventActions.Down:
                    Reset();
                    break;
                case MotionEventActions.Move:

                    mPaint.Color = new Color(mFingerOnColor);
                    mPaint.Alpha = (50);
                    GestureLockView child = GetChildIdByPos(x, y);
                    if (child != null)
                    {
                        int cId = child.Id;
                        if (!mChoose.Contains(cId))
                        {
                            mChoose.Add(cId);
                            child.SetMode(GestureLockView.Mode.STATUS_FINGER_ON);
                            if (mOnGestureLockViewListener != null)
                                mOnGestureLockViewListener.onBlockSelected(cId);
                            // 设置指引线的起点  
                            mLastPathX = child.Left / 2 + child.Right / 2;
                            mLastPathY = child.Top / 2 + child.Bottom / 2;

                            if (mChoose.Count == 1)// 当前添加为第一个  
                            {
                                mPath.MoveTo(mLastPathX, mLastPathY);
                            }
                            else
                            // 非第一个，将两者使用线连上  
                            {
                                mPath.LineTo(mLastPathX, mLastPathY);
                            }

                        }
                    }
                    // 指引线的终点  
                    mTmpTarget.X = x;
                    mTmpTarget.Y = y;

                    break;
                case MotionEventActions.Up:
                    mPaint.Color = new Color(mFingerUpColor);
                    mPaint.Alpha = (50);
                    this.mTryTimes--;

                    // 回调是否成功  
                    if (mOnGestureLockViewListener != null && mChoose.Count() > 0)
                    {
                        mOnGestureLockViewListener.onGestureEvent(CheckAnswer());
                        if (this.mTryTimes == 0)
                        {
                            mOnGestureLockViewListener.onUnmatchedExceedBoundary();
                        }
                    }

                    // 将终点设置位置为起点，即取消指引线  
                    mTmpTarget.X = mLastPathX;
                    mTmpTarget.Y = mLastPathY;

                    // 改变子元素的状态为UP  
                    ChangeItemMode();

                    // 计算每个元素中箭头需要旋转的角度  
                    for (int i = 0; i + 1 < mChoose.Count; i++)
                    {
                        int childId = mChoose[i];
                        int nextChildId = mChoose[i + 1];

                        GestureLockView startChild = (GestureLockView)FindViewById(childId);
                        GestureLockView nextChild = (GestureLockView)FindViewById(nextChildId);

                        int dx = nextChild.Left - startChild.Left;
                        int dy = nextChild.Top - startChild.Top;
                        // 计算角度  
                        int angle = (int)Java.Lang.Math.ToDegrees(Math.Atan2(dy, dx)) + 90;
                        startChild.SetArrowDegree(angle);
                    }
                    break;
            }
            Invalidate();
            return true;

        }
        private void ChangeItemMode()
        {
            foreach (var item in mGestureLockViews)
            {
                if (mChoose.Contains(item.Id))
                {
                    item.SetMode(GestureLockView.Mode.STATUS_FINGER_UP);
                }
            }
        }

        /// <summary>
        /// 错误之后重新给一次机会
        /// </summary>
        public void OpenReset()
        {
            mTryTimes = 1;
            Reset();
        }
        /// <summary>
        /// 正确后补满机会
        /// </summary>
        public void OpenResetForPass()
        {
            mTryTimes = 4;
            Reset();
        }
        /// <summary>
        /// 在验证以外的功能每次重置重试次数
        /// </summary>
        public void ResetRetryTime()
        {
            mTryTimes = 4;
        }

        /// <summary>
        /// 做一些必要的重置
        /// </summary>
        public void Reset()
        {
            mChoose.Clear();
            mPath.Reset();
            foreach (var item in mGestureLockViews)
            {
                item.SetMode(GestureLockView.Mode.STATUS_NO_FINGER);
                item.SetArrowDegree(-1);
            }
        }

        /// <summary>
        /// 检查用户绘制的手势是否正确
        /// </summary>
        /// <returns></returns>
        private bool CheckAnswer()
        {
            if (mAnswer.Length != mChoose.Count)
            {
                return false;
            }
            for (int i = 0; i < mAnswer.Length; i++)
            {
                if (mAnswer[i] != mChoose[i])
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 检查当前左边是否在child中 
        /// </summary>
        /// <param name="child"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool CheckPositionInChild(View child, int x, int y)
        {
            //设置了内边距，即x,y必须落入下GestureLockView的内部中间的小区域中，可以通过调整padding使得x,y落入范围不变大，或者不设置padding  
            var padding = (int)mGestureLockViewWidth * 0.15;
            if (x >= child.Left + padding && x <= child.Right - padding && y >= child.Top + padding && y <= child.Bottom - padding)
            {
                return true;
            }
            return false;
        }

        private GestureLockView GetChildIdByPos(int x, int y)
        {

            foreach (var item in mGestureLockViews)
            {
                if (CheckPositionInChild(item, x, y))
                {
                    return item;
                }
            }
            return null;
        }
        public void SetOnGestureLockViewListener(IOnGestureLockViewListener listener)
        {
            this.mOnGestureLockViewListener = listener;
        }
        public void SetAnswer(int[] answer)
        {
            this.mAnswer = answer;
        }
        public void SetUnMatchExceedBoundary(int boundary)
        {
            this.mTryTimes = boundary;
        }
        protected override void DispatchDraw(Canvas canvas)
        {
            base.DispatchDraw(canvas);
            if (mPath != null)
            {
                canvas.DrawPath(mPath, mPaint);
            }
            if (mChoose.Count > 0)
            {
                if (mLastPathX != 0 && mLastPathY != 0)
                {
                    canvas.DrawLine(mLastPathX, mLastPathY, mTmpTarget.X, mTmpTarget.Y, mPaint);
                }
            }
        }
        public interface IOnGestureLockViewListener
        {
            /// <summary>
            /// 单独选中元素的Id 
            /// </summary>
            /// <param name="cId"></param>
            void onBlockSelected(int cId);

            /// <summary>
            /// 是否匹配 
            /// </summary>
            /// <param name="matched"></param>
            void onGestureEvent(bool matched);

            /// <summary>
            /// 超过尝试次数 
            /// </summary>
            void onUnmatchedExceedBoundary();
        }
    }
}