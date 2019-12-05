using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Bumptech.Glide;
using Bumptech.Glide.Load;
using Bumptech.Glide.Load.Engine;
using Bumptech.Glide.Request.Target;
using Java.Lang;

namespace IdoMaster_GensouWorld.Activitys.Test
{
    [Activity(Label = "GildeTestActivity", Theme = "@style/Theme.PublicThemePlus")]
    public class GildeTestActivity : Ys.BeLazy.YsBaseActivity
    {
        private ImageView iv_1;
        private ImageView iv_2;
        private ImageView iv_3;
        private ImageView iv_4;

        private ImageSwitcher is_1;

        private ViewPager vp_1;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_gilde_test;
        }

        public override void B_BeforeInitView()
        {
        }

        public override void C_InitView()
        {
            vp_1 = FindViewById<ViewPager>(Resource.Id.vp_1);
            //is_1.SetFactory(new ImageSwicterFactory(this));
        }

        public override void D_BindEvent()
        {
        }

        public override void E_InitData()
        {

            var jk = new List<string>()
            {
              "https://cancanan-1257056207.cos.ap-chengdu.myqcloud.com/misimage/2bbaf06d-60d7-4b6e-a422-4d37b5daf830.gif",
           "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=3087766220,2961674138&fm=26&gp=0.jpg",
           "http://192.168.0.146:866/default/wimage/index/?id=9162d8cf-c88c-4571-83f5-238554a749ef",
           "https://cancanan-1257056207.cos.ap-chengdu.myqcloud.com/misimage/9cdca1f5-4a0e-4559-a559-527acce4819b.gif",
        };

            vp_1.Adapter = new ViewPagerA(this, jk);
            vp_1.SetPageTransformer(true, new DepthPgaeTransformer());

            var timer = new Timer() { Interval = 10 * 1000, AutoReset = true, Enabled = true };
            timer.Elapsed += delegate (object j, ElapsedEventArgs k)
            {
                vp_1.SetCurrentItem(vp_1.CurrentItem + 1, true);
            };

            //var url1 = "https://cancanan-1257056207.cos.ap-chengdu.myqcloud.com/misimage/2bbaf06d-60d7-4b6e-a422-4d37b5daf830.gif";
            //var url2 = "https://ss0.bdstatic.com/70cFvHSh_Q1YnxGkpoWK1HF6hhy/it/u=3087766220,2961674138&fm=26&gp=0.jpg";
            //var url3 = "http://192.168.0.146:866/default/wimage/index/?id=9162d8cf-c88c-4571-83f5-238554a749ef";
            //var url4 = "https://cancanan-1257056207.cos.ap-chengdu.myqcloud.com/misimage/9cdca1f5-4a0e-4559-a559-527acce4819b.gif";

            //var imp = new GlideListenerImp(() =>
            //{
            //    return false;
            //}, (j) =>
            //{
            //    is_1.SetImageDrawable(j);
            //    return true;
            //});
            //Glide.With(this).Load(url2).Listener(imp).Into((is_1.CurrentView) as ImageView); ;
            //Glide.With(this).Load(url1).Listener(imp).Into(iv_1); ;
            //Glide.With(this).Load(url2).Listener(imp).Into(iv_2); ;
            //Glide.With(this).Load(url3).Listener(imp).Into(iv_3); ;
            //Glide.With(this).Load(url4).Listener(imp).Into(iv_4); ;

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
        }


        private class GlideListenerImp : Java.Lang.Object, Bumptech.Glide.Request.IRequestListener
        {
            public Func<bool> onLoadFailedFunc;
            public Func<BitmapDrawable, bool> onResourceReadyFunc;

            public GlideListenerImp(Func<bool> onLoadFailedFunc, Func<BitmapDrawable, bool> onResourceReadyFunc)
            {
                this.onLoadFailedFunc = onLoadFailedFunc;
                this.onResourceReadyFunc = onResourceReadyFunc;
            }

            public bool OnLoadFailed(GlideException p0, Java.Lang.Object p1, ITarget p2, bool p3)
            {
                return (onLoadFailedFunc?.Invoke()).GetValueOrDefault();
            }

            public bool OnResourceReady(Java.Lang.Object p0, Java.Lang.Object p1, ITarget p2, DataSource p3, bool p4)
            {
                return (onResourceReadyFunc?.Invoke(p0 as BitmapDrawable)).GetValueOrDefault();
            }
        }

        private class ImageSwicterFactory : Java.Lang.Object, ViewSwitcher.IViewFactory
        {
            private Activity context;

            public ImageSwicterFactory(Activity context)
            {
                this.context = context;
            }

            public View MakeView()
            {
                return new ImageView(context)
                {
                    LayoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent),
                };
            }
        }


        private class ViewPagerA : PagerAdapter
        {
            private Activity context;
            private List<string> dataList;

            public ViewPagerA(Activity context)
            {
                this.context = context;
            }

            public ViewPagerA(Activity context, List<string> dataList) : this(context)
            {
                this.context = context;
                this.dataList = dataList;
            }

            public override int Count { get { return int.MaxValue; } }

            public override bool IsViewFromObject(View view, Java.Lang.Object @object)
            {
                return view == @object;
            }

            public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
            {
                var iv = new ImageView(context)
                {
                    LayoutParameters = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent),
                };
                iv.SetScaleType(ImageView.ScaleType.CenterCrop);
                var newPosition = position % dataList.Count;

                context.RunOnUiThread(() =>
                {
                    Glide.With(context).Load(dataList[newPosition]).Into(iv);
                    container.AddView(iv);
                });

                return iv;
            }

            public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
            {
                context.RunOnUiThread(() =>
                {
                    container.RemoveView(@object as View);
                });
            }

        }

        private class DepthPgaeTransformer : Java.Lang.Object, ViewPager.IPageTransformer
        {
            private const float MIN_SCALE = 0.75f;

            public void TransformPage(View page, float position)
            {
                int pageWidth = page.Width;

                if (position < -1)
                { // [-Infinity,-1)
                  // This page is way off-screen to the left.
                    page.Alpha = (0);

                }
                else if (position <= 0)
                { // [-1,0]
                  // Use the default slide transition when moving to the left page
                    page.Alpha = (1);
                    page.TranslationX = (0);
                    page.ScaleX = (1);
                    page.ScaleY = (1);

                }
                else if (position <= 1)
                { // (0,1]
                  // Fade the page out.
                    page.Alpha = (1 - position);

                    // Counteract the default slide transition
                    page.TranslationX = (pageWidth * -position);

                    // Scale the page down (between MIN_SCALE and 1)
                    float scaleFactor = MIN_SCALE
                            + (1 - MIN_SCALE) * (1 - Java.Lang.Math.Abs(position));
                    page.ScaleX = (scaleFactor);
                    page.ScaleY = (scaleFactor);

                }
                else
                { // (1,+Infinity]
                  // This page is way off-screen to the right.
                    page.Alpha = (0);
                }
            }
        }

    }

}
