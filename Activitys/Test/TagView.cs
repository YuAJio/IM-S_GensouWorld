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

    public class JoushiKousei
    {
        public bool IsNull { get; set; }
        public string Name { get; set; }
        public decimal Liang { get; set; }
        public string Person { get; set; }
        public string Canci { get; set; }
        public DateTime DateTime { get; set; }
        public Bitmap QRCode { get; set; }
    }
    public class TagView : View
    {
        private JoushiKousei jkData;

        public TagView(Context context) : base(context)
        {
        }
        /// <summary>
        /// 主线程上使用
        /// </summary>
        /// <param name="data"></param>
        public void SetJKData(JoushiKousei data)
        {
            this.jkData = data;
            this.Invalidate();
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        protected override void OnDraw(Canvas canvas)
        {
            if (jkData.IsNull)
                return;

            //100 *100  .5 ,1 1.5
            //base.OnDraw(canvas);
            //清空画布
            canvas.DrawColor(Color.Black);

            //绘制信息
            #region 初始化画笔
            var titlePaint = new Paint()
            {
                AntiAlias = true,
                TextSize = 40,
                Color = Color.White,
            };
            var contentPaint = new Paint()
            {
                AntiAlias = true,
                TextSize = 28,
                Color = Color.White
            };
            #endregion

            var textXRay = 143;
            canvas.DrawText("食 品 留 样 标 签", 180, 73, titlePaint);

            canvas.DrawText($"留样名称 :", 28, textXRay, contentPaint);
            canvas.DrawText($"{jkData.Name}", 169, textXRay, contentPaint);
            textXRay += 58;
            canvas.DrawText($"留样量 :", 28, textXRay, contentPaint);
            canvas.DrawText($"{jkData.Liang}g", 169, textXRay, contentPaint);
            textXRay += 58;
            canvas.DrawText($"留样人 :", 28, textXRay, contentPaint);
            canvas.DrawText($"{jkData.Person}", 169, textXRay, contentPaint);
            textXRay += 58;
            canvas.DrawText($"餐次 :", 28, textXRay, contentPaint);
            canvas.DrawText($"{jkData.Canci}", 169, textXRay, contentPaint);
            textXRay += 58;
            canvas.DrawText($"留样时间 :", 28, textXRay, contentPaint);
            canvas.DrawText($"{jkData.DateTime.ToString("yyyy-M-dd  HH:mm")}", 169, textXRay, contentPaint);

            canvas.DrawText($"追溯二维码 :", 472, 360, new Paint() { TextSize = 20, AntiAlias = true, Color = Color.Black });
            canvas.DrawBitmap(jkData.QRCode, 445, 180, new Paint() { Color = Color.Black });
        }


        public Bitmap GetViewBitMap()
        {
            var me = MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
            this.Measure(me, me);
            this.Layout(0, 0, this.MeasuredWidth, this.MeasuredHeight);
            this.BuildDrawingCache();
            var bitmap = this.GetDrawingCache(false);
            return bitmap;
        }

    }
}