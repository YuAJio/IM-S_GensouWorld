using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Nostra13.Universalimageloader.Core;
using IdoMaster_GensouWorld.Adapters;
using IdoMaster_GensouWorld.Threads;
using IdoMaster_GensouWorld.Utils;
using IMAS.Accounting;
using IMAS.CupCake.Extensions;
using IMAS.LocalDBManager.Models;
using IMAS.Tips.Logic.Accountings;
using IMAS.Tips.Logic.LocalDBManager;
using IMAS.Utils.Sp;
using Java.Lang;
using Java.Util;

namespace IdoMaster_GensouWorld.Activitys.BattlePage
{
    /// <summary>
    /// 战斗主页面
    /// </summary>
    [Activity(Label = "Battle_Main_Activity", Theme = "@style/Theme.PublicTheme")]
    public class Battle_Main_Activity : BaseActivity, ViewTreeObserver.IOnGlobalLayoutListener
    {
        #region 状态枚举
        private enum MenuIndex
        {
            /// <summary>
            /// 主菜单
            /// </summary>
            Main = 1,
            /// <summary>
            /// 战斗菜单
            /// </summary>
            Battle = 2,
            /// <summary>
            /// 道具菜单
            /// </summary>
            Item = 3,
            /// <summary>
            /// 战技
            /// </summary>
            Skill = 4,
            /// <summary>
            /// 魔法
            /// </summary>
            Magic = 5,
            /// <summary>
            /// 战斗信息
            /// </summary>
            Information = 6,
        }
        #endregion

        #region 线程相关
        private YurishBaseiHandler mHandler;
        private NormalRunnable runable_Alpha;
        private NormalRunnable runabla_Beta;
        /// <summary>
        /// 我的回合消息标识
        /// </summary>
        private const int SEND_MY_TRUN_MSG = 0x114;
        /// <summary>
        /// 敌方回合标识
        /// </summary>
        private const int SEND_ENEMY_TRUN_MSG = 0x514;
        /// <summary>
        /// 敌方死亡标识
        /// </summary>
        private const int SEND_ENEMY_DIE_MSG = 0x1919;
        /// <summary>
        /// 我方战败标识
        /// </summary>
        private const int SEND_SIDE_DIE_MSG = 0x810;
        #endregion

        #region 其他子线程
        /// <summary>
        /// 战斗信息保持底部的线程
        /// </summary>
        private ScrollViewStayDownRunnable runnable_StayDown;
        /// <summary>
        /// 控制战斗信息实时保持在最底部的线程
        /// </summary>
        public class ScrollViewStayDownRunnable : Java.Lang.Object, IRunnable
        {
            public Action action;
            public void Run()
            {
                action?.Invoke();
            }
        }
        #endregion

        #region UI控件
        /// <summary>
        /// 查看战斗信息
        /// </summary>
        private ImageView iv_Battle_Info;
        /// <summary>
        /// 怪物等级
        /// </summary>
        private TextView tv_Level;
        /// <summary>
        /// 怪物名字
        /// </summary>
        private TextView tv_MonsterName;
        /// <summary>
        /// 怪物血条
        /// </summary>
        private ProgressBar pb_MonsterHp;
        /// <summary>
        /// 怪物立绘
        /// </summary>
        private ImageView iv_MonsterPic;
        /// <summary>
        /// 技能或物品菜单
        /// </summary>
        private ListView lv_SkillOrItems;
        /// <summary>
        /// 战斗信息滚动栏
        /// </summary>
        private ScrollView sv_Battle_Msg;
        /// <summary>
        /// 战斗信息添加控件
        /// </summary>
        private LinearLayout ll_Battle_Msg;
        /// <summary>
        /// 操作主菜单
        /// </summary>
        private LinearLayout ll_Main_Menu;
        /// <summary>
        /// 操作战斗菜单
        /// </summary>
        private LinearLayout ll_Battle_Menu;
        /// <summary>
        /// 操作攻击菜单
        /// </summary>
        private LinearLayout ll_Attack_Menu;
        /// <summary>
        /// 攻击
        /// </summary>
        private Button bt_Attack;
        /// <summary>
        /// 使用道具
        /// </summary>
        private Button bt_Item;
        /// <summary>
        /// 逃跑
        /// </summary>
        private Button bt_Runaway;
        /// <summary>
        /// 普通攻击
        /// </summary>
        private Button bt_Normal_Attack;
        /// <summary>
        /// 战技攻击
        /// </summary>
        private Button bt_Skill_Attack;
        /// <summary>
        /// 魔法攻击
        /// </summary>
        private Button bt_Magic_Attack;
        /// <summary>
        /// 角色头像
        /// </summary>
        private ImageView iv_CharPic;
        /// <summary>
        /// 角色血量
        /// </summary>
        private TextView tv_Hp;
        /// <summary>
        /// 角色魔法
        /// </summary>
        private TextView tv_Mp;
        /// <summary>
        /// 角色体力
        /// </summary>
        private TextView tv_Sp;
        #endregion

