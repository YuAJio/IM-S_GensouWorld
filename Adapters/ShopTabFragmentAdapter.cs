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
using Android.Support.V4.App;
using IdoMaster_GensouWorld.Activitys.ShopPage;
using Java.Lang;

namespace IdoMaster_GensouWorld.Adapters
{
    /// <summary>
    /// 商店页面碎片适配器
    /// </summary>
    public class ShopTabFragmentAdapter : FragmentPagerAdapter
    {
        private Java.Lang.String[] titles;
        private Context context;
        private List<FragmentShopPage_Pager> fragments;
        private Android.Support.V4.App.FragmentManager fm;

        public ShopTabFragmentAdapter(List<FragmentShopPage_Pager> fragments, Java.Lang.String[] titles, Android.Support.V4.App.FragmentManager fm, Context context) : base(fm)
        {
            this.titles = titles;
            this.context = context;
            this.fragments = fragments;
            this.fm = fm;
        }

        public void RefreshData(List<FragmentShopPage_Pager> fragments, Java.Lang.String[] titles)
        {
            this.fragments = fragments;
            this.titles = titles;
            NotifyDataSetChanged();
        }
        public override int Count
        {
            get
            {
                return fragments == null ? 0 : fragments.Count;
            }
        }

        public override int GetItemPosition(Java.Lang.Object @object)
        {
            return PositionNone;
        }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return fragments == null ? null : fragments[position];
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return titles == null ? new Java.Lang.String() : titles[position];
        }
        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            return S(container, position);
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            var fragment = (@object as FragmentShopPage_Pager);
            fm.BeginTransaction().Hide(fragment).Commit();
        }

        private Java.Lang.Object S(ViewGroup container, int position)
        {
            var fragment = (FragmentShopPage_Pager)base.InstantiateItem(container, position);//fragments[position];//
            fm.BeginTransaction().Show(fragment).Commit();
            fragments[position] = fragment;
            return fragment;
        }
        private Java.Lang.Object F(ViewGroup container, int position)
        {
            var fragment = (FragmentShopPage_Pager)base.InstantiateItem(container, position);
            var tag = fragment.Tag;
            var ft = fm.BeginTransaction();
            {
                ft.Remove(fragment);
                fragment = null;
                fragment = fragments[position % fragments.Count];
                ft.Add(container.Id, fragment, tag);
                ft.Attach(fragment);
                ft.Commit();
            }

            return fragment;
        }



    }
}