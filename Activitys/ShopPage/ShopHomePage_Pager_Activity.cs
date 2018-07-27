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
using Android.Support.V4.View;
using CustomControl;
using IdoMaster_GensouWorld.Adapters;
using IMAS.LocalDBManager.Models;
using System.Threading.Tasks;
using IMAS.Tips.Logic.LocalDBManager;

namespace IdoMaster_GensouWorld.Activitys.ShopPage
{
    [Activity(Label = "ShopHomePage_Pager_Activity", Theme = "@style/Theme.PublicThemePlus")]
    public class ShopHomePage_Pager_Activity : BaseFragmentActivity
    {
        #region UI控件
        private Button bt_buy;
        private Button bt_sell;
        private Button bt_back;
        private TextView tv_Mani;
        private ViewPager vp_Pager;
        private SlipIndicator si_Indicator;
        #endregion

        private ShopTabFragmentAdapter tabAdapter;

        /// <summary>
        /// 标题
        /// </summary>
        private Java.Lang.String[] titles_Buy = new Java.Lang.String[] { new Java.Lang.String("装備"), new Java.Lang.String("飾り"), new Java.Lang.String("食べ物") };
        private Java.Lang.String[] titles_Sell = new Java.Lang.String[] { new Java.Lang.String("アイテム") };

        private List<FragmentShopPage_Pager> fragments_Buy;
        private List<FragmentShopPage_Pager> fragments_Sell;

        /// <summary>
        /// 目前页面标识
        /// 0:购买 1:出售
        /// </summary>
        private int Page_Flag = 0;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_home_shop_pager;
        }

        public override void B_BeforeInitView()
        {

        }

        public override void C_InitView()
        {
            bt_buy = FindViewById<Button>(Resource.Id.bt_buy);
            bt_sell = FindViewById<Button>(Resource.Id.bt_sell);
            bt_back = FindViewById<Button>(Resource.Id.bt_back);
            tv_Mani = FindViewById<TextView>(Resource.Id.tv_mani);
            vp_Pager = FindViewById<ViewPager>(Resource.Id.vp_Pager);
            si_Indicator = FindViewById<SlipIndicator>(Resource.Id.si_indicator);

        }

        public override void D_BindEvent()
        {
            si_Indicator.OnPageSelectedAction -= OnPageSelected;
            si_Indicator.OnPageSelectedAction += OnPageSelected;
            bt_buy.Click += OnClickListener;
            bt_sell.Click += OnClickListener;
            bt_back.Click += OnClickListener;
        }

        public override void E_InitData()
        {
            SQLiteGetProduceInfo();
            InitFragments();
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
            switch (v.Id)
            {
                case Resource.Id.bt_buy:
                    {
                        if (Page_Flag == 0)
                        {
                            return;
                        }
                        Page_Flag = 0;
                        //leftCallBack?.BuyClick();
                        CleanAllFragments();
                        CreateBuyFragments();
                    }
                    break;
                case Resource.Id.bt_sell:
                    {
                        if (Page_Flag == 1)
                        {
                            return;
                        }
                        Page_Flag = 1;
                        //leftCallBack?.SellClick();
                        CleanAllFragments();
                        CreateSellFragments();
                    }
                    break;
                case Resource.Id.bt_back:
                    {
                        this.Finish();
                    }
                    break;
            }
        }

        public override void G_OnAdapterItemClickListener(AdapterView.ItemClickEventArgs e)
        {

        }

        private void OnPageSelected(int position)
        {
            (tabAdapter.GetItem(position) as FragmentShopPage_Pager)?.ReLoadData();
            //  pageChangeCallBack?.OnTabChange(position);
        }

