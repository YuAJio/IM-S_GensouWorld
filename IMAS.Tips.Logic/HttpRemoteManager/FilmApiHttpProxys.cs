using IMAS.OkHttp.Bases;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IMAS.LocalDBManager.Models;
using IMAS.CupCake.Data;

namespace IMAS.Tips.Logic.HttpRemoteManager
{
    /// <summary>
    /// 4396电影院接口
    /// </summary>
    public class FilmApiHttpProxys : OkHttpBase
    {
        private static FilmApiHttpProxys _instance;

        private FilmApiHttpProxys() { }

        public static FilmApiHttpProxys GetInstance()
        {
            if (_instance == null)
            {
                _instance = new FilmApiHttpProxys();
            }
            return _instance;
        }

        public void Init(string host = "192.168.0.184", int port = 888)
        {
            _baseUrl = $"http://{host}:{port}";
            InitHeaders(new Dictionary<string, string>());
            //InitHeaders(new Dictionary<string, string>() {
            //    ["content-Type"]= "application/json"
            //});
        }

        /// <summary>
        /// 获取影片列表
        /// </summary>
        /// <param name="q">搜索关键字</param>
        /// <returns></returns>
        public async Task<Result<List<Model_VideoCover>>> GetVideoMsg(string q)
        {
            var data = NewPostParamDataDict();
            //var data = new Dictionary<string,string>();
            data.Add("q", q);
            //var jk = await AsyncPostJson<List<Model_VideoSearchs>>($"/api/MovieAPI/GetVideoMsg", data);
            return await AsyncPostJson<Result<List<Model_VideoCover>>>($"/api/MovieAPI/GetVideoMsg", data);
        }

        /// <summary>
        /// 获取影片详情
        /// </summary>
        /// <param name="herf">影片地址</param>
        /// <returns></returns>
        public async Task<Result<VideoMsg>> GetVideoInfo(string href)
        {
            var data = NewPostParamDataDict();
            data.Add("href", href);
            return await AsyncPostJson<Result<VideoMsg>>($"/api/MovieAPI/GetVideoInfo", data);
        }
        /// <summary>
        /// 获取影片播放地址
        /// </summary>
        /// <param name="herf">影片地址</param>
        /// <returns></returns>
        public async Task<Result<string>> GetPlayResources(string href,string name)
        {
            var data = NewPostParamDataDict();
            data.Add("name", name);
            data.Add("href", href);
            return await AsyncPostJson<Result<string>>($"/api/MovieAPI/GetPlayResources", data);
        }
    }

}
