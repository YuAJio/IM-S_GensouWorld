using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using IdoMaster_GensouWorld.CustomControl.Dialogs;
using IdoMaster_GensouWorld.Utils;
using Ys.BeLazy;
using static IdoMaster_GensouWorld.Utils.WifiSupport;

namespace IdoMaster_GensouWorld.Activitys.Test.OtherToooooools
{
    /// <summary>
    /// Wifi功能测试模块
    /// </summary>
    [Activity(Label = "WifiFunctionActivity", Theme = "@style/Theme.PublicThemePlus")]
    public class WifiFunctionActivity : BaseActivity
    {
        public const string WIFI_STATE_CONNECT = "已连接";
        public const string WIFI_STATE_ON_CONNECTING = "正在连接";
        public const string WIFI_STATE_UNCONNECT = "未连接";

        #region UI控件
        private RecyclerView rv_Wifi;
        private SwitchCompat sw_Wifi;
        private TextView tv_empty;
        #endregion

        private WiFiListAdapter adapter_wifi;
        private List<WifiBean> list_WifiList;

        /// <summary>
        /// 连接状态
        /// </summary>
        private enum ConnectType
        {
            /// <summary>
            /// 连接成功
            /// </summary>
            Sussuce = 1,
            /// <summary>
            /// 正在连接
            /// </summary>
            Connecting = 2,
            /// <summary>
            /// 未连接
            /// </summary>
            None = 0
        }

        /// <summary>
        /// 连接状态字段
        /// </summary>
        private ConnectType connectType = ConnectType.None;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_tools_wifi_fuc;
        }

        public override void B_BeforeInitView()
        {
            adapter_wifi = new WiFiListAdapter(this);
        }

