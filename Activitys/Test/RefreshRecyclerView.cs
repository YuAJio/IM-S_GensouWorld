using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace IdoMaster_GensouWorld.Activitys.Test
{
    /// <summary>
    /// 刷新那啥
    /// </summary>
    public class RefreshRecyclerView : SwipeRefreshLayout
    {
        /// <summary>
        /// 头部刷新
        /// </summary>
        public Action Refresh_Atama;
        /// <summary>
        /// 底部刷新
        /// </summary>
        public Action Refresh_Ashi;

        /// <summary>
        ///  加载中布局
        /// </summary>
        private LinearLayout v_AshiLoading;
        private ImageView iv_loading;

        public RecyclerView Az_RecyclerView { get; private set; }

        public RefreshRecyclerView(Context context) : base(context)
        {
            InitView();
        }

        public RefreshRecyclerView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            InitView();
        }

        private void InitView()
        {

            var childView = Inflate(this.Context, Resource.Layout.test_refreshlist, null);

            Az_RecyclerView = childView.FindViewById<RecyclerView>(Resource.Id.rv_list);
            v_AshiLoading = childView.FindViewById<LinearLayout>(Resource.Id.ll_foot);
            iv_loading = childView.FindViewById<ImageView>(Resource.Id.iv_loading);

            var anime = AnimationUtils.LoadAnimation(this.Context, Resource.Animation.anime_video_wait);
            var lin = new LinearInterpolator();
            anime.Interpolator = lin;
            if (anime != null)
                iv_loading.StartAnimation(anime);

            Az_RecyclerView.AddOnScrollListener(new AzOnScrollListener(OnScrolledAct: OnRecyclerViewScrollListener, OnScrollStateChangedAct: OnRecyclerViewScrollStateListener));

            this.AddView(childView);

            this.Refresh -= OnRefreshListener;
            this.Refresh += OnRefreshListener;

        }

        #region 下拉刷新
        public void OnRefreshListener(object sender, EventArgs e)
        {
            Az_RecyclerView.ScrollToPosition(0);
            IsAtataLoading = true;
            Refresh_Atama?.Invoke();
        }
        #endregion

        #region 上拉加载相关
        private bool IsAshiLoading = false;
        private bool IsAtataLoading = false;
        private bool IsNoMoreData = false;
        private int lastVisibleItem;
        private void OnRecyclerViewScrollListener(RecyclerView rv, int dx, int dy)
        {
            if (rv.GetLayoutManager() is LinearLayoutManager)
            {
                var lm = rv.GetLayoutManager() as LinearLayoutManager;
                lastVisibleItem = lm.FindLastVisibleItemPosition();
            }
            //if (dy > 0)
            //{
            //    var lm = rv.GetLayoutManager() as LinearLayoutManager;
            //    var visibleItemCount = lm.ChildCount;
            //    var totalItemCount = lm.ItemCount;
            //    var pastVisiblesItems = lm.FindFirstVisibleItemPosition();

            //    if (!IsAshiLoading && (visibleItemCount + pastVisiblesItems) >= totalItemCount)
            //    {
            //        IsAshiLoading = true;
            //        Refresh_Ashi?.Invoke();
            //    }
            //}
            //firstVisibleItem = lm.FindFirstVisibleItemPosition();
            //if (IsAshiLoading)
            //{
            //    if (totalItemCount > previousTotal)
            //    {//说明数据已经加载结束
            //        OnAshiFinish();
            //    }
            //}
            //if (!IsAshiLoading && totalItemCount - visibleItemCount <= firstVisibleItem)
            //{
            //    Refresh_Ashi?.Invoke();
            //    IsAshiLoading = true;
            //}
        }
        private void OnRecyclerViewScrollStateListener(RecyclerView recyclerView, int newState)
        {
            if (newState == RecyclerView.ScrollStateIdle && lastVisibleItem + 1 == recyclerView.GetAdapter().ItemCount)
            {
                if (!IsNoMoreData)
                    if (!IsAshiLoading && !IsAtataLoading)
                    {
                        v_AshiLoading.Visibility = ViewStates.Visible;
                        Az_RecyclerView.ScrollToPosition(recyclerView.GetAdapter().ItemCount);
                        Refresh_Ashi?.Invoke();
                        IsAshiLoading = true;
                    }
            }
        }
        #endregion

        public void OnAtamaFinish()
        {
            this.Refreshing = false;
            IsAtataLoading = false;
        }

        public void OnAshiFinish()
        {
            v_AshiLoading.Visibility = ViewStates.Gone;
            IsAshiLoading = false;
        }

        public void OnNoMoreData(bool bo)
        {
            IsNoMoreData = bo;
        }

        private class AzOnScrollListener : RecyclerView.OnScrollListener
        {
            private Action<RecyclerView, int, int> OnScrolledAct;
            private Action<RecyclerView, int> OnScrollStateChangedAct;

            public AzOnScrollListener(Action<RecyclerView, int, int> OnScrolledAct, Action<RecyclerView, int> OnScrollStateChangedAct)
            {
                this.OnScrolledAct = OnScrolledAct;
                this.OnScrollStateChangedAct = OnScrollStateChangedAct;
            }

            public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
            {
                base.OnScrolled(recyclerView, dx, dy);
                OnScrolledAct?.Invoke(recyclerView, dx, dy);
            }

            public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
            {
                base.OnScrollStateChanged(recyclerView, newState);
                OnScrollStateChangedAct?.Invoke(recyclerView, newState);

            }
        }


    }

}