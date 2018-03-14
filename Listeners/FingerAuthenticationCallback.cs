using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Hardware.Fingerprints;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace IdoMaster_GensouWorld.Listeners
{
    /// <summary>
    /// 指纹验证回调
    /// </summary>
    public class FingerAuthenticationCallback : FingerprintManager.AuthenticationCallback
    {
        public FingerAuthenticationCallback() { }

        /// <summary>
        /// 指纹验证回调
        /// </summary>
        /// <param name="Act_OnAuthenticationSucceeded">指纹识别成功回调</param>
        /// <param name="Act_OnAuthenticationFailed">指纹识别失败回调</param>
        /// <param name="Act_OnAuthenticationError">当多次指纹密码验证错误后，进入此方法；并且，不能短时间内调用指纹验证</param>
        /// <param name="Act_OnAuthenticationHelp">指纹帮助动作回调</param>
        public FingerAuthenticationCallback(
              Action<FingerprintManager.AuthenticationResult> Act_OnAuthenticationSucceeded,
              Action Act_OnAuthenticationFailed,
            Action<FingerprintState, ICharSequence> Act_OnAuthenticationError,
            Action<FingerprintState, ICharSequence> Act_OnAuthenticationHelp
           )
        {
            this.Act_OnAuthenticationSucceeded = Act_OnAuthenticationSucceeded;
            this.Act_OnAuthenticationFailed = Act_OnAuthenticationFailed;
            this.Act_OnAuthenticationError = Act_OnAuthenticationError;
            this.Act_OnAuthenticationHelp = Act_OnAuthenticationError;
        }

        private event Action<FingerprintState, ICharSequence> Act_OnAuthenticationError;
        private event Action Act_OnAuthenticationFailed;
        private event Action<FingerprintState, ICharSequence> Act_OnAuthenticationHelp;
        private event Action<FingerprintManager.AuthenticationResult> Act_OnAuthenticationSucceeded;

        public override void OnAuthenticationError([GeneratedEnum] FingerprintState errorCode, ICharSequence errString)
        {
            if (Act_OnAuthenticationError != null)
                Act_OnAuthenticationError?.Invoke(errorCode, errString);
            else
                base.OnAuthenticationError(errorCode, errString);
        }

        public override void OnAuthenticationFailed()
        {
            if (Act_OnAuthenticationFailed != null)
                Act_OnAuthenticationFailed.Invoke();
            else
                base.OnAuthenticationFailed();
        }
        public override void OnAuthenticationHelp([GeneratedEnum] FingerprintState helpCode, ICharSequence helpString)
        {
            if (Act_OnAuthenticationHelp != null)
                Act_OnAuthenticationHelp.Invoke(helpCode, helpString);
            else
                base.OnAuthenticationHelp(helpCode, helpString);
        }
        public override void OnAuthenticationSucceeded(FingerprintManager.AuthenticationResult result)
        {
            if (Act_OnAuthenticationSucceeded != null)
                Act_OnAuthenticationSucceeded.Invoke(result);
            else
                base.OnAuthenticationSucceeded(result);
        }
    }
}