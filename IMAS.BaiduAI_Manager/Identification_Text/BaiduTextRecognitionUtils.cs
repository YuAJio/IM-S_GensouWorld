using Baidu.Aip.Ocr;
using IMAS.Utils.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IMAS.BaiduAI.Identification_Text
{
    /// <summary>
    /// 百度文字识别类
    /// </summary>
    public class BaiduTextRecognitionUtils
    {
        private const string APP_ID = "10855003";
        private const string API_KEY = "0B26o8PFdezwwS5Dhrq0FbGj";
        private const string SECRET_KEY = "IjGlhwoK0M4QnBjtuzzix18qdH8D6aDQ ";
        /// <summary>
        /// 百度AI客户端
        /// </summary>
        private Ocr aiClient;

        #region 单例
        private static BaiduTextRecognitionUtils _Kagemusha;

        public BaiduTextRecognitionUtils()
        {
            if (aiClient == null)
            {
                aiClient = new Ocr(API_KEY, SECRET_KEY);
            }
        }
        public static BaiduTextRecognitionUtils GetKagemusha()
        {
            if (_Kagemusha == null)
            {
                _Kagemusha = new BaiduTextRecognitionUtils(); ;
            }
            return _Kagemusha;
        }
        #endregion

        /// <summary>
        /// 身份证识别
        /// </summary>
        /// <param name="picPath">图片地址</param>
        /// <param name="lightOrDark">0:反面,1:正面</param>
        /// <returns>结果Json</returns>
        public Newtonsoft.Json.Linq.JObject IdCard(string picPath, int lightOrDark)
        {
            if (!FilePathManager.GetInstance().CheckFileExist(picPath))
            {
                return null;
            }
            var image = File.ReadAllBytes(picPath);
            var idCardSide = lightOrDark == 0 ? "back" : "front";

            try
            {
                //如果有可选参数
                var option = new Dictionary<string, object>
                    {
                     //是否检测图像朝向，默认不检测，即：false。朝向是指输入图像是正常方向、逆时针旋转90/180/270度。可选值包括:               
                     //- true：检测朝向；                
                     //- false：不检测朝向。
                     {"detect_direction","true" },
                     //是否开启身份证风险类型(身份证复印件、临时身份证、身份证翻拍、修改过的身份证)功能，默认不开启，即：false。可选值:   
                     //true - 开启；   
                     //false - 不开启  
                     { "detect_risk", "false"}
                    };
                var result = aiClient.Idcard(image, idCardSide, option);

                //若没有可选参数
                // var result = aiClient.Idcard(image, idCardSide);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }


    }
}
