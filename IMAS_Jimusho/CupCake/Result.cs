using System.Threading.Tasks;

namespace IMAS.CupCake.Data
{
    /*
      * 
      * 通用返回调用，可避免抛出异常的方式
      * 
      */

    /// <summary>
    /// 处理结果对象
    /// </summary>
    public class Results
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 成功，默认有数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Results Success(int code = 0, string message = null)
        {
            IsSuccess = true;
            Code = code;
            Message = message;
            return this;
        }

        /// <summary>
        /// 失败，默认无数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Results Error(int code = 0, string message = null)
        {
            IsSuccess = false;
            Code = code;
            Message = message;
            return this;
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Results NewSuccess(int code = 0, string message = null)
        {
            return new Results { IsSuccess = true, Code = code, Message = message };
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Results NewError(int code = 0, string message = null)
        {
            return new Results { IsSuccess = false, Code = code, Message = message };
        }

        /// <summary>
        /// 成功，默认有数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Result<T> NewSuccess<T>(T data = default(T), int code = 0, string message = null)
        {
            return new Result<T> { IsSuccess = true, Data = data, Code = code, Message = message };
        }

        /// <summary>
        /// 失败，默认无数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Result<T> NewError<T>(int code = 0, string message = null, T data = default(T))
        {
            return new Result<T> { IsSuccess = false, Data = data, Code = code, Message = message };
        }

    }

    /// <summary>
    /// 泛型结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : Results
    {
        private T _data;

        /// <summary>
        /// 数据
        /// </summary>
        public virtual T Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// 成功，默认有数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Result<T> Success(T data, int code = 0, string message = null)
        {
            IsSuccess = true;
            Data = data;
            Code = code;
            Message = message;
            return this;
        }

        /// <summary>
        /// 失败，默认无数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result<T> Error(int code = 0, string message = null, T data = default(T))
        {
            IsSuccess = false;
            Data = data;
            Code = code;
            Message = message;
            return this;
        }
    }


    public static class ResultExtenssion
    {
        public static Task<TResult> WrapResultTask<TResult>(this TResult result) where TResult : Results
        {
            return Task.FromResult(result);
        }

        public static Task<Results> ConvertAndWrapTask(this object result)
        {
            return Task.FromResult(result as Results);
        }
    }
}