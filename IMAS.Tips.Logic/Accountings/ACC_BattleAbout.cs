using IMAS.LocalDBManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.Tips.Logic.Accountings
{
    /// <summary>
    /// 战斗相关计算
    /// </summary>
    public class ACC_BattleAbout
    {
        /// <summary>
        /// 角色普通攻击
        /// </summary>
        /// <param name="c_Data">角色信息</param>
        /// <param name="m_Data">怪物信息</param>
        /// <returns>造成的伤害</returns>
        public static int Acc_NormalAttack(Model_PlayerControllCharacter c_Data, Model_Monster m_Data)
        {
            double damage = c_Data.AttackPoint;
            double guard = m_Data.DefencePoint;
            switch (m_Data.MonsterType)
            {
                case Enums.MonsterType.Plant:
                    {
                        guard = m_Data.DefencePoint * 0.5;
                    }
                    break;
                case Enums.MonsterType.Animal:
                    {
                        guard = m_Data.DefencePoint * 0.8;
                    }
                    break;
                case Enums.MonsterType.Human:
                    {
                        guard = m_Data.DefencePoint * 1.0;
                    }
                    break;
                case Enums.MonsterType.Dragon:
                    {
                        guard = m_Data.DefencePoint * 2.0;
                    }
                    break;
            }
            damage -= guard;
            damage = damage < 0 ? 0 : damage;
            var r_Dmg = Convert.ToInt32(damage);
            m_Data.HealthPoint -= r_Dmg;
            return r_Dmg;
        }

        /// <summary>
        /// 使用技能伤害
        /// </summary>
        /// <param name="skills"></param>
        /// <param name="m_Data"></param>
        /// <returns></returns>
        public static int Acc_SkillOrMagicAttack(Model_Skills skills, Model_Monster m_Data, int CharacterINI = 0, int CharacterSTR = 0)
        {
            double damage = 0;
            double guard = 0;
            switch (skills.SkillMode)
            {
                case Enums.SkillsMode.Attack:
                    guard = m_Data.DefencePoint * 0.8;
                    damage = skills.Damage;
                    damage += CharacterSTR * 0.6;
                    break;
                case Enums.SkillsMode.Mana:
                    guard = m_Data.DefencePoint * 0.5;
                    damage = skills.Damage;
                    damage += CharacterINI * 1.0;
                    break;
                case Enums.SkillsMode.Mind:
                    guard = m_Data.DefencePoint * 0.1;
                    damage = skills.Damage;

                    break;
            }
            damage -= guard;
            damage = damage <= 0 ? 0 : damage;
            var r_Damage = Convert.ToInt32(damage);
            m_Data.HealthPoint -= r_Damage;
            return r_Damage;
        }
        /// <summary>
        /// 怪物普通攻击
        /// </summary>
        /// <param name="m_Data"></param>
        /// <param name="c_Data"></param>
        /// <returns></returns>
        public static int Acc_MonsterAttack(Model_Monster m_Data, Model_PlayerControllCharacter c_Data)
        {
            if (Acc_WeightRandom(3, 2))
            {
                return -2;
            }
            bool bo = Acc_WeightRandom(8, 2);
            double damage = m_Data.AttackPoint;
            double guard = c_Data.DefencePoint;
            guard = c_Data.DefencePoint * 0.5;
            damage -= guard;
            damage = damage < 0 ? 0 : damage;
            var r_Dmg = Convert.ToInt32(damage);
            c_Data.HealthPoint -= r_Dmg;
            return r_Dmg;
        }
        /// <summary>
        /// 怪物强力攻击
        /// </summary>
        /// <param name="m_Data"></param>
        /// <param name="c_Data"></param>
        /// <returns></returns>
        public static int Acc_MonsterHeavyAttack(Model_Monster m_Data, Model_PlayerControllCharacter c_Data)
        {
            bool bo = Acc_WeightRandom(8, 2);
            double damage = m_Data.AttackPoint * 1.5;
            double guard = c_Data.DefencePoint * 0.4; ;
            damage -= guard;
            damage = damage < 0 ? 0 : damage;
            var r_Dmg = Convert.ToInt32(damage);
            c_Data.HealthPoint -= r_Dmg;
            return r_Dmg;
        }

        /// <summary>
        /// 判断是否能逃跑成功
        /// </summary>
        /// <returns></returns>
        public static bool Acc_Runnaway()
        {
            return Acc_WeightRandom(4, 6);
        }

        /// <summary>
        /// 战斗胜利计算
        /// </summary>
        /// <param name="m_Data"></param>
        /// <param name="c_Data"></param>
        public static void Acc_MonsterdBeDefeat(Model_Monster m_Data, Model_PlayerControllCharacter c_Data, Model_ProducerInfo p_Data)
        {
            c_Data.Exp += m_Data.Exp;
            p_Data.Exp += m_Data.Exp;
            if (p_Data.Exp >= p_Data.LevelUpExp)
            {
                p_Data.Exp = 0;
                p_Data.Level += 1;
            }
            if (c_Data.Exp >= c_Data.LevelUpExp)
            {
                //升级了
                ACC_CharacterAbout.CharacterLevelUp(c_Data);
            }
            p_Data.Money += m_Data.DropMoney;
        }
        /// <summary>
        /// 战斗失败计算
        /// </summary>
        /// <param name="c_Data"></param>
        public static void Acc_CharacterBeDefeat(Model_PlayerControllCharacter c_Data, Model_ProducerInfo p_Data)
        {
            c_Data.HealthPoint = 1;
            p_Data.Money = Convert.ToInt32(p_Data.Money * 0.9);
        }

        /// <summary>
        /// 计算权重的事件
        /// </summary>
        /// <param name="w1">事件不触发的权重</param>
        /// <param name="w2">事件触发的权重</param>
        /// <returns><see langword="true"/>事件触发 <see langword="false"/>事件不触发</returns>
        public static bool Acc_WeightRandom(int w1, int w2)
        {
            var list_Event = new List<Model_Weight>()
            {
                new Model_Weight(){ EventId=0,PickWeight=w1},
                new Model_Weight(){ EventId=1,PickWeight=w2}
            };
            var obj_Event = new Model_Weight();
            var rm = new Random();
            var jk = ACC_EventWeight.GetRandomList(list_Event, 1);
            if (jk == null)
            {
                var i = rm.Next(list_Event.Count);
                obj_Event = list_Event[i];
            }
            else
            {
                obj_Event = jk[0];
            }
            return obj_Event.EventId == 1;
        }
    }
}
