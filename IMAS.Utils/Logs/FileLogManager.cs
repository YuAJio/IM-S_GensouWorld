using IMAS.Utils.Files;
using System;
using System.IO;

namespace IMAS.Utils.Logs
{
    public class FileLogManager
    {
        /// <summary>
        /// 日志文件目录
        /// </summary>
        public const string LOG = "Logs";

        private readonly string _logPrex = "log";
        /// <summary>
        /// 应用名称
        /// </summary>
        private string appName = "";
        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool isInit = false;

        private readonly object _syncObj = new object();

        private static FileLogManager _instance;
        private FileLogManager() { }
        /// <summary>
        /// 获取单例对象
        /// </summary>
        /// <returns></returns>
        public static FileLogManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FileLogManager();
            }
            return _instance;
        }

        /// <summary>
        /// 初始化
        /// @检测是否创建目录
        /// </summary>
        /// <param name="appName">应用的名称</param>
        public void Init(string appName)
        {
            this.appName = appName;
            //检测是否创建sdcard的根目录，没有创建并创建
            FilePathManager.GetInstance().CreateAppSDCardRootDir(appName);
            //检测是否创建sdcard的中的日志目录，没有创建并创建
            FilePathManager.GetInstance().CreateDir($"{FilePathManager.GetInstance().GetSdcardRootDirPath()}/{appName}/{LOG}");
            //
            isInit = true;
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="level"></param>
        /// <param name="key"></param>
        /// <param name="message"></param>
        public void Log(LogLevel level, string key, object message)
        {
            lock (_syncObj)
            {
                File.AppendAllText(GetLogFilePath(), $"{LevelString(level)} {DateTime.Now:yyyy-MM-dd H:mm:ss} {key}:{message}{"\r\n"}");//插入数据
            }
        }

        /// <summary>
        /// 获取日志文件
        /// </summary>
        /// <returns></returns>
        private string GetLogFilePath()
        {
            if (string.IsNullOrWhiteSpace(appName) || !isInit)
            {
                throw new Exception("请初始化日志管理器");
            }

            //文件名称
            var fileName = $"{_logPrex}_{ DateTime.Now:yyyy_MM_dd}.txt";
            var filePath = $"{FilePathManager.GetInstance().GetSdcardRootDirPath()}/{appName}/{LOG}/{fileName}";
            if (!FilePathManager.GetInstance().CheckFileExist(filePath))
            {
                //创建文件
                FilePathManager.GetInstance().CreatFile(filePath);
            }

            return filePath;
        }

        /// <summary>
        /// 清空历史日志文件
        /// @统一调用
        /// </summary>
        public void ClearHisLogFile()
        {
            var logDirPath = $"{FilePathManager.GetInstance().GetSdcardRootDirPath()}/{appName}/{LOG}";
            var files = FilePathManager.GetInstance().GetFileInfos(logDirPath);
            if (files != null && files.Length > 0)
            {
                foreach (var item in files)
                {
                    if ((DateTime.Now - item.CreationTime) > TimeSpan.FromDays(7))
                    {
                        item.Delete(); //删除文件
                    }
                }
            }
        }


        /// <summary>
        /// 日志等级
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        private string LevelString(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Crucial:
                    return "Crucial";
                case LogLevel.Error:
                    return "Error";
                case LogLevel.Warn:
                    return "Warn";
                case LogLevel.Info:
                    return "Info";
                case LogLevel.Debug:
                    return "Debug";
                default:
                    return string.Empty;
            }
        }
    }

    /// <summary>
    /// 告警等级
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 
        /// </summary>
        Crucial = 0,
        /// <summary>
        /// 
        /// </summary>
        Error = 1,
        /// <summary>
        /// 
        /// </summary>
        Warn = 2,
        /// <summary>
        /// 
        /// </summary>
        Info = 3,
        /// <summary>
        /// 
        /// </summary>
        Debug = 4
    }
}
