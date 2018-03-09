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

        public FingerAuthenticationCallback( Action<FingerprintState, ICharSequence> Act_OnAuthenticationError  )
        {

        }

        private event Action<FingerprintState, ICharSequence> Act_OnAuthenticationError;
        private event Action Act_OnAuthenticationFailed;
        private event Action<FingerprintState, ICharSequence> Act_OnAuthenticationHelp;
        private event Action<FingerprintManager.AuthenticationResult> Act_OnAuthenticationSucceeded;

        public override void OnAuthenticationError([GeneratedEnum] FingerprintState errorCode, ICharSequence errString)
        {
            if (Act_OnAuthenticationError != null)
                Act_OnAuthenticationError.Invoke(errorCode, errString);
            else
                base.OnAuthenticationError(errorCode, errString);
        }

        public override void OnAuthenticationFailed()
        {
            if (Act_OnAuthenticationFailed != null)
                Act_OnAuthenticationFailed.Invoke();
            base.OnAuthenticationFailed();
        }
        public override void OnAuthenticationHelp([GeneratedEnum] FingerprintState helpCode, ICharSequence helpString)
        {
            if (Act_OnAuthenticationHelp != null)
                Act_OnAuthenticationHelp.Invoke(helpCode, helpString);
            base.OnAuthenticationHelp(helpCode, helpString);
        }
        public override void OnAuthenticationSucceeded(FingerprintManager.AuthenticationResult result)
        {
            if (Act_OnAuthenticationSucceeded != null)
                Act_OnAuthenticationSucceeded.Invoke(result);
            base.OnAuthenticationSucceeded(result);
        }
    }
}