        #region 相关数据
        /// <summary>
        /// 处于什么菜单
        /// </summary>
        private MenuIndex menuIndex;
        /// <summary>
        /// 地图Id
        /// </summary>
        private int mapPkId;
        /// <summary>
        /// 角色等级
        /// </summary>
        private int charaLevel;
        /// <summary>
        /// 栖息在此地的怪物列表
        /// </summary>
        private List<Model_Monster> list_Monster;
        private Model_ProducerInfo ProducerInfo;
        private Model_Monster BattleMonsterInfo;
        private Model_PlayerControllCharacter BattleCharacterInfo;
        /// <summary>
        /// 技能Adapter
        /// </summary>
        private BattleSkillAdapter adapter_Skill;
        /// <summary>
        /// 道具Adapter
        /// </summary>
        private BattleItemAdapter adapter_Item;
        #endregion

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_battle_main;
        }

        public override void B_BeforeInitView()
        {
            mapPkId = Intent.GetIntExtra(IMAS_Constants.MapPagePkIdKey, -1);
            charaLevel = Intent.GetIntExtra(IMAS_Constants.CharacterLevelKey, -1);
            #region 初始化线程
            mHandler = new YurishBaseiHandler();
            mHandler.handlerAction += RunHandlerAction;
            runable_Alpha = new NormalRunnable(mHandler, SEND_MY_TRUN_MSG);
            runabla_Beta = new NormalRunnable(mHandler, SEND_ENEMY_TRUN_MSG);
            runnable_StayDown = new ScrollViewStayDownRunnable();
            runnable_StayDown.action += ScrollViewStayDownAction;
            #endregion
        }

        public override void C_InitView()
        {
            iv_Battle_Info = FindViewById<ImageView>(Resource.Id.iv_battle_info);
            tv_Level = FindViewById<TextView>(Resource.Id.tv_level);
            tv_MonsterName = FindViewById<TextView>(Resource.Id.tv_monster_name);
            pb_MonsterHp = FindViewById<ProgressBar>(Resource.Id.pb_monster_hp);
            iv_MonsterPic = FindViewById<ImageView>(Resource.Id.iv_monster_pic);
            lv_SkillOrItems = FindViewById<ListView>(Resource.Id.lv_skill_or_item);
            sv_Battle_Msg = FindViewById<ScrollView>(Resource.Id.sv_battle_info);
            ll_Battle_Msg = FindViewById<LinearLayout>(Resource.Id.ll_battle_info);
            ll_Main_Menu = FindViewById<LinearLayout>(Resource.Id.ll_main_menu);
            ll_Battle_Menu = FindViewById<LinearLayout>(Resource.Id.ll_battle_menu);
            ll_Attack_Menu = FindViewById<LinearLayout>(Resource.Id.ll_attack_menu);
            bt_Attack = FindViewById<Button>(Resource.Id.bt_attack);
            bt_Item = FindViewById<Button>(Resource.Id.bt_item);
            bt_Runaway = FindViewById<Button>(Resource.Id.bt_runaway);
            bt_Normal_Attack = FindViewById<Button>(Resource.Id.bt_normal);
            bt_Skill_Attack = FindViewById<Button>(Resource.Id.bt_skill);
            bt_Magic_Attack = FindViewById<Button>(Resource.Id.bt_magic);
            iv_CharPic = FindViewById<ImageView>(Resource.Id.iv_character_avatar);
            tv_Hp = FindViewById<TextView>(Resource.Id.tv_HP);
            tv_Mp = FindViewById<TextView>(Resource.Id.tv_MP);
            tv_Sp = FindViewById<TextView>(Resource.Id.tv_SP);

        }

        public override void D_BindEvent()
        {
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
            sv_Battle_Msg.ViewTreeObserver.RemoveGlobalOnLayoutListener(this);
            sv_Battle_Msg.ViewTreeObserver.AddOnGlobalLayoutListener(this);
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
            iv_Battle_Info.Click += OnClickListener;
            bt_Attack.Click += OnClickListener;
            bt_Item.Click += OnClickListener;
            bt_Runaway.Click += OnClickListener;
            bt_Normal_Attack.Click += OnClickListener;
            bt_Skill_Attack.Click += OnClickListener;
            bt_Magic_Attack.Click += OnClickListener;
            lv_SkillOrItems.ItemClick += OnAdapterItemClickListener;
            lv_SkillOrItems.ItemLongClick += OnAdapterItemLongClickListener;
        }

