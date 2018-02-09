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
using IMAS.LocalDBManager.Models;
using IMAS.Accounting;
using System.Threading.Tasks;
using IMAS.Tips.Logic.LocalDBManager;

namespace IdoMaster_GensouWorld.Activitys.BattlePage
{
    [Activity(Label = "Battle_Loading_Activity", Theme = "@style/Theme.PublicTheme")]
    public class Battle_Loading_Activity : BaseActivity
    {
        /// <summary>
        /// 地图Id
        /// </summary>
        private int mapId;
        /// <summary>
        /// 角色等级
        /// </summary>
        private int charaLevel;

        private Random ra;


        public override int A_GetContentViewId()
        {
            return 0;
        }

        public override void B_BeforeInitView()
        {
            mapId = Intent.GetIntExtra(IMAS_Constants.MapPagePkIdKey, -1);
            charaLevel = Intent.GetIntExtra(IMAS_Constants.CharacterLevelKey, -1);

            ra = new Random();
        }

        public override void C_InitView()
        {

        }

        public override void D_BindEvent()
        {

        }

        public override void E_InitData()
        {
            SQLiteUpdateMapMonster();
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }


        #region 添加怪物信息
        private List<Model_Monster> AddMonsters()
        {
            var list = new List<Model_Monster>();
            switch (mapId)
            {
                #region 创建百合树的怪物
                case IMAS_Constants.MAP_YURI_ID:
                    {
                        list.Add(new Model_Monster
                        {
                            Name = IMAS_Constants.Monster_Name_Slime,
                            MonsterId = IMAS_Constants.Monster_Id_Slime,
                            Pic_Path = IMAS_Constants.Monster_Pic_Slime,
                            Exp = 50,
                            //Level = Convert.ToInt32(charaLevel * levelStack[ra.Next(levelStack.Length)]),
                            MonsterType = IMAS.Tips.Enums.MonsterType.Animal,
                            MonsterLivePlace = IMAS.Tips.Enums.MonsterLivePlace.YuriTree,
                            BaseHealPoint = 100,
                            BaseAttPoint = 10,
                            BaseDefPoint = 5,
                            HpPotency = 30,
                            AttPotency = 5,
                            DefPotency = 3,
                            DropMoney = 30,
                        });
                        list.Add(new Model_Monster
                        {
                            Name = IMAS_Constants.Monster_Name_Fox,
                            MonsterId = IMAS_Constants.Monster_Id_Fox,
                            Pic_Path = IMAS_Constants.Monster_Pic_Fox,
                            Exp = 80,
                            //Level = Convert.ToInt32(charaLevel * levelStack[ra.Next(levelStack.Length)]),
                            MonsterType = IMAS.Tips.Enums.MonsterType.Animal,
                            MonsterLivePlace = IMAS.Tips.Enums.MonsterLivePlace.YuriTree,
                            BaseHealPoint = 150,
                            BaseAttPoint = 12,
                            BaseDefPoint = 10,
                            HpPotency = 40,
                            AttPotency = 6,
                            DefPotency = 4,
                            DropMoney = 60,
                        });
                        list.Add(new Model_Monster
                        {
                            Name = IMAS_Constants.Monster_Name_Wolf,
                            MonsterId = IMAS_Constants.Monster_Id_Wolf,
                            Pic_Path = IMAS_Constants.Monster_Pic_Wolf,
                            Exp = 120,
                            //Level = Convert.ToInt32(charaLevel * levelStack[ra.Next(levelStack.Length)]),
                            MonsterType = IMAS.Tips.Enums.MonsterType.Animal,
                            MonsterLivePlace = IMAS.Tips.Enums.MonsterLivePlace.YuriTree,
                            BaseHealPoint = 250,
                            BaseAttPoint = 20,
                            BaseDefPoint = 12,
                            HpPotency = 60,
                            AttPotency = 10,
                            DefPotency = 6,
                            DropMoney = 60,
                        });
                    }
                    break;
                #endregion
                #region 创建凛冬玫瑰的怪物
                case IMAS_Constants.MAP_WINTER_ID:
                    {

                    }
                    break;
                #endregion

                #region 创建香蕉海洋的怪物
                case IMAS_Constants.MAP_BANANA_ID:
                    {

                    }
                    break;
                    #endregion

            }
            return list;
        }
        #endregion

        #region SQLite相关
        private void SQLiteUpdateMapMonster()
        {
            ShowWaitDiaLog("Loading...");
            Task.Run(async () =>
            {
                var list = AddMonsters();
                var result = await IMAS_ProAppDBManager.GetInstance().UpdateMonsterInfomation(mapId, list);
                return result;
            }).ContinueWith(t =>
            {
                HideWaitDiaLog();
                if (t.Exception != null)
                {
                    WriteLogFile(IMAS.Utils.Logs.LogLevel.Error, IMAS_Constants.Log_SQLiteErrorKey, $"UpdateMapMonster Error{t.Exception.Message.ToString()}");
                    this.Finish();
                    return;
                }
                if (t.Result.IsSuccess)
                {
                    var intent = new Intent(this, typeof(Battle_Main_Activity));
                    intent.PutExtra(IMAS_Constants.MapPagePkIdKey, mapId);
                    intent.PutExtra(IMAS_Constants.CharacterLevelKey, charaLevel);
                    StartActivity(intent);
                }
                else
                {
                    ShowBigToast($"エラー{t.Result.Message}");
                }
                this.Finish();

            }, TaskScheduler.FromCurrentSynchronizationContext());

        }

        #endregion
    }
}