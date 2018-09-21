using Android.Content;
using IMAS.CupCake.Extensions;
using System;
using System.IO;
using System.Text;

namespace IMAS.Utils.Files
{
    /// <summary>
    /// 文件操作的工具类
    /// </summary>
    public class FileUtils
    {

        /// <summary>
        /// 获取日志信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dirName"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string GetLogInfo(Context context, string dirName, DateTime datetime)
        {
            var fileNames = Directory.GetFiles(dirName);
            if (fileNames != null && fileNames.Length > 0)
            {
                var tempFileName = $"log_{datetime:yyyy_MM_dd}.txt";
                var content = "";
                for (int i = fileNames.Length - 1; i > -1; i++)
                {
                    if (tempFileName.Equals(fileNames[i]))
                    {
                        content = ReadFileForString(context, tempFileName);
                        break;
                    }
                }

                return content;
            }

            return "";
        }


        /// <summary>
        /// 创建sn
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="filePath">文件绝对路径</param>
        /// <param name="isSaveLocal">是否保存到本地</param>
        /// <returns></returns>
        public static string CreateAndroidSn(Context context, string filePath, bool isSaveLocal = true)
        {
            var sn = "";
            var serial = Android.OS.Build.Serial;
            //var model = Android.OS.Build.Model;
            var androidId = "" + Android.Provider.Settings.Secure.GetString(context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            if (!string.IsNullOrWhiteSpace(serial) && !string.IsNullOrWhiteSpace(androidId))
            {
                sn = $"{androidId}{serial}".ToLower();
                if (isSaveLocal)
                {
                    try
                    {
                        if (!File.Exists(filePath))
                        {
                            using (File.Create(filePath))
                            {
                            }
                        }

                        //同步到本地以供查看
                        File.WriteAllText(filePath, sn);
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            return sn;
        }


        #region 文件(夹)操作



        #endregion


        #region SDCard工具类文件读写操作
        /// <summary>
        /// 获取文件内容string
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileString(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            try
            {
                var configContent = File.ReadAllText(fileName, Encoding.UTF8);
                return configContent;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 通过文件名称获取内容
        /// @：文件内容转成 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T GetFileObject<T>(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            try
            {
                var configContent = File.ReadAllText(fileName, Encoding.UTF8);
                return configContent.ToObject<T>();
            }
            catch (Exception)
            {
                return default(T);
            }

        }

        /// <summary>
        /// 保存对象到文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">文件名称</param>
        /// <param name="config">对象</param>
        public static void SaveFileObject<T>(string fileName, T config)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (!File.Exists(fileName))
            {
                return;
            }

            try
            {
                File.WriteAllText(fileName, config.ToJson(true));
            }
            catch (Exception) { }

        }



        /// <summary>
        /// 读取私有文件内容
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFileForString(Context context, string filePath)
        {
            StringBuilder sb = new StringBuilder();
            FileStream stream = null;
            try
            {
                //var content =File.ReadAllText(filePath);
                //sb.Append(content);
                stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                byte[] buffer = new byte[8192];
                int count = 0;
                //获取流
                do
                {
                    count = stream.Read(buffer, 0, buffer.Length);
                    if (count != 0)
                        sb.Append(Encoding.UTF8.GetString(buffer, 0, count));
                } while (count > 0);

            }
            catch (Exception ex)
            {
                Console.WriteLine("写文件异常 = " + ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return sb.ToString();
        }

        /// <summary>
        /// 获取对象 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T ReadFileForObject<T>(Context context, string filePath)
        {
            var content = ReadFileForString(context, filePath);
            //var kk = "{}".ToObject<T>();

            var obj = content.ToObject<T>();
            //var obj = SerializeHelper.JsonDeserialize<T>(content);
            return obj;
        }

        /// <summary>
        /// 文件内容
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static bool WriteFileForBytes(Context context, string filePath, byte[] data)
        {
            bool result = false;

            if (data == null || data.Length <= 0)
                return result;

            FileStream stream = null;

            try
            {
                stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                //var byteFile = Encoding.UTF8.GetBytes(data);
                stream.Write(data, 0, data.Length);
                stream.Flush();
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("写文件异常 = " + ex.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return result;
        }

        /// <summary>
        /// 文件内容
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="data">string数据</param>
        /// <returns></returns>
        public static bool WriteFileForString(Context context, string filePath, string data)
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(data))
                return result;

            FileStream stream = null;

            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);

                stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                var byteFile = Encoding.UTF8.GetBytes(data);
                stream.Write(byteFile, 0, byteFile.Length);
                stream.Flush();
                byteFile = null;
                result = true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("写文件异常 = " + ex.Message);

            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return result;
        }

        /// <summary>
        /// 文件内容
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="context">上下文</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="data">数据对象</param>
        /// <returns></returns>
        public static bool WriteFileForObject<T>(Context context, string filePath, T data)
        {
            var content = data.ToJson(); //SerializeHelper.JsonSerialize<T>(data);

            return WriteFileForString(context, filePath, content);
        }

        #endregion


        #region data/data/包名/files/目录下文件读写操作

        /// <summary>
        /// 小芒存储根目录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string AppRootPath(Context context)
        {
            return context.GetExternalFilesDir(null).AbsolutePath;
        }

        /// <summary>
        /// 读取私有文件内容
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadPrivateFileForString(Context context, string filePath)
        {
            var stream = context.OpenFileInput(filePath);
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            StringBuilder sb = new StringBuilder();

            string line = null;
            while ((line = sr.ReadLine()) != null)
            {
                sb.Append(line);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 写私有文件内容
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool WritePrivateFileForString(Context context, string filePath, string data)
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(data))
                return result;

            Stream stream = null;
            StreamWriter sw = null;

            try
            {
                stream = context.OpenFileOutput(filePath, FileCreationMode.WorldReadable);
                sw = new StreamWriter(stream, Encoding.UTF8);
                sw.Write(data);
                sw.Flush();
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("写文件异常 = " + ex.Message);
            }
            finally
            {
                if (sw != null)
                    sw.Close();
                if (stream != null)
                    stream.Close();
            }

            return result;
        }

        /// <summary>
        /// 读取私有文件内容
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T ReadPrivateFileForObject<T>(Context context, string filePath)
        {
            var content = ReadPrivateFileForString(context, filePath);
            var obj = content.ToObject<T>();
            //var obj = SerializeHelper.JsonDeserialize<T>(content);
            return obj;
        }


        /// <summary>
        /// 写私有文件内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="filePath">文件路径</param>
        /// <param name="data">数据对象</param>
        /// <returns></returns>
        public static bool WritePrivateFileForObject<T>(Context context, string filePath, T data)
        {
            var content = data.ToJson();
            return WritePrivateFileForString(context, filePath, content);
        }

        #endregion


        #region Assets文件读取工具类

        /// <summary>
        /// 复制assets中的文件到sdcard中+私有文件中
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assetsFileNmae"></param>
        /// <param name="sdcardFileName"></param>
        public static bool CopyAssetsApkToSDCard(Context context, string assetsFileNmae, string sdcardFileName)
        {
            if (context == null)
                throw new ArgumentException("context不能为空！");
            if (string.IsNullOrWhiteSpace(assetsFileNmae))
                return false;
            if (string.IsNullOrWhiteSpace(sdcardFileName))
                return false;
            //StringBuilder sb = new StringBuilder();
            try
            {
                FileStream fs = new FileStream(sdcardFileName, FileMode.CreateNew);

                byte[] buffer = new byte[8192];
                int count = 0;
                //获取流
                var stream = context.Assets.Open(assetsFileNmae);
                do
                {
                    count = stream.Read(buffer, 0, buffer.Length);
                    if (count != 0)
                        //sb.Append(Encoding.UTF8.GetString(buffer, 0, count));
                        fs.Write(buffer, 0, count);

                } while (count > 0);

                fs.Flush();
                fs.Close();
                stream.Close();

                return true;
                //var content = sb.ToString();
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        /// <summary>
        /// 获取Assets文件中文本文件中数据
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string ReadAssetsInfoForString(Context context, string filePath)
        {
            if (context == null)
                throw new ArgumentException("context不能为空！");

            if (string.IsNullOrWhiteSpace(filePath))
                return null;
            StringBuilder sb = new StringBuilder();

            try
            {
                byte[] buffer = new byte[8192];
                int count = 0;
                //获取流
                var stream = context.Assets.Open(filePath);
                do
                {
                    count = stream.Read(buffer, 0, buffer.Length);
                    if (count != 0)
                        //sb.Append(Encoding.ASCII.GetString(buffer, 0, count));
                        sb.Append(Encoding.UTF8.GetString(buffer, 0, count));
                } while (count > 0);

                var content = sb.ToString();
                return content;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        /// <summary>
        /// 把文件中的数据序列化数据对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="context">上下文</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static T ReadAssetsInfoForObject<T>(Context context, string filePath)
        {
            var content = ReadAssetsInfoForString(context, filePath);
            //content = content.Replace("\r\n", "");
            if (string.IsNullOrWhiteSpace(content))
            {
                return default(T);
            }

            var obj = content.ToObject<T>();
            //var obj = SerializeHelper.JsonDeserialize<T>(content);
            return obj;
        }

        #endregion


    }
}
