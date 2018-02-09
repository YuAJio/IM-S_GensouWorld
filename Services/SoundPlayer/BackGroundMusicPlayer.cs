
using System;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;

namespace IdoMaster_GensouWorld.Services
{
    public enum BGM_Enumeration
    {
        Error_Code = -1,
        /// <summary>
        /// 主题曲
        /// </summary>
        Main_BGM = 001
    }

    /// <summary>
    /// 播放背景音乐的服务
    /// </summary>
    [Service(Name = "com.yurishi.imas_protable.BackGroundMusicPlayer")]
    public class BackGroundMusicPlayer : Service, MediaPlayer.IOnCompletionListener
    {
        public const string MusicSelectKey = "Music_Id";
        private MediaPlayer player;

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {

            var music_Id = intent.GetIntExtra(BackGroundMusicPlayer.MusicSelectKey, -1);
            var enum_MusicId = (BGM_Enumeration)music_Id;

            switch (enum_MusicId)
            {
                case BGM_Enumeration.Error_Code:

                    return base.OnStartCommand(intent, flags, startId);
                case BGM_Enumeration.Main_BGM:
                    player = MediaPlayer.Create(this, Resource.Raw.bgm_main);
                    break;
            }
            if (player != null)
            {
                player.SetOnCompletionListener(this);
                player.Start();
            }
            return base.OnStartCommand(intent, flags, startId);
        }
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        /// <summary>
        /// BGM播放完成后的操作
        /// </summary>
        /// <param name="mp"></param>
        public void OnCompletion(MediaPlayer mp)
        {
            mp.Start();
        }

        #region 生命周期
        public override void OnDestroy()
        {
            base.OnDestroy();
            if (player != null)
            {
                if (player.IsPlaying)
                {
                    player.Stop();
                }
                player.Release();
            }
        }
        #endregion
    }
}