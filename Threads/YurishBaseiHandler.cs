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

namespace IdoMaster_GensouWorld.Threads
{
    /// <summary>
    /// 基础Handler类
    /// </summary>
    public class YurishBaseiHandler : Handler
    {
        public Action<Message> handlerAction;

        public override void HandleMessage(Message msg)
        {
            handlerAction?.Invoke(msg);
        }

    }
}