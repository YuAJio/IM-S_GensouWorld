using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace IMAS.Utils.Cryptographic
{
    /// <summary>
    /// Hash单项转译
    /// </summary>
   public class HashCryptographic
    {
        /// <summary>
        ///  转换字符串为SHA256哈希值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ChangeStringToSHA256(string value)
        {
            byte[] bytValue = Encoding.UTF8.GetBytes(value);
            try
            {
                SHA256 sha256 = new SHA256CryptoServiceProvider();
                byte[] retVal = sha256.ComputeHash(bytValue);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }
    }
}
