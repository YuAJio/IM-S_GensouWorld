using System;
using System.Collections.Generic;
using System.Text;
using IMAS.Tips.Enums;

namespace IMAS.LocalDBManager.Models
{
    public class Model_Items : A_BaseInfo
    {
        /// <summary>
        /// 道具类型
        /// </summary>
        public ItemEnumeration ItemType { get; set; }

        /// <summary>
        ///　道具阐释 
        /// </summary>
        public string ItemIllustrate { get; set; }
        /// <summary>
        /// 回复生命值
        /// </summary>
        public long HealingHealthPoint { get; set; }
        /// <summary>
        /// 回复魔法值
        /// </summary>
        public long HealingManaPoint { get; set; }
        /// <summary>
        /// 回复体力值
        /// </summary>
        public long HealingStaminaPoint { get; set; }
        /// <summary>
        /// 商店价格
        /// </summary>
        public long ShopPrice { get; set; }
        /// <summary>
        /// 增加攻击力
        /// </summary>
        public int ATTPromote { get; set; }
        /// <summary>
        /// 增加防御力
        /// </summary>
        public int DEFPromote { get; set; }
        /// <summary>
        /// 正面Buff 
        /// </summary>
        public Model_Buff BuffingList { get; set; }
        /// <summary>
        /// 负面Buff
        /// </summary>
        public Model_DeBuff DeBuffingList { get; set; }

    }
}
