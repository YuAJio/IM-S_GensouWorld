using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android;
using IdoMaster_GensouWorld.Activitys.Opening;
using System.Threading.Tasks;
using IMAS.Tips.Logic.LocalDBManager;
using IdoMaster_GensouWorld.Activitys.ProductionPage;
using static Android.Views.View;
using IdoMaster_GensouWorld.Adapters;
using IMAS.LocalDBManager.Models;
using IMAS.Tips.Enums;
using IdoMaster_GensouWorld.Services;
using IMAS.Utils.Sp;

namespace IdoMaster_GensouWorld.Activitys.MainPage
{
    [Activity(Label = "LoadGame_Activity", Theme = "@style/Theme.PublicTheme")]
    public class LoadGame_Activity : BaseActivity
    {
        #region UI控件
        private ImageView iv_back;
        private EditText et_account;
        private Button bt_start;

        private ListView lv_history;
        #endregion


        private SimpleHistorySearchAdapter adapter_history;
        private List<Model_SearchHistory> list_history;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_main_continue;
        }

        public override void B_BeforeInitView()
        {
            adapter_history = new SimpleHistorySearchAdapter(this);
            list_history = new List<Model_SearchHistory>();
        }

        public override void C_InitView()
        {
            lv_history = FindViewById<ListView>(Resource.Id.lv_history);
            iv_back = FindViewById<ImageView>(Resource.Id.iv_back);
            et_account = FindViewById<EditText>(Resource.Id.et_account);
            bt_start = FindViewById<Button>(Resource.Id.bt_start);
        }

        public override void D_BindEvent()
        {
            iv_back.Click += OnClickListener;
            bt_start.Click += OnClickListener;
            et_account.FocusChange += OnFocusChangeListener;
            lv_history.ItemClick += OnAdapterItemClickListener;
            adapter_history.clickAction += OnAdapterClickAction;

        }

        public override void E_InitData()
        {
            lv_history.Adapter = adapter_history;
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
            switch (v.Id)
            {
                case Resource.Id.iv_back:
                    this.Finish();
                    break;

                case Resource.Id.bt_start:
                    var account = et_account.Text.Trim();
                    if (string.IsNullOrEmpty(account))
                    {
                        ShowMsgShort("名前お入力してください");
                        return;
                    }
                    SQLiteQProducerInfo(account);
                    break;
            }
        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {
            var clickItem = adapter_history[e.Position];
            et_account.Text = clickItem.Message;
            SQLiteQProducerInfo(clickItem.Message);
        }

        /// <summary>
        /// 适配器点击效果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAdapterClickAction(int position, View v)
        {
            switch (v.Id)
            {
                case Resource.Id.iv_delete:
                    SQLiteDelteHistoryInput(list_history[position].pkId);
                    break;
            }
        }
        /// <summary>
        /// 监听输入框焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFocusChangeListener(object sender, FocusChangeEventArgs e)
        {
            var isShow = lv_history.Visibility == ViewStates.Visible ? true : false;
            if (e.HasFocus)
            {
                if (!isShow)
                {
                    SQLiteQHistoryInput();
                }
            }
            else
            {
                CoverUIControl(CoverFlag.Gone, lv_history);
            }
        }

        /// <summary>
        /// 跳转到序章页面
        /// </summary>
        private void GoToNextStep()
        {
            StartActivity(new Intent(this, typeof(Prologue_Activity)));
        }
        /// <summary>
        /// 跳转到事务所页面
        /// </summary>
        private void GoToHomePage()
        {
            StartActivity(new Intent(this, typeof(ProductionHomeActivity)));
            this.Finish();
        }
        #region SQLite相关

        #region 制作人
        /// <summary>
        /// 查询制作人信息
        /// </summary>
        /// <param name="name"></param>
        private void SQLiteQProducerInfo(string name)
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QProducerInfo(name);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    var data = t.Result.Data;
                    ShowSureConfim(null, "このデータお読み込みなの？", (j, k) =>
                    {
                        AndroidPreferenceProvider.GetInstance().PutString(IMAS_Constants.SpProducerInfoNameKey, name);
                        IMAS_Constants.Producer_Name = $"{name}";
                        if (!data.IsPrologueChapterFinish)
                        {
                            GoToNextStep();
                        }
                        else
                        {
                            GoToHomePage();
                        }
                        SQLiteInsertHistoryInput(name);

                        ApplicationContext.StopService(new Intent(this, typeof(BackGroundMusicPlayer)));
                    });
                }
                else
                {
                    ShowDiyToastLong(t.Result.Message);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

        #region 历史输入
        /// <summary>
        /// 查询搜索历史记录
        /// </summary>
        private void SQLiteQHistoryInput()
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QSearchHistoryInfo(InputHistoryType.ProducerName, "");
                return result;
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    return;
                }

                list_history = t.Result.Data;
                if (list_history != null && list_history.Any())
                {
                    list_history.OrderBy(r => r.UpdateTime);
                    CoverUIControl(CoverFlag.Visible, lv_history);
                    adapter_history.SetDataList(list_history);
                }
                else
                {
                    CoverUIControl(CoverFlag.Gone, lv_history);
                }


            }, TaskScheduler.FromCurrentSynchronizationContext());

        }
        /// <summary>
        /// 插入输入历史
        /// </summary>
        /// <param name="msg"></param>
        private void SQLiteInsertHistoryInput(string msg)
        {
            Task.Run(async () =>
            {
                var result = new IMAS.CupCake.Data.Result();
                var tempList = list_history.Where(r => r.Message == msg).FirstOrDefault();
                if (tempList != null)
                {
                    result = await IMAS_ProAppDBManager.GetInstance().UpdateSearchHistoryInfo(tempList.pkId);
                }
                else
                {
                    result = await IMAS_ProAppDBManager.GetInstance().InsertSearchHistoryInfo(InputHistoryType.ProducerName, msg);
                }
                return result;
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    return;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        /// <summary>
        /// 插入输入历史
        /// </summary>
        /// <param name="msg"></param>
        private void SQLiteDelteHistoryInput(int pkId)
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().DeleteSearchHistoryInfo(pkId);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    return;
                }
                if (t.Result.IsSuccess)
                {
                    SQLiteQHistoryInput();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion
        #endregion
    }
}