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
using Android.Media;
using IMAS.CupCake.Extensions;

namespace IdoMaster_GensouWorld.Utils
{
    public enum SE_Enumeration
    {
        [EnumDescription("错误音声")]
        Error_Code = -1,
        /// <summary>
        /// 钱声
        /// </summary>
        [EnumDescription("钱声")]
        SE_Money = 001,
        /// <summary>
        /// 真由氏嘟嘟噜
        /// </summary>
        [EnumDescription("嘟嘟噜")]
        SE_Duduru = 002,
        /// <summary>
        /// 微信摇一摇
        /// </summary>
        [EnumDescription("摇一摇")]
        SE_Shake = 003,
    }
    /// <summary>
    /// 音效播放类
    /// </summary>
    public class SoundEffectPlayer
    {
        public static SoundPool mSoundPlayer = new SoundPool(10, Stream.System, 5);
        public static SoundEffectPlayer sePlayer;

        static Context mContext;

        public static SoundEffectPlayer Init(Context context)
        {
            if (sePlayer == null)
            {
                sePlayer = new SoundEffectPlayer();
            }

            mContext = context;

            mSoundPlayer.Load(mContext, Resource.Raw.se_money, (int)SE_Enumeration.SE_Money);
            mSoundPlayer.Load(mContext, Resource.Raw.se_duduru, (int)SE_Enumeration.SE_Duduru);
            mSoundPlayer.Load(mContext, Resource.Raw.se_shake, (int)SE_Enumeration.SE_Shake);

            return sePlayer;

        }

        public static void Play(SE_Enumeration soundId)
        {
            mSoundPlayer.Play((int)soundId, 1, 1, 0, 0, 1);
        }

    }
}