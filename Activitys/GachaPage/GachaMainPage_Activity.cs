using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Com.Nostra13.Universalimageloader.Core;
using Com.Nostra13.Universalimageloader.Core.Assist;
using CustomControl;
using IdoMaster_GensouWorld.Utils;
using IMAS.LocalDBManager.Models;
using IMAS.Tips.Logic.Accountings;
using IMAS.Tips.Logic.LocalDBManager;
using IMAS.Utils.Files;
using IMAS.Utils.Sp;

namespace IdoMaster_GensouWorld.Activitys.GachaPage
{
    /// <summary>
    /// 扭蛋主页
    /// </summary>
    [Activity(Label = "GachaMainPage_Activity", Theme = "@style/Theme.PublicTheme")]
    public class GachaMainPage_Activity : BaseActivity, ShakeDetector.IOnShakeListener
    {
        #region UI控件
        private ImageView iv_back;
        private Button bt_gacha;
        private ImageView iv_char;
        private TextView tv_mani;
        private GalTextView tv_talk_Content;
        private RelativeLayout rl_message_Box;
        private RelativeLayout rl_father;
        #endregion
        /// <summary>
        /// 所持金钱
        /// </summary>
        private long producer_Money;
        /// <summary>
        /// 监听屏幕摇晃类
        /// </summary>
        private ShakeDetector shakeDetector;
        /// <summary>
        /// 是否可以摇晃
        /// </summary>
        private bool isCanShake = false;
        /// <summary>
        /// CanGchaCharacter列表
        /// </summary>
        private List<Model_PlayerControllCharacter> list_cgc;
        /// <summary>
        /// 抽选出来的对象
        /// </summary>
        private Model_PlayerControllCharacter object_cgc;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_gacha_main_page;
        }

        public override void B_BeforeInitView()
        {
            shakeDetector = new ShakeDetector(this);
            SoundEffectPlayer.Init(this);
            SQLiteQCCCList();
            SQLiteQProducerMoney();

        }

        public override void C_InitView()
        {
            iv_back = FindViewById<ImageView>(Resource.Id.iv_back);
            bt_gacha = FindViewById<Button>(Resource.Id.bt_gacha);
            iv_char = FindViewById<ImageView>(Resource.Id.iv_character);
            tv_mani = FindViewById<TextView>(Resource.Id.tv_mani);
            rl_message_Box = FindViewById<RelativeLayout>(Resource.Id.rl_messageBox);
            tv_talk_Content = FindViewById<GalTextView>(Resource.Id.tv_content);
            rl_father = FindViewById<RelativeLayout>(Resource.Id.fl_father);
        }

        public override void D_BindEvent()
        {
            bt_gacha.Click += OnClickListener;
            iv_back.Click += OnClickListener;
        }

        public override void E_InitData()
        {
            tv_talk_Content.SetDelayPlayTime(200);
            isCanShake = true;
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
            switch (v.Id)
            {
                case Resource.Id.iv_back:
                    {
                        this.Finish();
                    }
                    break;
                case Resource.Id.bt_gacha:
                    {
                        if (producer_Money < 1500)
                        {
                            ShowSureConfim(null, "お金が足りません", null);
                            return;
                        }
                        isCanShake = false;
                        QGachaChar();
                    }
                    break;
            }
        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }
        #region 抽选角色
        private void QGachaChar()
        {
            CoverUIControl(CoverFlag.Gone, bt_gacha);
            var rm = new Random();
            var jk = ACC_GachaRandom.GetRandomList<Model_PlayerControllCharacter>(list_cgc, 1);
            if (jk == null)
            {
                var i = rm.Next(list_cgc.Count);
                object_cgc = list_cgc[i];
            }
            else
            {
                object_cgc = jk[0];
            }
            try
            {
                SQLiteUpdateCCCInfo(object_cgc.CharacterId);
                var anime = AnimationUtils.LoadAnimation(this, Resource.Animation.screen_shake_anim);
                anime.AnimationEnd -= Anime_AnimationEnd;
                anime.AnimationEnd += Anime_AnimationEnd;
                rl_father.StartAnimation(anime);

                var vibrator = (Vibrator)Application.GetSystemService(Service.VibratorService);
                vibrator.Vibrate(new long[] { 0, 300, 100, 500 }, -1);

            }
            catch (Exception e)
            {

            }
        }

        private void Anime_AnimationEnd(object sender, Animation.AnimationEndEventArgs e)
        {
            ImageLoader.Instance.DisplayImage(object_cgc.CharacterTachie, iv_char, ImageLoaderHelper.CharacterPicImageOption());
            isCanShake = false;
            ShowFirstMeetChatBox(object_cgc.CharacterId);//显示对话框

            SQLiteUpdateCharacterInfo(object_cgc);//修改角色是否可以再次扭出来
        }
        #endregion

