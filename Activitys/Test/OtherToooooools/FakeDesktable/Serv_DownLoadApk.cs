//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using IMAS.OkHttp.Download;
//using IMAS.Tips.Logic.LocalDBManager;
//using IMAS.Utils.Files;

//namespace IdoMaster_GensouWorld.Activitys.Test.OtherToooooools.FakeDesktable
//{
//    /// <summary>
//    /// APK下载服务
//    /// </summary>
//    public class Serv_DownLoadApk : Service
//    {
//        private IProgressListener progressListener;

//        public Serv_DownLoadApk(IProgressListener listener)
//        {
//            this.progressListener = listener;
//        }

//        public override IBinder OnBind(Intent intent)
//        {
//            return null;
//        }

//        [return: GeneratedEnum]
//        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
//        {
//            var url = intent?.GetStringExtra("url");
//            StartDownLoad(url);
//            return base.OnStartCommand(intent, flags, startId);
//        }

//        /// <summary>
//        /// 开始下载APK
//        /// </summary>
//        /// <param name="url"></param>
//        private void StartDownLoad(string url)
//        {
//            var kk = url.Substring(url.LastIndexOf('/') + 1);
//            var filePath = $"{FilePathManager.GetInstance().GetPrivateRootDirPath()}/DownLoadApk/{kk}";
//            var tempFilepath = $"{filePath}.tmp";
//            new ProgressDownloader(url, tempFilepath, progressListener).Download(0);
//        }

//    }
//}