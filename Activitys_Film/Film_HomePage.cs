using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IdoMaster_GensouWorld.Listeners;
using IMAS.OkHttp.Bases;
using IMAS.Tips.Logic.HttpRemoteManager;

namespace IdoMaster_GensouWorld.Activitys_Film
{
    [Activity(Label = "Film_HomePage", Theme = "@style/Theme.PublicTheme")]
    public class Film_HomePage : BaseActivity
    {
        private EditText et_Search;
        private GridView gv_List;


        public override int A_GetContentViewId()
        {
            return Resource.Layout.film_activity_home_page;
        }

        public override void B_BeforeInitView()
        {
            FilmApiHttpProxys.GetInstance().Init();
        }

        public override void C_InitView()
        {
            et_Search = FindViewById<EditText>(Resource.Id.et_search);
            gv_List = FindViewById<GridView>(Resource.Id.gv_grid);
        }

        public override void D_BindEvent()
        {
            et_Search.SetOnKeyListener(new YsOnkeyListener(OnKeyFunction));
        }

        public override void E_InitData()
        {

        }

        public override void F_OnClickListener(View v, EventArgs e)
        {

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }
        /// <summary>
        /// 搜索方法
        /// </summary>
        /// <param name="txt"></param>
        private void SearchInfo(string msg)
        {
            HttpGetSearchResult(msg);
        }


        /// <summary>
        /// 监听搜索框搜索
        /// </summary>
        /// <param name="v"></param>
        /// <param name="keyCode"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool OnKeyFunction(View v, [GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Enter && e.Action == KeyEventActions.Down)
            {
                HideTheSoftKeybow();
                var txt = et_Search.Text.Trim();
                if (!string.IsNullOrEmpty(txt))
                {
                    SearchInfo(txt);
                }
                return false;
            }
            return true;
        }

        #region Http相关
        private void HttpGetSearchResult(string msg)
        {
            Task.Run(async () =>
            {
                var result = await FilmApiHttpProxys.GetInstance().SearchFilm(msg);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    Console.WriteLine("线程异常");
                    return;
                }
                if (t.Result.IsSuccess)
                {
                }
                else
                {
                    if (t.Result.Code == OkHttpBase.LocalExceptionState)
                    {
                        ShowMsgShort("网络异常，请检查网络");
                    }
                    else if (t.Result.Code == OkHttpBase.WebExceptionState)
                    {
                        ShowMsgShort("服务器连接异常");
                    }
                    else
                    {
                        ShowMsgShort($"{t.Result.Message}");
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion
    }
}