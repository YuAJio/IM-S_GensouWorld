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
using IMAS.LocalDBManager.Models;
using IdoMaster_GensouWorld.Utils;
using IMAS.Utils.Sp;
using IMAS.CupCake.Extensions;
using IMAS.Tips.Enums;

namespace IdoMaster_GensouWorld.Activitys
{
    /// <summary>
    /// 闪屏页面(做一些无法在主页做的操作)
    /// </summary>
    [Activity(Label = "アイドルマスター", MainLauncher = true, Theme = "@style/Theme.Main", Icon = "@mipmap/icon", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    //[Activity(Label = "FlashScreen", MainLauncher = true)]
    public class FlashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (IsFirstOpenApp())
            {
                AndroidPreferenceProvider.GetInstance().PutInt(IMAS_Constants.SpVersionKey, VersionInfoUtils.GetVersionCode());
                InitAllData();
            }
            else
            {
                InitAllData(false);
            }

        }
        /// <summary>
        /// 跳转到首页
        /// </summary>
        private void JumpToMainPage()
        {
            StartActivity(new Intent(this, typeof(MainPage.MainPage_Activity)));
            this.Finish();
        }

        private void InitAllData(bool dst = true)
        {
            Task.Run(async () =>
            {
                var result = new IMAS.CupCake.Data.Results();
                var r_shop = await InitTodaysSellGoods();
                if (!r_shop.IsSuccess)
                {
                    return result.Error(message: r_shop.Message);
                }
                if (dst)
                {
                    var r_char = await InitCharacterCanBeGacha();
                    if (!r_char.IsSuccess)
                        return result.Error(message: r_char.Message);

                    var r_map = await InitBattleMap();
                    if (!r_map.IsSuccess)
                        return result.Error(message: r_map.Message);
                }
                return result.Success();
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    Toast.MakeText(this, "エラー!", ToastLength.Long).Show();
                    return;
                }
                if (t.Result.IsSuccess)
                {
                    JumpToMainPage();
                }
            });
        }

