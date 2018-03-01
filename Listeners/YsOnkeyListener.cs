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

namespace IdoMaster_GensouWorld.Listeners
{
    /// <summary>
    /// 按键
    /// </summary>
    public class YsOnkeyListener : Java.Lang.Object, View.IOnKeyListener
    {
        public Func<View, Keycode, KeyEvent, bool> keyFunc;

        public YsOnkeyListener(Func<View, Keycode, KeyEvent, bool> keyFunc)
        {
            this.keyFunc = keyFunc;
        }

        public bool OnKey(View v, [GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            return keyFunc(v, keyCode, e);
        }
    }
}