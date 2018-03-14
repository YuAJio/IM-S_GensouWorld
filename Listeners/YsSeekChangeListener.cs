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

namespace IdoMaster_GensouWorld.Listeners
{
    /// <summary>
    /// 进度条更新回调
    /// </summary>
    public class YsSeekChangeListener : Java.Lang.Object, SeekBar.IOnSeekBarChangeListener
    {
        public Action<SeekBar, int, bool> Act_OnProgressChanged;
        public Action<SeekBar> Act_OnStartTrackingTouch;
        public Action<SeekBar> Act_OnStopTrackingTouch;

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {
            Act_OnProgressChanged?.Invoke(seekBar, progress, fromUser);
        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {
            Act_OnStartTrackingTouch?.Invoke(seekBar);
        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            Act_OnStopTrackingTouch?.Invoke(seekBar);
        }
    }
}