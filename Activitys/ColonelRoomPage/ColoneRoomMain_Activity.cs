using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IMAS.CupCake.Extensions;
using IMAS.LocalDBManager.Models;
using IMAS.Tips.Logic.LocalDBManager;
using IMAS.Utils.Sp;

namespace IdoMaster_GensouWorld.Activitys.ColonelRoomPage
{
    /// <summary>
    /// 制作人自室
    /// </summary>
    [Activity(Label = "ColoneRoomMain_Activity", Theme = "@style/Theme.PublicTheme")]
    public class ColoneRoomMain_Activity : BaseActivity
    {
        #region UI控件
        /// <summary>
        /// 休息
        /// </summary>
        private TextView tv_qk;
        /// <summary>
        /// 离开
        /// </summary>
        private TextView tv_leave;
        #endregion
        private Model_ProducerInfo info_Producer;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_colone_room_main;
        }

        public override void B_BeforeInitView()
        {
            var intent = Intent;
            if (intent != null)
            {
                var bundler = intent.GetBundleExtra("value");
                info_Producer = bundler.GetString("info").ToObject<Model_ProducerInfo>();
            }
        }

        public override void C_InitView()
        {
            tv_qk = FindViewById<TextView>(Resource.Id.tv_rest);
            tv_leave = FindViewById<TextView>(Resource.Id.tv_leave);
        }

        public override void D_BindEvent()
        {
            tv_qk.Click += OnClickListener;
            tv_leave.Click += OnClickListener;
        }

        public override void E_InitData()
        {

        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
            switch (v.Id)
            {
                case Resource.Id.tv_rest:
                    {//休息
                        ShowConfim(null, "少し休みしましょうが？(100円)",
                           (j, k) =>
                        {
                            //同意休息
                            if (info_Producer.Money < 100)
                            {
                                ShowDiyToastLong("お金が足りません");
                                return;
                            }
                            SQLiteQProducerInfo();
                        }, null);
                    }
                    break;
                case Resource.Id.tv_leave:
                    {//离开
                        BackToTheFutrue();
                    }
                    break;
            }
        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }


        private void BackToTheFutrue()
        {


            this.Finish();
        }

        #region SQLite相关
        /// <summary>
        /// 更新角色信息
        /// </summary>
        private void SQLiteUpdateCharacterInfo(Model_PlayerControllCharacter info_character)
        {
            Task.Run(async () =>
            {
                info_character.HealthPoint = info_character.MaxHealthPoint;
                info_character.ManaPoint = info_character.MaxManaPoint;
                info_character.StaminaPoint = info_character.MaxStaminaPoint;
                var result = await IMAS_ProAppDBManager.GetInstance().UpdateProducingCharacterInfo(info_character);
                return result;
            }).ContinueWith(t =>
            {
                HideWaitDiaLog();
                SQliteUpdateProducerMoney();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        private void SQLiteQProducerInfo()
        {
            ShowWaitDiaLog("QKQK...");
            var p_Id = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.SpProducerInfoId);
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QProducingCharacterInfo(p_Id);
                return result;
            }).ContinueWith(t =>
            {
                if (t.Result.IsSuccess)
                {
                    var data = t.Result.Data;
                    SQLiteUpdateCharacterInfo(data);
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
                var result = await IMAS_ProAppDBManager.GetInstance().UpdateProducerMoney(info_Producer.PkId, info_Producer.Money - 100);
                return result;
            });
        }
        #endregion
    }
}