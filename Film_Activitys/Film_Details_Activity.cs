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
using IMAS.OkHttp.Bases;
using IMAS.Tips.Logic.HttpRemoteManager;

namespace IdoMaster_GensouWorld.Film_Activitys
{
    [Activity(Label = "Film_Details_Activity", Theme = "@style/Theme.PublicTheme")]
    public class Film_Details_Activity : BaseActivity
    {
        private string Path;
        public override int A_GetContentViewId()
        {
            return Resource.Layout.film_activity_film_details;
        }

        public override void B_BeforeInitView()
        {
            Path = Intent.GetStringExtra("value");
        }

        public override void C_InitView()
        {

        }

        public override void D_BindEvent()
        {

        }

        public override void E_InitData()
        {
            HttpGetSearchResult(Path);
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }


        #region Http相关
        private void HttpGetSearchResult(string href)
        {
            ShowWaitDiaLog("请稍等//...");
            Task.Run(async () =>
            {
                var result = await FilmApiHttpProxys.GetInstance().GetVideoInfo(href);
                return result;
            }).ContinueWith(t =>
            {
                HideWaitDiaLog();
                if (t.Exception != null)
                {
                    Console.WriteLine("线程异常");
                    return;
                }
                if (t.Result.IsSuccess)
                {
                    var jk = t.Result.Data;
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