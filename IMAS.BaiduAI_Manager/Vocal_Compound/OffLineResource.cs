using Android.Content;
using Android.Content.Res;
using IdoMaster_GensouWorld;
using IMAS.Utils.Files;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.BaiduAI.Vocal_Compound
{
    /// <summary>
    /// 离线资源控制类
    /// </summary>
    public class OffLineResource
    {
        /// <summary>
        /// 语音声音模型
        /// </summary>
        public enum OfflineVocalType
        {
            VOICE_MALE,
            VOICE_FEMALE,
            VOICE_Duxiaoyao,
            VOICE_Duyaya,
        }

        private Context mContext;
        private AssetManager mAssetManager;
        private string textFilename;
        private string modelFilename;

        public OffLineResource(Context context, OfflineVocalType type)
        {
            mContext = context;
            mAssetManager = context.Assets;
            SetOfflineVocalType(type);
        }

        /// <summary>
        /// 获取文本文件
        /// </summary>
        /// <returns></returns>
        public string GetTextFileName()
        {
            return textFilename;
        }

        public string GetModelFileName()
        {
            return modelFilename;
        }

        public void SetOfflineVocalType(OfflineVocalType type)
        {
            string text = "bd_etts_ch_text.dat";
            string model = "";
            switch (type)
            {
                case OfflineVocalType.VOICE_MALE:
                    {
                        model = "bd_etts_ch_speech_male.dat";
                    }
                    break;
                case OfflineVocalType.VOICE_FEMALE:
                    {
                        model = "bd_etts_ch_speech_female.dat";
                    }
                    break;
                case OfflineVocalType.VOICE_Duxiaoyao:
                    {
                        throw new Exception("その声,一時的には支持しない");
                    }
                case OfflineVocalType.VOICE_Duyaya:
                    {
                        throw new Exception("その声,一時的には支持しない");
                    }
                default:
                    throw new Exception("その声,一時的には支持しない");
            }
            var destPath = $"{FilePathManager.GetInstance().GetPrivateRootDirPath()}/{IMAS_Constants.FileDir}";
            var b = FilePathManager.GetInstance().CreateDir(destPath);
            if (!b)
            {
                throw new Exception("文件初始化失败");
            }
            textFilename = $"{destPath}/{text}";
            //textFilename = $"/storage/sdcard0/baiduTTS/{text}";
            FileUtils.CopyAssetsApkToSDCard(mContext, text, textFilename);
            modelFilename = $"{destPath}/{model}";
            //modelFilename = $"/storage/sdcard0/baiduTTS/{model}"; 
            FileUtils.CopyAssetsApkToSDCard(mContext, model, modelFilename);

        }
    }
}
