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
using Android.Text;
using Android.Util;
using Android.Graphics;
using Java.Lang;
using IdoMaster_GensouWorld;
using IdoMaster_GensouWorld.Utils;
using Android.Support.V4.Content;
using static Android.Text.Layout;

namespace CustomControl
{
    public class GalTextView : View
    {
        private TextPaint textPaint;
        private float density;
        private string textContent;
        /// <summary>
        /// 画下最后一笔
        /// </summary>
        private bool drawLastOne = false;
        public int TextColor { get; set; }
        public string TextAlignment_az { get; set; }
        public float TextSize { get; set; }
        public float TextSpacingAdd { get; set; }
        public float TextSpacingMult { get; set; }

        public GalTextView(Context context) : base(context, null, 0)
        {
            Init();
        }
        public GalTextView(Context context, IAttributeSet attrs) : base(context, attrs, 0)
        {

            var a = context.ObtainStyledAttributes(attrs, Resource.Styleable.GalTextView);
            textContent = a.GetString(Resource.Styleable.GalTextView_textContent);
            TextColor = a.GetColor(Resource.Styleable.GalTextView_textColor, Color.Black);
            TextAlignment_az = a.GetString(Resource.Styleable.GalTextView_textXAlignment);
            TextSize = a.GetDimension(Resource.Styleable.GalTextView_textSize, 20);
            TextSpacingAdd = a.GetFloat(Resource.Styleable.GalTextView_textSpacingAdd, 0.0F);
            TextSpacingMult = a.GetFloat(Resource.Styleable.GalTextView_textSpacingMult, 1.0F);

            Init();
        }

        public GalTextView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            var a = context.ObtainStyledAttributes(attrs, Resource.Styleable.GalTextView);
            textContent = a.GetString(Resource.Styleable.GalTextView_textContent);
            TextColor = a.GetColor(Resource.Styleable.GalTextView_textColor, Color.Black);
            TextAlignment_az = a.GetString(Resource.Styleable.GalTextView_textXAlignment);
            TextSize = a.GetDimension(Resource.Styleable.GalTextView_textSize, 20);
            TextSpacingAdd = a.GetFloat(Resource.Styleable.GalTextView_textSpacingAdd, 0.0F);
            TextSpacingMult = a.GetFloat(Resource.Styleable.GalTextView_textSpacingMult, 1.0F);

            Init();
        }

        private string t_Content;
        private List<string> contents;
        public void SetTextContent(string content)
        {
            cnt = 0;
            totalText = "";
            this.t_Content = content;
            var mThread = new MyThread();
            mThread.threadRunAct -= OnThreadAct;
            mThread.threadRunAct += OnThreadAct;
            mThread.Run();
            //如果线程不是空就往下走
            if (mHandler != null)
            {
                mHandler.PostDelayed(runable_Alpha, Time);
            }
            //刷新控件
            Invalidate();
        }

        private void OnThreadAct()
        {
            contents = GalTextUtils.GetContentList(t_Content);
        }

        private void Init()
        {
            density = Resources.DisplayMetrics.Density;
            textPaint = new TextPaint();
            textPaint.Color = new Color(ContextCompat.GetColor(Context, Resource.Color.snow));
            textPaint.TextSize = TextSize;
        }
        private int cnt = 0;
        private string totalText = "";
        private void DrawText(Canvas canvas)
        {
            if (contents == null)
            {
                return;
            }
            if (cnt > contents.Count)
            {
                return;
            }
            totalText += contents[cnt];
            StaticLayout layout = null;
            if (drawLastOne)
            {
                totalText = t_Content;
                drawLastOne = false;
                cnt = contents.Count - 1;
            }
            switch (TextAlignment_az)
            {
                case "normal":
                    layout = new StaticLayout(totalText, textPaint, Width - (int)(20 * density), Alignment.AlignNormal, TextSpacingMult, TextSpacingAdd, true);
                    break;
                case "center":
                    layout = new StaticLayout(totalText, textPaint, Width - (int)(20 * density), Alignment.AlignCenter, TextSpacingMult, TextSpacingAdd, true);
                    break;
                case "opposite":
                    layout = new StaticLayout(totalText, textPaint, Width - (int)(20 * density), Alignment.AlignOpposite, TextSpacingMult, TextSpacingAdd, true);
                    break;
                default:
                    layout = new StaticLayout(totalText, textPaint, Width - (int)(20 * density), Alignment.AlignNormal, TextSpacingMult, TextSpacingAdd, true);
                    break;
            }
            //从(0,0)的位置开始绘制
            canvas.Translate(0 * density, 0 * density);
            layout.Draw(canvas);
            cnt++;
            StartText();
        }

        private long Time = 1 * 1000;
        public void SetDelayPlayTime(long time)
        {
            this.Time = time;
        }
        public void StartText()
        {
            if (cnt != contents.Count)
            {
                if (mHandler == null)
                {
                    mHandler = new MyHandler();
                    mHandler.handlerAction -= RunHandlerAction;
                    mHandler.handlerAction += RunHandlerAction;
                    runable_Alpha = new MyRunable(mHandler, SEND_TEXT_NEXT_GO);
                    mHandler.PostDelayed(runable_Alpha, Time);
                }
            }
        }
        /// <summary>
        /// 是否全部显示完成
        /// </summary>
        /// <returns></returns>
        public bool IsSendFinish()
        {
            var jk = cnt == contents?.Count;
            return jk;
        }
        /// <summary>
        /// 直接全部显示完成
        /// </summary>
        public void SetAllDraw()
        {
            drawLastOne = true;
            if (mHandler==null)
            {
                return;
            }
            mHandler.RemoveCallbacksAndMessages(null);
            Invalidate();
        }
        #region 重写方法
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            DrawText(canvas);
        }
        #endregion

        #region 线程相关

        #region Handler线程
        /// <summary>
        /// 发送文字往下走的线程
        /// </summary>
        private const int SEND_TEXT_NEXT_GO = 0x124;

        private MyHandler mHandler;
        private class MyHandler : Handler
        {
            public Action<Message> handlerAction;
            public override void HandleMessage(Message msg)
            {
                handlerAction?.Invoke(msg);
            }
        }
        private void RunHandlerAction(Message msg)
        {
            switch (msg.What)
            {
                case SEND_TEXT_NEXT_GO:
                    if (cnt >= contents.Count)
                    {
                        mHandler.RemoveCallbacksAndMessages(null);
                        return;
                    }
                    Invalidate();
                    mHandler.PostDelayed(runable_Alpha, Time);
                    break;
            }
        }


        private MyRunable runable_Alpha;

        /// <summary>
        /// 子线程Runnable
        /// </summary>
        public class MyRunable : Java.Lang.Object, IRunnable
        {
            private Handler mHandwel;
            private int mWhat;

            public MyRunable(Handler mHandwel, int what)
            {
                this.mHandwel = mHandwel;
                this.mWhat = what;
            }
            public void Run()
            {
                Message msg = new Message();
                msg.What = mWhat;
                mHandwel.SendMessage(msg);
            }
        }

        #endregion
        #region Thread线程
        private class MyThread : Thread
        {
            public Action threadRunAct;

            public override void Run()
            {
                base.Run();
                threadRunAct?.Invoke();
            }
        }
        #endregion

        #endregion

    }
}