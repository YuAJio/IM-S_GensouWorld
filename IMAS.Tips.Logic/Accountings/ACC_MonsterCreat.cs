using IMAS.LocalDBManager.Models;
using IMAS.Tips.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.Accounting
{
    public class ACC_MonsterCreat
    {
        /// <summary>
        /// 加工怪物们
        /// </summary>
        /// <param name="datas">怪物列表</param>
        public static void CreatAccMonsterData(List<Model_Monster> datas)
        {
            datas.ForEach(r =>
            {
                r.MaxHealthPoint = r.BaseHealPoint + (r.Level * r.HpPotency);
                r.MaxManaPoint = 0;
                r.MaxStaminaPoint = 0;
                r.HealthPoint = r.MaxHealthPoint;
                r.ManaPoint = r.MaxManaPoint;
                r.StaminaPoint = r.MaxStaminaPoint;
                r.AttackPoint = r.BaseAttPoint + (r.Level * r.AttPotency);
                r.DefencePoint = r.BaseDefPoint + (r.Level * r.DefPotency);
            });
        }
        /// <summary>
        /// 加工怪物
        /// </summary>
        /// <param name="data">怪物对象</param>
        public static void CreatAccMonsterData(Model_Monster data)
        {
            data.MaxHealthPoint = data.BaseHealPoint + (data.Level * data.HpPotency);
            data.MaxManaPoint = 0;
            data.MaxStaminaPoint = 0;
            data.HealthPoint = data.MaxHealthPoint;
            data.ManaPoint = data.MaxManaPoint;
            data.StaminaPoint = data.MaxStaminaPoint;
            data.AttackPoint = data.BaseAttPoint + (data.Level * data.AttPotency);
            data.DefencePoint = data.BaseDefPoint + (data.Level * data.DefPotency);
        }
    }

}
