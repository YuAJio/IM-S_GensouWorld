using System.Text;

using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Android.Util;
using Android.Animation;
using Java.Lang;
using System;
using IdoMaster_GensouWorld;

namespace CustomControl
{
    public class SlipIndicator : HorizontalScrollView, ViewPager.IOnPageChangeListener
    {
        private ViewPager mViewPager;
        private LinearLayout myLinearLayout;
        //ICYOnPageChangeListener mListener;
        private int oldSelected;

        public SlipIndicator(Context context) : base(context)
        {
            Init(context);
        }
        public SlipIndicator(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }
        public SlipIndicator(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init(context);
        }

        private void Init(Context mcontext)
        {
            this.HorizontalScrollBarEnabled = false;
            myLinearLayout = new LinearLayout(mcontext);
            myLinearLayout.Orientation = Orientation.Horizontal;
            myLinearLayout.SetGravity(GravityFlags.CenterVertical);
            AddView(myLinearLayout, new ViewGroup.LayoutParams(LayoutParams.WrapContent, LayoutParams.MatchParent));
        }

        /// <summary>
        /// 页面选中页改变
        /// </summary>
        public event Action<int> OnPageSelectedAction;

        /// <summary>
        /// 设置viewpager
        /// </summary>
        /// <param name="viewPager"></param>
        public void SetViewPager(ViewPager viewPager)
        {
            if (viewPager == null || mViewPager == viewPager)
            {
                return;
            }
            mViewPager = viewPager;
            mViewPager.AddOnPageChangeListener(this);
            //mViewPager.SetOnPageChangeListener(this);
            notifyDataSetChanged();
            //setViewPager(viewPager, 0);
        }

        public void setViewPager(ViewPager viewPager, int initPos)
        {
            if (mViewPager == viewPager)
            {
                return;
            }
            if (mViewPager != null)
            {
                mViewPager.SetOnPageChangeListener(null);
            }
            PagerAdapter adapter = viewPager.Adapter;
            if (adapter == null)
            {

            }
            mViewPager = viewPager;
            viewPager.SetOnPageChangeListener(this);
            notifyDataSetChanged();
            setCurrentItem(initPos);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="initPos"></param>
        public void RefreshTab(int position)
        {
            notifyDataSetChanged();
            if (position < 0)
            {
                return;
            }
            OnPageSelected(position);
            //setCurrentItem(initPos);
        }

        /// <summary>
        /// 刷新tab
        /// </summary>
        private void notifyDataSetChanged()
        {
            myLinearLayout.RemoveAllViews();
            PagerAdapter mAdapter = mViewPager.Adapter;
            int count = mAdapter.Count;
            for (int i = 0; i < count; i++)
            {
                var txt = new Java.Lang.String(mAdapter.GetPageTitle(i));
                addTab(i, txt);
            }
            RequestLayout();
        }

        //代码添加顶部textview
        private void addTab(int index, Java.Lang.String text)
        {
            MyTabView tabView = new MyTabView(this.Context);
            tabView.index = index;
            tabView.Focusable = true;
            tabView.Click += TabView_Click; ;
            tabView.Text = text.ToString();
            tabView.SetTextSize(ComplexUnitType.Sp, 13);
            tabView.SetTextColor(Resources.GetColor(Resource.Color.darkgray));
            tabView.SetPadding(20, 0, 20, 0);
            tabView.Gravity = GravityFlags.CenterVertical;
            tabView.LayoutParameters = new ViewGroup.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.MatchParent);
            myLinearLayout.AddView(tabView);
        }
        private void TabView_Click(object sender, EventArgs e)
        {
            MyTabView tabView = (MyTabView)sender;
            oldSelected = mViewPager.CurrentItem;
            int newSelected = tabView.index;
            setCurrentItem(newSelected);
        }
        private void animation(View view)
        {
            ObjectAnimator scaleX = ObjectAnimator.OfFloat(view, "scaleX", 1.1f);
            ObjectAnimator scaleY = ObjectAnimator.OfFloat(view, "scaleY", 1.1f);
            ObjectAnimator fade = ObjectAnimator.OfFloat(view, "alpha", 1f);
            AnimatorSet animSet = new AnimatorSet();
            animSet.Play(scaleX).With(scaleY).With(fade);
            animSet.SetDuration(500);
            animSet.Start();
        }
        private void animation2(View view)
        {

            ObjectAnimator scaleX = ObjectAnimator.OfFloat(view, "scaleX", 1f);
            ObjectAnimator scaleY = ObjectAnimator.OfFloat(view, "scaleY", 1f);
            ObjectAnimator fade = ObjectAnimator.OfFloat(view, "alpha", 0.5f);
            AnimatorSet animSet = new AnimatorSet();
            animSet.Play(scaleX).With(scaleY).With(fade);
            animSet.SetDuration(500);
            animSet.Start();
        }
        public void setCurrentItem(int item)
        {
            if (mViewPager == null)
            {

            }
            int mSelectedTabIndex = item;
            mViewPager.CurrentItem = item;
            int tabCount = myLinearLayout.ChildCount;
            for (int i = 0; i < tabCount; i++)
            {
                TextView child = (TextView)myLinearLayout.GetChildAt(i);
                bool isSelected = (i == item);
                child.Selected = isSelected;
                if (isSelected)
                {
                    //animation(child);
                    child.SetTextColor(Context.Resources.GetColor(Resource.Color.skyblue));
                    child.SetBackgroundResource(Resource.Drawable.bg_shop_tab);
                    child.SetPadding(20, 0, 20, 0);
                    //animateToTab(item);//动画效果
                }
                else
                {
                    //animation2(child);
                    child.SetTextColor(Context.Resources.GetColor(Resource.Color.darkgray));
                    child.SetBackgroundResource(0);
                    child.SetPadding(20, 0, 20, 0);
                    //child.SetBackgroundColor(Android.Graphics.Color.Transparent);
                }
            }
        }

