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

namespace IdoMaster_GensouWorld.Listeners
{
    /// <summary>
    /// Asakura播放器回调
    /// </summary>
    public class KsyAzEventCallBack : Java.Lang.Object, IAzErrorEventListener
    {
        /// <summary>
        /// 缓冲更新
        /// </summary>
        public Action<int> Act_OnBufferingUpdate;

        /// <summary>
        /// 视频宽高发生变化时会有此回调，并告知最新的视频宽高
        /// 该回调主要在以下情况会出现：
        /// 视频开播
        /// 播放过程中视频宽高发生变化
        ///播放器在解码后发现视频的宽高发生变化，会发起此回调
        ///对于个别特殊视频，在OnPreparedListener回调里获取的视频宽高为0，但是在解码之后即可获取视频的真实宽高，就会发起此回调
        /// </summary>
        public Action<int, int, int, int> Act_OnVideoSizeChanged;

        /// <summary>
        /// 该监听器定义于IMediaPlayer
        /// 其功能是监听在播放器播放完成时发出的onCompletion回调
        /// </summary>
        public Action Act_OnCompletion;

        /// <summary>
        /// 该监听器定义于IMediaPlayer
        ///   其功能是在播放器准备完成，可以开播时发出onPrepared回调，用户必须设置该监听器
        ///   正常情况下, 在收到此回调之后即可获取视频的宽高等信息, 但有多媒体文件在封装时存在问
        /// 题，导致在此回调接口里获取的视频宽高为0
        /// 解决此问题的方法为设置 OnVideoSizeChangedListener 监听器，该监听器会将视频真实宽高
        /// 告知用户
        /// </summary>
        public Action Act_OnPrepared;

        /// <summary>
        /// 进度完成
        /// </summary>
        public Action Act_OnSeekComplete;

        /// <summary>
        /// 错误
        /// </summary>
        public Func<int, int, bool> Fuc_OnError;

        /// <summary>
        /// 该监听器定义于IMediaPlayer
        /// 其功能是监听在播放器发出的消息通知onInfo回调，下面简要介绍较为重要的消息通知
        /// 1:未指定的播放器信息
        /// 3:视频开始渲染
        /// 700:视频复杂，解码器效率不足
        /// 701:播放器开始缓存数据
        /// 702:播放器缓存完毕
        /// 800:视频封装有误
        /// 801:此视频不能seek
        /// 802:已获得新的元数据
        /// 901:不支持此字幕
        /// 902:读取字幕超时
        /// 10001:视频方向改变
        /// 10002:音频开始播放
        /// 40020:建议reload
        /// 41000:播放器使用硬解播放视频
        /// 41001:播放器使用软解播放视频
        /// 50001:reload成功
        /// </summary>
        public Func<int, int, bool> Fuc_OnInfo;

        public KsyAzEventCallBack() { }

        public KsyAzEventCallBack(Func<int, int, bool> Fuc_OnError, Func<int, int, bool> Fuc_OnInfo)
        {
            this.Fuc_OnError = Fuc_OnError;
            this.Fuc_OnInfo = Fuc_OnInfo;
        }

        public void OnBufferingUpdate(int p0)
        {
            Act_OnBufferingUpdate?.Invoke(p0);
        }

        public void OnCompletion()
        {
            Act_OnCompletion?.Invoke();
        }

        public bool OnError(int p0, int p1)
        {
            return Fuc_OnError(p0, p1);
        }

        public bool OnInfo(int p0, int p1)
        {
            return Fuc_OnInfo(p0, p1);
        }

        public void OnPrepared()
        {
            Act_OnPrepared?.Invoke();
        }

        public void OnSeekComplete()
        {
            Act_OnSeekComplete?.Invoke();
        }

        public void OnVideoSizeChanged(int p0, int p1, int p2, int p3)
        {
            Act_OnVideoSizeChanged?.Invoke(p0, p1, p2, p3);
        }
    }
}