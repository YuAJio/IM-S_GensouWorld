using Android.Content;
using Android.OS;
using Baidu.Aip.Ocr;
using Com.Baidu.Tts.Client;
using IdoMaster_GensouWorld;
using IMAS.Utils.Sp;
using System;
using System.Collections.Generic;
using System.Text;
using static IMAS.BaiduAI.Vocal_Compound.OffLineResource;

namespace IMAS.BaiduAI.Vocal_Compound
{
    /// <summary>
    /// 百度语音合成类
    /// </summary>
    public class BaiduVocalManager
    {
        private const string APP_ID = "10855003";
        private const string API_KEY = "0B26o8PFdezwwS5Dhrq0FbGj";
        private const string SECRET_KEY = "IjGlhwoK0M4QnBjtuzzix18qdH8D6aDQ ";


        #region 单例
        private static BaiduVocalManager _Kagemusha;
        private BaiduVocalManager() { }

        public static BaiduVocalManager GetKagemusha()
        {
            if (_Kagemusha == null)
            {
                _Kagemusha = new BaiduVocalManager(); ;
            }
            return _Kagemusha;
        }

        #endregion

        /// <summary>
        /// TtsMode.MIX; 离在线融合，在线优先； TtsMode.ONLINE 纯在线； 没有纯离线
        /// </summary>
        private TtsMode mTtsMode = TtsMode.Mix;

        private OfflineVocalType offlineVoice = OfflineVocalType.VOICE_FEMALE;
        /// <summary>
        /// 离线资源文件
        /// </summary>
        private OffLineResource mOfflineResource;
        /// <summary>
        /// 语音合成控制对象
        /// </summary>
        private SpeechSynthesizer mSpeechSynthesizer;
        /// <summary>
        /// 主线程
        /// </summary>
        private Handler mMainHandler;
        private Context mContext;

        #region 外部调用方法

        /// <summary>
        /// 设置在线发声音人
        /// </summary>
        /// <param name="speaker">speaker : 0~4</param>
        public void SetBaiduAITtsParamSpeakerKey(int speaker)
        {
            AndroidPreferenceProvider.GetInstance().PutInt(IMAS_Constants.BaiduAITtsParamSpeakerKey, speaker);
        }
        /// <summary>
        /// 设置合成的音量
        /// </summary>
        /// <param name="volume">speaker : 0~9</param>
        public void SetBaiduAITtsParamVolumeKey(int volume)
        {
            AndroidPreferenceProvider.GetInstance().PutInt(IMAS_Constants.BaiduAITtsParamVolumeKey, volume);
        }

        /// <summary>
        /// 设置合成的语速
        /// </summary>
        /// <param name="speed">speaker : 0~9</param>
        public void SetBaiduAITtsParamSpeedKey(int speed)
        {
            AndroidPreferenceProvider.GetInstance().PutInt(IMAS_Constants.BaiduAITtsParamSpeedKey, speed);
        }

        /// <summary>
        /// 设置合成语调
        /// </summary>
        /// <param name="pitch">speaker : 0~9</param>
        public void SetBaiduAITtsParamPitchKey(int pitch)
        {
            AndroidPreferenceProvider.GetInstance().PutInt(IMAS_Constants.BaiduAITtsParamPitchKey, pitch);
        }
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="mainHandler">主线程</param>
        public void Init(Context context, Handler mainHandler)
        {
            mContext = context;
            mMainHandler = mainHandler;

            if (mTtsMode == TtsMode.Mix)
            {
                mOfflineResource = new OffLineResource(context, offlineVoice);
            }
            mSpeechSynthesizer = SpeechSynthesizer.Instance;
            mSpeechSynthesizer.SetContext(mContext);
            mSpeechSynthesizer.SetSpeechSynthesizerListener(new TtsMessageListener());

            mSpeechSynthesizer.SetAppId(APP_ID);
            mSpeechSynthesizer.SetApiKey(API_KEY, SECRET_KEY);

            SetVocalOption();

            if (mTtsMode == TtsMode.Mix)
            {
                mSpeechSynthesizer.SetParam(SpeechSynthesizer.ParamTtsTextModelFile, mOfflineResource.GetTextFileName());
                mSpeechSynthesizer.SetParam(SpeechSynthesizer.ParamTtsSpeechModelFile, mOfflineResource.GetModelFileName());
                //授权检测接口(只是通过AuthInfo进行检验授权是否成功。选择纯在线可以不必调用auth方法。
                var authInfo = mSpeechSynthesizer.Auth(mTtsMode);
                if (authInfo.IsSuccess)
                {
                    Console.Write("验证通过，离线正式授权文件存在。");

                }
                else
                {
                    Console.Write($"鉴权失败 = {authInfo.TtsError?.DetailMessage}");
                }
            }

            var rst = mSpeechSynthesizer.InitTts(mTtsMode);
            if (rst != 0)
            {
                Console.Write($"初始化失败 {rst}");
            }
            else
            {
                Console.Write($"合成引擎初始化成功");
            }
        }


