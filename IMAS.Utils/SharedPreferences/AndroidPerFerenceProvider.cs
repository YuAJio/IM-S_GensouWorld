using Android.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMAS.Utils.Sp
{
    /// <summary>
    /// SharedPreference 帮助类
    /// </summary>
    public class AndroidPreferenceProvider
    {
        private Context context;
        /// <summary>
        /// sp名称
        /// </summary>
        private string spName = "sp";

        private static AndroidPreferenceProvider instance;

        private AndroidPreferenceProvider()
        {
            var a = DateTime.Now.ToString("yyyyMMdd hh:mm:ss");
        }
        /// <summary>
        /// 获取实例对象
        /// </summary>
        /// <param name="cxt">上下文</param>
        /// <returns></returns>
        public static AndroidPreferenceProvider GetInstance()
        {
            if (instance == null)
            {
                instance = new AndroidPreferenceProvider();
            }

            return instance;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="cxt"></param>
        public void Init(Context cxt, string spName)
        {
            context = cxt;
            this.spName = spName;
        }

        /// <summary>
        /// 获取shareP对象
        /// </summary>
        /// <param name="loginId">登录用户编号</param>
        /// <returns></returns>
        private ISharedPreferences GetPreferences(string loginId)
        {
            if (string.IsNullOrWhiteSpace(loginId))
            {
                return context.GetSharedPreferences(this.spName, FileCreationMode.Private);
            }

            return context.GetSharedPreferences(this.spName + loginId, FileCreationMode.Private);
        }

        /// <summary>
        /// 获取编辑者
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        private ISharedPreferencesEditor GetEditor(string loginId)
        {

            return GetPreferences(loginId).Edit();
        }

        #region 写入数据

        /// <summary>
        /// 写入string数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="loginId"></param>
        public void PutString(string key, string value, string loginId = null)
        {
            var edit = GetEditor(loginId);
            edit.PutString(key, value);
            edit.Commit();
        }

        /// <summary>
        /// 写入int数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="loginId"></param>
        public void PutInt(string key, int value, string loginId = null)
        {
            var edit = GetEditor(loginId);
            edit.PutInt(key, value);
            edit.Commit();
        }

        /// <summary>
        /// 写入long数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="loginId"></param>
        public void PutLong(string key, long value, string loginId = null)
        {
            var edit = GetEditor(loginId);
            edit.PutLong(key, value);
            edit.Commit();
        }

        /// <summary>
        /// 写入long数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="loginId"></param>
        public void PutBoolean(string key, bool value, string loginId = null)
        {
            var edit = GetEditor(loginId);
            edit.PutBoolean(key, value);
            edit.Commit();
        }

        /// <summary>
        /// 写入float数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="loginId"></param>
        public void PutFloat(string key, float value, string loginId = null)
        {
            var edit = GetEditor(loginId);
            edit.PutFloat(key, value);
            edit.Commit();
        }

        /// <summary>
        /// 写入float数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="loginId"></param>
        public void PutFloat(string key, List<string> values, string loginId = null)
        {
            var edit = GetEditor(loginId);
            edit.PutStringSet(key, values);
            edit.Commit();
        }

        #endregion

        #region 读取数据


        /// <summary>
        /// 获取string数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public string GetString(string key, string loginId = null)
        {
            var pre = GetPreferences(loginId);

            return pre.GetString(key, "");
        }


        /// <summary>
        /// 获取bool数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public bool GetBoolean(string key, string loginId = null)
        {
            var pre = GetPreferences(loginId);

            return pre.GetBoolean(key, false);
        }


        /// <summary>
        /// 获取float数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public float GetFloat(string key, string loginId = null)
        {
            var pre = GetPreferences(loginId);

            return pre.GetFloat(key, 0);
        }

        /// <summary>
        /// 获取int数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public int GetInt(string key, int defValue = 0, string loginId = null)
        {
            var pre = GetPreferences(loginId);

            return pre.GetInt(key, defValue);
        }


        /// <summary>
        /// 获取long数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public long GetLong(string key, string loginId = null)
        {
            var pre = GetPreferences(loginId);

            return pre.GetLong(key, 0);
        }

        /// <summary>
        /// 获取long数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public List<string> GetStringSet(string key, string loginId = null)
        {
            var pre = GetPreferences(loginId);

            return (List<string>)pre.GetStringSet(key, null);
        }

        #endregion

    }
}
