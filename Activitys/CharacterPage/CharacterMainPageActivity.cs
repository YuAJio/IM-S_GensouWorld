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
using IMAS.Utils.Sp;
using IMAS.LocalDBManager.Models;
using IMAS.CupCake.Extensions;
using Com.Nostra13.Universalimageloader.Core;
using IdoMaster_GensouWorld.Utils;
using IMAS.Tips.Enums;
using Android.Util;
using CustomControl;
using IdoMaster_GensouWorld.Adapters;
using IMAS.Accounting;
using Android.Support.Percent;
using Android.Support.V4.Content;

namespace IdoMaster_GensouWorld.Activitys.CharacterPage
{
    /// <summary>
    /// 角色信息页面
    /// </summary>
    [Activity(Label = "CharacterMainPageActivity", Theme = "@style/Theme.PublicTheme")]
    public class CharacterMainPageActivity : BaseActivity
    {
        #region UI控件
        /// <summary>
        /// 返回
        /// </summary>
        private ImageView iv_back;
        /// <summary>
        /// 名字
        /// </summary>
        private TextView tv_Name;
        /// <summary>
        /// 生命值
        /// </summary>
        private TextView tv_Hp;
        /// <summary>
        /// 魔法值
        /// </summary>
        private TextView tv_Mp;
        /// <summary>
        /// 体力值
        /// </summary>
        private TextView tv_Sp;
        /// <summary>
        /// 经验值
        /// </summary>
        private ProgressBar pb_exp;
        /// <summary>
        /// 装备武器
        /// </summary>
        private TextView tv_Weapon;
        /// <summary>
        /// 装备衣物
        /// </summary>
        private TextView tv_Armour;
        /// <summary>
        /// 装备饰品
        /// </summary>
        private TextView tv_Kazari;
        /// <summary>
        /// 攻击力
        /// </summary>
        private TextView tv_Att;
        /// <summary>
        /// 防御力
        /// </summary>
        private TextView tv_Def;
        /// <summary>
        /// 技能列表
        /// </summary>
        private Button bt_Skill;
        /// <summary>
        /// 角色立绘
        /// </summary>
        private ImageView iv_Character;
        /// <summary>
        /// 武器装备
        /// </summary>
        private LinearLayout ll_Weapon;
        /// <summary>
        /// 衣服装备
        /// </summary>
        private LinearLayout ll_Equip;
        /// <summary>
        /// 其他装备
        /// </summary>
        private LinearLayout ll_Other;
        #endregion
        /// <summary>
        /// 角色Id
        /// </summary>
        private int id_Character;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_character_main_page;
        }

        public override void B_BeforeInitView()
        {
        }

        public override void C_InitView()
        {
            iv_back = FindViewById<ImageView>(Resource.Id.iv_back);
            tv_Name = FindViewById<TextView>(Resource.Id.tv_name);
            tv_Hp = FindViewById<TextView>(Resource.Id.tv_HP);
            tv_Mp = FindViewById<TextView>(Resource.Id.tv_MP);
            tv_Sp = FindViewById<TextView>(Resource.Id.tv_SP);
            pb_exp = FindViewById<ProgressBar>(Resource.Id.pb_exp);
            tv_Att = FindViewById<TextView>(Resource.Id.tv_Att);
            tv_Def = FindViewById<TextView>(Resource.Id.tv_Def);
            tv_Weapon = FindViewById<TextView>(Resource.Id.tv_weapon);
            tv_Armour = FindViewById<TextView>(Resource.Id.tv_clothes);
            tv_Kazari = FindViewById<TextView>(Resource.Id.tv_ornament);
            bt_Skill = FindViewById<Button>(Resource.Id.bt_skill_List);
            ll_Weapon = FindViewById<LinearLayout>(Resource.Id.ll_weapon);
            ll_Equip = FindViewById<LinearLayout>(Resource.Id.ll_clothes);
            ll_Other = FindViewById<LinearLayout>(Resource.Id.ll_ornament);
            iv_Character = FindViewById<ImageView>(Resource.Id.iv_char_body);
        }

        public override void D_BindEvent()
        {
            iv_back.Click += OnClickListener;
            bt_Skill.Click += OnClickListener;
            iv_Character.Click += OnClickListener;
            ll_Weapon.Click += OnClickListener;
            ll_Equip.Click += OnClickListener;
            ll_Other.Click += OnClickListener;
        }