        //private Handler myHandler = new Handler();

        private MyRunnable mTabSelector;
        private void animateToTab(int position)
        {
            View tabView = myLinearLayout.GetChildAt(position);
            if (mTabSelector != null)
            {
                RemoveCallbacks(mTabSelector);
            }
            mTabSelector = new MyRunnable(tabView, this);
            //mTabSelector = null;
            //myHandler.PostDelayed(mTabSelector, 100);
            Post(mTabSelector);
            //myHandler.RemoveCallbacksAndMessages(null);
        }
        //public void setMyOnPageChangeListener(ICYOnPageChangeListener listener)
        //{
        //    mListener = listener;
        //}

        public void OnPageScrollStateChanged(int state)
        {
            //if (mListener != null)
            //{
            //    mListener.onPageScrollStateChanged(state);
            //}
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
            //if (mListener != null)
            //{
            //    mListener.onPageScrolled(position, positionOffset, positionOffsetPixels);
            //}
        }

        public void OnPageSelected(int position)
        {
            setCurrentItem(position);
            if (OnPageSelectedAction != null)
            {
                OnPageSelectedAction(position);
            }
            //if (mListener != null)
            //{
            //    mListener.onPageSelected(position);
            //}
        }
    }
    public class MyRunnable : Java.Lang.Object, IRunnable
    {
        View tabView;
        SlipIndicator mview;
        public MyRunnable(View tabView, SlipIndicator mview)
        {
            this.tabView = tabView;
            this.mview = mview;
        }
        public void Run()
        {
            int scrollPos = tabView.Left - (mview.Width - tabView.Width) / 2;//计算华东距离
            mview.SmoothScrollTo(scrollPos, 0);
        }
    }

    public class MyTabView : TextView
    {
        public int index;
        public MyTabView(Context context) : base(context)
        {

        }
        public MyTabView(Context context, int index) : base(context)
        {
            this.index = index;
        }

    }
}