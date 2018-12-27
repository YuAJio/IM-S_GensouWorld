using IMAS.OkHttp.Bases;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IMAS.LocalDBManager.Models;
using IMAS.CupCake.Data;
using IMAS.CupCake.Extensions;
using System.Security.Cryptography;
using System.IO;

namespace IMAS.Tips.Logic.HttpRemoteManager
{
    public class NanaSakanaProxys : OkHttpBase
    {
        private static NanaSakanaProxys _Instance;

        public static NanaSakanaProxys GetInstance()
        {
            if (_Instance == null)
                _Instance = new NanaSakanaProxys();
            return _Instance;
        }

        /// <summary>
        /// 七鱼APPkey
        /// </summary>
        private string appKey = "";
        /// <summary>
        /// 七鱼AppSecret
        /// </summary>
        private string appSecret = "";



        public void Init(string appKey, string appSecret, string host = "qiyukf.com", int port = 443)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
            _baseUrl = $"https://{host}:{port}";
            InitHeaders(new Dictionary<string, string>());
        }

        /// <summary>
        /// 获取现在需要时间
        /// </summary>
        /// <returns></returns>
        private string GetNowUTCTime()
        {
            var jk = ((DateTime.UtcNow.ToUniversalTime().Ticks - new DateTime(1970, 1, 1, 0, 0, 0, 0).Ticks) / 10000000).ToString();
            return jk;
        }
        private string GetCheckSum(string contetn, string time)
        {
            var jk = QiyuPushCheckSum.Encode(appSecret, contetn, time);
            return jk;
        }


        /// <summary>
        /// 创建工单
        /// </summary>
        /// <returns></returns>
        public async Task<Result<NanaBaseMd>> CreatWorkBill(Md_Request_CreatWorkBill requestJson)
        {
            var json = requestJson.ToJson();
            var time = GetNowUTCTime();
            return await AsyncPostJson<Result<NanaBaseMd>>($"/openapi/worksheet/create?appKey={appKey}&time={time}&checksum={GetCheckSum(json, time)}", json);
        }


        public async Task<Result<NanaBaseMd>> CheckOutWorkBill(Md_Request_CreatWorkBill requestJson)
        {
            var json = requestJson.ToJson();
            var time = GetNowUTCTime();
            return await AsyncPostJson<Result<NanaBaseMd>>($"/openapi/worksheet/create?appKey={appKey}&time={time}&checksum={GetCheckSum(json, time)}", json);
        }


        #region 模型
        public class NanaBaseMd
        {
            /// <summary>
            /// 响应码
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 错误码为非200时,是错误信息,错误码是200时,是返回结果
            /// </summary>
            public string message { get; set; }
        }

        #region 创建工单
        /// <summary>
        /// 创建工单对象
        /// 
        /// userMobile和userEmail二者必填其一；
        /// targetStaffId和targetGroupId二者必填其一；
        /// 创建一个工单时最多可以添加5个附件，其中二进制格式的附件总大小不得超过20MB。
        /// </summary>
        public class Md_Request_CreatWorkBill
        {
            /// <summary>
            /// 工单标题，不超过30个字符
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 开发者的应用里的用户ID，不超过64个字符
            /// </summary>
            public string uid { get; set; }
            /// <summary>
            /// 分类ID，0表示未分类
            /// </summary>
            public int typeId { get; set; }
            /// <summary>
            /// 工单内容，不超过3000个字符
            /// </summary>
            public string content { get; set; }
            /// <summary>
            /// 用户姓名，不超过128个字符
            /// </summary>
            public string userName { get; set; }
            /// <summary>
            /// 用户联系方式，不超过128个字符
            /// </summary>
            public string userMobile { get; set; }
            /// <summary>
            /// 用户联系邮箱，不超过255个字符
            /// </summary>
            public string userEmail { get; set; }
            /// <summary>
            /// 指定客服ID
            /// </summary>
            public int targetStaffId { get; set; }
            /// <summary>
            /// 指定客服分组ID
            /// </summary>
            public int targetGroupId { get; set; }
            /// <summary>
            /// 托管客服ID
            /// </summary>
            public int staffId { get; set; }
            /// <summary>
            /// 优先级,5=一般;8=紧急;10=非常紧急
            /// </summary>
            public int priority { get; set; }
            /// <summary>
            /// 附件列表
            /// </summary>
            public List<FuJIan> attachments { get; set; }
            /// <summary>
            /// 附加属性对，json格式，不超过1024个字符
            /// </summary>
            public string properties { get; set; }