        public override void E_InitData()
        {
            adapter_Item = new BattleItemAdapter(this);
            adapter_Skill = new BattleSkillAdapter(this);

            SQLiteQProducerInfo();
            SQLiteQProducerCharacterInfo();
            SQLiteQMonsterInfo();
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
            switch (v.Id)
            {
                case Resource.Id.iv_battle_info:
                    {
                        //战斗信息切换栏
                        if (iv_Battle_Info.Selected)
                        {
                            iv_Battle_Info.Selected = false;
                            IntoBattleControllMenu();
                        }
                        else
                        {
                            iv_Battle_Info.Selected = true;
                            IntoBattleMessageMenu();
                        }
                    }
                    break;
                case Resource.Id.bt_attack:
                    {
                        //战斗
                        menuIndex = MenuIndex.Battle;
                        IntoBattleMenu();
                    }
                    break;
                case Resource.Id.bt_item:
                    {
                        //使用道具
                        menuIndex = MenuIndex.Item;
                        IntoItemMenu();
                    }
                    break;
                #region 战斗指令
                case Resource.Id.bt_normal:
                    {
                        //普通攻击
                        var dmg = ACC_BattleAbout.Acc_NormalAttack(BattleCharacterInfo, BattleMonsterInfo);
                        //if (BattleMonsterInfo.HpPotency < 0)
                        //{
                        //    mHandler.SendMessageDelayed(mHandler.ObtainMessage(SEND_ENEMY_DIE_MSG), LongTime(2.5));
                        //    return;
                        //}
                        IntoBattleMessageMenu();
                        if (IsMonsterBeDefeat())
                            return;
                        mHandler.PostDelayed(() =>
                        {
                            AddBattleMsg($"{BattleCharacterInfo.Name}の普通攻撃");
                        }, LongTime(1));
                        mHandler.PostDelayed(() =>
                        {
                            AddBattleMsg($"{BattleMonsterInfo.Name}は {dmg} のダメージお受けた");
                            SetMonsterInfo(BattleMonsterInfo);
                        }, LongTime(2));
                        mHandler.PostDelayed(runabla_Beta, LongTime(2.1));
                    }
                    break;
                case Resource.Id.bt_skill:
                    {
                        //使用战技
                        menuIndex = MenuIndex.Skill;
                        IntoSkillMenu();
                    }
                    break;
                case Resource.Id.bt_magic:
                    {
                        //使用魔法
                        menuIndex = MenuIndex.Magic;
                        IntoMagicMenu();
                    }
                    break;
                #endregion
                case Resource.Id.bt_runaway:
                    {
                        if (ACC_BattleAbout.Acc_Runnaway())
                        {
                            ShowMsgLong("逃げました");
                            this.Finish();
                        }
                        else
                        {
                            ShowMsgLong("逃げならないでした");
                            IntoBattleMessageMenu();
                            mHandler.PostDelayed(runabla_Beta, LongTime(1));
                        }

                    }
                    break;
            }
        }

