//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Com.Dou361.Ijkplayer.Widget;

//namespace IdoMaster_GensouWorld.Film_Activitys
//{
//    [Activity(Label = "Film_Details_Activity_IJK")]
//    public class Film_Details_Activity_IJK : BaseActivity
//    {
//        private VPView player;
//        private Context mContext;
//        /// <summary>
//        /// 唤醒锁
//        /// </summary>
//        private PowerManager.WakeLock wakeLock;

//        public override int A_GetContentViewId()
//        {
//            return Resource.Layout.film_activity_film_details_ijk;
//        }

//        public override void B_BeforeInitView()
//        {
//            var pm = (PowerManager)GetSystemService(PowerService);
//            wakeLock = pm.NewWakeLock(WakeLockFlags.ScreenBright, "liveTAG");
//            wakeLock.Acquire();
//        }

//        public override void C_InitView()
//        {

//        }

//        public override void D_BindEvent()
//        {

//        }

//        public override void E_InitData()
//        {

//        }

//        public override void F_OnClickListener(View v, EventArgs e)
//        {

//        }

//        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
//        {

//        }
//    }
//}