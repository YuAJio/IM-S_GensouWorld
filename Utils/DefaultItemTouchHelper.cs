using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;

namespace IdoMaster_GensouWorld.Utils
{
    /// <summary>
    /// RecyclerView拖动控件工具
    /// </summary>
    public class DefaultItemTouchHelper : AzuminItemTouchHelper
    {
        private DefautItemTouchHelpCallback itemTouchHelpCallBack;

        public DefaultItemTouchHelper(DefautItemTouchHelpCallback.IOnItemTouchCallbackEvent onItemTouchE) : base(new DefautItemTouchHelpCallback(onItemTouchE))
        {
            itemTouchHelpCallBack = (DefautItemTouchHelpCallback)GetCallback();
        }

        public void SetDragEnable(bool canDrag)
        {
            itemTouchHelpCallBack.IsCanDrag = canDrag;
        }

        public void SetSwipEnable(bool canSwipe)
        {
            itemTouchHelpCallBack.IsCanSwipe = canSwipe;
        }

    }



    public class AzuminItemTouchHelper : ItemTouchHelper
    {
        private readonly Callback mCallBack;
        public AzuminItemTouchHelper(Callback callback) : base(callback)
        {
            this.mCallBack = callback;
        }
        public Callback GetCallback()
        {
            return mCallBack;
        }
    }
    public class DefautItemTouchHelpCallback : ItemTouchHelper.Callback
    {
        /// <summary>
        /// Item操作回调
        /// </summary>
        private IOnItemTouchCallbackEvent onItemTouchEvent;
        /// <summary>
        /// 是否可以拖拽
        /// </summary>
        public bool IsCanDrag { private get; set; }

        /// <summary>
        /// 是否可以被滑动
        /// </summary>
        public bool IsCanSwipe { private get; set; }


        public DefautItemTouchHelpCallback(IOnItemTouchCallbackEvent onItemTouchEvent)
        {
            this.onItemTouchEvent = onItemTouchEvent;
        }

        public void SetOnItemTouchEvent(IOnItemTouchCallbackEvent onItemTouchEvent)
        {
            this.onItemTouchEvent = onItemTouchEvent;
        }

        public override bool IsLongPressDragEnabled { get { return IsCanDrag; } }
        public override bool IsItemViewSwipeEnabled { get { return IsCanSwipe; } }

        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            var layoutManager = recyclerView.GetLayoutManager();
            if (layoutManager is GridLayoutManager)
            {
                int dragFlag = ItemTouchHelper.Left | ItemTouchHelper.Right | ItemTouchHelper.Up | ItemTouchHelper.Down;
                int swipeFlag = 0;
                // create make
                return MakeMovementFlags(dragFlag, swipeFlag);
            }
            else if (layoutManager is LinearLayoutManager linearLayoutManager)
            {
                var orientation = linearLayoutManager.Orientation;
                var dragFlag = 0;
                var swipeFlag = 0;

                // 为了方便理解，相当于分为横着的ListView和竖着的ListView
                if (orientation == LinearLayoutManager.Horizontal)
                {// 如果是横向的布局
                    swipeFlag = ItemTouchHelper.Up | ItemTouchHelper.Down;
                    dragFlag = ItemTouchHelper.Left | ItemTouchHelper.Right;
                }
                else if (orientation == LinearLayoutManager.Vertical)
                {// 如果是竖向的布局，相当于ListView
                    dragFlag = ItemTouchHelper.Up | ItemTouchHelper.Down;
                    swipeFlag = ItemTouchHelper.Left | ItemTouchHelper.Right;
                }
                return MakeMovementFlags(dragFlag, swipeFlag);
            }
            return 0;
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return onItemTouchEvent != null ? onItemTouchEvent.OnMove(viewHolder.AdapterPosition, target.AdapterPosition) : false;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            if (onItemTouchEvent != null)
                onItemTouchEvent.OnSwiped(viewHolder.AdapterPosition);
        }


        public interface IOnItemTouchCallbackEvent
        {
            /// <summary>
            /// 当某个Item被滑动删除的时候
            /// </summary>
            /// <param name="adapterPosition">item的Position</param>
            void OnSwiped(int adapterPosition);


            /// <summary>
            /// 当两个item位置互换的时候
            /// </summary>
            /// <param name="srcPosition">拖拽的item的Position</param>
            /// <param name="targetPosition">目的地的Item的Position</param>
            /// <returns>true:处理了  false:没处理</returns>
            bool OnMove(int srcPosition, int targetPosition);

        }

    }
}