        public override void E_InitData()
        {
            SQLiteQProducerInfo();
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
                case Resource.Id.ll_weapon:
                    {//角色武器
                        ShowChoseSoubiDialog(ItemEnumeration.WeaponItems);
                    }
                    break;
                case Resource.Id.ll_clothes:
                    {//角色服装
                        ShowChoseSoubiDialog(ItemEnumeration.EquipmentItems);
                    }
                    break;
                case Resource.Id.ll_ornament:
                    {//角色饰品
                        ShowChoseSoubiDialog(ItemEnumeration.OtherItems);
                    }
                    break;
                case Resource.Id.bt_skill_List:
                    {//角色技能列表
                        ShowCharacterSkillsDialog();
                    }
                    break;
                case Resource.Id.iv_char_body:
                    {//角色立绘

                    }
                    break;
                case Resource.Id.tv_skill:
                    {
                        ChoseSkillList();
                    }
                    break;
                case Resource.Id.tv_magic:
                    {
                        ChoseMagicList();
                    }
                    break;

            }
        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {
            switch (v.Id)
            {
                case Resource.Id.lv_skills:
                    {
                        var choseSkills = adapter_Skills[e.Position];

                        tv_skillName.Text = choseSkills.Name;
                        tv_skillIntroduce.Text = $"”{choseSkills.SkillIllustrate}”\n敵に{choseSkills.Damage}の{EnumDescription.GetFieldText(choseSkills.SkillMode)}ダメージをあげる,{EnumDescription.GetFieldText(choseSkills.SkillType)}属性";

                        var sb = new StringBuilder();
                        if (choseSkills.CostHp > 0)
                            sb.Append($"CostHp:{choseSkills.CostHp}\t");
                        if (choseSkills.CostMp > 0)
                            sb.Append($"CostMp:{choseSkills.CostMp}\t");
                        if (choseSkills.CostSp > 0)
                            sb.Append($"CostSp:{choseSkills.CostSp}\t");
                        tv_skillCost.Text = sb.ToString();
                        if (!choseSkills.IsSelect)
                        {
                            list_Skills.ForEach(r => r.IsSelect = false);
                            var list = new List<Model_Skills>();
                            if (skillIndext == 1)
                                list = list_Skills.Where(r => r.SkillMode == SkillsMode.Attack).ToList();
                            else if (skillIndext == 2)
                                list = list_Skills.Where(r => r.SkillMode == SkillsMode.Mana || r.SkillMode == SkillsMode.Mind).ToList();
                            list[e.Position].IsSelect = true;
                            adapter_Skills.SetDataList(list);
                        }
                    }
                    break;
            }
        }

        private void OnItemLongPressListener(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var clickItem = adapter_soubi[e.Position];

            ShowItemIllustratePopWindow(e.View, clickItem.ItemIllustrate);
        }

