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
using Com.Amap.Api.Maps2d;
using Com.Amap.Api.Maps2d.Model;

namespace IdoMaster_GensouWorld.Listeners
{
    /// <summary>
    /// 高德地图标记点击事件监听
    /// </summary>
    public class YsOnMarkerClickListener : Java.Lang.Object, AMap.IOnMarkerClickListener
    {
        public YsOnMarkerClickListener(Func<Marker, bool> @event)
        {
            this.onMarkerClickAct = @event;
        }

        private Func<Marker, bool> onMarkerClickAct;

        public bool OnMarkerClick(Marker p0)
        {
            if (onMarkerClickAct != null)
                return onMarkerClickAct.Invoke(p0);
            return false;
        }
    }
}