        /// <summary>
        /// 接受Msg 做处理
        /// </summary>
        /// <param name="msg"></param>
        private void RunHandlerAction(Message msg)
        {
            switch (msg.What)
            {
                case SEND_MY_TRUN_MSG:
                    {//己方回合
                        AddBattleMsg("===== おれのターン! =====");
                        IntoMainMenu();
                        SetCharacterInfo(BattleCharacterInfo);
                    }
                    break;
                case SEND_ENEMY_TRUN_MSG:
                    {//敌方回合
                        AddBattleMsg("===== 敵のターン! =====");
                        var rDamage = ACC_BattleAbout.Acc_MonsterAttack(BattleMonsterInfo, BattleCharacterInfo);
                        if (rDamage == -2)
                        {//如果触发了怪物技能
                            rDamage = ACC_BattleAbout.Acc_MonsterHeavyAttack(BattleMonsterInfo, BattleCharacterInfo);
                            mHandler.PostDelayed(() =>
                            {
                                AddBattleMsg($"{BattleMonsterInfo.Name}の強力攻撃！");
                            }, LongTime(1));
                            mHandler.PostDelayed(() =>
                            {
                                AddBattleMsg($"{BattleCharacterInfo.Name}は {rDamage} のダメージを受けた、痛いそう");
                            }, LongTime(2));
                        }
                        else
                        {
                            mHandler.PostDelayed(() =>
                            {
                                AddBattleMsg($"{BattleMonsterInfo.Name}の普通攻撃！");
                            }, LongTime(1));
                            mHandler.PostDelayed(() =>
                            {
                                AddBattleMsg($"{BattleCharacterInfo.Name}は {rDamage} のダメージを受けた");
                            }, LongTime(2));
                        }
                        if (BattleCharacterInfo.HealthPoint <= 0)
                        {
                            //角色战败
                            mHandler.SendMessageDelayed(mHandler.ObtainMessage(SEND_SIDE_DIE_MSG), LongTime(3));
                            return;
                        }
                        mHandler.PostDelayed(runable_Alpha, LongTime(5));
                    }
                    break;
                case SEND_ENEMY_DIE_MSG:
                    {//敌方死亡
                        AddBattleMsg($"┏━━━━━━━┓");
                        AddBattleMsg($"┃\t\t********\t\t\t\t┃");
                        AddBattleMsg($"┃\t\t**勝利**\t┃ ");
                        AddBattleMsg($"┃\t\t********\t\t\t\t┃");
                        AddBattleMsg($"┗━━━━━━━┛");
                        ACC_BattleAbout.Acc_MonsterdBeDefeat(BattleMonsterInfo, BattleCharacterInfo, ProducerInfo);
                        ShowConfim(null, "どうする？", (j, k) =>
                        {
                            AddBattleMsg($"敵を探す...");
                            mHandler.PostDelayed(() =>
                            {
                                IntoBattleControllMenu();
                                PickOneMonster();
                            }, LongTime(1.5));
                        }, (j, k) =>
                        {
                            this.Finish();
                        }, "続く", "帰る");

                    }
                    break;
                case SEND_SIDE_DIE_MSG:
                    {
                        AddBattleMsg($"┏━━━━━━━┓");
                        AddBattleMsg($"┃\t\t********\t\t\t\t┃");
                        AddBattleMsg($"┃\t\t**败北**\t┃ ");
                        AddBattleMsg($"┃\t\t********\t\t\t\t┃");
                        AddBattleMsg($"┗━━━━━━━┛");
                        ShowSureConfim(null, "負けました、キャラクターの治療のためにマニーを15%失った", (j, k) =>
                        {
                            mHandler.PostDelayed(() =>
                            {
                                ACC_BattleAbout.Acc_CharacterBeDefeat(BattleCharacterInfo, ProducerInfo);
                                QuitTheBattle();
                            }, LongTime(0.5));
                        }, "はい", false);

                    }
                    break;
            }
        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {
            var dmg = 0;
            switch (v.Id)
            {
                case Resource.Id.lv_skill_or_item:
                    {
                        switch (menuIndex)
                        {
                            case MenuIndex.Item:
                                {
                                    var item = adapter_Item[e.Position];
                                    dmg = ACC_HpMpSpState.UseHealingItemsAcc(BattleCharacterInfo, item);
                                    switch (dmg)
                                    {
                                        case -1:
                                            {
                                                ShowMsgLong("Hpの回復の必要ない");
                                            }
                                            break;
                                        case -2:
                                            {
                                                ShowMsgLong("Mpの回復の必要ない");
                                            }
                                            break;
                                        case -3:
                                            {
                                                ShowMsgLong("Spの回復の必要ない");
                                            }
                                            break;
                                        default:
                                            SetCharacterInfo(BattleCharacterInfo);
                                            IntoBattleMessageMenu();
                                            mHandler.PostDelayed(() =>
                                            {
                                                AddBattleMsg($"{BattleCharacterInfo.Name}は{item.Name}を使いえた");
                                            }, LongTime(1));
                                            mHandler.PostDelayed(() =>
                                            {
                                                if (item.HealingHealthPoint > 0)
                                                {
                                                    AddBattleMsg($"{BattleCharacterInfo.Name}は {item.HealingHealthPoint} のHpを回復した");
                                                }
                                                else if (item.HealingManaPoint > 0)
                                                {
                                                    AddBattleMsg($"{BattleCharacterInfo.Name}は {item.HealingManaPoint} のMpを回復した");
                                                }
                                                else if (item.HealingStaminaPoint > 0)
                                                {
                                                    AddBattleMsg($"{BattleCharacterInfo.Name}は {item.HealingStaminaPoint} のSpを回復した");
                                                }
                                            }, LongTime(2));
                                            var list_data = adapter_Item.DataList.ToList();
                                            list_data.Remove(item);
                                            adapter_Item.SetDataList(list_data);
                                            mHandler.PostDelayed(runabla_Beta, LongTime(2.1));
                                            break;
                                    }

                                }
                                break;
                            case MenuIndex.Skill:
                                {
                                    var skill = adapter_Skill[e.Position];
                                    if (!IsPonitEnough(skill))
                                        return;
                                    dmg = ACC_BattleAbout.Acc_SkillOrMagicAttack(skill, BattleMonsterInfo, 0, BattleCharacterInfo.STR);
                                    //消耗
                                    BattleCharacterInfo.HealthPoint -= skill.CostHp;
                                    BattleCharacterInfo.ManaPoint -= skill.CostMp;
                                    BattleCharacterInfo.StaminaPoint -= skill.CostSp;

                                    IntoBattleMessageMenu();
                                    if (IsMonsterBeDefeat())
                                        return;
                                    mHandler.PostDelayed(() =>
                                    {
                                        AddBattleMsg($"{BattleCharacterInfo.Name}は{skill.Name}を使いえた");
                                    }, LongTime(1));
                                    mHandler.PostDelayed(() =>
                                    {
                                        SetMonsterInfo(BattleMonsterInfo);
                                        AddBattleMsg($"{BattleMonsterInfo.Name}は {dmg} の{EnumDescription.GetFieldText(skill.SkillType)}ダメージを受けた");
                                    }, LongTime(2));
                                    mHandler.PostDelayed(runabla_Beta, LongTime(2.1));
                                    SetCharacterInfo(BattleCharacterInfo);
                                }
                                break;
                            case MenuIndex.Magic:
                                {
                                    var magic = adapter_Skill[e.Position];
                                    if (!IsPonitEnough(magic))
                                        return;
                                    dmg = ACC_BattleAbout.Acc_SkillOrMagicAttack(magic, BattleMonsterInfo, BattleCharacterInfo.INT, 0);
                                    //消耗
                                    BattleCharacterInfo.HealthPoint -= magic.CostHp;
                                    BattleCharacterInfo.ManaPoint -= magic.CostMp;
                                    BattleCharacterInfo.StaminaPoint -= magic.CostSp;

                                    IntoBattleMessageMenu();
                                    if (IsMonsterBeDefeat())
                                        return;
                                    mHandler.PostDelayed(() =>
                                    {
                                        AddBattleMsg($"{BattleCharacterInfo.Name}は{magic.Name}を使いえた");
                                    }, LongTime(1));
                                    mHandler.PostDelayed(() =>
                                    {
                                        SetMonsterInfo(BattleMonsterInfo);
                                        AddBattleMsg($"{BattleMonsterInfo.Name}は {dmg} の{EnumDescription.GetFieldText(magic.SkillType)}ダメージを受けた");
                                    }, LongTime(2));
                                    mHandler.PostDelayed(runabla_Beta, LongTime(2.1));
                                    SetCharacterInfo(BattleCharacterInfo);
                                }
                                break;
                        }
                    }
                    break;
            }

        }
        private void OnAdapterItemLongClickListener(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var v = sender as View;
            switch (v.Id)
            {
                case Resource.Id.lv_skill_or_item:
                    {
                        //显示技能或是物品的详情
                        switch (menuIndex)
                        {
                            case MenuIndex.Item:
                                {
                                    var item = adapter_Item[e.Position];
                                    ShowItemIllustratePopWindow(v, item.ItemIllustrate);
                                }
                                break;
                            case MenuIndex.Skill:
                                {
                                    var skill = adapter_Skill[e.Position];
                                    if (skill.CostHp > 0)
                                    {
                                        ShowItemIllustratePopWindow(v, $"{skill.SkillIllustrate}\nCOST:{skill.CostHp} HP");
                                    }
                                    else if (skill.CostSp > 0)
                                    {
                                        ShowItemIllustratePopWindow(v, $"{skill.SkillIllustrate}\nCOST:{skill.CostSp} SP");
                                    }
                                }
                                break;
                            case MenuIndex.Magic:
                                {
                                    var magic = adapter_Skill[e.Position];
                                    if (magic.CostMp > 0)
                                    {
                                        ShowItemIllustratePopWindow(v, $"{magic.SkillIllustrate}\nCOST:{magic.CostMp} MP");
                                    }
                                    else if (magic.CostHp > 0)
                                    {
                                        ShowItemIllustratePopWindow(v, $"{magic.SkillIllustrate}\nCOST:{magic.CostHp} HP");
                                    }
                                }
                                break;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 检测怪物是否死亡
        /// </summary>
        private bool IsMonsterBeDefeat()
        {
            if (BattleMonsterInfo.HealthPoint < 0)
            {
                mHandler.SendMessageDelayed(mHandler.ObtainMessage(SEND_ENEMY_DIE_MSG), LongTime(0.5));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检测是否能够使用技能
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        private bool IsPonitEnough(Model_Skills skill)
        {
            if (BattleCharacterInfo.HealthPoint < skill.CostHp)
            {
                ShowMsgLong("Hpが足りない");
                return false;
            }
            if (BattleCharacterInfo.ManaPoint < skill.CostMp)
            {
                ShowMsgLong("Mpが足りない");
                return false;
            }
            if (BattleCharacterInfo.StaminaPoint < skill.CostSp)
            {
                ShowMsgLong("Spが足りない");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 保持低调
        /// </summary>
        private void ScrollViewStayDownAction()
        {
            sv_Battle_Msg.FullScroll(FocusSearchDirection.Down);
        }

        /// <summary>
        /// 标准化延迟时间
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private long LongTime(long time)
        {
            return time * 1000;
        }
        private long LongTime(double time)
        {
            return Convert.ToInt64(time * 1000);
        }

        /// <summary>
        /// 退出战斗
        /// </summary>
        private void QuitTheBattle()
        {
            SQLiteUpdateProducerInfo();
            SQLiteUpdateCharacterInfo();
        }

        /// <summary>
        /// 技能道具详情
        /// </summary>
        private PopupWindowHelper popHelper;
        /// <summary>
        /// 显示技能道具详情
        /// </summary>
        /// <param name="v"></param>
        /// <param name="itemIll"></param>
        private void ShowItemIllustratePopWindow(View v, string itemIll)
        {
            var popView = LayoutInflater.From(this).Inflate(Resource.Layout.popwin_item_illustrate, null);
            popView.FindViewById<TextView>(Resource.Id.tv_Illustrate).Text = itemIll;
            popHelper = new PopupWindowHelper(popView);
            DisplayMetrics metrics = Resources.DisplayMetrics;
            int width = metrics.WidthPixels;
            int height = metrics.HeightPixels;
            popHelper.ShowAtLocation(v, GravityFlags.Center, 0, 0);
        }

        #region 菜单的切换
        /// <summary>
        /// 切换到战斗菜单
        /// </summary>
        private void IntoBattleMenu()
        {
            CoverUIControl(CoverFlag.Gone, ll_Main_Menu, lv_SkillOrItems);
            CoverUIControl(CoverFlag.Visible, ll_Attack_Menu);
        }
        /// <summary>
        /// 切换到主菜单
        /// </summary>
        private void IntoMainMenu()
        {
            CoverUIControl(CoverFlag.Gone, ll_Attack_Menu, lv_SkillOrItems, sv_Battle_Msg);
            CoverUIControl(CoverFlag.Visible, ll_Main_Menu, ll_Battle_Menu);
        }
        /// <summary>
        /// 切换到技能菜单
        /// </summary>
        private void IntoSkillMenu()
        {
            CoverUIControl(CoverFlag.Gone, ll_Attack_Menu);
            CoverUIControl(CoverFlag.Visible, lv_SkillOrItems);
            if (lv_SkillOrItems.Adapter == null || lv_SkillOrItems.Adapter is BattleItemAdapter)
            {
                lv_SkillOrItems.Adapter = adapter_Skill;
            }
            var dataList = BattleCharacterInfo.ChartSkills.ToObject<List<Model_Skills>>();
            var data = dataList.Where(r => r.SkillMode == IMAS.Tips.Enums.SkillsMode.Attack).ToList();
            adapter_Skill.SetDataList(data);
        }
        /// <summary>
        /// 切换到魔法菜单
        /// </summary>
        private void IntoMagicMenu()
        {
            CoverUIControl(CoverFlag.Gone, ll_Attack_Menu);
            CoverUIControl(CoverFlag.Visible, lv_SkillOrItems);
            if (lv_SkillOrItems.Adapter == null || lv_SkillOrItems.Adapter is BattleItemAdapter)
            {
                lv_SkillOrItems.Adapter = adapter_Skill;
            }
            var dataList = BattleCharacterInfo.ChartSkills.ToObject<List<Model_Skills>>();
            var data = dataList.Where(r => r.SkillMode == IMAS.Tips.Enums.SkillsMode.Mana || r.SkillMode == IMAS.Tips.Enums.SkillsMode.Mind).ToList();
            adapter_Skill.SetDataList(data);
        }
        /// <summary>
        /// 切换到物品菜单
        /// </summary>
        private void IntoItemMenu()
        {
            var dataList = ProducerInfo.Items_Healing.ToObject<List<Model_Items>>();
            if (dataList == null || !dataList.Any())
            {
                ShowSureConfim(null, "アイテムいません", null);
                return;
            }
            CoverUIControl(CoverFlag.Gone, ll_Main_Menu);
            CoverUIControl(CoverFlag.Visible, lv_SkillOrItems);
            if (lv_SkillOrItems.Adapter == null || lv_SkillOrItems.Adapter is BattleSkillAdapter)
            {
                lv_SkillOrItems.Adapter = adapter_Item;
            }
            adapter_Item.SetDataList(dataList);
        }

        /// <summary>
        /// 切换到战斗信息菜单
        /// </summary>
        private void IntoBattleMessageMenu()
        {
            menuIndex = MenuIndex.Information;
            CoverUIControl(CoverFlag.Gone, ll_Battle_Menu);
            CoverUIControl(CoverFlag.Visible, sv_Battle_Msg);
        }
        /// <summary>
        /// 切换到战斗信息菜单
        /// </summary>
        private void IntoBattleControllMenu()
        {
            menuIndex = MenuIndex.Main;
            CoverUIControl(CoverFlag.Gone, sv_Battle_Msg, lv_SkillOrItems, ll_Attack_Menu);
            CoverUIControl(CoverFlag.Visible, ll_Battle_Menu, ll_Main_Menu);
        }
        #endregion

        /// <summary>
        /// 添加战斗信息
        /// </summary>
        /// <param name="msg">添加的内容</param>
        private void AddBattleMsg(string msg)
        {
            var layoutParmas = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            {
                TopMargin = 5
            };
            var textView = new TextView(this)
            {
                TextSize = 14,
                Text = msg,
                LayoutParameters = layoutParmas
            };
            textView.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.black));
            ll_Battle_Msg.AddView(textView);
            DontBlindMove = true;
        }

        /// <summary>
        /// 选择一个怪物进行战斗
        /// </summary>
        private void PickOneMonster()
        {
            var raPick = new System.Random();
            BattleMonsterInfo = list_Monster[raPick.Next(list_Monster.Count)];
            RandomMonsterData(BattleMonsterInfo);
            SetMonsterInfo(BattleMonsterInfo);
        }

        /// <summary>
        /// 设置整体页面的信息
        /// </summary>
        /// <param name="mData"></param>
        /// <param name="cData"></param>
        private void SetAllPageText(Model_Monster mData = null, Model_PlayerControllCharacter cData = null)
        {
            if (mData != null)
            {
                SetMonsterInfo(mData);
            }
            if (cData != null)
            {
                SetCharacterInfo(cData);
            }
        }

        /// <summary>
        /// 设置怪物的信息
        /// </summary>
        /// <param name="data"></param>
        private void SetMonsterInfo(Model_Monster data)
        {
            decimal hp = Convert.ToDecimal(data.HealthPoint);
            decimal mhp = Convert.ToDecimal(data.MaxHealthPoint);
            var jk = hp / mhp;
            SetMonsterHpProgress(jk);
            tv_MonsterName.Text = data.Name;
            tv_Level.Text = $"Lv.{data.Level}";

            ImageLoader.Instance.DisplayImage(data.Pic_Path, iv_MonsterPic, ImageLoaderHelper.CharacterPicImageOption());
        }
        /// <summary>
        /// 设置怪物血量
        /// </summary>
        /// <param name="hp"></param>
        private void SetMonsterHpProgress(decimal hp)
        {
            hp = hp * 100;
            pb_MonsterHp.Progress = (int)hp;
        }
        /// <summary>
        /// 设置角色的信息
        /// </summary>
        /// <param name="data"></param>
        private void SetCharacterInfo(Model_PlayerControllCharacter data)
        {
            tv_Hp.Text = $"{data.HealthPoint} / {data.MaxHealthPoint}";
            tv_Mp.Text = $"{data.ManaPoint} / {data.MaxManaPoint}";
            tv_Sp.Text = $"{data.StaminaPoint} / {data.MaxStaminaPoint}";

            var jk = data.HealthPoint / data.MaxHealthPoint;
            jk *= 100;
            if (jk <= 15)
            {
                tv_Hp.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.darkred));
            }
            else
            {
                tv_Hp.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.black));
            }
        }

        #region 加工怪物
        private decimal[] levelStack = new decimal[] { 0.50m, 0.85m, 1.00m, 1.50m, 2.00m, 5.00m };
        private string[] nameStack = new string[] { "受伤的", "弱小的", "", "强壮的", "凶狠的", "千年的" };

        /// <summary>
        /// 随机化怪物战斗力
        /// </summary>
        /// <param name="item"></param>
        private void RandomMonsterData(Model_Monster item)
        {
            var ra = new System.Random();
            var indext = ra.Next(levelStack.Length);
            item.Level = Convert.ToInt32(charaLevel * levelStack[indext]);
            if (item.Level < 1)
                item.Level = 1;
            item.Name = $"{nameStack[indext]} {item.Name}";
            ACC_MonsterCreat.CreatAccMonsterData(item);
        }
        #endregion

        #region SQLite相关

        #region 获取信息
        /// <summary>
        /// 获取制作人信息
        /// </summary>
        private void SQLiteQProducerInfo()
        {
            ShowWaitDiaLog("Loading...");
            var p_Id = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.SpProducerInfoId);
            Task.Run(async () =>
            {
                return await IMAS_ProAppDBManager.GetInstance().QProducerInfo(p_Id);
            }).ContinueWith(t =>
            {
                HideWaitDiaLog();
                if (t.Exception != null)
                {
                    ShowBigToast("エラー");
                    return;
                }
                if (t.Result.IsSuccess)
                {
                    ProducerInfo = t.Result.Data;
                }
                else
                {
                    ProducerInfo = new Model_ProducerInfo();
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        private void SQLiteQProducerCharacterInfo()
        {
            var p_Id = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.SpProducerInfoId);

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
                        BattleCharacterInfo = data;
                        ImageLoader.Instance.DisplayImage(data.CharacterTachie, iv_CharPic, ImageLoaderHelper.CircleImageOptions());
                        SetCharacterInfo(data);
                    }
                }
                else
                {
                    ShowDiyToastLong(t.Result.Message);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// 获取怪物信息
        /// </summary>
        private void SQLiteQMonsterInfo()
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QBattleMapMonsterInfomation(mapPkId);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    WriteLogFile(IMAS.Utils.Logs.LogLevel.Error, IMAS_Constants.Log_SQLiteErrorKey, "GetMonsterError" + t.Exception.Message.ToString());
                    return;
                }
                if (t.Result.IsSuccess)
                {
                    var data = t.Result.Data;
                    list_Monster = data;
                    PickOneMonster();
                }
                else
                {
                    list_Monster = new List<Model_Monster>();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext()); ;
        }
        #endregion

        #region 更新信息
        /// <summary>
        /// 更新制作人信息
        /// </summary>
        private void SQLiteUpdateProducerInfo()
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().UpdateProducerInfo(ProducerInfo);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    WriteLogFile(IMAS.Utils.Logs.LogLevel.Error, IMAS_Constants.Log_SQLiteErrorKey, "UpdateProcuderInfoError" + t.Exception.Message.ToString());
                    return;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext()); ;
        }
        /// <summary>
        /// 更新制作人角色信息
        /// </summary>
        private void SQLiteUpdateCharacterInfo()
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().UpdateProducingCharacterInfo(BattleCharacterInfo);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    WriteLogFile(IMAS.Utils.Logs.LogLevel.Error, IMAS_Constants.Log_SQLiteErrorKey, "UpdateCharacterInfoError" + t.Exception.Message.ToString());
                    return;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext()); ;
        }
        #endregion

        #endregion

        #region 重写接口的方法
        /// <summary>
        /// 控制ScrollView别瞎JB动的变量
        /// </summary>
        private bool DontBlindMove = false;
        /// <summary>
        /// 当ScrollView发生变动时触发
        /// </summary>
        public void OnGlobalLayout()
        {
            if (DontBlindMove)
            {
                DontBlindMove = !DontBlindMove;
                if (runnable_StayDown != null)
                {
                    runnable_StayDown.Run();
                }
            }

        }

        #endregion

        #region 重写系统的各种方法
        /// <summary>
        /// 关闭所有线程
        /// </summary>
        public override void Finish()
        {
            mHandler.RemoveCallbacksAndMessages(null);
            QuitTheBattle();
            base.Finish();
        }

        /// <summary>
        /// 监听返回键
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
#if !DEBUG
                ShowMsgLong("俺わ逃げられないだよ！");
                return false;
#else
                switch (menuIndex)
                {
                    case MenuIndex.Main:
                    case MenuIndex.Information:
                        {
                            return base.OnKeyDown(keyCode, e);
                        }
                    case MenuIndex.Battle:
                    case MenuIndex.Item:
                        {
                            //如果是战斗菜单或是道具菜单
                            //返回主菜单
                            IntoMainMenu();
                            return false;
                        }
                    case MenuIndex.Magic:
                    case MenuIndex.Skill:
                        {
                            //如果是战技菜单或是魔法菜单
                            //返回战斗菜单
                            IntoBattleMenu();
                            return false;
                        }
                }
                return base.OnKeyDown(keyCode, e);
#endif
            }
            else
            {
                return base.OnKeyDown(keyCode, e);
            }
        }

        #endregion
    }
}