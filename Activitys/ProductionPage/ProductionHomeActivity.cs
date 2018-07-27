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
using System.Threading.Tasks;
using IMAS.Tips.Logic.LocalDBManager;
using IdoMaster_GensouWorld.Services;
using Java.Lang;
using IMAS.Tips.Enums;
using IdoMaster_GensouWorld.Activitys.ShopPage;
using IdoMaster_GensouWorld.Activitys.CharacterPage;
using IMAS.LocalDBManager.Models;
using IMAS.Utils.Sp;
using IdoMaster_GensouWorld.Activitys.GachaPage;
using IdoMaster_GensouWorld.Activitys.BattlePage;
using IMAS.CupCake.Extensions;
using Com.Nostra13.Universalimageloader.Core;
using IdoMaster_GensouWorld.Utils;
using IdoMaster_GensouWorld.Activitys.ColonelRoomPage;
using IMAS.BaiduAI.Vocal_Compound;
using IMAS.Utils.Files;

namespace IdoMaster_GensouWorld.Activitys.ProductionPage
{
    /// <summary>
    /// 事务所主页
    /// </summary>
    [Activity(Label = "ProductionHomeActivity", Theme = "@style/Theme.PublicThemePlus")]
    public class ProductionHomeActivity : BaseActivity
    {
        #region UI控件
        ///// <summary>
        ///// 父布局
        ///// </summary>
        //private LinearLayout ll_father;
        /// <summary>
        /// 等级
        /// </summary>
        private TextView tv_level;
        /// <summary>
        /// 制作人姓名
        /// </summary>
        private TextView tv_name;
        /// <summary>
        /// 体力
        /// </summary>
        private TextView tv_ap;
        /// <summary>
        /// マニー
        /// </summary>
        private TextView tv_money;
        /// <summary>
        /// 经验值
        /// </summary>
        private TextView tv_exp;
        /// <summary>
        /// 天气图标
        /// </summary>
        private ImageView iv_weather;
        /// <summary>
        /// 进度条
        /// </summary>
        private ProgressBar pb_exp;
        /// <summary>
        /// 菜单_出击
        /// </summary>
        private ImageView iv_menu_attack;
        /// <summary>
        /// 菜单_ガチャ
        /// </summary>
        private ImageView iv_menu_lottery;
        /// <summary>
        /// 菜单_角色
        /// </summary>
        private ImageView iv_menu_character;
        /// <summary>
        /// 菜单_商店
        /// </summary>
        private ImageView iv_menu_shop;
        /// <summary>
        /// 菜单_自室
        /// </summary>
        private ImageView iv_menu_room;
        /// <summary>
        /// 角色立绘
        /// </summary>
        private ImageView iv_character;
        /// <summary>
        /// 背景图片
        /// </summary>
        private ImageView iv_background;
        #endregion

        /// <summary>
        /// 制作人信息
        /// </summary>
        private Model_ProducerInfo info_Producer;

        /// <summary>
        /// 操作角色Id(-1为没有)
        /// </summary>
        private long CharId = -1;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_home_production;
        }

        public override void B_BeforeInitView()
        {

        }

        public override void C_InitView()
        {
            tv_level = FindViewById<TextView>(Resource.Id.tv_level);
            tv_name = FindViewById<TextView>(Resource.Id.tv_name);
            tv_ap = FindViewById<TextView>(Resource.Id.tv_ap_number);
            tv_money = FindViewById<TextView>(Resource.Id.tv_total_money);
            tv_exp = FindViewById<TextView>(Resource.Id.tv_exp);
            iv_weather = FindViewById<ImageView>(Resource.Id.iv_weather);
            pb_exp = FindViewById<ProgressBar>(Resource.Id.pb_exp);
            iv_menu_attack = FindViewById<ImageView>(Resource.Id.iv_menu_attack);
            iv_menu_lottery = FindViewById<ImageView>(Resource.Id.iv_menu_lottery);
            iv_menu_character = FindViewById<ImageView>(Resource.Id.iv_menu_character);
            iv_menu_shop = FindViewById<ImageView>(Resource.Id.iv_menu_shop);
            iv_menu_room = FindViewById<ImageView>(Resource.Id.iv_menu_room);
            iv_character = FindViewById<ImageView>(Resource.Id.iv_character);
            iv_background = FindViewById<ImageView>(Resource.Id.iv_background);
        }

        public override void D_BindEvent()
        {
            iv_menu_attack.Click += OnClickListener;
            iv_menu_lottery.Click += OnClickListener;
            iv_menu_character.Click += OnClickListener;
            iv_menu_shop.Click += OnClickListener;
            iv_menu_room.Click += OnClickListener;
            iv_character.Click += OnClickListener;
        }

