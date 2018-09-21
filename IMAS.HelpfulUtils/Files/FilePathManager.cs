using Android.Content;
using Android.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IMAS.Utils.Files
{
    /// <summary>
    /// app文件系统路径管理
    /// </summary>
    public class FilePathManager
    {
        private static FilePathManager _instance;

        private FilePathManager()
        {

        }

        /// <summary>
        /// 获取单例示例
        /// </summary>
        /// <returns></returns>
        public static FilePathManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FilePathManager();
            }

            return _instance;
        }


        private Context mContext;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="cxt"></param>
        public void Init(Context cxt)
        {
            mContext = cxt;
        }

        /// <summary>
        /// 获取私有文件根目录地址
        /// </summary>
        /// <returns></returns>
        public string GetPrivateRootDirPath()
        {
            var kk = mContext.GetExternalFilesDir(null).AbsolutePath;
            return kk;
        }

        /// <summary>
        /// 获取SDcard文件根目录地址
        /// </summary>
        /// <returns></returns>
        public string GetSdcardRootDirPath()
        {
            var kk = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            return kk;
        }


        /// <summary>
        /// 创建应用的sdcar根目录(使用前做SD卡存在检测)
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <returns></returns>
        public bool CreateAppSDCardRootDir(string appName)
        {
            var path = Path.Combine(GetSdcardRootDirPath(), appName);
            return CreateDir(path);
        }


        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path">目录绝对路径</param>
        /// <returns></returns>
        public bool CreateDir(string path)
        {
            if (!CheckDirExist(path))
            {
                var dir = Directory.CreateDirectory(path);
                if (dir != null)
                    return true;
                else
                    return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检测目录是否存在
        /// </summary>
        /// <param name="path">目录绝对路径</param>
        /// <returns></returns>
        public bool CheckDirExist(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 获取目录下的文件集合
        /// </summary>
        /// <param name="path"></param>
        /// <param name="suffix">后缀名称(jpg,png等)</param>
        /// <returns></returns>
        public List<string> GetFiles(string path, string suffix)
        {
            var files = Directory.GetFiles(path);
            if (files != null && files.Length > 0)
            {
                var list = new List<string>(files);
                return list.Where(t => t.Contains($".{suffix}")).ToList();
            }
            return null;
        }

        /// <summary>
        /// 检测文件是否存在
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public bool CheckFileExist(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path"></param>
        public void CreatFile(string path)
        {
            using (File.Create(path))
            {

            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        public void DeleteFile(string path)
        {
            File.Delete(path);
        }


        /// <summary>
        /// 获取目录中的文件信息集合
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <returns></returns>
        public FileInfo[] GetFileInfos(string dirPath)
        {
            var dirinfo = new DirectoryInfo(dirPath);
            if (dirinfo != null)
            {
                return dirinfo.GetFiles();
            }

            return null;
        }

        /// <summary>
        /// 通过Uri获取文件路径
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="uri">Uri路径</param>
        /// <returns></returns>
        public string GetRealFilePath(Context context, Android.Net.Uri uri)
        {
            if (uri == null)
                return null;
            var scheme = uri.Scheme;
            string data = null;
            if (scheme == null)
                data = uri.Path;
            else if (ContentResolver.SchemeFile.Equals(scheme))
            {
                data = uri.Path;
            }
            else if (ContentResolver.SchemeContent.Equals(scheme))
            {
                using (var cursor = context.ContentResolver.Query(uri, new string[] { MediaStore.Images.ImageColumns.Data }, null, null, null))
                {
                    if (cursor != null)
                    {
                        if (cursor.MoveToFirst())
                        {
                            var index = cursor.GetColumnIndex(MediaStore.Images.ImageColumns.Data);
                            if (index > -1)
                            {
                                data = cursor.GetString(index);
                            }
                        }
                        cursor.Close();
                    }
                }
            }
            return data;
        }

    }
}
