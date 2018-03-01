using IMAS.OkHttp.Bases;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IMAS.LocalDBManager.Models;

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

        public void Init(string host, int port)
        {
            _baseUrl = $"{host}:{port}";

            InitHeaders(new Dictionary<string, string>());
        }

        public async Task<List<IMAS.LocalDBManager.Models.Model_Buff>> Ahahha()
        {
            return null;
        }
    }

}
