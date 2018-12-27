using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace IdoMaster_GensouWorld.CustomControl
{
    public class FloatView : View
    {
        private float x;
        private float y;
        //private float mTouchStartX;
        //private float mTouchStartY;

        //悬浮窗原始位置
        private float startPositionX = 0;
        private float startPositionY = 0;


        //开始触控的坐标，移动时的坐标（相对于屏幕左上角的坐标）
        private int mTouchStartX, mTouchStartY, mTouchCurrentX, mTouchCurrentY;
        //开始时的坐标和结束时的坐标（相对于自身控件的坐标）
        private int mStartX, mStartY, mStopX, mStopY;
        private bool isMove;//判断悬浮窗是否移动

        private IWindowManager wm;
        private WindowManagerLayoutParams wmParams;

        public FloatView(Context context) : base(context)
        {
            InitSomething(context);
        }

        public FloatView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            InitSomething(context);
        }

        public FloatView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            InitSomething(context);
        }

        public FloatView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            InitSomething(context);
        }

        private void InitSomething(Context context)
        {
            var jk = context.ApplicationContext.GetSystemService(Context.WindowService);
            wmParams = ((IMAS_Application)context.ApplicationContext).GetWindowManagerLayoutParams();

        }

        public void SetWindowManager(IWindowManager window)
        {
            wm = window;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            //x = e.RawX;
            //y = e.RawY - Top;
            switch (e.ActionMasked)
            {
                case MotionEventActions.Down:
                    {
                        mTouchStartX = (int)e.RawX;
                        mTouchStartY = (int)e.RawY;
                        startPositionX = this.wmParams.X;
                        startPositionY = this.wmParams.Y;

                        ////获取相对View的坐标，即以此View左上角为原点
                        //mTouchStartX = e.GetY();
                        //mTouchStartY = e.GetY();
                        ////记录悬浮窗原始位置
                        //startPositionX = this.wmParams.X;
                        //startPositionY = this.wmParams.Y;
                    }
                    break;
                case MotionEventActions.Move:
                    {
                        mTouchCurrentX = (int)e.RawX;
                        mTouchCurrentY = (int)e.RawY;
                        wmParams.X += mTouchCurrentX - mTouchStartX;
                        wmParams.Y += mTouchCurrentY - mTouchStartY;
                        wm.UpdateViewLayout(this, wmParams);
                        //UpdateViewPosition();
                        mTouchStartX = mTouchCurrentX;
                        mTouchStartY = mTouchCurrentY;
                    }
                    break;
                case MotionEventActions.Up:
                    {
                        if (Math.Abs(wmParams.X - startPositionX) < 5 &&
                           Math.Abs(wmParams.Y - startPositionY) < 5)
                        {
                            //点击了
                            CallOnClick();
                        }
                        //UpdateViewPosition();
                        //mTouchStartX = mTouchStartY = 0;
                    }
                    break;
            }
            return true;
        }


        private void UpdateViewPosition()
        {
            //更新浮动窗口位置参数
            wmParams.X = (int)(x - mTouchStartX);
            wmParams.Y = (int)(y - mTouchStartY);
            wm.UpdateViewLayout(this, wmParams);
        }


    }
}