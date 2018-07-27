//using Com.Baidu.Tts.Client;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace IMAS.BaiduAI.Vocal_Compound
//{
//    public class TtsMessageListener : Java.Lang.Object, ISpeechSynthesizerListener
//    {
//        /// <summary>
//        /// 合成开始
//        /// </summary>
//        /// <param name="utteranceId"></param>
//        public void OnSynthesizeStart(string utteranceId)
//        {
//            Console.Write("OnSynthesizeStart utteranceId = " + utteranceId);
//        }
//        /// <summary>
//        /// 语音流 16K采样率 16bits编码 单声道 。
//        /// </summary>
//        /// <param name="utteranceId"></param>
//        /// <param name="bytes">二进制语音 ，注意可能有空data的情况，可以忽略</param>
//        /// <param name="progress">如合成“百度语音问题”这6个字， progress肯定是从0开始，到6结束。 但progress无法和合成到第几个字对应。</param>
//        public void OnSynthesizeDataArrived(string utteranceId, byte[] bytes, int progress)
//        {
//            Console.Write("OnSynthesizeDataArrived utteranceId = " + utteranceId);
//        }

//        /// <summary>
//        /// 合成完成
//        /// </summary>
//        /// <param name="utteranceId"></param>
//        public void OnSynthesizeFinish(string utteranceId)
//        {
//            Console.Write("OnSynthesizeFinish utteranceId = " + utteranceId);
//        }

//        /// <summary>
//        /// 播放开始
//        /// </summary>
//        /// <param name="utteranceId"></param>
//        public void OnSpeechStart(string utteranceId)
//        {
//            Console.Write("OnSpeechStart utteranceId = " + utteranceId);
//        }

//        /// <summary>
//        /// 播放正常结束，每句播放正常结束都会回调，如果过程中出错，则回调onError,不再回调此接口
//        /// </summary>
//        /// <param name="utteranceId"></param>
//        public void OnSpeechFinish(string utteranceId)
//        {
//            Console.Write("OnSpeechFinish utteranceId = " + utteranceId);
//        }

//        /// <summary>
//        /// 播放进度回调接口，分多次回调
//        /// </summary>
//        /// <param name="utteranceId"></param>
//        /// <param name="progress">如合成“百度语音问题”这6个字， progress肯定是从0开始，到6结束。 但progress无法保证和合成到第几个字对应。</param>
//        public void OnSpeechProgressChanged(string utteranceId, int progress)
//        {
//            Console.Write("OnSpeechProgressChanged utteranceId = " + utteranceId);
//        }

//        /// <summary>
//        /// 当合成或者播放过程中出错时回调此接口
//        /// </summary>
//        /// <param name="utteranceId"></param>
//        /// <param name="speechError"></param>
//        public void OnError(string utteranceId, SpeechError speechError)
//        {
//            Console.Write("OnError utteranceId = " + utteranceId + " speechError.Code = " + speechError.Code + " speechError.Description = " + speechError.Description);
//        }

//    }
//}