        /// <summary>
        /// 设置合成语音的属性
        /// </summary>
        private void SetVocalOption()
        {
            var speaker = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.BaiduAITtsParamSpeakerKey, 1);
            if (speaker < 0 || speaker > 4)
            {
                speaker = 0;
            }
            mSpeechSynthesizer.SetParam(SpeechSynthesizer.ParamSpeaker, $"{speaker}");// 设置在线发声音人： 0 普通女声（默认） 1 普通男声 2 特别男声 3 情感男声<度逍遥> 4 情感儿童声<度丫丫>
            var volume = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.BaiduAITtsParamVolumeKey, 5);
            if (volume < 0 || volume > 9)
            {
                volume = 5;
            }
            mSpeechSynthesizer.SetParam(SpeechSynthesizer.ParamVolume, $"{volume}");// 设置合成的音量，0-9 ，默认 5
            var speed = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.BaiduAITtsParamSpeedKey, 5);
            if (speed < 0 || speed > 9)
            {
                speed = 5;
            }
            mSpeechSynthesizer.SetParam(SpeechSynthesizer.ParamSpeed, $"{speed}");// 设置合成的语速，0-9 ，默认 5
            var pitch = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.BaiduAITtsParamPitchKey, 5);
            if (pitch < 0 || pitch > 9)
            {
                pitch = 5;
            }
            mSpeechSynthesizer.SetParam(SpeechSynthesizer.ParamPitch, $"{pitch}");//设置合成的语调，0-9 ，默认 5
            /**
             * // 该参数设置为TtsMode.MIX生效。即纯在线模式不生效。
               // MIX_MODE_DEFAULT 默认 ，wifi状态下使用在线，非wifi离线。在线状态下，请求超时6s自动转离线
               // MIX_MODE_HIGH_SPEED_SYNTHESIZE_WIFI wifi状态下使用在线，非wifi离线。在线状态下， 请求超时1.2s自动转离线
               // MIX_MODE_HIGH_SPEED_NETWORK ， 3G 4G wifi状态下使用在线，其它状态离线。在线状态下，请求超时1.2s自动转离线
               // MIX_MODE_HIGH_SPEED_SYNTHESIZE, 2G 3G 4G wifi状态下使用在线，其它状态离线。在线状态下，请求超时1.2s自动转离线
             */
            mSpeechSynthesizer.SetParam(SpeechSynthesizer.ParamMixMode, SpeechSynthesizer.MixModeHighSpeedNetwork);

        }

        #region 播放控制

        /// <summary>
        /// 合成并播放
        /// </summary>
        /// <param name="text">小于1024 GBK字节，即512个汉字或者字母数字</param>
        /// <returns></returns>
        public int Speak(string text)
        {
            return mSpeechSynthesizer.Speak(text);
        }

        /// <summary>
        /// 合成并播放
        /// </summary>
        /// <param name="text">小于1024 GBK字节，即512个汉字或者字母数字</param>
        /// <param name="utteranceId">用于listener的回调，默认"0"</param>
        /// <returns></returns>
        public int Speak(string text, string utteranceId)
        {
            return mSpeechSynthesizer.Speak(text, utteranceId);
        }

        /// <summary>
        /// 批量播放
        /// </summary>
        /// <param name="dictTxts">批量数据：key：text；value：utteranceId</param>
        /// <returns></returns>
        public int BatchSpeak(Dictionary<string, string> dictTxts)
        {
            if (dictTxts == null || dictTxts.Count <= 0)
            {
                return 0;
            }

            var list = new List<SpeechSynthesizeBag>();
            foreach (var item in dictTxts)
            {
                var bag = new SpeechSynthesizeBag();
                bag.SetText(item.Key);
                bag.UtteranceId = item.Value;
                list.Add(bag);
            }

            return mSpeechSynthesizer.BatchSpeak(list);
        }

        /// <summary>
        /// 只合成不播放
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int Synthesize(string text)
        {
            return mSpeechSynthesizer.Synthesize(text);
        }

        /// <summary>
        /// 只合成不播放
        /// </summary>
        /// <param name="text"></param>
        /// <param name="utteranceId"></param>
        /// <returns></returns>
        public int Synthesize(string text, string utteranceId)
        {
            return mSpeechSynthesizer.Synthesize(text, utteranceId);
        }
        #endregion

        #region 引擎控制
        /// <summary>
        /// 暂停
        /// </summary>
        /// <returns></returns>
        public int Pause()
        {
            return mSpeechSynthesizer.Pause();
        }
        /// <summary>
        /// 重启
        /// </summary>
        /// <returns></returns>
        public int Resume()
        {
            return mSpeechSynthesizer.Resume();
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        public int Stop()
        {
            return mSpeechSynthesizer.Stop();
        }
        /// <summary>
        /// 释放对象
        /// </summary>
        public void Release()
        {
            mSpeechSynthesizer.Stop();
            mSpeechSynthesizer.Release();
            mSpeechSynthesizer = null;
        }

        #endregion

    }
}
