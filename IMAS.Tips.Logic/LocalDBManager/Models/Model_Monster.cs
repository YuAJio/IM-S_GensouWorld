using IMAS.Tips.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    /// <summary>
    /// 怪物模型
    /// </summary>
    public class Model_Monster : BaseInfo_Character
    {
        /// <summary>
        /// 怪物Id
        /// </summary>
        public long MonsterId { get; set; }
        /// <summary>
        /// 基础生命值
        /// </summary>
        public int BaseHealPoint { get; set; }
        /// <summary>
        /// 基础攻击力
        /// </summary>
        public int BaseAttPoint { get; set; }
        /// <summary>
        /// 基础防御力
        /// </summary>
        public int BaseDefPoint { get; set; }
        /// <summary>
        /// 立绘地址
        /// </summary>
        public string Pic_Path { get; set; }
        /// <summary>
        /// 攻击潜力值
        /// </summary>
        public int AttPotency { get; set; }
        /// <summary>
        /// 防御力潜力值
        /// </summary>
        public int DefPotency { get; set; }
        /// <summary>
        /// 生命值潜力
        /// </summary>
        public int HpPotency { get; set; }
        /// <summary>
        /// 生活的场所
        /// </summary>
        public MonsterLivePlace MonsterLivePlace { get; set; }
        /// <summary>
        /// 怪物类型
        /// </summary>
        public MonsterType MonsterType { get; set; }
        /// <summary>
        /// 掉落金币
        /// </summary>
        public long DropMoney { get; set; }
        /// <summary>
        /// 掉落的物品
        /// </summary>
        public List<Model_Items> DropItems { get; set; }
    }
}
