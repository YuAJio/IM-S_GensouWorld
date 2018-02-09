using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IMAS.CupCake.Data;
using IMAS.LocalDBManager.Models;

namespace IMAS.Accounting
{
    /// <summary>
    /// 各种属性计算
    /// </summary>
    public class ACC_HpMpSpState
    {
        public static int UseHealingItemsAcc(Model_PlayerControllCharacter data, Model_Items usingItem)
        {
            if (data.HealthPoint >= data.MaxHealthPoint && usingItem.HealingHealthPoint > 0)
            {
                return -1;
            }
            if (data.ManaPoint >= data.MaxManaPoint && usingItem.HealingManaPoint > 0)
            {
                return -2;
            }
            if (data.StaminaPoint >= data.MaxStaminaPoint && usingItem.HealingStaminaPoint > 0)
            {
                return -3;
            }

            if (data.HealthPoint < data.MaxHealthPoint)
            {//计算回复生命值
                if (usingItem.HealingHealthPoint >= (data.MaxHealthPoint - data.HealthPoint))
                {
                    data.HealthPoint = data.MaxHealthPoint;
                }
                else
                {
                    data.HealthPoint += usingItem.HealingHealthPoint;
                }
            }
            if (data.HealthPoint < data.MaxHealthPoint)
            {//计算回复生命值
                if (usingItem.HealingHealthPoint >= (data.MaxHealthPoint - data.HealthPoint))
                {
                    data.HealthPoint = data.MaxHealthPoint;
                }
                else
                {
                    data.HealthPoint += usingItem.HealingHealthPoint;
                }
            }
            if (data.HealthPoint < data.MaxHealthPoint)
            {//计算回复生命值
                if (usingItem.HealingHealthPoint >= (data.MaxHealthPoint - data.HealthPoint))
                {
                    data.HealthPoint = data.MaxHealthPoint;
                }
                else
                {
                    data.HealthPoint += usingItem.HealingHealthPoint;
                }
            }
            return 1;
        }
    }
}