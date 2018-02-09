using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{

    public enum BuffStatus
    {
        On = 1,
        Off = 0
    }
    public class Model_Buff
    {
        /// <summary>
        /// 兴奋
        /// </summary>
        public BuffStatus Buff_Wktk;
    }

    public class Model_DeBuff
    {
        /// <summary>
        /// 流血
        /// </summary>
        public BuffStatus DeBuff_Bleed;
        /// <summary>
        /// 中毒
        /// </summary>
        public BuffStatus DeBuff_Poisoning;
        /// <summary>
        /// 发狂
        /// </summary>
        public BuffStatus DeBuff_Crazy;
    }
}