        #region 操作Framgnet方法
        /// <summary>
        /// 初始化Fragments列表 
        /// </summary>
        private void InitFragments()
        {
            fragments_Buy = new List<FragmentShopPage_Pager>();
            fragments_Sell = new List<FragmentShopPage_Pager>();
            for (int i = 0; i < 3; i++)
            {
                var fragment = new FragmentShopPage_Pager();
                var bundle = new Bundle();
                bundle.PutInt(IMAS_Constants.ShopPageFlagKey, i);
                fragment.Arguments = bundle;
                fragments_Buy.Add(fragment);
            }
            fragments_Sell.Add(new FragmentShopPage_Pager());
            tabAdapter = new ShopTabFragmentAdapter(fragments_Buy, titles_Buy, this.SupportFragmentManager, this);
            vp_Pager.OffscreenPageLimit = 0;
            vp_Pager.Adapter = tabAdapter;
            si_Indicator.SetViewPager(vp_Pager);
            si_Indicator.RefreshTab(0);
        }
        /// <summary>
        /// 清除所有Fragments
        /// </summary>
        private void CleanAllFragments()
        {
            try
            {
                if (SupportFragmentManager.Fragments != null || SupportFragmentManager.Fragments.Count > 0)
                {
                    {
                        var ft = SupportFragmentManager.BeginTransaction();
                        foreach (var item in SupportFragmentManager.Fragments)
                        {
                            ft.Remove(item);
                        }
                        ft.Commit();
                        SupportFragmentManager.ExecutePendingTransactions();
                    }
                    switch (Page_Flag)
                    {
                        case 0:
                            fragments_Buy?.Clear();
                            tabAdapter.RefreshData(fragments_Buy, titles_Buy);
                            break;
                        case 1:
                            fragments_Sell?.Clear();
                            tabAdapter.RefreshData(fragments_Sell, titles_Sell);
                            break;
                    }
                    si_Indicator.RefreshTab(-1);
                    vp_Pager.RemoveAllViewsInLayout();

                }
                else
                {
                    switch (Page_Flag)
                    {
                        case 0:
                            fragments_Buy?.Clear();
                            tabAdapter.RefreshData(fragments_Buy, titles_Buy);
                            break;
                        case 1:
                            fragments_Sell?.Clear();
                            tabAdapter.RefreshData(fragments_Sell, titles_Sell);
                            break;
                    }
                    si_Indicator.RefreshTab(-1);
                    vp_Pager.RemoveAllViewsInLayout();
                }
            }
            catch (Exception ex)
            {

            }

        }
        /// <summary>
        /// 创建购买页面
        /// </summary>
        private void CreateBuyFragments()
        {
            fragments_Buy.Clear();
            for (int i = 0; i < 3; i++)
            {
                var fragment = new FragmentShopPage_Pager();
                var bundle = new Bundle();
                bundle.PutInt(IMAS_Constants.ShopPageFlagKey, i);
                fragment.Arguments = bundle;
                fragments_Buy.Add(fragment);
            }
            tabAdapter.RefreshData(fragments_Buy, titles_Buy);
            si_Indicator.RefreshTab(0);
        }
        /// <summary>
        /// 创建出售页面
        /// </summary>
        private void CreateSellFragments()
        {
            fragments_Sell.Clear();
            var bundle = new Bundle();
            bundle.PutInt(IMAS_Constants.ShopPageFlagKey, -1);
            var fragment = new FragmentShopPage_Pager();
            fragment.Arguments = bundle;
            fragments_Sell.Add(fragment);
            tabAdapter.RefreshData(fragments_Sell, titles_Sell);
            si_Indicator.RefreshTab(0);
        }

        #endregion




        #region 暴露在外部的方法和属性
        public static long Producer_Money;
        public RelativeLayout GetFatherView()
        {
            return FindViewById<RelativeLayout>(Resource.Id.rl_father);
        }

        public Button GetDropView()
        {
            return FindViewById<Button>(Resource.Id.bt_back);
        }
        public Model_ProducerInfo GetProducerInfo()
        {
            return info_Producer == null ? new Model_ProducerInfo() : info_Producer;
        }
        public void SetMoneyText()
        {
            tv_Mani.Text = $"マニー:\n\t{Producer_Money}";
        }
        #endregion

        #region fragment接口传递
        //private IOnLeftClickListener leftCallBack;
        private IOnPageChangeListener pageChangeCallBack;
        private IOnScreenTouchListener screenTouchCallBack;
        private IOnProducerInfoQListener producerInfoCallBack;

        public void SetCallBack(IOnPageChangeListener c2, IOnScreenTouchListener c3, IOnProducerInfoQListener c4)
        {
            //this.leftCallBack = c1;
            this.pageChangeCallBack = c2;
            this.screenTouchCallBack = c3;
            this.producerInfoCallBack = c4;
        }

        //public interface IOnLeftClickListener
        //{
        //    void BuyClick();

        //    void SellClick();
        //}

        public interface IOnPageChangeListener
        {
            void OnTabChange(int currentTab);
        }

        public interface IOnScreenTouchListener
        {
            void OnHandLeaveScreen();
        }
        public interface IOnProducerInfoQListener
        {
            void OnCompliect(Model_ProducerInfo info);
        }
        #endregion

        #region SQLite相关
        private Model_ProducerInfo info_Producer;
        public void SQLiteGetProduceInfo()
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QProducerInfo(IMAS_Constants.Producer_Name);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    info_Producer = t.Result.Data;
                    Producer_Money = info_Producer.Money;
                    tv_Mani.Text = $"マニー:\n\t{Producer_Money}";
                    producerInfoCallBack?.OnCompliect(info_Producer);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        /// <summary>
        /// 修改制作人身上的金币
        /// </summary>
        public void SQliteUpdateProducerMoney()
        {
            Task.Run(async () =>
            {
#if DEBUG
                if (Producer_Money <= 2000)
                {
                    Producer_Money = 65535;
                }
#endif
                var result = await IMAS_ProAppDBManager.GetInstance().UpdateProducerMoney(info_Producer.PkId, Producer_Money);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    Producer_Money = 0;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            if (ev.Action == MotionEventActions.Up)
            {
                screenTouchCallBack?.OnHandLeaveScreen();
            }
            return base.DispatchTouchEvent(ev);
        }

        public override void Finish()
        {
            SQliteUpdateProducerMoney();
            this.SetResult(Result.Ok);
            base.Finish();
        }

    }
}