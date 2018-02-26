using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Baidu.Aip.Ocr;
using IMAS.CupCake.Extensions;

namespace IdoMaster_GensouWorld.Utils
{
    /// <summary>
    /// 百度语音合成类
    /// </summary>
    public class BaiduVocalUtils
    {
        private const string APP_ID = "10855003";
        private const string API_KEY = "0B26o8PFdezwwS5Dhrq0FbGj";
        private const string SECRET_KEY = "IjGlhwoK0M4QnBjtuzzix18qdH8D6aDQ ";
        /// <summary>
        /// 百度AI客户端
        /// </summary>
        private Ocr aiClient;

        #region 单例
        public BaiduVocalUtils _Kagemusha;

        public BaiduVocalUtils GetKagemusha()
        {
            if (_Kagemusha == null)
            {
                aiClient = new Ocr(API_KEY, SECRET_KEY);
                _Kagemusha = this;
            }
            return _Kagemusha;
        }

        #endregion


    }
}