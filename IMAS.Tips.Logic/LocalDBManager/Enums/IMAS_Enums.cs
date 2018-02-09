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
using IMAS.CupCake.Extensions;

namespace IMAS.Tips.Enums
{
    /// <summary>
    /// 章节
    /// </summary>
    public enum ChapterEnumeration
    {
        [EnumDescription("序章")]
        Prologue = 0,
        [EnumDescription("第一章")]
        Chapter_One = 1,
        [EnumDescription("第二章")]
        Chapter_Two = 2,
    }

    /// <summary>
    /// 输入历史
    /// </summary>
    public enum InputHistoryType
    {
        [EnumDescription("制作人姓名")]
        ProducerName = 0x001,
    }

    /// <summary>
    /// 道具类型
    /// </summary>
    public enum ItemEnumeration
    {
        /// <summary>
        /// 恢复类
        /// </summary>
        [EnumDescription("Healing")]
        HealingItems = 0x101,

        /// <summary>
        /// 装备类
        /// </summary>
        [EnumDescription("Equipment")]
        EquipmentItems = 0x102,

        /// <summary>
        /// 武器类
        /// </summary>
        [EnumDescription("Weapon")]
        WeaponItems = 0x103,

        /// <summary>
        /// 道具类
        /// </summary>
        [EnumDescription("OtherItems")]
        OtherItems = 0x104,
    }

    /// <summary>
    /// 怪物生活场所
    /// </summary>
    public enum MonsterLivePlace
    {
        /// <summary>
        /// 百合树
        /// </summary>
        [EnumDescription("YuriTree")]
        YuriTree = 0x01,
        /// <summary>
        /// 凛冬玫瑰
        /// </summary>
        [EnumDescription("WinterRose")]
        WinterRose = 0x02,
        /// <summary>
        /// 香蕉海洋
        /// </summary>
        [EnumDescription("BananaOcean")]
        BananaOcean = 0x03,
    }

    /// <summary>
    /// 怪物类型
    /// </summary>
    public enum MonsterType
    {
        /// <summary>
        /// 植物系
        /// </summary>
        [EnumDescription("Plant")]
        Plant = 0x001,
        /// <summary>
        /// 动物系
        /// </summary>
        [EnumDescription("Animal")]
        Animal = 0x002,
        /// <summary>
        /// 人型
        /// </summary>
        [EnumDescription("Human")]
        Human = 0x003,
        /// <summary>
        /// 西方龙系
        /// </summary>
        [EnumDescription("Dragon")]
        Dragon = 0x004,

    }
    /// <summary>
    /// 技能攻击类型
    /// </summary>
    public enum SkillsMode
    {
        /// <summary>
        /// 物理类型
        /// </summary>
        [EnumDescription("物理")]
        Attack = 1,
        /// <summary>
        /// 魔法类型
        /// </summary>
        [EnumDescription("魔法")]
        Mana = 2,
        /// <summary>
        /// 精神攻击
        /// </summary>
        [EnumDescription("精神")]
        Mind = 3
    }
    /// <summary>
    /// 技能攻击属性
    /// </summary>
    public enum SkillsType
    {
        /// <summary>
        /// 火属性
        /// </summary>
        [EnumDescription("炎")]
        Fire = 0x1,
        /// <summary>
        /// 水属性
        /// </summary>
        [EnumDescription("氷")]
        Water = 0x2,
        /// <summary>
        /// 木属性
        /// </summary>
        [EnumDescription("木")]
        Timber = 0x3,
        /// <summary>
        /// 光属性
        /// </summary>
        [EnumDescription("光")]
        Light = 0x4,
        /// <summary>
        /// 暗属性
        /// </summary>
        [EnumDescription("闇")]
        Dark = 0x5,
    }
}