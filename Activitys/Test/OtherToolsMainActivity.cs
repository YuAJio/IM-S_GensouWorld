using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using IdoMaster_GensouWorld.Activitys.Test.OtherToooooools;
using IdoMaster_GensouWorld.Activitys.Test.OtherToooooools.SettingsActivitys;
using IdoMaster_GensouWorld.Adapters;
using Ys.BeLazy;

namespace IdoMaster_GensouWorld.Activitys.Test
{
    /// <summary>
    /// 工具页主菜单
    /// </summary>
    [Activity(Label = "OtherToolsMainActivity", Theme = "@style/Theme.PublicThemePlus")]
    public class OtherToolsMainActivity : BaseActivity
    {
        private RecyclerView rv_list;
        private SimpleApdater adapter_1;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_other_tools_main;
        }

        public override void B_BeforeInitView()
        {
            adapter_1 = new SimpleApdater(this);
        }

        public override void C_InitView()
        {
            rv_list = FindViewById<RecyclerView>(Resource.Id.rv_menu);
            rv_list.SetLayoutManager(new LinearLayoutManager(this));

        }

        public override void D_BindEvent()
        {
            adapter_1.onButtonClickAct -= OnItemClickActionImp;
            adapter_1.onButtonClickAct += OnItemClickActionImp;
            rv_list.SetAdapter(adapter_1);
        }

        public override void E_InitData()
        {
            var list = new List<Moudle_Menu>
            {
                new Moudle_Menu() { Title = "获取WIFI列表", Path = typeof(WifiFunctionActivity) },
                new Moudle_Menu() { Title = "自定义设置页面", Path = typeof(YsSettingActivity) }
            };

            adapter_1.SetDataList(list);
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }

        private void OnItemClickActionImp(View sender, int positon)
        {
            var act = adapter_1[positon].Path;
            StartActivity(new Intent(this, act));
        }

        private class SimpleApdater : YsBaseRvAdapter<Moudle_Menu>
        {
            public Action<View, int> onButtonClickAct;
            public SimpleApdater(Context context)
            {
                this.context = context;
            }

            public override Moudle_Menu this[int position] { get { return list_data[position]; } }

            protected override void AbOnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
            {
                var data = list_data[position];
                var holder = viewHolder as ViewHolder;
                holder.bt_Menu.Text = data.Title;

                holder.bt_Menu.Tag = position;
                holder.bt_Menu.Click -= onButtonClick;
                holder.bt_Menu.Click += onButtonClick;
            }

            private void onButtonClick(object sender, EventArgs e)
            {
                var positoin = Convert.ToInt32((sender as View).Tag);
                onButtonClickAct?.Invoke(sender as View, positoin);
            }

            protected override RecyclerView.ViewHolder AbOnCreateViewHolder(ViewGroup parent, int viewType)
            {
                var jk = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_other_tools_main, parent, false);
                return new ViewHolder(jk);
            }
            private class ViewHolder : RecyclerView.ViewHolder
            {
                public Android.Support.V7.Widget.AppCompatButton bt_Menu;
                public ViewHolder(View itemView) : base(itemView)
                {
                    bt_Menu = itemView.FindViewById<AppCompatButton>(Resource.Id.bt_menu);
                }
            }
        }

        private class Moudle_Menu
        {
            public Type Path { get; set; }
            public string Title { get; set; }
        }
    }
}