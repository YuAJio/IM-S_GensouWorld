using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using IdoMaster_GensouWorld.Adapters;
using IdoMaster_GensouWorld.Film_Adapters;
using IdoMaster_GensouWorld.Listeners;
using IMAS.OkHttp.Bases;
using IMAS.Tips.Logic.HttpRemoteManager;

namespace IdoMaster_GensouWorld.Film_Activitys
{
    [Activity(Label = "Film_HomePage", Theme = "@style/Theme.PublicThemePlus")]
    public class Film_HomePage : BaseActivity
    {
        private EditText et_Search;
        private RecyclerView rv_searchList;


        private Film_FilmSearchList_Adapter adapter;

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
            rv_searchList = FindViewById<RecyclerView>(Resource.Id.gv_grid);

            rv_searchList.SetLayoutManager(new GridLayoutManager(this, 3));
            rv_searchList.AddItemDecoration(new Z_RecyclerViewAsGrid_Spacing(3, 10, true));
        }

        public override void D_BindEvent()
        {
            et_Search.SetOnKeyListener(new YsOnkeyListener(OnKeyFunction));
            //rv_searchList.click += OnAdapterItemClickListener;
        }

        public override void E_InitData()
        {
            adapter = new Film_FilmSearchList_Adapter(this);
            adapter.onItemClickAct -= OnRvItemClick;
            adapter.onItemClickAct += OnRvItemClick;
            rv_searchList.SetAdapter(adapter);

            HttpGetSearchResult("");
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }

        private void OnRvItemClick(View v, int positon)
        {
            var clickItem = adapter[positon];

            var intent = new Intent(this, typeof(Film_Details_Activity));
            intent.PutExtra("value", clickItem.Href);
            StartActivity(intent);
        }
        /// <summary>
        /// 搜索方法
        /// </summary>
        /// <param name="txt"></param>
        private void SearchInfo(string msg)
        {
            HttpGetSearchResult(msg);
        }


        #region Http相关
        private void HttpGetSearchResult(string msg)
        {
            ShowWaitDiaLog("请稍等//...");
            Task.Run(async () =>
            {
                var result = await FilmApiHttpProxys.GetInstance().GetVideoMsg(msg);
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
                    adapter.SetDataList(jk);
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
            else
            {
                return false;
            }
        }
        public override void Finish()
        {
            if (!string.IsNullOrEmpty(et_Search.Text))
            {
                et_Search.Text = "";
                HttpGetSearchResult("");
            }
            else
                base.Finish();
        }

    }
}