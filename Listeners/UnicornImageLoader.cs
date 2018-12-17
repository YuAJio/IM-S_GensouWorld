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
using Com.Qiyukf.Unicorn.Api;

namespace IdoMaster_GensouWorld.Listeners
{
    public class UnicornImageLoader : Java.Lang.Object, IUnicornImageLoader
    {
        public void LoadImage(string p0, int p1, int p2, IImageLoaderListener p3)
        {
        }

        public Bitmap LoadImageSync(string p0, int p1, int p2)
        {
            return null;
        }
    }
}