        #region 显示初次见面对话框
        private void ShowFirstMeetChatBox(long CharId)
        {
            CoverUIControl(CoverFlag.Visible, rl_message_Box);
            var chatContentList = FileUtils.ReadAssetsInfoForObject<List<Model_ChatContent_Plus>>(this, "CCC_ChatList.json");
            var chatContent = chatContentList.Where(r => r.CharacterId == CharId).FirstOrDefault();

            tv_talk_Content.SetTextContent(chatContent.Chat_FirstMeet);

        }

        #endregion

        #region 接口回调
        public void OnShake()
        {
            if (!isCanShake)
            {
                return;
            }
            else
            {
                if (producer_Money < 1500)
                {
                    ShowSureConfim(null, "お金が足りません", null);
                    return;
                }
                isCanShake = false;
                SoundEffectPlayer.Play(SE_Enumeration.SE_Shake);
                QGachaChar();
            }

        }

        #endregion


        #region 图片加载进度监听
        public void OnLoadingCancelled(string p0, View p1)
        {

        }

        public void OnLoadingComplete(string p0, View p1, Bitmap p2)
        {
            SoundEffectPlayer.Play(SE_Enumeration.SE_Duduru);
        }

        public void OnLoadingFailed(string p0, View p1, FailReason p2)
        {

        }

        public void OnLoadingStarted(string p0, View p1)
        {

        }
        #endregion

        #region SQLite相关
        /// <summary>
        /// 查询可Gacha的角色列表
        /// </summary>
        private void SQLiteQCCCList()
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QCanControllCharacterList();
                return result;
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    return;
                }
                if (t.Result.IsSuccess)
                {
                    list_cgc = t.Result.Data;
                }
                else
                {
                    list_cgc = new List<Model_PlayerControllCharacter>();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        /// <summary>
        /// 更新角色信息
        /// </summary>
        private void SQLiteUpdateCharacterInfo(Model_PlayerControllCharacter info_character)
        {
            Task.Run(async () =>
            {
                info_character.IsGacha = false;
                var result = await IMAS_ProAppDBManager.GetInstance().UpdateProducingCharacterInfo(info_character);
                return result;
            });
        }
        /// <summary>
        /// 查询制作人金钱
        /// </summary>
        private void SQLiteQProducerMoney()
        {
            var p_Id = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.SpProducerInfoIdKey);
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QProducerMoney(p_Id);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    return;
                }
                producer_Money = t.Result.Data;
                if (t.Result.Data < 1500)
                {
                    isCanShake = false;
                }
                tv_mani.Text = $"¥\t{t.Result.Data}";
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// 更新所培养角色信息
        /// </summary>
        /// <param name="charId"></param>
        private void SQLiteUpdateCCCInfo(long charId)
        {
            var p_Id = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.SpProducerInfoIdKey);
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().InsertProducingCharacterInfo(p_Id, charId);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    return;
                }
                if (t.Result.IsSuccess)
                {
                    isRefresh = true;
                    var money = producer_Money;
                    money -= 1500;

#if !DEBUG
                             SQliteUpdateProducerMoney(p_Id, money);    
#endif

                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

        }
        /// <summary>
        /// 修改制作人身上的金币
        /// </summary>
        public void SQliteUpdateProducerMoney(int PkId, long money)
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().UpdateProducerMoney(PkId, money);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    isRefresh = true;
                    tv_mani.Text = $"¥\t{money}";
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

        #region 生命周期

        protected override void OnStart()
        {
            base.OnStart();
            Console.Write("===================OnStart===================");
            shakeDetector.RegisterOnShakeListener(this);
            shakeDetector.Start();
        }

        protected override void OnStop()
        {
            base.OnStop();
            Console.Write("===================OnStop===================");
            shakeDetector.Stop();
            shakeDetector.UnRegisterOnShakeListener(this);
        }
        #endregion

        #region 监听屏幕点击
        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                if (!tv_talk_Content.IsSendFinish())
                {
                    tv_talk_Content.SetAllDraw();
                }
            }
            return base.OnTouchEvent(e);
        }
        #endregion

        #region 重写结束Activity
        private bool isRefresh = false;
        public override void Finish()
        {
            if (isRefresh)
            {
                this.SetResult(Result.Ok);
            }
            //if (shakeDetector != null)
            //{
            //    shakeDetector.Stop();
            //    shakeDetector.UnRegisterOnShakeListener(this);
            //}
            base.Finish();
        }

        #endregion
    }
}