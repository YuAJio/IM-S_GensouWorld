using System;
using Java.Lang;
using Android.Content;

namespace IMAS.HelpfulUtils.Zatsu
{
    class CrashHandler : Java.Lang.Object, Thread.IUncaughtExceptionHandler
    {
        private Context mContext;
        private Thread.IUncaughtExceptionHandler mDefaultHandler;

        /// <summary>
        /// 异常信息回调
        /// </summary>
        public Action<Throwable> ThrowableCallback;

        public void Init(Context cxt)
        {
            mContext = cxt;
            mDefaultHandler = Thread.DefaultUncaughtExceptionHandler;
            Thread.DefaultUncaughtExceptionHandler = this;
        }


        private CrashHandler()
        {

        }

        private static CrashHandler _instance;

        /// <summary>
        /// 获取单例对象
        /// </summary>
        /// <returns></returns>
        public static CrashHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CrashHandler();
            }
            return _instance;
        }

        /// <summary>
        /// 异常监听返回方法
        /// </summary>
        /// <param name="t"></param>
        /// <param name="e"></param>
        public void UncaughtException(Thread t, Throwable e)
        {
            if (mDefaultHandler != null)
                mDefaultHandler.UncaughtException(t, e);
            ThrowableCallback?.Invoke(e);
        }
    }
}
