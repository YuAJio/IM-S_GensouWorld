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
using IdoMaster_GensouWorld.Listeners;

namespace IdoMaster_GensouWorld.Activitys.Test
{
    [Activity(Label = "GaodeMapActivity", Theme = "@style/Theme.PublicThemePlus")]
    public class GaodeMapActivity : BaseActivity
    {
        private MapView mv_map;
        private Button bt_click;

        private AMap object_map;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_gaodemap;
        }

        public override void B_BeforeInitView()
        {

        }

        public override void C_InitView()
        {
            mv_map = FindViewById<MapView>(Resource.Id.mv_map);
            bt_click = FindViewById<Button>(Resource.Id.bt_click);
        }

        public override void D_BindEvent()
        {
            bt_click.Click += OnClickListener;
        }

        public override void E_InitData()
        {
            mv_map.OnCreate(this.saveInstanceState);
            InitMyLocationPoint();
            InitMapUiSettings();
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
            switch (v.Id)
            {
                case Resource.Id.bt_click:
                    {
                        AddMakerInMap("Position Zero !", 30.5625381008, 104.1876733303);
                    }
                    break;
            }
        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }

        private bool OnMarkerClickEvent(Marker marker)
        {
            if (marker.IsInfoWindowShown)
                marker.HideInfoWindow();
            else
                marker.ShowInfoWindow();

            ShowMsgLong(marker.Title);
            return true;
        }


        /// <summary>
        /// 初始化地图对象
        /// </summary>
        /// <param name="isInitMarkerEvent">是否初始化标记事件</param>
        private void InitAMapObject(bool isInitMarkerEvent = false)
        {
            if (object_map == null)
                object_map = mv_map.Map;
            if (object_map == null)
                InitAMapObject(isInitMarkerEvent);

            if (isInitMarkerEvent)
                object_map.SetOnMarkerClickListener(new YsOnMarkerClickListener(OnMarkerClickEvent));
            object_map.SetInfoWindowAdapter(new YsInfoWIndowAdapter(this));
            object_map.MapType = AMap.MapTypeSatellite;
        }

        /// <summary>
        /// 初始化我的定位蓝点
        /// </summary>
        private void InitMyLocationPoint()
        {
            InitAMapObject(true);

            MyLocationStyle myLocationStyle;
            myLocationStyle = new MyLocationStyle();
            myLocationStyle.InvokeMyLocationType(MyLocationStyle.LocationTypeLocate);
            object_map.SetMyLocationStyle(myLocationStyle);
            var latLng = new LatLng(30.5625381008, 104.1876733303);
            object_map.MoveCamera(CameraUpdateFactory.NewLatLngZoom(latLng, 17));
        }

        /// <summary>
        /// マーカー おえでする
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        private void AddMakerInMap(string title, double lon, double lat)
        {
            var latLng = new LatLng(lon, lat);
            var markerOption = new MarkerOptions();
            markerOption.InvokePosition(latLng);
            markerOption.InvokeTitle("");
            markerOption.InvokeSnippet("");
            var marker = object_map.AddMarker(markerOption);
            marker.ShowInfoWindow();
        }

        private void InitMapUiSettings()
        {
            var mUiSettings = object_map.UiSettings;
            ///不显示加减号控件
            mUiSettings.ZoomControlsEnabled = false;

            ///显示定位按钮
            mUiSettings.MyLocationButtonEnabled = true;
            object_map.MyLocationEnabled = true;

            ///手势控制是否生效
            mUiSettings.ZoomGesturesEnabled = true;//缩放手势
            mUiSettings.ScrollGesturesEnabled = true;//滑动手势
            //mUiSettings.RotateGesturesEnabled = true;//旋转手势
            //mUiSettings.TiltGesturesEnabled = true;//倾斜手势
            mUiSettings.SetAllGesturesEnabled(true);//所有手势
        }

        #region 地图InfoWindowAdapter
        private class YsInfoWIndowAdapter : Java.Lang.Object, AMap.IInfoWindowAdapter
        {
            private readonly Context context;
            public YsInfoWIndowAdapter(Context context)
            {
                this.context = context;
            }


            public View GetInfoWindow(Marker marker)
            {
                if (infoWindow == null)
                    infoWindow = View.Inflate(context, Resource.Layout.item_battle_skill_or_item, null);
                Render(marker, infoWindow);
                return infoWindow;
            }

            public View GetInfoContents(Marker marker)
            {
                return null;
            }

            View infoWindow = null;


            public void Render(Marker marker, View view)
            {
                var tv_Text = view.FindViewById<TextView>(Resource.Id.tv_name);
                tv_Text.Text = "修改后的东西你知道吗?";
            }
        }
        #endregion

        #region 生命周期管理地图

        protected override void OnDestroy()
        {
            base.OnDestroy();
            mv_map.OnDestroy();
        }
        protected override void OnResume()
        {
            base.OnResume();
            mv_map.OnResume();
        }
        protected override void OnPause()
        {
            base.OnPause();
            mv_map.OnPause();
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            mv_map.OnSaveInstanceState(outState);
        }
        #endregion
    }
}