            public class FuJIan
            {
                /// <summary>
                /// 附件的文件名，不超过128个字符
                /// </summary>
                public string fileName { get; set; }
                /// <summary>
                /// 附件的类型，1=文件的Base64，目前仅支持此类型
                /// </summary>
                public int type = 1;
                /// <summary>
                /// 对应type的内容
                /// </summary>
                public string payload { get; set; }
            }


        }
        #endregion

        #region 查看工单
        /// <summary>
        /// 查看工单请求信息
        /// </summary>
        public class Md_Request_CheckWorkBillList
        {
            /// <summary>
            /// 开发者的应用里的用户ID，不超过128个字符
            /// </summary>
            public string uid { get; set; }
            /// <summary>
            /// 一次请求获取的工单数上限，最大为100
            /// </summary>
            public int limit { get; set; }
            /// <summary>
            /// 偏移量，(默认为0)
            /// </summary>
            public int offset { get; set; }
        }

        /// <summary>
        /// 获取工单信息
        /// </summary>
        public class Md_WorkBillList
        {
            /// <summary>
            /// 工单id
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// 工单标题
            /// </summary>
            public string title { get; set; }
            /// <summary>
            /// 工单状态
            /// </summary>
            public int status { get; set; }
            /// <summary>
            /// 工单创建时间
            /// </summary>
            public string createtime { get; set; }

        }
        #endregion

        #region 查看工单状态
        public class Md_Request_CheckWorkBillState
        {
            /// <summary>
            /// 开发者的应用里的用户ID，不超过128个字符
            /// </summary>
            public string uid { get; set; }
            /// <summary>
            /// 工单ID
            /// </summary>
            public int sheetId { get; set; }
            /// <summary>
            /// 1.不返回历史评论，2.返回最新的一条评论，3.返回所有历史评论
            /// </summary>
            public int history { get; set; }
            /// <summary>
            /// 布尔型,是否带上工单信息
            /// </summary>
            public bool details { get; set; }
        }

        public class Md_CheckWorkBillState
        {
            public string title { get; set; }
            public string content { get; set; }
            public int priority { get; set; }
            public int typeId { get; set; }
            public string typeName { get; set; }
            public List<FuJian> attachments { get; set; }
            public class FuJian
            {
                /// <summary>
                /// 工单附件名称
                /// </summary>
                public string name { get; set; }
                /// <summary>
                /// 工单附件大小,单位Byte
                /// </summary>
                public byte size { get; set; }
                /// <summary>
                /// 工单附件下载地址
                /// </summary>
                public string url { get; set; }
            }
        }
        #endregion


        #endregion

        #region ChckSum计算方法
        class QiyuPushCheckSum
        {
            private static char[] HEX_DIGITS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

            public static string Encode(string appSecret, string nonce, string time)
            {
                string content = appSecret + nonce + time;
                var buffer = Encoding.UTF8.GetBytes(content);
                var data = SHA1.Create().ComputeHash(buffer);
                return getFormattedText(data);
            }

            private static string getFormattedText(byte[] bytes)
            {
                int len = bytes.Length;
                StringBuilder buf = new StringBuilder(len * 2);
                for (int j = 0; j < len; j++)
                {
                    buf.Append(HEX_DIGITS[(bytes[j] >> 4) & 0x0f]);
                    buf.Append(HEX_DIGITS[bytes[j] & 0x0f]);
                }
                return buf.ToString();
            }
        }

        class CommonHelper
        {
            /// <summary>
            /// 通过字符串获取MD5值，返回32位字符串。
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static string GetMD5String(string str)
            {
                MD5 md5 = MD5.Create();
                byte[] data = Encoding.UTF8.GetBytes(str);
                byte[] data2 = md5.ComputeHash(data);
                return GetbyteToString(data2);
            }
            /// <summary>
            /// 获取MD5值。HashAlgorithm.Create("MD5") 或 MD5.Create() HashAlgorithm.Create("SHA256") 或 SHA256.Create()
            /// </summary>
            /// <param name="str"></param>
            /// <param name="hash"></param>
            /// <returns></returns>
            public static string GetMD5String(string str, HashAlgorithm hash)
            {
                byte[] data = Encoding.UTF8.GetBytes(str);
                byte[] data2 = hash.ComputeHash(data);
                return GetbyteToString(data2);
            }

            public static string GetMD5FromFile(string path)
            {
                MD5 md5 = MD5.Create();
                if (!File.Exists(path))
                {
                    return "";
                }
                FileStream stream = File.OpenRead(path);
                byte[] data2 = md5.ComputeHash(stream);

                return GetbyteToString(data2);
            }

            private static string GetbyteToString(byte[] data)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        #endregion
    }
}