        public override void E_InitData()
        {
            SetBackGround();
            SQLiteQProducerInfo();
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
            switch (v.Id)
            {
                case Resource.Id.iv_menu_attack:
                    //出击
                    {
                        StartActivity(new Intent(this, typeof(Battle_Map_Select_Activity)));
                    }
                    break;
                case Resource.Id.iv_menu_lottery:
                    //ガチャ
                    {
                        StartActivityForResult(new Intent(this, typeof(GachaMainPage_Activity)), IMAS_Constants.OnPageRefreshKey);
                    }
                    break;
                case Resource.Id.iv_menu_character:
                    //角色
#if !DEBUG
                    if (string.IsNullOrEmpty(info_Producer.ProducingCharacter))
                    {
                        ShowSureConfim("ヒント", "まだキャラクターがいません,ガチャしてください", null, "はい", false);
                        return;
                    }
#endif
                    StartActivity(new Intent(this, typeof(CharacterMainPageActivity)));
                    break;
                case Resource.Id.iv_menu_shop:
                    {
                        //商店
                        StartActivityForResult(new Intent(this, typeof(ShopHomePage_Pager_Activity)), IMAS_Constants.OnPageRefreshKey);
                    }
                    break;
                case Resource.Id.iv_menu_room:
                    {
                        //自室
                        var intent = new Intent(this, typeof(ColoneRoomMain_Activity));
                        var bundle = new Bundle();
                        var producerInfo = info_Producer.ToJson();
                        bundle.PutString("Info", producerInfo);
                        intent.PutExtra("value", bundle);
                        StartActivityForResult(intent, IMAS_Constants.OnPageRefreshKey);
                    }
                    break;

                case Resource.Id.iv_character:
                    {
                        ////点击角色
                        if (CharId <= 0)
                            return;//没有角色就不讲话
                        var chatContentList = FileUtils.ReadAssetsInfoForObject<List<Model_ChatContent_Plus>>(this, IMAS_Constants.Ass_ChatContent);
                        var chatContent = chatContentList.Where(r => r.CharacterId == CharId).FirstOrDefault();
                        var ra = new Random();
                        var vocalText = chatContent.Touch_Serifu[ra.Next(chatContent.Touch_Serifu.Count - 1)];
                        //BaiduVocalManager.GetKagemusha().Speak(vocalText);
                    }
                    break;
            }
        }

        private void TestRaundom()
        {
            var jk = 0;
            var jc = 0;
            var js = 0;
            var jy = 0;

            var tList = new List<string> { "a", "b", "c", "d" };
            var ra = new Random();
            var wCount = 0;
            while (wCount < 10000)
            {
                var value = tList[ra.Next(tList.Count)];
                switch (value)
                {
                    case "a":
                        jk += 1;
                        break;
                    case "b":
                        jc += 1;
                        break;
                    case "c":
                        js += 1;
                        break;
                    case "d":
                        jy += 1;
                        break;
                }
            }
            Console.Write(jk + jc + js + jy);
        }
        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// 读取背景图片地址并设置
        /// </summary>
        /// <param name="path"></param>
        private void SetBackGround()
        {
            var jk = AndroidPreferenceProvider.GetInstance().GetString(IMAS_Constants.SpRoomBackGroundPathKey);
            ImageLoader.Instance.DisplayImage(jk, iv_background, ImageLoaderHelper.BackGroundImageOption());
        }

        #region SQLite操作
        /// <summary>
        /// 获取制作人信息
        /// </summary>
        private void SQLiteQProducerInfo()
        {
            var p_Name = AndroidPreferenceProvider.GetInstance().GetString(IMAS_Constants.SpProducerInfoNameKey);
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QProducerInfo(p_Name);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    var data = t.Result.Data;
                    tv_exp.Text = $"{data.Exp} / {data.LevelUpExp}";
                    var jk = data.Exp / (double)data.LevelUpExp * 100;
                    pb_exp.Progress = (int)jk;
                    tv_ap.Text = $"体力：{data.AP}";
                    tv_level.Text = $"LV.{data.Level}";
                    tv_money.Text = $"マニー：{data.Money}";
                    tv_name.Text = $"{p_Name}";

                    info_Producer = data;
                    if (!string.IsNullOrEmpty(data.ProducingCharacter))
                    {
                        SQLiteQCharacterInfo();
                    }
                    SQliteChangeProducerInfo(data.PkId);
                    AndroidPreferenceProvider.GetInstance().PutInt(IMAS_Constants.SpProducerInfoIdKey, data.PkId);
                }
                else
                {
                    ShowDiyToastLong(t.Result.Message);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        /// <summary>
        /// 修改制作人 序章已完成
        /// </summary>
        private void SQliteChangeProducerInfo(int pkId)
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().UpdateProducerChapterInfo(pkId, ChapterEnumeration.Prologue, true);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                }
                else
                {
                    ShowDiyToastLong(t.Result.Message);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        private void SQLiteQCharacterInfo()
        {
            var p_Id = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.SpProducerInfoIdKey);

            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QProducingCharacterInfo(p_Id);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    var data = t.Result.Data;
                    if (data != null)
                    {
                        CharId = data.CharacterId;
                        ImageLoader.Instance.DisplayImage(data.CharacterTachie, iv_character, ImageLoaderHelper.CharacterPicImageOption());
                    }
                }
                else
                {
                    ShowDiyToastLong(t.Result.Message);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

        #region 退出游戏
        long waitTime = 2000;// 如果两次按下时间在2000毫秒以内，则退出
        long touchTime = 0;// 记录上一次按下的时间

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (e.Action == KeyEventActions.Down && Keycode.Back == keyCode)
            {
                long currentTime = Java.Lang.JavaSystem.CurrentTimeMillis();
                if ((currentTime - touchTime) >= waitTime)
                {
                    Toast.MakeText(this, "再びクリックお終了します", ToastLength.Short).Show();
                    touchTime = currentTime;
                }
                else
                {
                    ShowConfim(null, "ゲームお終了しますが?", (j, k) =>
                    {
                        //  APPManager.GetInstance().KillAllActivity();
                        CleanAllActivity();
                        IMAS_Application.Sington.OpenActivityList.Clear();
                        //关闭服务
                        ApplicationContext.StopService(new Intent(this, typeof(BackGroundMusicPlayer)));
                        this.Finish();
                    }, null);
                }
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }

        private void CleanAllActivity()
        {
            foreach (var item in IMAS_Application.Sington.OpenActivityList)
            {
                item.Finish();
            }
            IMAS_Application.Sington.OpenActivityList.Clear();
            GC.Collect();
        }

        #endregion

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                switch (requestCode)
                {
                    case IMAS_Constants.OnPageRefreshKey:
                        {
                            //刷新
                            SQLiteQProducerInfo();
                            SetBackGround();
                        }
                        break;
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}