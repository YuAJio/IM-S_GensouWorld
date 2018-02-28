using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.LocalDBManager.Models
{
    /// <summary>
    /// 角色聊天文本
    /// </summary>
    public class Model_ChatContent_Plus
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long CharacterId { get; set; }
        /// <summary>
        /// 初次见面文本
        /// </summary>
        public string Chat_FirstMeet { get; set; }
        /// <summary>
        /// 触摸语音文本
        /// </summary>
        public List<string> Touch_Serifu { get; set; }
    }
}
