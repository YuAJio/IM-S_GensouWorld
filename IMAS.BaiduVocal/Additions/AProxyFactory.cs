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
using Com.Baidu.Tts.Aop;

namespace Com.Baidu.Tts.Aop
{
    public abstract partial class AProxyFactory : global::Java.Lang.Object, global::Com.Baidu.Tts.Aop.IProxyFactory
    {
        public Java.Lang.Object CreateProxied()
        {
            throw new Exception("");
        }
    }
}