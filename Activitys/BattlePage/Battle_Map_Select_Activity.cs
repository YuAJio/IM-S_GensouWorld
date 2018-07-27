using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Percent;
using Android.Views;
using Android.Widget;
using IMAS.CupCake.Extensions;
using IMAS.LocalDBManager.Models;
using IMAS.Tips.Logic.LocalDBManager;
using IMAS.Utils.Sp;

namespace IdoMaster_GensouWorld.Activitys.BattlePage
{
    /// <summary>
    /// 选择战斗地图页面
    /// </summary>
    [Activity(Label = "Battle_Map_Select_Activity", Theme = "@style/Theme.PublicThemePlus")]
    public class Battle_Map_Select_Activity : BaseActivity
    {
        #region UI控件
        /// <summary>
        /// 凛冬玫瑰
        /// </summary>
        private ImageView iv_winnter;
        /// <summary>
        /// 百合树
        /// </summary>
        private ImageView iv_yuri;
        /// <summary>
        /// 香蕉海洋
        /// </summary>
        private ImageView iv_banana;
        #endregion
        /// <summary>
        /// 地图列表
        /// </summary>
        private List<Model_BattleMap> list_Maps;
        /// <summary>
        /// 制作人信息
        /// </summary>
        private Model_ProducerInfo producerInfo;

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_select_battle_map;
        }

        public override void B_BeforeInitView()
        {

        }

        public override void C_InitView()
        {
            iv_banana = FindViewById<ImageView>(Resource.Id.iv_banana);
            iv_yuri = FindViewById<ImageView>(Resource.Id.iv_yuri);
            iv_winnter = FindViewById<ImageView>(Resource.Id.iv_winter);
        }

        public override void D_BindEvent()
        {
            iv_yuri.Click += OnClickListener;
            iv_winnter.Click += OnClickListener;
            iv_banana.Click += OnClickListener;
        }

        public override void E_InitData()
        {
            SQLiteQProducerInfo();
            SQLiteQBattleMap();
        }

        public override void F_OnClickListener(View v, EventArgs e)
        {
            var map = new Model_BattleMap();
            if (list_Maps == null || !list_Maps.Any())
            {
                return;
            }
            switch (v.Id)
            {
                case Resource.Id.iv_winter:
                    {
                        map = list_Maps.Where(r => r.MapId == IMAS_Constants.MAP_WINTER_ID).FirstOrDefault();
                        if (producerInfo.Level < map.RestrictionLevel)
                        {
                            ShowBigToast("レヴェルが足りません");
                            return;
                        }
                        ReallyToLoading(map.MapId);
                    }
                    break;
                case Resource.Id.iv_yuri:
                    {
                        map = list_Maps.Where(r => r.MapId == IMAS_Constants.MAP_YURI_ID).FirstOrDefault();
                        if (producerInfo.Level < map.RestrictionLevel)
                        {
                            ShowBigToast("レヴェルが足りません");
                            return;
                        }
                        ReallyToLoading(map.MapId);
                    }
                    break;
                case Resource.Id.iv_banana:
                    {
                        map = list_Maps.Where(r => r.MapId == IMAS_Constants.MAP_BANANA_ID).FirstOrDefault();
                        if (producerInfo.Level < map.RestrictionLevel)
                        {
                            ShowBigToast("レヴェルが足りません");
                            return;
                        }
                        ReallyToLoading(map.MapId);
                    }
                    break;
            }
        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// 跳转到读取页面
        /// </summary>
        /// <param name="mapId"></param>
        private void ReallyToLoading(int mapId)
        {
            SQLiteQCharacterInfomation(mapId);
        }

        #region SQLite相关
        /// <summary>
        /// 查询战斗地图
        /// </summary>
        private void SQLiteQBattleMap()
        {
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QBattleMapInfomation();
                return result;
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    ShowBigToast("エラー");
                    return;
                }
                if (t.Result.IsSuccess)
                {
                    list_Maps = t.Result.Data;
                }
                else
                {
                    ShowDiyToastLong("");
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SQLiteQProducerInfo()
        {
            ShowWaitDiaLog("Loading...");
            var p_Id = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.SpProducerInfoIdKey);
            Task.Run(async () =>
            {
                return await IMAS_ProAppDBManager.GetInstance().QProducerInfo(p_Id);
            }).ContinueWith(t =>
            {
                HideWaitDiaLog();
                if (t.Exception != null)
                {
                    ShowBigToast("エラー");
                    return;
                }
                if (t.Result.IsSuccess)
                {
                    producerInfo = t.Result.Data;
                }
                else
                {
                    producerInfo = new Model_ProducerInfo();
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        private void SQLiteQCharacterInfomation(int mapId)
        {
            var p_Id = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.SpProducerInfoIdKey);
            ShowWaitDiaLog("Loading...");
            Task.Run(async () =>
            {
                var result = await IMAS_ProAppDBManager.GetInstance().QProducingCharacterInfo(p_Id);
                return result;
            }).ContinueWith(t =>
            {
                HideWaitDiaLog();
                if (t.Result.IsSuccess)
                {
                    var data = t.Result.Data;
                    if (data != null)
                    {
                        var intent = new Intent(this, typeof(Battle_Loading_Activity));
                        intent.PutExtra(IMAS_Constants.MapPagePkIdKey, mapId);
                        intent.PutExtra(IMAS_Constants.CharacterLevelKey, data.Level);
                        StartActivity(intent);
                        this.Finish();
                    }
                }
                else
                {
                    ShowDiyToastLong(t.Result.Message);
                    if (t.Result.Code == 201)
                    {
                        this.Finish();
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion
    }
}