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

        public async Task<Result> SearchFilm(string searchMsg)
        {
            var data = NewPostParamDataDict();
            //var data = new Dictionary<string,string>();

            data.Add("q", searchMsg);

            return await AsyncPostJson<Result<List<Model_VideoSearchs>>>("/Home/GetVideoMsgs", data);
        }
    }

}