        #region  查看角色技能
        private int skillIndext = 1;
        private void ChoseSkillList()
        {
            if (skillIndext != 1)
            {
                skillIndext = 1;
                var senki = list_Skills.Where(r => r.SkillMode == SkillsMode.Attack).ToList(); ;
                adapter_Skills.SetDataList(senki);
                tv_Skill.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.skyblue));
                tv_Magic.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.dimgrey));
            }
        }
        private void ChoseMagicList()
        {
            if (skillIndext != 2)
            {
                skillIndext = 2;
                var maho = list_Skills.Where(r => r.SkillMode == SkillsMode.Mana || r.SkillMode == SkillsMode.Mind).ToList(); ;
                adapter_Skills.SetDataList(maho);
                tv_Magic.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.skyblue));
                tv_Skill.SetTextColor(ContextCompat.GetColorStateList(this, Resource.Color.dimgrey));
            }
        }
        #region UI控件
        /// <summary>
        /// 技能名称
        /// </summary>
        private TextView tv_skillName;
        /// <summary>
        /// 技能介绍
        /// </summary>
        private TextView tv_skillIntroduce;
        /// <summary>
        /// 技能消耗
        /// </summary>
        private TextView tv_skillCost;

        private TextView tv_Skill;
        private TextView tv_Magic;
        #endregion
        ///// <summary>
        ///// 技能ListView
        ///// </summary>
        //private ListView lv_Skills;
        /// <summary>
        /// 技能列表
        /// </summary>
        private List<Model_Skills> list_Skills;
        /// <summary>
        /// 技能适配器
        /// </summary>
        private CharacterSkillsAdapter adapter_Skills;
        /// <summary>
        /// 显示角色技能
        /// </summary>
        private void ShowCharacterSkillsDialog()
        {
            if (info_character == null)
            {
                return;
            }
            var jk = new AlertDialog.Builder(this);
            var view = View.Inflate(this, Resource.Layout.pop_character_skills, null);
            tv_Skill = view.FindViewById<TextView>(Resource.Id.tv_skill);
            tv_Magic = view.FindViewById<TextView>(Resource.Id.tv_magic);
            var lv_Skills = view.FindViewById<ListView>(Resource.Id.lv_skills);
            if (tv_skillName == null)
            {
                tv_skillName = view.FindViewById<TextView>(Resource.Id.tv_skill_name);
                tv_skillIntroduce = view.FindViewById<TextView>(Resource.Id.tv_skill_introduce);
                tv_skillCost = view.FindViewById<TextView>(Resource.Id.tv_cost_point);
            }
            tv_Skill.Click -= OnClickListener;
            tv_Magic.Click -= OnClickListener;

            tv_Skill.Click += OnClickListener;
            tv_Magic.Click += OnClickListener;

            lv_Skills.ItemClick -= OnAdapterItemClickListener;
            lv_Skills.ItemClick += OnAdapterItemClickListener;

            list_Skills = info_character.ChartSkills.ToObject<List<Model_Skills>>();
            var list = list_Skills.Where(r => r.SkillMode == SkillsMode.Attack).ToList(); ;

            if (adapter_Skills == null)
            {
                adapter_Skills = new CharacterSkillsAdapter(this);
            }
            if (lv_Skills.Adapter == null)
            {
                lv_Skills.Adapter = adapter_Skills;
            }

            adapter_Skills.SetDataList(list);
            jk.SetView(view);
            _ConfigDialog = jk.Show();
            _ConfigDialog.Window.ClearFlags(WindowManagerFlags.DimBehind);
            _ConfigDialog.Window.SetWindowAnimations(Resource.Style.DialogFadeAnimation);
        }

        #endregion

        #region 选择装备
        /// <summary>
        /// 选择装备列表
        /// </summary>
        private ListView lv_selectSouBi;
        /// <summary>
        /// 所持装备列表
        /// </summary>
        private ChoseSoubiListAdapter adapter_soubi;
        /// <summary>
        /// 弹出选择装备列表
        /// </summary>
        /// <param name="itemType"></param>
        private void ShowChoseSoubiDialog(ItemEnumeration itemType)
        {
            var jk = new AlertDialog.Builder(this);
            var view = View.Inflate(this, Resource.Layout.pop_soubi_select, null);
            lv_selectSouBi = view.FindViewById<ListView>(Resource.Id.lv_chose_equip);
            if (adapter_soubi != null)
            {
                lv_selectSouBi.Adapter = adapter_soubi;
            }
            lv_selectSouBi.ItemClick -= OnAlerPopItemClickListener;
            lv_selectSouBi.ItemClick += OnAlerPopItemClickListener;
            lv_selectSouBi.ItemLongClick -= OnItemLongPressListener;
            lv_selectSouBi.ItemLongClick += OnItemLongPressListener;
            jk.SetView(view);

            SQLiteGetProduceItems(itemType, jk);
        }

        /// <summary>
        /// 选择装备ItemClick监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAlerPopItemClickListener(object sender, AdapterView.ItemClickEventArgs e)
        {
            var clickItem = adapter_soubi[e.Position];
            var soubiDict = info_character.ChartEquip.ToObject<Dictionary<string, Model_Items>>();
            var list = adapter_soubi.DataList.ToList();
            if (clickItem.IsSelect)
            {//如果是装备状态
                info_character.AttackPoint -= clickItem.ATTPromote;
                info_character.DefencePoint -= clickItem.DEFPromote;
                //adapter_soubi[e.Position].IsSelect = false;
                //adapter_soubi.NotifyDataSetChanged();
                soubiDict[EnumDescription.GetFieldText(clickItem.ItemType)] = null;
            }
            else
            {//如果是未装备状态
                switch (clickItem.ItemType)
                {
                    case ItemEnumeration.HealingItems:
                        #region 如果是回复道具
                        //var result = ACC_HpMpSpState.UseHealingItemsAcc(info_character, clickItem);
                        //if (result.IsSuccess)
                        //{
                        //    list.RemoveAt(e.Position);
                        //    if (!list.Any())
                        //    {
                        //        _ConfigDialog.Dismiss();
                        //    }
                        //    else
                        //    {
                        //        adapter_soubi.SetDataList(list);
                        //    }
                        //    info_character = result.Data;
                        //}
                        //else
                        //{
                        //    ShowMsgLong(result.Message);
                        //}
                        #endregion
                        break;
                    default:
                        #region 如果是装备型道具
                        //卸下以前的装备
                        var lastSoubi = list.Where(r => r.IsSelect).FirstOrDefault();
                        if (lastSoubi != null)
                        {
                            lastSoubi.IsSelect = false;
                            info_character.AttackPoint -= lastSoubi.ATTPromote;
                            info_character.DefencePoint -= lastSoubi.DEFPromote;
                        }
                        //装备选中的装备
                        soubiDict[EnumDescription.GetFieldText(clickItem.ItemType)] = clickItem;
                        info_character.AttackPoint += clickItem.ATTPromote;
                        info_character.DefencePoint += clickItem.DEFPromote;
                        info_character.ChartEquip = soubiDict.ToJson();
                        list[e.Position].IsSelect = true;//
                        _ConfigDialog.Dismiss();
                        #endregion
                        break;
                }
            }
            SQLiteUpdateProducerItem(list, clickItem.ItemType);
        }
        #endregion
        /// <summary>
        /// 设置此页面Text
        /// </summary>
        private void SetPageTextView(Model_PlayerControllCharacter data)
        {
            tv_Name.Text = data.Name;
            tv_Hp.Text = $"{data.HealthPoint}/{data.MaxHealthPoint}";
            tv_Mp.Text = $"{data.ManaPoint}/{data.MaxManaPoint}";
            tv_Sp.Text = $"{data.StaminaPoint}/{data.MaxStaminaPoint}";
            tv_Att.Text = $"攻撃力：{data.AttackPoint}";
            tv_Def.Text = $"防御力：{data.DefencePoint}";

            var c_Equip = data.ChartEquip.ToObject<Dictionary<string, Model_Items>>();

            var str_Weapon = c_Equip[EnumDescription.GetFieldText(ItemEnumeration.WeaponItems)];
            var str_Equipment = c_Equip[EnumDescription.GetFieldText(ItemEnumeration.EquipmentItems)];
            var str_Other = c_Equip[EnumDescription.GetFieldText(ItemEnumeration.OtherItems)];
            tv_Weapon.Text = str_Weapon == null ? "なし" : str_Weapon.Name;
            tv_Armour.Text = str_Equipment == null ? "なし" : str_Equipment.Name;
            tv_Kazari.Text = str_Other == null ? "なし" : str_Other.Name;
            var jk = data.Exp / data.LevelUpExp;
            pb_exp.Progress = (int)jk;

            ImageLoader.Instance.DisplayImage(data.CharacterTachie, iv_Character, ImageLoaderHelper.CharacterPicImageOption());
        }

        #region 显示道具详情
        private PopupWindowHelper popHelper;

        private void ShowItemIllustratePopWindow(View v, string itemIll)
        {
            var popView = LayoutInflater.From(this).Inflate(Resource.Layout.popwin_item_illustrate, null);
            popView.FindViewById<TextView>(Resource.Id.tv_Illustrate).Text = itemIll;
            popHelper = new PopupWindowHelper(popView);
            popHelper.ShowAsDropDown(v, 0, -10);
        }

        #endregion

        #region SQLite相关
        /// <summary>
        /// CCC信息
        /// </summary>
        private Model_PlayerControllCharacter info_character;
        /// <summary>
        /// 获取角色信息
        /// </summary>
        private void SQLiteQProducerInfo()
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
                        SetPageTextView(data);
                        info_character = data;
                    }
                }
                else
                {
                    ShowDiyToastLong(t.Result.Message);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        /// <summary>
        /// 获取角色身上物品
        /// </summary>
        private void SQLiteGetProduceItems(ItemEnumeration itemType, AlertDialog.Builder builder)
        {
            var p_Id = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.SpProducerInfoId);
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QProducerItemDetails(p_Id, itemType);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    var data = t.Result.Data;
                    if (adapter_soubi == null)
                    {
                        adapter_soubi = new ChoseSoubiListAdapter(this);
                        lv_selectSouBi.Adapter = adapter_soubi;
                    }
                    if (data.Any())
                    {
                        adapter_soubi.SetDataList(data);
                        _ConfigDialog = builder.Show();
                        _ConfigDialog.Window.ClearFlags(WindowManagerFlags.DimBehind);
                        _ConfigDialog.Window.SetWindowAnimations(Resource.Style.DialogFadeAnimation);
                    }
                    else
                    {
                        ShowSureConfim(null, "何もない、空っぽだ", null);
                    }

                }
                else
                {
                    ShowSureConfim(null, "何もない、空っぽだ", null);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        /// <summary>
        /// 更新角色身上道具
        /// </summary>
        /// <param name="list_Items"></param>
        private void SQLiteUpdateProducerItem(List<Model_Items> list_Items, ItemEnumeration itemType)
        {
            var p_Id = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.SpProducerInfoId);
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().UpdateProducerItemInfo(p_Id, list_Items, itemType);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    isUpdateCharaInfo = true;
                    SetPageTextView(info_character);
                }
                else
                {
                    ShowBigToast(t.Result.Message);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private bool isUpdateCharaInfo = false;
        /// <summary>
        /// 更新角色信息
        /// </summary>
        private void SQLiteUpdateCharacterInfo()
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().UpdateProducingCharacterInfo(info_character);
                return result;
            }).ContinueWith(t =>
            {

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion


        public override void Finish()
        {
            if (isUpdateCharaInfo)
            {
                SQLiteUpdateCharacterInfo();
            }
            base.Finish();
        }
    }
}