        public override void C_InitView()
        {
            rv_Wifi = FindViewById<RecyclerView>(Resource.Id.rv_wifi);
            sw_Wifi = FindViewById<SwitchCompat>(Resource.Id.sw_wifi);
            tv_empty = FindViewById<TextView>(Resource.Id.tv_empty);
            rv_Wifi.SetLayoutManager(new GridLayoutManager(this, 2));
            rv_Wifi.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Vertical));
            rv_Wifi.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Horizontal));

            sw_Wifi.Checked = WifiSupport.IsOpenWifi(this);
        }

        public override void D_BindEvent()
        {
            list_WifiList = new List<WifiBean>();
            sw_Wifi.CheckedChange += OnSwitchCheckChange;
        }


        public override void E_InitData()
        {
            PrepareInitTrueData();
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }

        private void OnSwitchCheckChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked)
            {//打开wifi
                WifiSupport.OpenWifi(this);
                Task.Run(() =>
                {
                    Thread.Sleep(500);
                }).ContinueWith(x =>
                {
                    PrepareInitTrueData();
                }, TaskScheduler.FromCurrentSynchronizationContext()); ;

            }
            else
            {//关闭wifi
                WifiSupport.CloseWifi(this);
                ChangeWifiUI(false);
            }
        }

        /// <summary>
        /// 准备真的开始初始化数据
        /// </summary>
        private void PrepareInitTrueData()
        {
            isHavePermission = ChechPermission();
            if (!isHavePermission && WifiSupport.IsOpenWifi(this))
                //未获取权限,申请权限
                RequestPermission();
            else if (isHavePermission && WifiSupport.IsOpenWifi(this))
                //已经获取权限
                InitTrueData();
            else
            {
                ShowMsgLong("WIFI处于关闭状态");
                ChangeWifiUI(false);
            }

        }

        /// <summary>
        /// 真的开始初始化数据
        /// </summary>
        private void InitTrueData()
        {
            rv_Wifi.SetAdapter(adapter_wifi);

            if (IsOpenWifi(this) && isHavePermission)
            {
                ChangeWifiUI(true);
                //SortScaResul();
                WifiListChange();
            }
            else
            {
                ChangeWifiUI(false);
                ShowMsgLong("WIFI处于关闭状态或权限获取失败Geso");
            }
            adapter_wifi.onItemClickAct += delegate (View view, int position)
            {
                var data = adapter_wifi[position];
                switch (data.State)
                {
                    case WIFI_STATE_CONNECT:
                        {//连接中
                            var wifiInfo = WifiSupport.GetConnectedWifiInfo(this);
                            ShowIsConnectingWifiDialog(wifiInfo);
                        }
                        break;
                    case WIFI_STATE_UNCONNECT:
                        {//未连接
                            var capabilities = data.Capabiliteies;
                            if (WifiSupport.GetWifiCipher(capabilities) == WifiSupport.WifiCipherType.WIFICIPHER_NOPASS)
                            {//无需密码
                                var tempConfig = WifiSupport.IsExsits(data.WifiName, this);
                                if (tempConfig == null)
                                {
                                    var exsits = WifiSupport.CreateWifiConfig(data.WifiName, null, WifiSupport.WifiCipherType.WIFICIPHER_NOPASS);
                                    WifiSupport.AddNetWork(exsits, this);
                                }
                                else
                                    WifiSupport.AddNetWork(tempConfig, this);

                            }
                            else
                            {
                                //需要密码,弹出输入密码dialog
                                var tempConfig = WifiSupport.IsExsits(data.WifiName, this);
                                if (tempConfig != null)
                                {
                                    if (WifiSupport.AddNetWork(tempConfig, this))
                                    {//连接失败,密码错误.
                                        ShowDiyToastLong("密码错误,请重新输入密码");
                                        WifiSupport.RemoveSaveingWIFI(this, tempConfig.NetworkId);
                                        ShowInputPassWordDialog(data.WifiName, data.Capabiliteies);
                                    }
                                }
                                else
                                    ShowInputPassWordDialog(data.WifiName, data.Capabiliteies);
                            }
                            break;
                        }

                }
            };
        }

        /// <summary>
        /// 改变页面的展示
        /// </summary>
        /// <param name="bo">true:wifi打开中 false:wifi关闭中</param>
        private void ChangeWifiUI(bool bo)
        {

            if (bo)
            {
                CoverUIControl(CoverFlag.Visible, rv_Wifi);
                CoverUIControl(CoverFlag.Gone, tv_empty);
            }
            else
            {
                CoverUIControl(CoverFlag.Gone, rv_Wifi);
                CoverUIControl(CoverFlag.Visible, tv_empty);
            }
        }

        /// <summary>
        /// 清理掉搜寻出来的wifi
        /// </summary>
        public void CleanScaResul()
        {
            list_WifiList.Clear();
            adapter_wifi.SetDataList(null);
            ChangeWifiUI(false);
        }

        /// <summary>
        /// 获取wifi列表然后将实体类转换为自己的实体类
        /// </summary>
        public void SortScaResul()
        {
            var scanResults = WifiSupport.NoSameName(WifiSupport.GetWifiScanResult(this));
            list_WifiList.Clear();
            if (scanResults.Any())
            {
                var jk = scanResults.Select(x => new WifiBean()
                {
                    WifiName = x.Ssid,
                    State = WIFI_STATE_UNCONNECT,
                    Capabiliteies = x.Capabilities,
                    IsSaveing = (WifiSupport.IsExsits(x.Ssid, this) != null),
                    Level = WifiManager.CalculateSignalLevel(x.Level, 100) /*WifiSupport.GetLevel(x.Level)*/
                }).ToList();
                jk.OrderBy(x => x.Level);
                list_WifiList.AddRange(jk);
                adapter_wifi.SetDataList(list_WifiList);
            }
        }

        /// <summary>
        /// 网络状态发生改变 调用方法
        /// </summary>
        private void WifiListChange()
        {
            SortScaResul();
            var connectedWifiInfo = WifiSupport.GetConnectedWifiInfo(this);
            if (connectedWifiInfo != null)
                if (connectedWifiInfo.NetworkId >= 0)
                    WifiListSet(connectedWifiInfo.SSID);

        }

        /// <summary>
        /// 将"已连接"或者"正在连接"的wifi热点放置在第一个位置
        /// </summary>
        /// <param name="wifiName"></param>
        /// <param name="type"></param>
        private void WifiListSet(string wifiName)
        {
            var index = -1;
            var wifiInfo = new WifiBean();
            if (!list_WifiList.Any())
                return;

            list_WifiList.ForEach(x =>
            {
                x.State = WIFI_STATE_UNCONNECT;
            });
            //根据信号强度排序
            list_WifiList.OrderBy(x => x.Level);
            for (int i = 0; i < list_WifiList.Count; i++)
            {
                var x = list_WifiList[i];
                var name = $@"""{x.WifiName}""";
                if (index == -1 && name == wifiName)
                {
                    index = i;
                    wifiInfo.Level = x.Level;
                    wifiInfo.WifiName = x.WifiName;
                    wifiInfo.Capabiliteies = x.Capabiliteies;
                    switch (connectType)
                    {
                        case ConnectType.Sussuce:
                            wifiInfo.State = WIFI_STATE_CONNECT;
                            break;
                        case ConnectType.Connecting:
                            wifiInfo.State = WIFI_STATE_ON_CONNECTING;
                            break;
                        case ConnectType.None:
                            {
                                if (wifiName == name)
                                    wifiInfo.State = WIFI_STATE_CONNECT;
                            }
                            break;
                    }
                }
            }
            if (index != -1)
            {
                list_WifiList.RemoveAt(index);
                list_WifiList.Insert(0, wifiInfo);
                adapter_wifi.SetDataList(list_WifiList);
            }
        }

        #region 输入框相关
        /// <summary>
        /// 显示密码输入框
        /// </summary>
        /// <param name="name"></param>
        /// <param name="capabilities"></param>
        private void ShowInputPassWordDialog(string name, string capabilities)
        {
            var dialog = new WifiLinkDialog(this, name, capabilities);
            dialog.SetCancelable(true);
            dialog.Show();
        }
        /// <summary>
        /// 显示wifi信息框(断开用)
        /// </summary>
        /// <param name="name"></param>
        private void ShowIsConnectingWifiDialog(WifiInfo info)
        {
            var dialog = new WifiLinkDialog(this, info);
            dialog.SetCancelable(true);
            dialog.Show();
        }

        #endregion

        #region WIFI状态广播
        /// <summary>
        /// wifi状态广播
        /// </summary>
        private WIFIBroadCastReceiver wifiReceiver;
        /// <summary>
        /// 监听WIFI状态
        /// </summary>
        private class WIFIBroadCastReceiver : BroadcastReceiver
        {
            private WifiFunctionActivity wifiActivity;
            public WIFIBroadCastReceiver(WifiFunctionActivity wifiActivity)
            {
                this.wifiActivity = wifiActivity;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                var aciton = intent.Action;
                switch (aciton)
                {
                    case WifiManager.NetworkStateChangedAction:
                        {
                            var info = (NetworkInfo)intent.GetParcelableExtra(WifiManager.ExtraNetworkInfo);
                            var jk = info.GetState();
                            if (NetworkInfo.State.Disconnected == info.GetState())
                            {//wifi没连接上

                                wifiActivity.list_WifiList.ForEach(x =>
                                {//没连接上 将所有的连接状态都设置为"未连接"
                                    x.State = WIFI_STATE_UNCONNECT;
                                });
                                wifiActivity.adapter_wifi.SetDataList(wifiActivity.list_WifiList);
                            }
                            else if (NetworkInfo.State.Connected == info.GetState())
                            {//wifi连接上了
                                var connectedWifiInfo = WifiSupport.GetConnectedWifiInfo(wifiActivity);
                                wifiActivity.connectType = ConnectType.Sussuce;
                                wifiActivity.WifiListSet(connectedWifiInfo.SSID);
                                wifiActivity.HideWaitDiaLog();
                            }
                            else if (NetworkInfo.State.Connecting == info.GetState())
                            {//正在连接
                                wifiActivity.ShowWaitDiaLog("wifi连接中");
                                var connectedWifiInfo = WifiSupport.GetConnectedWifiInfo(wifiActivity);
                                wifiActivity.connectType = ConnectType.Connecting;
                                wifiActivity.WifiListSet(connectedWifiInfo.SSID);
                            }
                        }
                        break;
                    case WifiManager.WifiStateChangedAction:
                        {
                            var state = (WifiStatus)intent.GetIntExtra(WifiManager.ExtraWifiState, 0);
                            switch (state)
                            {
                                case WifiStatus.Current:
                                    {//???

                                    }
                                    break;
                                case WifiStatus.Disabled:
                                    {//关闭WIFI
                                        wifiActivity.CleanScaResul();
                                        wifiActivity.sw_Wifi.Checked = false;
                                    }
                                    break;
                                case WifiStatus.Enabled:
                                    {//打开WIFI
                                     //wifiActivity.SortScaResul();
                                        wifiActivity.sw_Wifi.Checked = true;
                                    }
                                    break;
                            }
                        }
                        break;
                    case WifiManager.ScanResultsAvailableAction:
                        {
                            wifiActivity.WifiListChange();
                        }
                        break;
                }

            }

        }
        #endregion

        #region Adapter
        /// <summary>
        /// Wifi适配器
        /// </summary>
        private class WiFiListAdapter : YsBaseRvAdapter<WifiBean>
        {
            public WiFiListAdapter(Context context)
            {
                this.context = context;
            }

            public override WifiBean this[int position] { get { return list_data[position]; } }

            /// <summary>
            /// 清理所有数据
            /// </summary>
            /// <param name="data"></param>
            public void ReplaceAll(List<WifiBean> data)
            {
                if (this.list_data.Any())
                    this.list_data.Clear();
                this.SetContainerList(data);
            }

            protected override void AbOnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
            {
                var holder = viewHolder as ViewHolder;
                var data = list_data[position];

                holder.tv_WifiName.Text = data.WifiName;

                //设置WIFI状态信息
                var stateInfo = "";
                var stateVisible = ViewStates.Gone;
                if (data.State == WIFI_STATE_ON_CONNECTING || data.State == WIFI_STATE_CONNECT)
                {
                    stateInfo = $"{data.State}";
                    stateVisible = ViewStates.Visible;
                }
                else
                {
                    stateInfo = data.IsSaveing ? "以保存的WIFI" : "";
                    stateVisible = data.IsSaveing ? ViewStates.Visible : ViewStates.Gone;
                }
                holder.tv_WifiStatus.Text = stateInfo;
                holder.tv_WifiStatus.Visibility = stateVisible;


                //可以传递给adapter的数据都是经过处理的,
                //已连接或是正在连接装填的wifi都是处于集合中的首位,
                //所以可以写出如下判断
                var color = ContextCompat.GetColorStateList(context, Resource.Color.black);
                if (position == 0 && (data.State == WIFI_STATE_ON_CONNECTING || data.State == WIFI_STATE_CONNECT))
                    color = ContextCompat.GetColorStateList(context, Resource.Color.violet);
                holder.tv_WifiName.SetTextColor(color);

                ///设置wifi状态图片
                var b =
                  data.Level > 0 && data.Level <= 20 ? 1 :
                  data.Level > 20 && data.Level <= 40 ? 2 :
                  data.Level > 40 && data.Level <= 60 ? 3 :
                  data.Level > 60 && data.Level <= 80 ? 4 :
                  data.Level > 80 && data.Level <= 100 ? 5
                    : 0;
                var backSRC = Resource.Mipmap.icon_wifi_level_0;
                switch (b)
                {
                    case 1:
                        backSRC = Resource.Mipmap.icon_wifi_level_1;
                        break;
                    case 2:
                        backSRC = Resource.Mipmap.icon_wifi_level_2;
                        break;
                    case 3:
                        backSRC = Resource.Mipmap.icon_wifi_level_3;
                        break;
                    case 4:
                        backSRC = Resource.Mipmap.icon_wifi_level_4;
                        break;
                    case 5:
                        backSRC = Resource.Mipmap.icon_wifi_level_5;
                        break;
                }
                holder.iv_WifiIcon.SetBackgroundResource(backSRC);
            }

            protected override RecyclerView.ViewHolder AbOnCreateViewHolder(ViewGroup parent, int viewType)
            {
                var jk = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_wifi_function, parent, false);
                return new ViewHolder(jk);
            }

            private class ViewHolder : RecyclerView.ViewHolder
            {
                public TextView tv_WifiName;
                public TextView tv_WifiStatus;
                public ImageView iv_WifiIcon;
                public ViewHolder(View itemView) : base(itemView)
                {
                    tv_WifiName = itemView.FindViewById<TextView>(Resource.Id.tv_item_wifi_name);
                    tv_WifiStatus = itemView.FindViewById<TextView>(Resource.Id.tv_item_wifi_status);
                    iv_WifiIcon = itemView.FindViewById<ImageView>(Resource.Id.iv_wifi_pic);
                }
            }

        }

        #endregion

        #region 检测权限以及wifi开启状态
        /// <summary>
        /// 权限请求码
        /// </summary>
        private const int PerMission_Reuqest_Code = 114;

        /// <summary>
        /// 两个危险权限需要动态申请
        /// </summary>
        private static string[] Needed_Permisstion = new string[] {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        private bool isHavePermission;


        /// <summary>
        /// 检查是否已经授予权限
        /// </summary>
        /// <returns></returns>
        private bool ChechPermission()
        {
            foreach (var permission in Needed_Permisstion)
            {
                if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(this, permission) != Android.Content.PM.Permission.Granted)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 申请权限
        /// </summary>
        private void RequestPermission()
        {
            ActivityCompat.RequestPermissions(this, Needed_Permisstion, PerMission_Reuqest_Code);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            var hasAllpermission = true;
            if (requestCode == PerMission_Reuqest_Code)
            {
                foreach (var i in grantResults)
                {
                    if (i != Permission.Granted)
                    {
                        hasAllpermission = false;
                        break;
                    }
                }

                if (hasAllpermission)
                {
                    isHavePermission = true;
                    if (WifiSupport.IsOpenWifi(this) && isHavePermission)
                    {//如果wifi开关是开 并且 已经获取权限

                    }
                    else
                        ShowMsgLong("WIFI处于关闭状态或权限获取失败");
                }
                else
                {
                    isHavePermission = false;
                    ShowMsgLong("获取权限失败");
                }
            }

        }


        #endregion

        #region 绳命周期
        protected override void OnResume()
        {
            base.OnResume();
            //注册WIFI状态监听广播
            wifiReceiver = new WIFIBroadCastReceiver(this);
            var filter = new IntentFilter();
            filter.AddAction(WifiManager.WifiStateChangedAction);
            filter.AddAction(WifiManager.ScanResultsAvailableAction);
            filter.AddAction(WifiManager.NetworkStateChangedAction);
            this.RegisterReceiver(wifiReceiver, filter);

        }
        protected override void OnPause()
        {
            base.OnPause();
            //解绑WIFI状态监听
            this.UnregisterReceiver(wifiReceiver);
        }
        #endregion
    }
}