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
using Android.Animation;
using IMAS.LocalDBManager.Models;
using Android.Graphics;
using Android.Views.Animations;
using static Android.Animation.ValueAnimator;
using IdoMaster_GensouWorld.Utils;
using System.Threading.Tasks;
using IMAS.Tips.Logic.LocalDBManager;
using IdoMaster_GensouWorld.Adapters;
using IMAS.CupCake.Extensions;
using System.Threading;

namespace IdoMaster_GensouWorld.Activitys.ShopPage
{
    public class FragmentShopPage_Pager : BaseFragment,
        //ShopHomePage_Pager_Activity.IOnLeftClickListener,
        ShopHomePage_Pager_Activity.IOnPageChangeListener,
        ShopHomePage_Pager_Activity.IOnScreenTouchListener,
        ShopHomePage_Pager_Activity.IOnProducerInfoQListener,
        Animator.IAnimatorListener
    {
        #region 页面Flag枚举
        private enum ShopPageFlag
        {
            /// <summary>
            /// 购买
            /// </summary>
            BuyPage = 0,
            /// <summary>
            /// 出售
            /// </summary>
            SellPage = 1,
        }

        #endregion

        #region UI控件
        private ListView lv_list;
        #endregion

        private ShopPageFlag shopFlag = ShopPageFlag.BuyPage;

        #region List列表相关
        private ShopItemAdapter adapter_Goods;
        private List<Model_Items> list_Goods;
        private List<Model_Items> list_Producer_Items;
        private LayoutAnimationController lac;
        #endregion
        public override int GetFragmentContentViewId()
        {
            return Resource.Layout.fragment_shop_list;
        }

        public override void InitFragmentView(View view)
        {
            lv_list = FindViewById<ListView>(Resource.Id.lv_list);

            lv_list.ItemClick -= OnItemClickListener;
            lv_list.ItemClick += OnItemClickListener;

            lv_list.ItemLongClick -= OnItemLongPressListener;
            lv_list.ItemLongClick += OnItemLongPressListener;

        }

        public override void InitFragmentData()
        {
            SoundEffectPlayer.Init(mContext);
            list_Goods = new List<Model_Items>();
            list_Producer_Items = new List<Model_Items>();
            adapter_Goods = new ShopItemAdapter(mContext);
            lv_list.Adapter = adapter_Goods;

            InitListViewAnimation();

            SQLiteGetProduceItems();
        }

        public override void SetOnFragmentClick(View v, EventArgs e)
        {

        }

        private void OnItemClickListener(object sender, AdapterView.ItemClickEventArgs e)
        {
            #region 检测动画是否存在
            if (valueAnimator != null)
            {
                valueAnimator.Pause();
                rl_father.RemoveView(iv_AnimeGoods);
            }
            #endregion
            switch (shopFlag)
            {
                case ShopPageFlag.BuyPage:
                    #region 商店页面操作
                    var clickItem_Buy = adapter_Goods[e.Position];
                    var waitForInsertObj = new Model_Items();
                    waitForInsertObj = clickItem_Buy;
                    #region 检测钱够不够
                    if (ShopHomePage_Pager_Activity.Producer_Money < waitForInsertObj.ShopPrice)
                    {
                        ShowDiyToastLong("お金が足りません");
                        return;
                    }
                    #endregion
                    var costPrice = waitForInsertObj.ShopPrice;
                    SQLiteUpdateProducerItem(waitForInsertObj, costPrice);
                    AddMyBackBag((LinearLayout)e.View);
                    #endregion
                    break;
                case ShopPageFlag.SellPage:
                    #region 出售页面操作 
                    var clickItem_sell = adapter_Goods[e.Position];
                    SQLiteUpdateProducerItem(clickItem_sell, clickItem_sell.ShopPrice);
                    #endregion
                    break;
            }
        }
        private void OnItemLongPressListener(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var clickItem = list_Goods[e.Position];

            ShowItemIllustratePopWindow(e.View, clickItem.ItemIllustrate);
        }

        /// <summary>
        /// 播放购物音效 
        /// </summary>
        private void PlayMoneyDropSount()
        {
            SoundEffectPlayer.Play(SE_Enumeration.SE_Money);
        }

        #region 显示道具详情
        private PopupWindowHelper popHelper;

        private void ShowItemIllustratePopWindow(View v, string itemIll)
        {
            var popView = LayoutInflater.From(mContext).Inflate(Resource.Layout.popwin_item_illustrate, null);
            popView.FindViewById<TextView>(Resource.Id.tv_Illustrate).Text = itemIll;
            popHelper = new PopupWindowHelper(popView);
            popHelper.ShowAsDropDown(v, 0, -10);
        }

        #endregion

        #region 动画效果
        private void InitListViewAnimation()
        {
            lac = new LayoutAnimationController(AnimationUtils.LoadAnimation(mContext, Resource.Animation.list_item_zoom_in));
            lac.Order = DelayOrder.Normal;
            lv_list.LayoutAnimation = lac;
            lv_list.StartLayoutAnimation();
        }

        private void InitDeleteAnime()
        {
            lac.SetAnimation(mContext, Resource.Animation.list_item_zoom_out);
            lac.Order = DelayOrder.Normal;
            lv_list.LayoutAnimation = lac;
            lv_list.StartLayoutAnimation();
        }
        #endregion

        #region 购物车动画相关

        private ImageView iv_AnimeGoods;
        private RelativeLayout rl_father;
        private PathMeasure mPathMeasure;
        private ValueAnimator valueAnimator;
        private Button bt_DropView;
        /// <summary>
        ///  贝塞尔曲线中间过程的点的坐标
        /// </summary>
        private float[] mCurrentPosition = new float[2];
        private void AddMyBackBag(LinearLayout ll_StartAnimeView)
        {
            //   一、创造出执行动画的主题---imageview
            //代码new一个imageview，图片资源是上面的imageview的图片
            // (这个图片就是执行动画的图片，从开始位置出发，经过一个抛物线（贝塞尔曲线），移动到购物车里)
            iv_AnimeGoods = new ImageView(mContext);
            iv_AnimeGoods.SetBackgroundResource(Resource.Mipmap.icon_shoppingbag);
            RelativeLayout.LayoutParams paramsa = new RelativeLayout.LayoutParams(100, 100);
            rl_father.AddView(iv_AnimeGoods, paramsa);

            //    二、计算动画开始/结束点的坐标的准备工作
            //得到父布局的起始点坐标（用于辅助计算动画开始/结束时的点的坐标）
            int[] parentLocation = new int[2];
            rl_father.GetLocationInWindow(parentLocation);

            //得到商品图片的坐标（用于计算动画开始的坐标）
            int[] startLoc = new int[2];
            ll_StartAnimeView.GetLocationInWindow(startLoc);

            //得到购物车图片的坐标(用于计算动画结束后的坐标)
            int[] endLoc = new int[2];
            bt_DropView.GetLocationInWindow(endLoc);


            //    三、正式开始计算动画开始/结束的坐标
            //开始掉落的商品的起始点：商品起始点-父布局起始点+该商品图片的一半
            float startX = startLoc[0] - parentLocation[0] + ll_StartAnimeView.Width / 2;
            float startY = startLoc[1] - parentLocation[1] + ll_StartAnimeView.Height / 2;

            //商品掉落后的终点坐标：购物车起始点-父布局起始点+购物车图片的1/5
            float toX = endLoc[0] - parentLocation[0] + bt_DropView.Width / 5;
            float toY = endLoc[1] - parentLocation[1];

            //    四、计算中间动画的插值坐标（贝塞尔曲线）（其实就是用贝塞尔曲线来完成起终点的过程）
            //开始绘制贝塞尔曲线
            Path path = new Path();
            //移动到起始点（贝塞尔曲线的起点）
            path.MoveTo(startX, startY);
            //使用二次萨贝尔曲线：注意第一个起始坐标越大，贝塞尔曲线的横向距离就会越大，一般按照下面的式子取即可
            path.QuadTo((startX + toX) / 2, startY, toX, toY);
            //mPathMeasure用来计算贝塞尔曲线的曲线长度和贝塞尔曲线中间插值的坐标，
            // 如果是true，path会形成一个闭环
            mPathMeasure = new PathMeasure(path, false);

            //★★★属性动画实现（从0到贝塞尔曲线的长度之间进行插值计算，获取中间过程的距离值）
            valueAnimator = ValueAnimator.OfFloat(0, mPathMeasure.Length);
            valueAnimator.SetDuration(500);
            // 匀速线性插值器
            valueAnimator.SetInterpolator(new LinearInterpolator());
            valueAnimator.Update -= UpdateListener;
            valueAnimator.Update += UpdateListener;
            valueAnimator.Start();
            valueAnimator.AddListener(this);
        }

        private void UpdateListener(object sender, AnimatorUpdateEventArgs e)
        {
            var animation = e.Animation;
            // 当插值计算进行时，获取中间的每个值，
            // 这里这个值是中间过程中的曲线长度（下面根据这个值来得出中间点的坐标值）
            float value = (float)animation.AnimatedValue;
            // ★★★★★获取当前点坐标封装到mCurrentPosition
            // boolean getPosTan(float distance, float[] pos, float[] tan) ：
            // 传入一个距离distance(0<=distance<=getLength())，然后会计算当前距
            // 离的坐标点和切线，pos会自动填充上坐标，这个方法很重要。
            mPathMeasure.GetPosTan(value, mCurrentPosition, null);//mCurrentPosition此时就是中间距离点的坐标值
                                                                  // 移动的商品图片（动画图片）的坐标设置为该中间点的坐标
            iv_AnimeGoods.TranslationX = mCurrentPosition[0];
            iv_AnimeGoods.TranslationY = mCurrentPosition[1];
        }
        public void OnAnimationCancel(Animator animation)
        {

        }

        public void OnAnimationEnd(Animator animation)
        {
            rl_father.RemoveView(iv_AnimeGoods);
        }

        public void OnAnimationRepeat(Animator animation)
        {

        }

        public void OnAnimationStart(Animator animation)
        {

        }
        #endregion

        /// <summary>
        /// 重新获取数据
        /// </summary>
        public void ReLoadData()
        {
            var shopType = this.Arguments.GetInt(IMAS_Constants.ShopPageFlagKey);
            if (shopType == -1)
            {
                shopFlag = ShopPageFlag.SellPage;
            }
            switch (shopFlag)
            {
                case ShopPageFlag.BuyPage:
                    SQliteQShopSellItem(shopType);
                    break;
                case ShopPageFlag.SellPage:
                    SQLiteGetProduceItems();
                    break;
            }

        }

        #region 实现Activity接口
        private Model_ProducerInfo info_Producer;
        public void OnCompliect(Model_ProducerInfo info)
        {
            this.info_Producer = info;
        }

        public void OnHandLeaveScreen()
        {
            if (popHelper != null)
            {
                if (popHelper.IsShowing())
                {
                    popHelper.Dismiss();
                }
            }
        }

        public void OnTabChange(int currentTab)
        {

        }

        public void SellClick()
        {
            shopFlag = ShopPageFlag.SellPage;
        }
        public void BuyClick()
        {
            shopFlag = ShopPageFlag.BuyPage;
        }
        #endregion

        #region SQLite相关
        private void SQliteQShopSellItem(int currentIndex)
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QTodaysShopSellGoodsForObject();
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    var objext = t.Result.Data;
                    var list = new List<Model_Items>();
                    switch (currentIndex)
                    {
                        case 0:
                            list = objext.TodaySell_Fuku.ToObject<List<Model_Items>>();
                            break;
                        case 1:
                            list = objext.TodaySell_Kazari.ToObject<List<Model_Items>>();
                            break;
                        case 2:
                            list = objext.TodaySell_Tabemono.ToObject<List<Model_Items>>();
                            break;
                    }
                    if (adapter_Goods == null)
                    {
                        adapter_Goods = new ShopItemAdapter(mContext);
                    }
                    adapter_Goods.SetDataList(list);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// 获取角色身上物品
        /// </summary>
        private void SQLiteGetProduceItems()
        {
            Task.Run(async () =>
            {
                Thread.Sleep(500);
                var result = await IMAS_ProAppDBManager.GetInstance().QProducerItemDetails(info_Producer.PkId);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    list_Producer_Items = t.Result.Data;
                    if (list_Producer_Items == null)
                    {
                        list_Producer_Items = new List<Model_Items>();
                    }
                    if (list_Producer_Items.Any())
                    {
                        list_Producer_Items = list_Producer_Items.OrderBy(r => r.ItemType).ToList();
                    }
                    if (shopFlag == ShopPageFlag.SellPage)
                    {
                        adapter_Goods.SetDataList(list_Producer_Items);
                    }

                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// 更新制作人身上的道具
        /// </summary>
        /// <param name="list_Items"></param>
        private void SQLiteUpdateProducerItem(Model_Items list_Items, long price)
        {
            PlayMoneyDropSount();
            Task.Run(async () =>
            {
                switch (shopFlag)
                {
                    case ShopPageFlag.BuyPage:
                        list_Items.ShopPrice = (long)(price * 0.8);//减少待插入道具的贩卖价格
                        list_Producer_Items.Add(list_Items);//插入待贩卖道具
                        break;
                    case ShopPageFlag.SellPage:
                        list_Producer_Items.Remove(list_Items);
                        break;
                }
                var result = await IMAS_ProAppDBManager.GetInstance().UpdateProducerItemInfo(info_Producer.PkId, list_Producer_Items, list_Items.ItemType);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    var jk = mContext as ShopHomePage_Pager_Activity;
                    switch (shopFlag)
                    {
                        case ShopPageFlag.BuyPage:
                            ShopHomePage_Pager_Activity.Producer_Money -= price;//减少钱
                            break;
                        case ShopPageFlag.SellPage:
                            ShopHomePage_Pager_Activity.Producer_Money += price;//增加钱
                            adapter_Goods.NotifyDataSetChanged();
                            break;
                    }
                    jk.SetMoneyText();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

        public override void OnAttach(Context context)
        {
            try
            {
                base.OnAttach(context);
                if (context is ShopHomePage_Pager_Activity)
                {
                    var jk = Activity as ShopHomePage_Pager_Activity;
                    jk.SetCallBack(this, this, this);
                    rl_father = jk.GetFatherView();
                    bt_DropView = jk.GetDropView();
                    info_Producer = jk.GetProducerInfo();
                }
            }
            catch (Exception e) { }
        }

    }
}