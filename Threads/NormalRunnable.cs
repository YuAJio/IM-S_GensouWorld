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
using Java.Lang;

namespace IdoMaster_GensouWorld.Threads
{
    /// <summary>
    /// 最基础Runnable类,只做发送消息
    /// </summary>
    public class NormalRunnable : Java.Lang.Object, IRunnable
    {
        private YurishBaseiHandler MHandler;
        private int mWhat;

        public NormalRunnable(YurishBaseiHandler handler, int what)
        {
            this.MHandler = handler;
            this.mWhat = what;
        }

        public void Run()
        {
            var msg = new Message()
            {
                What = mWhat
            };
            MHandler.SendMessage(msg);
        }
    }
}