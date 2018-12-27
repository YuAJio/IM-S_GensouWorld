using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace IdoMaster_GensouWorld.Utils
{

    /// <summary>
    /// Wifi支援组
    /// </summary>
    public class WifiSupport
    {
        private const string TAG = "WifiSupport";

        public enum WifiCipherType
        {
            WIFICIPHER_WEP, WIFICIPHER_WPA, WIFICIPHER_NOPASS, WIFICIPHER_INVALID
        }

        public WifiSupport() { }

        public static List<ScanResult> GetWifiScanResult(Context context)
        {
            bool b = context == null;
            return ((WifiManager)context.ApplicationContext.GetSystemService(Context.WifiService)).ScanResults.ToList();
        }

        public static bool IsWifiEnable(Context context)
        {
            return ((WifiManager)context.ApplicationContext.GetSystemService(Context.WifiService)).IsWifiEnabled;
        }

        public static WifiInfo GetConnectedWifiInfo(Context context)
        {
            return ((WifiManager)context.ApplicationContext.GetSystemService(Context.WifiService)).ConnectionInfo;
        }

        public static List<WifiConfiguration> GetConfigurations(Context context)
        {
            return ((WifiManager)context.ApplicationContext.GetSystemService(Context.WifiService)).ConfiguredNetworks.ToList();
        }


        public static WifiConfiguration CreateWifiConfig(string SSID, string password, WifiCipherType type)
        {

            WifiConfiguration config = new WifiConfiguration();
            config.AllowedAuthAlgorithms.Clear();
            config.AllowedGroupCiphers.Clear();
            config.AllowedKeyManagement.Clear();
            config.AllowedPairwiseCiphers.Clear();
            config.AllowedProtocols.Clear();
            config.Ssid = "\"" + SSID + "\"";

            if (type == WifiCipherType.WIFICIPHER_NOPASS)
            {
                //            config.wepKeys[0] = "";  //注意这里
                config.AllowedKeyManagement.Set(Convert.ToInt32(KeyManagementType.None));
                //            config.wepTxKeyIndex = 0;
            }

            if (type == WifiCipherType.WIFICIPHER_WEP)
            {
                config.PreSharedKey = "\"" + password + "\"";
                config.HiddenSSID = true;
                config.AllowedAuthAlgorithms.Set((int)AuthAlgorithmType.Open);
                config.AllowedGroupCiphers.Set((int)GroupCipherType.Ccmp);
                config.AllowedGroupCiphers.Set((int)GroupCipherType.Tkip);
                config.AllowedGroupCiphers.Set((int)GroupCipherType.Wep40);
                config.AllowedGroupCiphers.Set((int)GroupCipherType.Wep104);
                config.AllowedKeyManagement.Set((int)KeyManagementType.None);
                config.WepTxKeyIndex = 0;
            }

            if (type == WifiCipherType.WIFICIPHER_WPA)
            {
                config.PreSharedKey = "\"" + password + "\"";
                config.HiddenSSID = true;
                config.AllowedAuthAlgorithms.Set((int)AuthAlgorithmType.Open);
                config.AllowedGroupCiphers.Set((int)GroupCipherType.Tkip);
                config.AllowedGroupCiphers.Set((int)GroupCipherType.Ccmp);
                config.AllowedKeyManagement.Set((int)KeyManagementType.WpaPsk);
                config.AllowedPairwiseCiphers.Set((int)PairwiseCipherType.Tkip);
                config.AllowedPairwiseCiphers.Set((int)PairwiseCipherType.Ccmp);
                config.StatusField = WifiStatus.Enabled;
            }
            return config;

        }


        /// <summary>
        /// 连接某个Wifi热点
        /// </summary>
        /// <param name="config"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool AddNetWork(WifiConfiguration config, Context context)
        {
            WifiManager wifimanager = (WifiManager)context.ApplicationContext.GetSystemService(Context.WifiService);

            WifiInfo wifiinfo = wifimanager.ConnectionInfo;

            if (null != wifiinfo)
            {
                wifimanager.DisableNetwork(wifiinfo.NetworkId);
            }

            bool result = false;

            if (config.NetworkId >= 0)
            {
                result = wifimanager.EnableNetwork(config.NetworkId, true);
                wifimanager.UpdateNetwork(config);
            }
            else
            {

                int i = wifimanager.AddNetwork(config);
                result = false;

                if (i > 0)
                {
                    if (wifimanager.EnableNetwork(i, true))
                    {
                        wifimanager.SaveConfiguration();
                        return true;
                    }
                }
            }
            return result;

        }

        /// <summary>
        /// 判断wifi热点支持的加密方式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static WifiCipherType GetWifiCipher(string s)
        {

            if (string.IsNullOrEmpty(s))
            {
                return WifiCipherType.WIFICIPHER_INVALID;
            }
            else if (s.Contains("WEP"))
            {
                return WifiCipherType.WIFICIPHER_WEP;
            }
            else if (s.Contains("WPA") || s.Contains("WPA2") || s.Contains("WPS"))
            {
                return WifiCipherType.WIFICIPHER_WPA;
            }
            else
            {
                return WifiCipherType.WIFICIPHER_NOPASS;
            }
        }

        /// <summary>
        /// 查看以前是否也配置过这个网络
        /// </summary>
        /// <param name="SSID"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static WifiConfiguration IsExsits(string SSID, Context context)
        {
            var wifimanager = (WifiManager)context.ApplicationContext.GetSystemService(Context.WifiService);
            var existingConfigs = wifimanager.ConfiguredNetworks.ToList();
            return existingConfigs.Where(x => x.Ssid == "\"" + SSID + "\"").FirstOrDefault();
        }


        /// <summary>
        /// 打开WIFI
        /// </summary>
        /// <param name="context"></param>
        public static void OpenWifi(Context context)
        {
            var wifimanager = (WifiManager)context.ApplicationContext.GetSystemService(Context.WifiService);
            if (!wifimanager.IsWifiEnabled)
                wifimanager.SetWifiEnabled(true);
        }

        /// <summary>
        /// 关闭WIFI
        /// </summary>
        /// <param name="context"></param>
        public static void CloseWifi(Context context)
        {
            var wifimanager = (WifiManager)context.ApplicationContext.GetSystemService(Context.WifiService);
            if (wifimanager.IsWifiEnabled)
                wifimanager.SetWifiEnabled(false);
        }

        public static bool IsOpenWifi(Context context)
        {
            var wifimanager = (WifiManager)context.ApplicationContext.GetSystemService(Context.WifiService);
            var b = wifimanager.IsWifiEnabled;
            return b;
        }

        /// <summary>
        /// 将idAddress转化成string类型的Id字符串
        /// </summary>
        /// <param name="idString"></param>
        /// <returns></returns>
        public static string GetStringId(int idString)
        {
            StringBuffer sb = new StringBuffer();
            int b = (idString >> 0) & 0xff;
            sb.Append(b + ".");
            b = (idString >> 8) & 0xff;
            sb.Append(b + ".");
            b = (idString >> 16) & 0xff;
            sb.Append(b + ".");
            b = (idString >> 24) & 0xff;
            sb.Append(b);
            return sb.ToString();
        }


        /// <summary>
        /// 设置安全性
        /// </summary>
        /// <param name="capabilities"></param>
        /// <returns></returns>
        public static string GetCapabilitiesString(string capabilities)
        {
            if (capabilities.Contains("WEP"))
                return "WEP";
            else if (capabilities.Contains("WPA") || capabilities.Contains("WPA2") || capabilities.Contains("WPS"))
                return "WPA/WPA2";
            else
                return "OPEN";
        }

        public static bool GetIsWifiEnabled(Context context)
        {
            WifiManager wifimanager = (WifiManager)context.ApplicationContext.GetSystemService(Context.WifiService);
            return wifimanager.IsWifiEnabled;
        }

        public static void GetReplace(Context context, List<WifiBean> list)
        {
            WifiInfo wifi = WifiSupport.GetConnectedWifiInfo(context);
            List<WifiBean> listCopy = new List<WifiBean>();
            listCopy.AddRange(list);
            for (int i = 0; i < list.Count; i++)
            {
                if (("\"" + list[i].WifiName + "\"").Equals(wifi.SSID))
                {
                    listCopy.Insert(0, list[i]);
                    listCopy.RemoveAt(i + 1);
                    listCopy[0].State = "已连接";
                }
            }
            list.Clear();
            list.AddRange(listCopy);
        }

        /// <summary>
        /// 去除同名WIFI
        /// </summary>
        /// <param name="oldSr">需要去除同名的列表</param>
        /// <returns>返回不包含同命的列表</returns>
        public static List<ScanResult> NoSameName(List<ScanResult> oldSr)
        {
            List<ScanResult> newSr = new List<ScanResult>();
            foreach (var result in oldSr)
            {
                if (!string.IsNullOrEmpty(result.Ssid) && !ContainName(newSr, result.Ssid))
                    newSr.Add(result);
            }
            return newSr;
        }

        /// <summary>
        /// 判断一个扫描结果中，是否包含了某个名称的WIFI
        /// </summary>
        /// <param name="sr">扫描结果</param>
        /// <param name="name">要查询的名称</param>
        /// <returns>返回true表示包含了该名称的WIFI，返回false表示不包含</returns>
        public static bool ContainName(List<ScanResult> sr, string name)
        {
            foreach (ScanResult result in sr)
            {
                if (!string.IsNullOrEmpty(result.Ssid) && result.Ssid.Equals(name))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取正在连接中的WIFI信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static WifiInfo GetConnectingWifiInfo(Context context)
        {
            WifiManager wifimanager = (WifiManager)context.ApplicationContext.GetSystemService(Context.WifiService);

            WifiInfo wifiinfo = wifimanager.ConnectionInfo;

            return wifiinfo;
        }

        /// <summary>
        /// 断开wifi连接
        /// </summary>
        /// <param name="context"></param>
        public static bool CutTheConnect(Context context, int? wifiId)
        {
            WifiManager wifimanager = (WifiManager)context.ApplicationContext.GetSystemService(Context.WifiService);
            if (wifiId != null && wifiId > 0)
                return wifimanager.DisableNetwork(Convert.ToInt32(wifiId));
            return false;
        }

        /// <summary>
        /// 从以保存WIFI列表中删除
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fqnq"></param>
        public static void RemoveSaveingWIFI(Context context, int netId)
        {
            var wifimanager = (WifiManager)context.ApplicationContext.GetSystemService(Context.WifiService);
            wifimanager.RemoveNetwork(netId);
        }


        /// <summary>
        /// 返回wifi强度
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetLevel(int level)
        {
            if (Java.Lang.Math.Abs(level) < 50)
                return 1;
            else if (Java.Lang.Math.Abs(level) < 75)
                return 2;
            else if (Java.Lang.Math.Abs(level) < 90)
                return 3;
            else
                return 4;
        }


        public class WifiBean
        {
            /// <summary>
            /// WIFI名称
            /// </summary>
            public string WifiName { get; set; }
            /// <summary>
            /// 是否是连接过的wifi
            /// </summary>
            public bool IsSaveing { get; set; }
            /// <summary>
            /// WIFI强度
            /// </summary>
            public int Level { get; set; }
            /// <summary>
            /// 已连接 正在连接 未连接 三种状态
            /// </summary>
            public string State { get; set; }
            /// <summary>
            /// 加密方式
            /// </summary>
            public string Capabiliteies { get; set; }

        }
    }
}