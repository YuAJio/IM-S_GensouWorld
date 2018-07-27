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

namespace Android.Support.V7.Widget.Helper
{
    class AzuminItemTouchHelper : ItemTouchHelper
    {
        private Callback mCallBack;
        public AzuminItemTouchHelper(Callback callback) : base(callback)
        {
            this.mCallBack = callback;
        }
        public Callback GetCallback()
        {
            return mCallBack;
        }
    }
}