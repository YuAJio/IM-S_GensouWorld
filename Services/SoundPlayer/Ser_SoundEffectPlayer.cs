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
using IdoMaster_GensouWorld.Activitys.ProductionPage;
using Java.IO;

namespace IdoMaster_GensouWorld.Services
{
    public enum SE_Enumeration
    {
        Error_Code = -1,
        /// <summary>
        /// 钱声
        /// </summary>
        Money_SE = 001
    }

    public class CMyBinder : Binder
    {
        private Ser_SoundEffectPlayer service;
        public CMyBinder(Ser_SoundEffectPlayer service)
        {
            this.service = service;
        }

        public Ser_SoundEffectPlayer GetService()
        {
            return service;
        }

    }

    [Service(Name = "com.yurishi.imas_protable.SoundEffectPlayer")]
    public class Ser_SoundEffectPlayer : Service
    {
        public const string SESelectKey = "SE_Id";
        private MediaPlayer player;

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {

            var se_Id = intent.GetIntExtra(SESelectKey, -1);

            switch ((SE_Enumeration)se_Id)
            {
                case SE_Enumeration.Error_Code:
                    break;
                case SE_Enumeration.Money_SE:
                    if (player != null)
                    {
                        player.Start();
                        break;
                    }
                    player = MediaPlayer.Create(this, Resource.Raw.se_money);
                    break;
            }
            if (player != null)
            {
                player.Start();
            }

            return base.OnStartCommand(intent, flags, startId);
        }

        private CMyBinder mCMyBinder;


        public override IBinder OnBind(Intent intent)
        {
            mCMyBinder = new CMyBinder(this);
            var se_Id = intent.GetIntExtra(SESelectKey, -1);

            switch ((SE_Enumeration)se_Id)
            {
                case SE_Enumeration.Error_Code:
                    break;
                case SE_Enumeration.Money_SE:
                    player = MediaPlayer.Create(this, Resource.Raw.se_money);

                    break;
            }
            return mCMyBinder;
        }


        public override bool OnUnbind(Intent intent)
        {
            return base.OnUnbind(intent);
        }

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
            player = null;
        }

        public void StartPlaySE()
        {
            if (player != null)
            {
                try
                {
                    if (player.IsPlaying)
                    {
                        player.Stop();
                    }
                    player.Start();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}