using IdoMaster_GensouWorld;
using IMAS.LocalDBManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.Tips.Logic.Accountings
{
    /// <summary>
    /// 角色相关计算
    /// </summary>
    public class ACC_CharacterAbout
    {
        /// <summary>
        /// 角色升级
        /// </summary>
        /// <param name="c_Data"></param>
        public static void CharacterLevelUp(Model_PlayerControllCharacter c_Data)
        {
            c_Data.Exp = 0;
            c_Data.Level += 1;
            c_Data.STR = Convert.ToInt32(c_Data.STR + c_Data.STR_Potential);
            c_Data.DEX = Convert.ToInt32(c_Data.DEX + c_Data.DEX_Potential);
            c_Data.INT = Convert.ToInt32(c_Data.INT + c_Data.INT_Potential);
            c_Data.MaxHealthPoint += c_Data.HP_Potential;
            c_Data.MaxManaPoint += c_Data.MP_Potential; ;
            c_Data.MaxStaminaPoint += c_Data.SP_Potential; ;
            c_Data.HealthPoint = c_Data.MaxHealthPoint;
            c_Data.ManaPoint = c_Data.MaxManaPoint;
            c_Data.StaminaPoint = c_Data.MaxStaminaPoint;
            c_Data.LevelUpExp += 200;
            switch (c_Data.CharacterId)
            {
                case IMAS_Constants.KISARAGI_CHIHAYA_ID:
                    {
                        c_Data.AttackPoint = Convert.ToInt32(c_Data.STR * 2.7);
                        c_Data.DefencePoint = Convert.ToInt32(c_Data.DEX * 7.2);
                    }
                    break;
                case IMAS_Constants.HOSHII_MIKI_ID:
                    {
                        c_Data.AttackPoint = Convert.ToInt32(c_Data.STR * 4.2);
                        c_Data.DefencePoint = Convert.ToInt32(c_Data.DEX * 2.0);
                    }
                    break;
                case IMAS_Constants.TAKATSUKI_YAYOI_ID:
                    {
                        c_Data.AttackPoint = Convert.ToInt32(c_Data.STR * 2.5);
                        c_Data.DefencePoint = Convert.ToInt32(c_Data.DEX * 3.5);
                    }
                    break;
                case IMAS_Constants.SHIJOU_TAKANE_ID:
                    {
                        c_Data.AttackPoint = Convert.ToInt32(c_Data.STR * 3.0);
                        c_Data.DefencePoint = Convert.ToInt32(c_Data.DEX * 2.6);
                    }
                    break;
            }
        }
    }
}
