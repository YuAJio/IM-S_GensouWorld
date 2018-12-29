using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using Com.Liaoinstan.SpringViewLib.Container;
using Com.Liaoinstan.SpringViewLib.Widgets;
using Com.Tubb.Smrv;
using Hardware.Print;
using IdoMaster_GensouWorld.Activitys.Test;
using IdoMaster_GensouWorld.Adapters;
using IdoMaster_GensouWorld.Utils;
using ZXing.QrCode;

namespace IdoMaster_GensouWorld.Activitys.MainPage
{
    [Activity(Label = "PrintTestActivity", Theme = "@style/Theme.PublicThemePlus")]
    public class PrintTestActivity : BaseActivity, DefautItemTouchHelpCallback.IOnItemTouchCallbackEvent
    {
        private RecyclerView rr_list;
        private SpringView sv_list;

        private NashaAdapter adapter;
        private List<TagView> views;
        private Printer m_print = new Printer();

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_print_test;

        }

        public override void B_BeforeInitView()
        {
            adapter = new NashaAdapter(this);
        }

        public override void C_InitView()
        {
            sv_list = FindViewById<SpringView>(Resource.Id.sv_list);
            //sv_list.SetType(SpringView.Types.Follow);
            sv_list.SetGive(SpringView.Give.None);
            sv_list.SetListener(new SpringViewListener(OnLoadMore, OnRefresh));
            sv_list.Header = new AcFunHeader(this, Resource.Mipmap.bg_acfun_refresh_head);
            sv_list.Footer = new AcFunFooter(this, Resource.Mipmap.bg_acfun_refresh_foot);

            rr_list = FindViewById<RecyclerView>(Resource.Id.rv_list);
            rr_list.SetLayoutManager(new LinearLayoutManager(this));


            #region 滑动拖拽事件绑定
            var itemTouchHelper = new DefaultItemTouchHelper(this);
            itemTouchHelper.AttachToRecyclerView(rr_list);
            itemTouchHelper.SetDragEnable(true);
            itemTouchHelper.SetSwipEnable(true);
            #endregion
        }

        public override void D_BindEvent()
        {
            rr_list.SetAdapter(adapter);

            //rr_list.Refresh_Ashi -= OnRefresh_Ash;
            //rr_list.Refresh_Atama -= OnRefresh_Atama;

            //rr_list.Refresh_Ashi += OnRefresh_Ash;
            //rr_list.Refresh_Atama += OnRefresh_Atama;
        }

        public override void E_InitData()
        {
            var data = new List<RefreshInfo>()
            {
                new RefreshInfo(){Name="女仆中国" },
                new RefreshInfo(){Name="甩甩手吧" },
                new RefreshInfo(){Name="哪里都搞失踪" },
                new RefreshInfo(){Name="没有糖吗?" },
                new RefreshInfo(){Name="我可不敢买" },
                new RefreshInfo(){Name="这可真大" },
                new RefreshInfo(){Name="算吧了,交不起" },
                new RefreshInfo(){Name="飞过去把,帮我定了" },
                new RefreshInfo(){Name="打了四个,好累" },
            };
            adapter.SetDataList(data);
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }


        public void OnSwiped(int adapterPosition)
        {
            adapter.DataList.RemoveAt(adapterPosition);
            adapter.NotifyItemRemoved(adapterPosition);
        }

        public bool OnMove(int srcPosition, int targetPosition)
        {
            var dataList = adapter.DataList.ToList();
            var cliItem = dataList[srcPosition];
            dataList.Remove(cliItem);
            dataList.Insert(targetPosition, cliItem);
            adapter.NotifyItemMoved(srcPosition, targetPosition);
            return true;
        }

        private void OnRefresh()
        {
            adapter.DataList.Insert(0, new RefreshInfo() { Name = "The Sound Of Slience" });
            adapter.NotifyItemInserted(0);
            sv_list.OnFinishFreshAndLoad();
        }

        private int RefreshLimt = 0;

        private void OnLoadMore()
        {
            if (RefreshLimt > 5)
                RefreshLimt = 0;
            else
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1 * 1000);
                }).ContinueWith(t =>
                {
                    adapter.DataList.Add(new RefreshInfo() { Name = "Embers" });
                    adapter.NotifyItemInserted(adapter.ItemCount);
                    RefreshLimt++;
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            sv_list.OnFinishFreshAndLoad();
        }


        //private void OnRefresh_Atama()
        //{
        //    //烫头
        // 
        //}

        //

        //private void OnRefresh_Ash()
        //{
        //    //洗脚
        //   
        //}

        #region 适配器
        public class RefreshInfo
        {
            public string Name { get; set; }
        }
        public class NashaAdapter : AsakuraBaseRvAdapter<RefreshInfo>
        {
            public NashaAdapter(Context context)
            {
                this.context = context;
            }

            public void SetDataList(List<RefreshInfo> dataList)
            {
                this.SetContainerList(dataList);
            }

            public Action<int, int> onItemImageClickListener;

            public override RefreshInfo this[int position] { get { return list_data[position]; } }

            protected override void AbOnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var viewHolder = holder as ViewHolder;
                var data = list_data[position];

                viewHolder.tv_name.Text = $"{data.Name}";

            }

            protected override RecyclerView.ViewHolder AbOnCreateViewHolder(ViewGroup parent, int viewType)
            {
                return new ViewHolder(View.Inflate(context, Resource.Layout.item_chose_soubi_item, null));
            }

            private void OnImageClicKListener(object sender, EventArgs eventArgs)
            {
                var v = sender as View;
                var position = Convert.ToInt32(v.Tag);

                onItemImageClickListener?.Invoke(position, v.Id);
            }


            private class ViewHolder : RecyclerView.ViewHolder
            {
                public TextView tv_name;

                public ViewHolder(View itemView) : base(itemView)
                {
                    tv_name = itemView.FindViewById<TextView>(Resource.Id.tv_name);
                }
            }
        }
        #endregion

        private class SpringViewListener : SpringView.IOnFreshListener
        {
            public SpringViewListener(Action actOne, Action actTwo)
            {
                this.OnLoadMoreAct = actOne;
                this.OnRefreshAct = actTwo;
            }

            private Action OnLoadMoreAct;
            private Action OnRefreshAct;

            public void OnLoadMore()
            {
                OnLoadMoreAct?.Invoke();
            }

            public void OnRefresh()
            {
                OnRefreshAct?.Invoke();
            }
        }

    }

  
}