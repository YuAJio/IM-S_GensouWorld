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

namespace IdoMaster_GensouWorld
{
    public class IMAS_Constants
    {
        #region 页面跳转ResultCode
        /// <summary>
        /// ActivityResult刷新requestCode
        /// </summary>
        public const int OnPageRefreshKey = 0x01;
        #endregion

        #region 页面跳转专递信息Key
        /// <summary>
        /// 商店页面标识
        /// </summary>
        public const string ShopPageFlagKey = "ShopPageFlag";
        /// <summary>
        /// 地图Id标识
        /// </summary>
        public const string MapPagePkIdKey = "MapPkId";
        /// <summary>
        /// 角色等级
        /// </summary>
        public const string CharacterLevelKey = "CharacterLevel";
        #endregion

        #region 本地文件系统相关常量
        /// <summary>
        /// sdcard 根目录
        /// </summary>
        public const string SDCardRoot = "XCCloudSupplier";
        /// <summary>
        /// 图片文件夹名称
        /// </summary>
        public const string ImageDir = "Images";
        /// <summary>
        /// 普通文件夹名称
        /// </summary>
        public const string FileDir = "Files";
        /// <summary>
        /// 下载文件夹名称
        /// </summary>
        public const string DownloadDir = "Downloads";
        /// <summary>
        /// 库包文件夹名称
        /// </summary>
        public const string LibDir = "Libs";
        /// <summary>
        /// 缓存文件夹名称
        /// </summary>
        public const string CacheDir = "Caches";
        /// <summary>
        /// 视频文件目录名称
        /// </summary>
        public const string VideoDir = "Videos";
        /// <summary>
        /// 数据库文件目录名称
        /// </summary>
        public const string DBDir = "DB";
        #endregion

        #region Log相关常量
        /// <summary>
        /// 数据库逻辑错误
        /// </summary>
        public const string Log_SQLiteErrorKey = "SQLiteError";
        #endregion

        #region Sp常量
        /// <summary>
        /// Sp文件名称 根目录
        /// </summary>
        public const string SpFileName = "Idol_Master_Pro_Sp";
        /// <summary>
        /// Sp制作人Id
        /// </summary>
        public const string SpProducerInfoId = "ProducerInfo_Id";
        /// <summary>
        /// Sp制作人Name
        /// </summary>
        public const string SpProducerInfoName = "ProducerInfo_Name";
        /// <summary>
        /// App版本Key
        /// </summary>
        public const string SpVersionKey = "VersionKey";

        #endregion

        #region 网络资源网址
        #region 背景图片
        /// <summary>
        /// 街景
        /// </summary>
        public const string BackGround_Pic_Street = "http://img2.imgtn.bdimg.com/it/u=423512483,2084224215&fm=27&gp=0.jpg";
        /// <summary>
        /// 事务所外
        /// </summary>
        public const string BackGround_Pic_Building_Out = "https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1512389134324&di=369de01c165725af180572919dd207bd&imgtype=0&src=http%3A%2F%2Fupload.shunwang.com%2F2013%2F0923%2F1379922303343.jpg";
        #endregion
        #region 角色立绘
        /// <summary>
        /// 测试角色立绘
        /// </summary>
        public const string Tachie_Pic_Test = "http://img2.imgtn.bdimg.com/it/u=3532641803,3750388261&fm=27&gp=0.jpg";
        #endregion
        #region 怪物立绘
        public const string Monster_Pic_Slime = "http://img5.imgtn.bdimg.com/it/u=3446648512,1507889903&fm=27&gp=0.jpg";
        public const string Monster_Pic_Fox = "https://ss2.bdstatic.com/70cFvnSh_Q1YnxGkpoWK1HF6hhy/it/u=2363898146,2947160386&fm=200&gp=0.jpg";
        public const string Monster_Pic_Wolf = "http://img4.imgtn.bdimg.com/it/u=4150932689,1425033513&fm=200&gp=0.jpg";
        #endregion

        #endregion

        #region 全局存储常量
        #region 名字与ID
        #region 角色名字与ID
        public static string Producer_Name;
        public const long Producer_Name_ID = 765;
        public const string AMAMI_HARUKA = "天海春香";
        public const long AMAMI_HARUKA_ID = 1;
        public const string KISARAGI_CHIHAYA = "如月千早";
        public const long KISARAGI_CHIHAYA_ID = 72;
        public const string HOSHII_MIKI = "星井美希";
        public const long HOSHII_MIKI_ID = 3;
        public const string KIKUCHI_MAKOTO = "菊地真";
        public const long KIKUCHI_MAKOTO_ID = 5;
        public const string HAGIWARA_YUKIHO = "萩原雪歩";
        public const long HAGIWARA_YUKIHO_ID = 82;
        public const string FUTAMI_MAMI = "双海真美";
        public const long FUTAMI_MAMI_ID = 69;
        public const string FITAMI_AMI = "双海亜美";
        public const long FITAMI_AMI_ID = 96;
        public const string MINASE_IORI = "水瀬伊織";
        public const long MINASE_IORI_ID = 160;
        public const string TAKATSUKI_YAYOI = "高槻やよい";
        public const long TAKATSUKI_YAYOI_ID = 861;
        public const string SHIJOU_TAKANE = "四条貴音";
        public const long SHIJOU_TAKANE_ID = 49;
        public const string MIURA_AZUSA = "三浦あずさ";
        public const long MIURA_AZUSA_ID = 333;
        public const string AKITSUKI_RITSUKO = "秋月律子";
        public const long AKITSUKI_RITSUKO_ID = 22;
        #endregion
        #region 地图名字与ID
        public const string MAP_YURI_NAME = "百合樹　リリィウッド";
        public const int MAP_YURI_ID = 0x01;
        public const string MAP_WINTER_NAME = "冬季　ウィンターローズ";
        public const int MAP_WINTER_ID = 0x02;
        public const string MAP_BANANA_NAME = "香蕉 バナナオーシャン　海洋";
        public const int MAP_BANANA_ID = 0x03;
        #endregion
        #region 怪物名字与ID
        public const string Monster_Name_Slime = "スライム";
        public const long Monster_Id_Slime = 0x1001;

        public const string Monster_Name_Fox = "きつね";
        public const long Monster_Id_Fox = 0x1002;

        public const string Monster_Name_Wolf = "オオカミ";
        public const long Monster_Id_Wolf = 0x1003;
        #endregion
        #endregion
        #region 事件代号
        /// <summary>
        /// 对话进行
        /// </summary>
        public const int EVENT_CHAT_GO_ON = 0x010;
        #endregion
        #endregion
    }
}