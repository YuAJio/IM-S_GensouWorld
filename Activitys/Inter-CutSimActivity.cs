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
using Com.Ksyun.Media.Player;
using Com.Nostra13.Universalimageloader.Core;
using IdoMaster_GensouWorld.Listeners;
using IdoMaster_GensouWorld.Utils;
using IMAS.OkHttp.Download;
using IMAS.Utils.Sp;

namespace IdoMaster_GensouWorld.Activitys
{
    /// <summary>
    /// 插播类型
    /// </summary>
    public enum Cut_In_ShowType
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image = 0x101,
        /// <summary>
        /// 视频
        /// </summary>
        Video = 0x201,
    }

    /// <summary>
    /// 插播播放页面
    /// </summary>
    [Activity(Label = "Inter_CutSimActivity", Theme = "@android:style/Theme.Translucent.NoTitleBar.Fullscreen")
        , IntentFilter(actions: new string[] { "Inter_CutSimAction" })]
    public class Inter_CutSimActivity : Activity
    {
        /// <summary>
        /// Intent标识 插播内容
        /// </summary>
        public const string IntentFlag_Cut_In_Type = "Cut_In_Type";

        /// <summary>
        /// 是否还活着
        /// </summary>
        public bool IsAlive { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            IMAS_Application.Sington.OpenActivityList.Add(this);
            IsAlive = true;

            var type = Intent.GetIntExtra(IntentFlag_Cut_In_Type, -1);
            if (type == -1)
            {
                this.Finish();
                return;
            }

            switch ((Cut_In_ShowType)type)
            {
                case Cut_In_ShowType.Image:
                    {
                        CraeteImageCutInLayout();
                    }
                    break;
                case Cut_In_ShowType.Video:
                    {
                        CreateVideoCutInLayou();
                        PlayVideo();
                    }
                    break;
                default:
                    this.Finish();
                    break;
            }
        }

        #region 图片插播任务相关
        /// <summary>
        /// 创建图片插播任务
        /// </summary>
        private void CraeteImageCutInLayout()
        {
            var imageView = new ImageView(this)
            {
                LayoutParameters = GetFullScreenLP(),
            };
            imageView.SetScaleType(ImageView.ScaleType.CenterCrop);

            var picPath = "http://anihonetwallpaper.com/image/2017/04/40184-GRANBLUE_FANTASY-PC.jpg";
            ImageLoader.Instance.DisplayImage(picPath, imageView, ImageLoaderHelper.CharacterPicImageOption());

            this.SetContentView(imageView);
        }
        #endregion

        #region 视频播放相关
        /// <summary>
        /// 视频播放地址
        /// </summary>
        private string V_PlyaerUrl = "";
        /// <summary>
        /// 视频载体控件
        /// </summary>
        private FrameLayout fl_v_father;


        /// <summary>
        /// 创建视频插播任务
        /// </summary>
        private void CreateVideoCutInLayou()
        {
            fl_v_father = new FrameLayout(this)
            {
                LayoutParameters = GetFullScreenLP()
            };
            fl_v_father.SetBackgroundColor(Android.Graphics.Color.Transparent);

            this.SetContentView(fl_v_father);

        }


        #region 播放器相关
        /// <summary>
        /// 播放器主体
        /// </summary>
        private KSYTextureView mTextureView;
        /// <summary>
        /// 是否循环播放
        /// </summary>
        private bool isLoopPlay = false;
        private bool isPlaySound = false;
        /// <summary>
        /// 初始化金山云播放器
        /// </summary>
        private void InitKsyPlayer()
        {
            if (mTextureView == null)
            {
                mTextureView = new KSYTextureView(this);
                mTextureView.LayoutParameters = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.MatchParent);
                //mTextureView.SetBackgroundColor(Color.Black);
                //设置一些控件属性
                mTextureView.KeepScreenOn = true;
                mTextureView.BufferTimeMax = 2;
                mTextureView.Looping = isLoopPlay;
                mTextureView.SetBufferSize(15);
                mTextureView.SetTimeout(5, 30);//参数1：准备超时时间，参数二：读取超时时间
                mTextureView.SetScreenOnWhilePlaying(true);
                //mTextureView.SetBackgroundResource(Resource.Drawable.defaultimg);
                mTextureView.SetVideoScalingMode(0);//0:no; 1:VIDEO_SCALING_MODE_SCALE_TO_FIT; 2:VIDEO_SCALING_MODE_SCALE_TO_FIT_WITH_CROPPING
                fl_v_father.AddView(mTextureView);
                KSYTextureViewManager.Instance.SetHardWareDecodeMode(mTextureView);//设置硬解码
                KSYTextureViewManager.Instance.SetAzEventListener(mTextureView, imp_KsyIroIro);
            }
        }

        /// <summary>
        /// 播放视频
        /// </summary>
        private void PlayVideo()
        {
            var playSource = "http://f.us.sinaimg.cn/000xj5DPlx07rQGHDFPq01041200cnn70E010.mp4?label=mp4_hd&template=852x480.28.0&Expires=1554195794&ssig=Hc10jOU%2BCB&KID=unistore,video";
            CreateKsyImp();
            ReleaseKsyPlayer();
            InitKsyPlayer();
            if (IMAS.Utils.Files.FilePathManager.GetInstance().CheckFileExist($"{IMAS.Utils.Files.FilePathManager.GetInstance().GetPrivateRootDirPath()}/dVideo/cacheDVideo.mp4"))
                mTextureView.DataSource = $"{IMAS.Utils.Files.FilePathManager.GetInstance().GetPrivateRootDirPath()}/dVideo/cacheDVideo.mp4";
            else
            {
                CreateProgressDownloader(playSource);
                mTextureView.DataSource = playSource;
            }
            mTextureView.PrepareAsync();
        }

        /// <summary>
        /// 释放播放控件
        /// </summary>
        private void ReleaseKsyPlayer()
        {
            if (mTextureView != null)
            {
                mTextureView?.Release();
                fl_v_father?.RemoveView(mTextureView);
                mTextureView = null;
            }
        }

        #region 回调
        private KsyAzEventCallBack imp_KsyIroIro;
        private void CreateKsyImp()
        {
            if (imp_KsyIroIro == null)
            {
                imp_KsyIroIro = new KsyAzEventCallBack();
                imp_KsyIroIro.Act_OnCompletion += OnComplete;
                imp_KsyIroIro.Act_OnSeekComplete += OnSeekComplete;
            }
            else
            {
                imp_KsyIroIro.Act_OnCompletion -= OnComplete;
                imp_KsyIroIro.Act_OnSeekComplete -= OnSeekComplete;
                imp_KsyIroIro = null;
                CreateKsyImp();
            }
        }

        private void OnComplete()
        {
            //如果不是循环播放,视频东西播放完毕之后
            if (!isLoopPlay)
                this.Finish();
        }

        private void OnSeekComplete()
        {
            //如果是循环播放,视频东西播放完毕之后
            if (isLoopPlay)
            {

            }
        }

        #endregion


        #endregion

        #endregion

        private ViewGroup.LayoutParams GetFullScreenLP()
        {
            var lp = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            return lp;
        }

        #region 下载监听
        /// <summary>
        /// 总长度
        /// </summary>
        private long mContentLength;
        /// <summary>
        /// 总大小
        /// </summary>
        private long mTotalBytes;
        /// <summary>
        /// 下载断点的大小 
        /// </summary>
        private long mBreakPoints = 0L;

        /// <summary>
        /// 下载文件名称
        /// </summary>
        private string mFileName;
        ///// <summary>
        ///// 文件后缀名
        ///// </summary>
        //private string mLastName;
        /// <summary>
        /// 下载文件临时文件名称
        /// </summary>
        private string mTempFileName;
        /// <summary>
        /// 下载器
        /// </summary>
        private ProgressDownloader downloader;

        /// <summary>
        /// 记录上次下载时间
        /// </summary>
        private DateTime downloadLastTime = DateTime.Now;
        /// <summary>
        /// 下载的文件路径
        /// </summary>
        private string mDownloadUrl;

        private DownloadProgressImp downloadProgressImp;

        /// <summary>
        /// 创建下载器
        /// </summary>
        private void CreateProgressDownloader(string url)
        {
            mDownloadUrl = $"{url}";
            mFileName = "cacheDVideo";
            mTempFileName = mFileName + ".tmp";
            var localFilePath = $"{IMAS.Utils.Files.FilePathManager.GetInstance().GetPrivateRootDirPath()}/dVideo";
            IMAS.Utils.Files.FilePathManager.GetInstance().CreateDir(localFilePath);
            downloadProgressImp = new DownloadProgressImp();
            downloadProgressImp.OnPreExeuteAct += OnPreExecute;
            downloadProgressImp.UpdateAct += Update;
            downloadProgressImp.UpdateErrorAct += UpdateError;
            //downloader = new ProgressDownloader(mContext, CVTClientApplication.isLoadLocalKey, mDownloadUrl, $"{FilePathManager.Singletone.GetDownloadDir()}/{mTempFileName}", this);
            downloader = new ProgressDownloader(mDownloadUrl, $"{localFilePath}/{mTempFileName}", downloadProgressImp);
            mBreakPoints = GetBreakpointLast();
            downloader.Download(mBreakPoints);


        }


        /// <summary>
        /// 获取下载的断点
        /// </summary>
        /// <returns></returns>
        private long GetBreakpointLast()
        {
            return AndroidPreferenceProvider.GetInstance().GetLong(mFileName);
        }

        /// <summary>
        /// baocun
        /// </summary>
        public void SaveBreakpointLast()
        {
            AndroidPreferenceProvider.GetInstance().PutLong(mFileName, mTotalBytes);
        }


        /// <summary>
        /// 删除当前数据项
        /// </summary>
        public void RemoveBreakpoint()
        {
            AndroidPreferenceProvider.GetInstance().Remove(mFileName);
        }

        /// <summary>
        /// 改变文件名称
        /// </summary>
        /// <returns></returns>
        private bool ChangeFileName()
        {
            try
            {
                Java.IO.File srcDir = new Java.IO.File($"{IMAS.Utils.Files.FilePathManager.GetInstance().GetPrivateRootDirPath()}/dVideo/{mTempFileName}");
                bool isOk = srcDir.RenameTo(new Java.IO.File($"{IMAS.Utils.Files.FilePathManager.GetInstance().GetPrivateRootDirPath()}/dVideo/{mFileName}.mp4"));
                return isOk;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }


        private void OnPreExecute(long contentLength)
        {
            if (mContentLength == 0L)
            {
                mContentLength = contentLength + mBreakPoints;
            }
            this.RunOnUiThread(() =>
            {
                var persent = ((mBreakPoints) * 1.0f / this.mContentLength);
            });
        }

        /// <summary>
        /// 下载进度更新回调
        /// </summary>
        /// <param name="totalBytes"></param>
        /// <param name="done"></param>
        private void Update(long totalBytes, bool done)
        {
            //mTotalBytes = totalBytes + mBreakPoints;
            //this.RunOnUiThread(() =>
            //{
            //    var persent = ((mTotalBytes) * 1.0f / this.mContentLength);
            //});


            if (done)
            {
                downloader.Pause();//停止下载
                RemoveBreakpoint();

                //改变文件名称
                ChangeFileName();

                this.RunOnUiThread(() =>
                {
                    //显示说明下载完成
                    Toast.MakeText(this, "缓存完成", ToastLength.Short).Show();

                });
                return;
            }
            if ((DateTime.Now - downloadLastTime) > TimeSpan.FromSeconds(10))//10秒
            {
                downloadLastTime = DateTime.Now;
                SaveBreakpointLast();
            }
        }

        /// <summary>
        /// 下载错误回调
        /// </summary>
        private void UpdateError()
        {
            downloader?.Pause();
            downloader = null;
            if (!string.IsNullOrWhiteSpace(mDownloadUrl))
            {
                CreateProgressDownloader(mDownloadUrl);
            }
            else
            {
                this.RunOnUiThread(() =>
                {
                    PlayVideo();
                });
            }
        }
        private class DownloadProgressImp : IProgressListener
        {
            public Action<long> OnPreExeuteAct { get; set; }
            public Action<long, bool> UpdateAct { get; set; }
            public Action UpdateErrorAct { get; set; }
            public void OnPreExecute(ProgressDownloader sender, long contentLength)
            {
                OnPreExeuteAct?.Invoke(contentLength);
            }

            public void Update(ProgressDownloader sender, long totalBytes, bool done)
            {
                UpdateAct?.Invoke(totalBytes, done);
            }

            public void UpdateError(ProgressDownloader sender, string errorMsg)
            {
                UpdateErrorAct?.Invoke();
            }
        }
        #endregion

        #region 重写基类方法
        protected override void OnDestroy()
        {
            IsAlive = false;
            IMAS_Application.Sington.OpenActivityList.Remove(this);
            base.OnDestroy();
        }
        #endregion
    }
}