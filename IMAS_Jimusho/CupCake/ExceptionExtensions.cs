using System;
using System.Text;

namespace IMAS.CupCake.Extensions
{  /// <summary>
   /// 
   /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetMessage(this Exception ex)
        {
            if (ex == null)
                return string.Empty;
            if (ex.InnerException == null)
                return ex.Message;
            return GetMessage(ex.InnerException);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static string GetStackTrace(this Exception ex, StringBuilder sb = null)
        {
            if (sb == null)
                sb = new StringBuilder();
            if (ex == null)
            {
                return sb.ToString();
            }
            sb.AppendLine(ex.StackTrace);
            if (ex.InnerException == null)
            {
                return sb.ToString();
            }
            return GetStackTrace(ex.InnerException, sb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string FullMessage(this Exception ex)
        {
            return ex.ToJson(true);
            /*var aex = ex as AggregateException;
            if (aex != null)
            {
                var sb = new StringBuilder();
                foreach (var innerException in aex.Flatten().InnerExceptions)
                {
                    sb.AppendLine(Environment.NewLine + "异常：" +  innerException.GetMessage() + Environment.NewLine + innerException.GetStackTrace());
                }
                return sb.ToString();
            }
            return Environment.NewLine + "异常" +  ex.GetMessage() + Environment.NewLine + ex.GetStackTrace();*/
        }
    }
}