        #region 检测是否是第一次使用
        private bool IsFirstOpenApp()
        {
            var versionCode = VersionInfoUtils.GetVersionCode();
            var versionCode_Sp = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.SpVersionKey);
            return versionCode != versionCode_Sp;
            //if (versionCode != versionCode_Sp)
            //{
            //    Toast.MakeText(this, "初めてのスタート", ToastLength.Short).Show();
            //}
        }

        #endregion

        #region 初始化商店贩卖商品
        private List<Model_Items> GetFukuList()
        {
            var list = new List<Model_Items>();
            list.Add(new Model_Items()
            {
                Id = 0x20001,
                ShopPrice = 3000,
                Name = "セーラー服",
                DEFPromote = 20,
                ItemType = IMAS.Tips.Enums.ItemEnumeration.EquipmentItems,
                ItemIllustrate = "正統のセーラー服，(ﾉ)`ω´(ヾ)かわい",
            });
            list.Add(new Model_Items()
            {
                Id = 0x20002,
                ShopPrice = 5000,
                Name = "コスプレ服",
                DEFPromote = 15,
                ItemType = IMAS.Tips.Enums.ItemEnumeration.EquipmentItems,
                ItemIllustrate = "とある巫女の服，脇下ちょと冷たい",
            });
            list.Add(new Model_Items()
            {
                Id = 0x20003,
                ShopPrice = 15000,
                Name = "ダーク騎士の鎧",
                DEFPromote = 50,
                ItemType = IMAS.Tips.Enums.ItemEnumeration.EquipmentItems,
                ItemIllustrate = "真っ暗のとでも重いの鎧",
            });
            list.Add(new Model_Items()
            {
                Id = 0x20004,
                ShopPrice = 8000,
                Name = "ビキニ",
                DEFPromote = 5,
                ItemType = IMAS.Tips.Enums.ItemEnumeration.EquipmentItems,
                ItemIllustrate = "セクシのビニキ、キャアアア！",
            });
            return list;
        }
        private List<Model_Items> GetKazariList()
        {
            var list = new List<Model_Items>();
            list.Add(new Model_Items()
            {
                Id = 0x21001,
                ShopPrice = 9000,
                Name = "狂気の首輪",
                ATTPromote = 120,
                DeBuffingList = new Model_DeBuff() { DeBuff_Crazy = BuffStatus.On },
                ItemType = IMAS.Tips.Enums.ItemEnumeration.OtherItems,
                ItemIllustrate = "あんまり気持ちじゃないの首輪,狂気が感じる",
            });
            list.Add(new Model_Items()
            {
                Id = 0x21002,
                ShopPrice = 80,
                Name = "お守り",
                DEFPromote = 10,
                ATTPromote = 10,
                ItemType = IMAS.Tips.Enums.ItemEnumeration.OtherItems,
                ItemIllustrate = "とある神社のお守り,安い！",
            });
            list.Add(new Model_Items()
            {
                Id = 0x21003,
                ShopPrice = 1500,
                Name = "サクラの耳飾り",
                DEFPromote = 10,
                ATTPromote = 10,
                ItemType = IMAS.Tips.Enums.ItemEnumeration.OtherItems,
                ItemIllustrate = "ピンクの耳飾り,いい匂いがする，女子中学生が似合ういます",
            });
            list.Add(new Model_Items()
            {
                Id = 0x21004,
                ShopPrice = 1500,
                Name = "サクラの指輪",
                DEFPromote = 10,
                ATTPromote = 10,
                ItemType = IMAS.Tips.Enums.ItemEnumeration.OtherItems,
                ItemIllustrate = "ピンクの指輪,落ちだら,背德感が半端ね,奥さんが似合う",
            });
            return list;
        }
        private List<Model_Items> GetFoodList()
        {
            var list = new List<Model_Items>();
            list.Add(new Model_Items()
            {
                Id = 0x22001,
                ShopPrice = 500,
                Name = "ラーメン",
                HealingHealthPoint = 300,
                ItemType = IMAS.Tips.Enums.ItemEnumeration.HealingItems,
                ItemIllustrate = "ラーメンが大好きの面妖さん",
            });
            list.Add(new Model_Items()
            {
                Id = 0x22002,
                ShopPrice = 300,
                Name = "コーラ",
                HealingStaminaPoint = 80,
                ItemType = IMAS.Tips.Enums.ItemEnumeration.HealingItems,
                ItemIllustrate = "三年A組,将軍先生！",
            });
            list.Add(new Model_Items()
            {
                Id = 0x22003,
                ShopPrice = 350,
                Name = "リンゴ飴",
                HealingHealthPoint = 100,
                HealingManaPoint = 50,
                ItemType = IMAS.Tips.Enums.ItemEnumeration.HealingItems,
                ItemIllustrate = "真っ赤色のリンゴ飴,とでも甘い！",
            });
            list.Add(new Model_Items()
            {
                Id = 0x22004,
                ShopPrice = 100,
                Name = "団子",
                HealingHealthPoint = 30,
                HealingManaPoint = 30,
                HealingStaminaPoint = 30,
                ItemType = IMAS.Tips.Enums.ItemEnumeration.HealingItems,
                ItemIllustrate = "三つ色の団子,うまそう",
            });

            return list;
        }
        #endregion

        #region 初始化可扭取角色信息
        #region 初始化角色技能
        private List<Model_Skills> CreateCharacterSkills_Eins()
        {
            var data = new List<Model_Skills>()
            {
                new Model_Skills(){
                    Name="目が逢う瞬間",
                    SkillType=SkillsType.Water,
                    SkillMode=SkillsMode.Attack,
                   CostSp=20,
                   Damage=100,
                   SkillIllustrate="目と目が逢う 瞬間好きだと気づいた"
                },

                    new Model_Skills(){
                    Name="鳥の詩",
                    SkillType=SkillsType.Water,
                    SkillMode=SkillsMode.Mana,
                   CostMp=15,
                   Damage=80,
                     SkillIllustrate="消える飛行機雲 僕たちは見送った"
                    },

                new Model_Skills(){
                    Name="蒼い鳥",
                    SkillType=SkillsType.Water,
                    SkillMode=SkillsMode.Mana,
                   CostMp=20,
                   Damage=100,
                     SkillIllustrate="蒼い鳥,もし幸せ"
                },

                new Model_Skills(){
                    Name="細氷",
                    SkillType=SkillsType.Water,
                    SkillMode=SkillsMode.Mana,
                   CostMp=80,
                   Damage=350,
                     SkillIllustrate="私　もがきながら歩き出すの"
                },
            };
            return data;
        }

        private List<Model_Skills> CreateCharacterSkills_Zwei()
        {
            var data = new List<Model_Skills>()
            {
                new Model_Skills(){
                    Name="ふるふるフューチャー☆",
                    SkillType=SkillsType.Light,
                    SkillMode=SkillsMode.Mind,
                   CostHp=15,
                   Damage=120,
                    SkillIllustrate="大好きハニー　いちごみたいに　純粋なの"
                },

                    new Model_Skills(){
                    Name="Relations",
                    SkillType=SkillsType.Light,
                    SkillMode=SkillsMode.Mind,
                   CostHp=25,
                   Damage=180,
                    SkillIllustrate="「べつに」なんて言わないで  「ちがう」って言って"
                    },

                new Model_Skills(){
                    Name="深紅",
                    SkillType=SkillsType.Fire,
                    SkillMode=SkillsMode.Mind,
                   CostHp=80,
                   Damage=300,
                    SkillIllustrate="深紅の空燃え立つように!"
                },

                new Model_Skills(){
                    Name="高鳴る",
                    SkillType=SkillsType.Light,
                    SkillMode=SkillsMode.Mind,
                   CostHp=50,
                   Damage=210,
                    SkillIllustrate="高鳴る鼓動で壊れそう!"
                },
            };
            return data;
        }

        private List<Model_Skills> CreateCharacterSkills_Drei()
        {
            var data = new List<Model_Skills>()
            {
                new Model_Skills(){
                    Name="キラメキラリ",
                    SkillType=SkillsType.Timber,
                    SkillMode=SkillsMode.Attack,
                   CostSp=30,
                   Damage=150,
                   SkillIllustrate="キラメキラリ ずっとチュッと"
                },

                    new Model_Skills(){
                    Name="ふらりのもじぴったん",
                    SkillType=SkillsType.Timber,
                    SkillMode=SkillsMode.Attack,
                   CostSp=40,
                   Damage=200,
                  SkillIllustrate="ぴったん たんた ぜのぴったん"
                    },

                new Model_Skills(){
                    Name="スマイル体操",
                    SkillType=SkillsType.Timber,
                    SkillMode=SkillsMode.Attack,
                   CostSp=50,
                   Damage=220,
                     SkillIllustrate="スマイル体操 いくよう!"
                },

                new Model_Skills(){
                    Name="Do-Dai",
                    SkillType=SkillsType.Light,
                    SkillMode=SkillsMode.Mind,
                   CostSp=100,
                   Damage=500,
                     SkillIllustrate="本日はみんなに私のとっておきの"
                },
            };
            return data;
        }
        private List<Model_Skills> CreateCharacterSkills_Vier()
        {
            var data = new List<Model_Skills>()
            {
                new Model_Skills(){
                    Name="風花",
                    SkillType=SkillsType.Timber,
                    SkillMode=SkillsMode.Attack,
                   CostSp=20,
                   Damage=130,
                   SkillIllustrate="光の外へ心は向かっていく"
                },

                    new Model_Skills(){
                    Name="月のワルツ",
                    SkillType=SkillsType.Timber,
                    SkillMode=SkillsMode.Attack,
                   CostSp=40,
                   Damage=200,
                      SkillIllustrate="光の外へ心は向かっていく"
                    },

                new Model_Skills(){
                    Name="オーバーマスター",
                    SkillType=SkillsType.Timber,
                    SkillMode=SkillsMode.Attack,
                   CostSp=50,
                   Damage=220,
                      SkillIllustrate="光の外へ心は向かっていく"
                },

                new Model_Skills(){
                    Name="99Nights",
                    SkillType=SkillsType.Timber,
                    SkillMode=SkillsMode.Attack,
                   CostSp=99,
                   Damage=500,
                      SkillIllustrate="光の外へ心は向かっていく"
                },
            };
            return data;
        }
        #endregion

        private Model_PlayerControllCharacter CreateCharacter_Eins()
        {
            #region 初始化角色装备
            var dict = new Dictionary<string, Model_Items>();
            dict.Add(EnumDescription.GetFieldText(ItemEnumeration.WeaponItems), null);
            dict.Add(EnumDescription.GetFieldText(ItemEnumeration.EquipmentItems), null);
            dict.Add(EnumDescription.GetFieldText(ItemEnumeration.OtherItems), null);
            #endregion
            var data = new Model_PlayerControllCharacter
            {
                Name = "キサラギーチハヤ",
                CharacterId = IMAS_Constants.KISARAGI_CHIHAYA_ID,
                Level = 1,
#if DEBUG
                GachaWeight = 60,
#else
                GachaWeight = 1,
#endif
                IsGacha = true,
                STR = 6,
                DEX = 15,
                INT = 15,
                HealthPoint = 72,
                MaxHealthPoint = 72,
                ManaPoint = 100,
                MaxManaPoint = 100,
                StaminaPoint = 120,
                MaxStaminaPoint = 120,
                Exp = 0,
                LevelUpExp = 1000,
                HP_Potential = 50,
                MP_Potential = 30,
                SP_Potential = 30,
                STR_Potential = 2,
                DEX_Potential = 3,
                INT_Potential = 3,
                ChartSkills = CreateCharacterSkills_Eins().ToJson(),
                ChartEquip = dict.ToJson(),
                CharacterTachie = "http://pic.ffsky.net/images/2015/08/15/b50c5c9d9f3b3679b4ed6b76e753242e.md.png"
            };
            data.AttackPoint = Convert.ToInt32(data.STR * 2.7);
            data.DefencePoint = Convert.ToInt32(data.DEX * 7.2);
            return data;
        }

        private Model_PlayerControllCharacter CreateCharacter_Zwei()
        {
            #region 初始化角色装备
            var dict = new Dictionary<string, Model_Items>();
            dict.Add(EnumDescription.GetFieldText(ItemEnumeration.WeaponItems), null);
            dict.Add(EnumDescription.GetFieldText(ItemEnumeration.EquipmentItems), null);
            dict.Add(EnumDescription.GetFieldText(ItemEnumeration.OtherItems), null);
            #endregion
            var data = new Model_PlayerControllCharacter
            {
                Name = "ホシイーミキ",
                Level = 1,
                IsGacha = true,
                GachaWeight = 5,
                CharacterId = IMAS_Constants.HOSHII_MIKI_ID,
                STR = 15,
                DEX = 15,
                INT = 10,
                HealthPoint = 60,
                MaxHealthPoint = 60,
                ManaPoint = 80,
                MaxManaPoint = 80,
                StaminaPoint = 150,
                MaxStaminaPoint = 150,
                Exp = 0,
                LevelUpExp = 1000,
                HP_Potential = 80,
                MP_Potential = 10,
                SP_Potential = 40,
                STR_Potential = 4,
                DEX_Potential = 2,
                INT_Potential = 2,
                ChartSkills = CreateCharacterSkills_Zwei().ToJson(),
                ChartEquip = dict.ToJson(),
                CharacterTachie = "http://pic.ffsky.net/images/2015/08/15/10e949d55ed3f40340f6c7f5a5fde7e5.md.png"
            };
            data.AttackPoint = Convert.ToInt32(data.STR * 4.2);
            data.DefencePoint = Convert.ToInt32(data.DEX * 2.0);
            return data;
        }
        private Model_PlayerControllCharacter CreateCharacter_Drei()
        {
            #region 初始化角色装备
            var dict = new Dictionary<string, Model_Items>();
            dict.Add(EnumDescription.GetFieldText(ItemEnumeration.WeaponItems), null);
            dict.Add(EnumDescription.GetFieldText(ItemEnumeration.EquipmentItems), null);
            dict.Add(EnumDescription.GetFieldText(ItemEnumeration.OtherItems), null);
            #endregion
            var data = new Model_PlayerControllCharacter
            {
                Name = "たかつきーやよい",
                CharacterId = IMAS_Constants.TAKATSUKI_YAYOI_ID,
                Level = 1,
                IsGacha = true,
                GachaWeight = 5,
                STR = 10,
                DEX = 10,
                INT = 15,
                HealthPoint = 50,
                MaxHealthPoint = 50,
                ManaPoint = 120,
                MaxManaPoint = 120,
                StaminaPoint = 130,
                MaxStaminaPoint = 130,
                Exp = 0,
                LevelUpExp = 1000,
                HP_Potential = 50,
                MP_Potential = 30,
                SP_Potential = 30,
                STR_Potential = 3,
                DEX_Potential = 2,
                INT_Potential = 3,
                ChartEquip = dict.ToJson(),
                ChartSkills = CreateCharacterSkills_Drei().ToJson(),
                CharacterTachie = "http://pic.ffsky.net/images/2015/08/15/cb25763bc2919f1eff72ea003b559ee2.md.png"
            };
            data.AttackPoint = Convert.ToInt32(data.STR * 2.5);
            data.DefencePoint = Convert.ToInt32(data.DEX * 3.5);
            return data;
        }
        private Model_PlayerControllCharacter CreateCharacter_Vier()
        {
            #region 初始化角色装备
            var dict = new Dictionary<string, Model_Items>();
            dict.Add(EnumDescription.GetFieldText(ItemEnumeration.WeaponItems), null);
            dict.Add(EnumDescription.GetFieldText(ItemEnumeration.EquipmentItems), null);
            dict.Add(EnumDescription.GetFieldText(ItemEnumeration.OtherItems), null);
            #endregion
            var data = new Model_PlayerControllCharacter
            {
                Name = "シジョウータカネ",
                Level = 1,
                IsGacha = true,
                GachaWeight = 5,
                CharacterId = IMAS_Constants.SHIJOU_TAKANE_ID,
                STR = 8,
                DEX = 20,
                INT = 10,
                HealthPoint = 80,
                MaxHealthPoint = 80,
                ManaPoint = 100,
                MaxManaPoint = 100,
                StaminaPoint = 100,
                MaxStaminaPoint = 100,
                Exp = 0,
                LevelUpExp = 1000,
                HP_Potential = 80,
                MP_Potential = 15,
                SP_Potential = 20,
                STR_Potential = 3,
                DEX_Potential = 5,
                INT_Potential = 3,
                ChartEquip = dict.ToJson(),
                ChartSkills = CreateCharacterSkills_Vier().ToJson(),
                CharacterTachie = "http://pic.ffsky.net/images/2015/08/15/9ace68290139f88387ca457913cfd89c.md.png"
            };
            data.AttackPoint = Convert.ToInt32(data.STR * 3.0);
            data.DefencePoint = Convert.ToInt32(data.DEX * 2.6);
            return data;
        }
        #endregion

        #region 初始化战斗地图信息

        private Model_BattleMap CreatBatteMap_Yuri()
        {
            var data = new Model_BattleMap()
            {
                MapName = IMAS_Constants.MAP_YURI_NAME,
                MapId = IMAS_Constants.MAP_YURI_ID,
                RestrictionLevel = 1,
                LivedMonster = ""
            };
            return data;
        }

        private Model_BattleMap CreatBatteMap_Winter()
        {
            var data = new Model_BattleMap()
            {
                MapName = IMAS_Constants.MAP_WINTER_NAME,
                MapId = IMAS_Constants.MAP_WINTER_ID,
                RestrictionLevel = 10,
                LivedMonster = ""
            };
            return data;
        }

        private Model_BattleMap CreatBatteMap_Banana()
        {
            var data = new Model_BattleMap()
            {
                MapName = IMAS_Constants.MAP_BANANA_NAME,
                MapId = IMAS_Constants.MAP_BANANA_ID,
                RestrictionLevel = 15,
                LivedMonster = ""
            };
            return data;
        }

        #endregion

        #region 初始化SQLite数据
        /// <summary>
        /// 初始化商店贩卖物品信息
        /// </summary>
        /// <returns></returns>
        private async Task<IMAS.CupCake.Data.Results> InitTodaysSellGoods()
        {
            var list_Fuku = GetFukuList();
            var list_Kazari = GetKazariList();
            var list_Food = GetFoodList();

            return await IMAS_ProAppDBManager.GetInstance().InsertTodaysSellGoods(DateTime.Now, list_Fuku, list_Kazari, list_Food);
        }
        /// <summary>
        /// 初始化可扭曲角色信息
        /// </summary>
        /// <returns></returns>
        private async Task<IMAS.CupCake.Data.Results> InitCharacterCanBeGacha()
        {
            var list = new List<Model_PlayerControllCharacter>() { CreateCharacter_Eins(), CreateCharacter_Zwei(), CreateCharacter_Drei(), CreateCharacter_Vier() };

            return await IMAS_ProAppDBManager.GetInstance().InsertProducingCharactersInfo(list);
        }
        /// <summary>
        /// 初始化可进入地图信息
        /// </summary>
        /// <returns></returns>
        private async Task<IMAS.CupCake.Data.Results> InitBattleMap()
        {
            var list = new List<Model_BattleMap>() { CreatBatteMap_Yuri(), CreatBatteMap_Winter(), CreatBatteMap_Banana() };

            return await IMAS_ProAppDBManager.GetInstance().InsertBattleMapInfomation(list);
        }
        #endregion
    }
}