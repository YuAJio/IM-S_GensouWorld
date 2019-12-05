using System;
using System.Collections.Generic;
using System.Text;
using Java.IO;
using Square.OkHttp3;

namespace IMAS.OkHttp.Bases
{
    /// <summary>
    ///  自定义okhttp回调
    /// </summary>
    public class MyOkHttpCallback : Java.Lang.Object, ICallback
    {
        /// <summary>
        /// 错误回调
        /// </summary>
        private Action<Request, IOException> onFailure;

        /// <summary>
        /// 结果响应回调
        /// </summary>
        private Action<Response> onResponse;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onResponse"></param>
        /// <param name="onFailure"></param>
        public MyOkHttpCallback(Action<Response> onResponse, Action<Request, IOException> onFailure)
        {
            this.onResponse = onResponse;
            this.onFailure = onFailure;
        }

        public void OnFailure(ICall p0, IOException p1)
        {
            if (onFailure != null)
                onFailure(p0.Request(), p1);
        }

        public void OnResponse(ICall call, Response response)
        {
            if (onResponse != null)
            {
                onResponse(response);
            }
        }
    }
}
