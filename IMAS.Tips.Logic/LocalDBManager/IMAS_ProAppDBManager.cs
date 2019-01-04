using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMAS.CupCake.Data;
using IMAS.LocalDBManager.Models;
using IMAS.Tips.Enums;
using IMAS.Utils.Cryptographic;
using IMAS.CupCake.Extensions;
using IMAS.OkHttp.Models;

namespace IMAS.Tips.Logic.LocalDBManager
{
    public class IMAS_ProAppDBManager
    {
        private static IMAS_ProAppDBManager _instance;

        private IMAS_ProAppDBManager() { }
        /// <summary>
        /// 获取单例对象
        /// </summary>
        /// <returns></returns>
        public static IMAS_ProAppDBManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new IMAS_ProAppDBManager();
            }

            return _instance;
        }
        /// <summary>
        /// 上下文
        /// </summary>
        private Context mContext;
        /// <summary>
        /// 数据库操作对象
        /// </summary>
        private IMAS_ProAppDB dbInstance;

        /// <summary>
        /// 检测数据库是否可用
        /// </summary>
        /// <returns></returns>
        public bool CheckManageAppDBManager()
        {
            if (dbInstance == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="dbPath">数据库路径</param>
        public void Init(Context context, string dbPath)
        {
            this.mContext = context;
            dbInstance = new IMAS_ProAppDB(dbPath, 0);
            //var b = Utils.Files.FilePathManager.GetInstance().CheckFileExist(dbPath);
            dbInstance.Init();
        }

        #region 初始化信息
        /// <summary>
        /// 插入商店贩卖信息
        /// </summary>
        /// <param name="date">星期几贩卖</param>
        /// <param name="fuku">服装</param>
        /// <param name="kazari">饰品</param>
        /// <param name="tabemono">食物</param>
        /// <returns></returns>
        public async Task<Results> InsertTodaysSellGoods(DateTime date, List<Model_Items> fuku, List<Model_Items> kazari, List<Model_Items> tabemono)
        {
            var result = new Results();
            var info = new Model_TodaySellGoods();
            info.TodaySell_Fuku = fuku?.ToJson();
            info.TodaySell_Kazari = kazari?.ToJson();
            info.TodaySell_Tabemono = tabemono?.ToJson();
            info.TodayWeekDate = date.DayOfWeek.ToString();

            var count = await dbInstance.InsertAsync(info);
            if (count <= 0)
            {
                return result.Error(message: "商店リセット失敗");
            }
            return result.Success();
        }

        /// <summary>
        /// 获取商店商品
        /// </summary>
        /// <param name="itemType">商品类型</param>
        /// <returns></returns>
        public async Task<Result<List<Model_Items>>> QTodaysShopSellGoods(ItemEnumeration itemType)
        {
            var result = new Result<List<Model_Items>>();
            var dateTime = DateTime.Now.DayOfWeek.ToString();

            var q = $"select * from {typeof(Model_TodaySellGoods).Name} where dateTime = ?";
            var q_Result = await dbInstance.QueryAsync<Model_TodaySellGoods>(q, dateTime);
            var q_Result_Ex = q_Result.FirstOrDefault();
            if (q_Result_Ex != null)
            {
                switch (itemType)
                {
                    case ItemEnumeration.HealingItems:
                        return result.Success(data: q_Result_Ex.TodaySell_Tabemono.ToObject<List<Model_Items>>());
                    case ItemEnumeration.EquipmentItems:
                        return result.Success(data: q_Result_Ex.TodaySell_Fuku.ToObject<List<Model_Items>>());
                    case ItemEnumeration.OtherItems:
                        return result.Success(data: q_Result_Ex.TodaySell_Kazari.ToObject<List<Model_Items>>());
                    default:
                        return result.Error(message: "まだだ");
                }
            }
            else
            {
                return result.Error(message: "データいません");
            }
        }
        /// <summary>
        /// 获取商店商品(直接拿商品对象)
        /// </summary>
        /// <returns></returns>
        public async Task<Result<Model_TodaySellGoods>> QTodaysShopSellGoodsForObject()
        {
            var result = new Result<Model_TodaySellGoods>();
            var dateTime = DateTime.Now.DayOfWeek.ToString();

            var q = $"select * from {typeof(Model_TodaySellGoods).Name} where TodayWeekDate = ?";
            try
            {
                var q_Result = await dbInstance.QueryAsync<Model_TodaySellGoods>(q, dateTime);
                var q_Result_Ex = q_Result.FirstOrDefault();
                if (q_Result_Ex != null)
                {
                    return result.Success(data: q_Result_Ex);
                }
                else
                {
                    return result.Error(message: "データいません");
                }
            }
            catch (Exception e)
            {
                return result.Error(message: "データいません");
            }


        }
        #endregion

        #region 制作人信息相关
        /// <summary>
        /// 将制作人的信息插入进数据库
        /// </summary>
        /// <param name="name">制作人姓名</param>
        /// <param name="birthday">制作人生日</param>
        /// <returns></returns>
        public async Task<Results> InsertProducerInfo(string name, DateTime birthday)
        {
            var result = new Results();
            var info = new Model_ProducerInfo();
            var ra = new Random();

            name = HashCryptographic.ChangeStringToSHA256(name);

            info.Prod_Name = name;
            info.Prod_Brithday = birthday;
            info.Identity_Number = ra.Next(10000, 20000);
            info.Level = 1;
            info.AP = 100;
            info.Exp = 0;
            info.LevelUpExp = 100;
            info.Money = 2000;

            var count = await dbInstance.InsertAsync(info);
            if (count <= 0)
            {
                return result.Error(message: "データ挿入失敗");
            }
            return result.Success();
        }

        /// <summary>
        /// 检查数据库中是否有该制作人
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Result<Model_ProducerInfo>> QProducerInfo(string name)
        {
            var result = new Result<Model_ProducerInfo>();
            name = HashCryptographic.ChangeStringToSHA256(name);
            //查询语句
            var q = $"select * from {typeof(Model_ProducerInfo).Name} where Prod_Name = ?";

            var q_result = await dbInstance.QueryAsync<Model_ProducerInfo>(q, name);
            var q_result_ex = q_result.FirstOrDefault();
            if (q_result_ex == null)
            {
                return result.Error(message: "データいません");
            }
            return result.Success(q_result_ex);
        }

        /// <summary>
        /// 检查数据库中是否有该制作人
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Result<Model_ProducerInfo>> QProducerInfo(int pkId)
        {
            var result = new Result<Model_ProducerInfo>();

            var jk = await dbInstance.FindAsync<Model_ProducerInfo>(pkId);
            if (jk == null)
                return result.Error(message: "データいません");
            return result.Success(data: jk);
            ////查询语句
            //var q = $"select * from {typeof(Model_ProducerInfo).Name} where PkId = ?";

            //var q_result = await dbInstance.QueryAsync<Model_ProducerInfo>(q, pkId);
            //var q_result_ex = q_result.FirstOrDefault();
            //if (q_result_ex == null)
            //{
            //    return result.Error(message: "データいません");
            //}
            //return result.Success(q_result_ex);
        }

        /// <summary>
        /// 更新制作人信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<Results> UpdateProducerInfo(Model_ProducerInfo info)
        {
            var jk = await dbInstance.InsertOrReplaceAsync(info);
            if (jk <= 0)
            {
                return Results.NewError(message: "プロデューサーデータ更新失敗");
            }
            return Results.NewSuccess();
        }
        /// <summary>
        /// 更新制作人信息
        /// </summary>
        /// <param name="pkId"></param>
        /// <param name="chapter"></param>
        /// <param name="isFinish"></param>
        /// <returns></returns>
        public async Task<Results> UpdateProducerChapterInfo(int pkId, ChapterEnumeration chapter, bool isFinish)
        {
            var result = new Results();
            var detail = await dbInstance.GetAsync<Model_ProducerInfo>(pkId);
            if (detail == null)
            {
                return result.Error(message: "データいません");
            }
            switch (chapter)
            {
                case ChapterEnumeration.Prologue:
                    detail.IsPrologueChapterFinish = isFinish;
                    break;
                case ChapterEnumeration.Chapter_One:

                    break;
                case ChapterEnumeration.Chapter_Two:
                    break;
            }
            var num = await dbInstance.UpdateAsync(detail);
            if (num <= 0)
            {
                return result.Error(message: "データ更新失敗");
            }
            return result.Success();
        }
        /// <summary>
        /// 查询制作人所持金钱
        /// </summary>
        /// <returns></returns>
        public async Task<Result<long>> QProducerMoney(int pkId)
        {
            var result = new Result<long>();

            var q = $"select * from {typeof(Model_ProducerInfo).Name} where PkId = ?";
            var q_result = await dbInstance.QueryAsync<Model_ProducerInfo>(q, pkId);
            var detail = q_result.FirstOrDefault();
            if (detail == null)
            {
                return result.Error(message: "データいません", data: 0);
            }
            return result.Success(data: detail.Money);
            //var detail = await dbInstance.GetAsync<Model_ProducerInfo>(pkId);
            //if (detail == null)
            //{
            //    return result.Error(message: "データいません", data: 0);
            //}
            //return result.Success(data: detail.Money);
        }

        #endregion

        #region 所培养偶像相关
        /// <summary>
        /// 插入可培养的偶像信息
        /// </summary>
        /// <returns></returns>
        public async Task<Results> InsertProducingCharactersInfo(List<Model_PlayerControllCharacter> list)
        {
            var result = new Results();
            var count = await dbInstance.InsertAllAsync(list);
            if (count <= 0)
            {
                return result.Error(message: "データ挿入失敗");
            }
            return result.Success();
        }
        /// <summary>
        /// 更新现在所培养偶像的信息
        /// </summary>
        /// <returns></returns>
        public async Task<Results> UpdateProducingCharacterInfo(Model_PlayerControllCharacter data)
        {
            var C_data = await dbInstance.GetAsync<Model_PlayerControllCharacter>(data.PkId);
            C_data = data;
            var num = await dbInstance.UpdateAsync(C_data);
            if (num <= 0)
            {
                return Results.NewError(message: "データ更新失敗");
            }
            return Results.NewSuccess();
        }
        /// <summary>
        /// 查询现在可扭出来的角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<Result<List<Model_PlayerControllCharacter>>> QCanControllCharacterList()
        {
            var result = new Result<List<Model_PlayerControllCharacter>>();

            var q = $"select * from {typeof(Model_PlayerControllCharacter).Name}";

            var q_Result = await dbInstance.QueryAsync<Model_PlayerControllCharacter>(q);
            q_Result = q_Result.Where(r => r.IsGacha).ToList();
            if (q_Result == null)
            {
                return result.Error(message: "データいません");
            }
            return result.Success(data: q_Result);
        }

        /// <summary>
        /// 查询现在培养的偶像
        /// </summary>
        /// <param name="pkId"></param>
        /// <returns></returns>
        public async Task<Result<Model_PlayerControllCharacter>> QProducingCharacterInfo(int p_Id)
        {
            var result = new Result<Model_PlayerControllCharacter>();
            var jk = await dbInstance.GetAsync<Model_ProducerInfo>(p_Id);
            if (jk == null)
            {
                return result.Error(message: "データいません");
            }
            if (String.IsNullOrEmpty(jk.ProducingCharacter))
            {
                return result.Error(message: "キャラクターいません", code: 201);
            }
            var idsList = jk.ProducingCharacter.ToObject<List<long>>();
            var objs = new object[idsList.Count];
            var sb = new StringBuilder();
            sb.Append($"select * from {typeof(Model_PlayerControllCharacter).Name} where CharacterId in ( ");
            for (int i = 0; i < idsList.Count; i++)
            {
                if (i == idsList.Count - 1)
                {
                    sb.Append("? ");
                }
                else
                {
                    sb.Append("?, ");
                }
                objs[i] = idsList[i];
            }
            sb.Append(")");
            var q = sb.ToString();
            var list = await dbInstance.QueryAsync<Model_PlayerControllCharacter>(q, objs);
            if (list == null || !list.Any())
            {
                return result.Error(message: "アイドルのデータいません");
            }
            return result.Success(data: list.FirstOrDefault());
        }

        /// <summary>
        /// 查询现在培养的偶像们
        /// </summary>
        /// <param name="pkId"></param>
        /// <returns></returns>
        public async Task<Result<List<Model_PlayerControllCharacter>>> QProducingCharactersInfo(int p_Id)
        {
            var result = new Result<List<Model_PlayerControllCharacter>>();
            var jk = await dbInstance.GetAsync<Model_ProducerInfo>(p_Id);
            if (jk == null)
            {
                return result.Error(message: "データいません");
            }
            var idsList = jk.ProducingCharacter.ToObject<List<long>>();
            var objs = new object[idsList.Count];
            var sb = new StringBuilder();
            sb.Append($"select * from {typeof(Model_PlayerControllCharacter).Name} where PkId in ( ");
            for (int i = 0; i < idsList.Count; i++)
            {
                if (i == idsList.Count - 1)
                {
                    sb.Append("? ");
                }
                else
                {
                    sb.Append("?, ");
                }
                objs[i] = idsList[i];
            }
            sb.Append(")");
            var q = sb.ToString();
            var list = await dbInstance.QueryAsync<Model_PlayerControllCharacter>(q, objs);
            if (list == null || !list.Any())
            {
                return result.Error(message: "アイドルのデータいません");
            }
            return result.Success(data: list);
        }

        #endregion

        #region 角色信息修改相关
        /// <summary>
        /// 查询角色物品信息(全部)
        /// </summary>
        /// <param name="pkId"></param>
        /// <returns></returns>
        public async Task<Result<List<Model_Items>>> QProducerItemDetails(int pkId)
        {
            var result = new Result<List<Model_Items>>();
            //var q = $"select * from {typeof(Model_ProducerInfo).Name} where PkId = ? and ItemTypes = ?";
            var q = $"select * from {typeof(Model_ProducerInfo).Name} where PkId = ?";
            var q_result = await dbInstance.QueryAsync<Model_ProducerInfo>(q, pkId);
            var q_result_ex = q_result.FirstOrDefault();
            if (q_result_ex == null)
            {
                return result.Error(message: "データいません");
            }
            var jk1 = q_result_ex.Items_Weapon.ToObject<List<Model_Items>>();
            var jk2 = q_result_ex.Items_Armor.ToObject<List<Model_Items>>();
            var jk3 = q_result_ex.Items_Other.ToObject<List<Model_Items>>();
            var jk4 = q_result_ex.Items_Healing.ToObject<List<Model_Items>>();
            if (jk1 == null && jk2 == null && jk3 == null && jk4 == null)
            {
                return result.Error(message: "データいません");
            }
            var jk = new List<Model_Items>();
            jk.AddRange(jk1);
            jk.AddRange(jk2);
            jk.AddRange(jk3);
            jk.AddRange(jk4);
            return result.Success(data: jk);
        }
        /// <summary>
        /// 查询角色物品信息(分类型)
        /// </summary>
        /// <param name="pkId"></param>
        /// <returns></returns>
        public async Task<Result<List<Model_Items>>> QProducerItemDetails(int pkId, ItemEnumeration itemType)
        {
            var result = new Result<List<Model_Items>>();
            //var q = $"select * from {typeof(Model_ProducerInfo).Name} where PkId = ? and ItemTypes = ?";
            var q = $"select * from {typeof(Model_ProducerInfo).Name} where PkId = ?";
            var q_result = await dbInstance.QueryAsync<Model_ProducerInfo>(q, pkId);
            var q_result_ex = q_result.FirstOrDefault();
            if (q_result_ex == null)
            {
                return result.Error(message: "データいません");
            }
            var jk1 = q_result_ex.Items_Weapon.ToObject<List<Model_Items>>();
            var jk2 = q_result_ex.Items_Armor.ToObject<List<Model_Items>>();
            var jk3 = q_result_ex.Items_Other.ToObject<List<Model_Items>>();
            var jk4 = q_result_ex.Items_Healing.ToObject<List<Model_Items>>();
            if (jk1 == null && jk2 == null && jk3 == null && jk4 == null)
            {
                return result.Error(message: "データいません");
            }
            switch (itemType)
            {
                case ItemEnumeration.HealingItems:
                    return result.Success(data: jk4);
                case ItemEnumeration.EquipmentItems:
                    return result.Success(data: jk2);
                case ItemEnumeration.WeaponItems:
                    return result.Success(data: jk1);
                case ItemEnumeration.OtherItems:
                    return result.Success(data: jk3);
            }
            return result.Error(message: "コードエラー");
        }
        /// <summary>
        /// 修改角色所持金
        /// </summary>
        /// <param name="pkId">角色Id</param>
        /// <param name="money">マーニー</param>
        /// <returns></returns>
        public async Task<Results> UpdateProducerMoney(int pkId, long money)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("Money", money);
            var count = await dbInstance.UpdateAsync<Model_ProducerInfo>(pkId, dict);
            if (count > 0)
            {
                return Results.NewSuccess();
            }
            else
            {
                return Results.NewError(message: "データ更新失敗");
            }

            //var result = new Result();
            //var detail = await dbInstance.GetAsync<Model_ProducerInfo>(pkId);
            //if (detail == null)
            //{
            //    return result.Error(message: "データいません");
            //}
            //detail.Money = money;
            //var num = await dbInstance.UpdateAsync(detail);
            //if (num <= 0)
            //{
            //    return result.Error(message: "データ更新失敗");
            //}
            //return result.Success();
        }
        /// <summary>
        /// 修改角色身上物品
        /// </summary>
        /// <param name="pkId">角色Id</param>
        /// <param name="itemList">物品列表</param>
        /// <param name="itemType">物品类型</param>
        /// <returns></returns>
        public async Task<Results> UpdateProducerItemInfo(int pkId, List<Model_Items> itemList, ItemEnumeration itemType)
        {
            var jk = itemList.ToJson();
            var dict = new Dictionary<string, object>();
            switch (itemType)
            {
                case ItemEnumeration.HealingItems:
                    dict.Add("Items_Healing", jk);
                    break;
                case ItemEnumeration.EquipmentItems:
                    dict.Add("Items_Armor", jk);
                    break;
                case ItemEnumeration.WeaponItems:
                    dict.Add("Items_Weapon", jk);
                    break;
                case ItemEnumeration.OtherItems:
                    dict.Add("Items_Other", jk);
                    break;
            }
            var count = await dbInstance.UpdateAsync<Model_ProducerInfo>(pkId, dict);
            if (count > 0)
            {
                return Results.NewSuccess();
            }
            else
            {
                return Results.NewError(message: "データ更新失敗");
            }
        }

        /// <summary>
        /// 插入或更新所培养的偶像
        /// </summary>
        /// <param name="Producer_PkId">制作人Id</param>
        /// <param name="CCC_PkId">偶像Id</param>
        /// <returns></returns>
        public async Task<Results> InsertProducingCharacterInfo(int Producer_PkId, long CCC_Id)
        {
            try
            {
                var q = $"select * from {typeof(Model_ProducerInfo).Name} where PkId = ?";
                var q_result = await dbInstance.QueryAsync<Model_ProducerInfo>(q, Producer_PkId);
                var q_result_ex = q_result.FirstOrDefault();
                if (q_result == null)
                {
                    return Results.NewError(message: "データいません");
                }
                var idsList = new List<long>();
                if (!string.IsNullOrEmpty(q_result_ex.ProducingCharacter))
                {
                    idsList = q_result_ex.ProducingCharacter.ToObject<List<long>>();
                }
                idsList.Add(CCC_Id);

                var dict = new Dictionary<string, object>
            {
                { "ProducingCharacter", idsList.ToJson() }
            };
                var i_result = await dbInstance.UpdateAsync<Model_ProducerInfo>(Producer_PkId, dict);
                if (i_result > 0)
                {
                    return Results.NewSuccess();
                }
                return Results.NewError(message: "データ更新失敗");
            }
            catch (Exception ex)
            {

                throw;
            }
            //var ids_List = new List<int>() { };
            //foreach (var item in datas)
            //{
            //    ids_List.Add(item: item.PkId);
            //}
            //var returnResult = new Result();
            //var dict = new Dictionary<string, object>();
            //dict.Add("ProducingCharacter",ids_List);

            //return returnResult;
        }
        #endregion

        #region 战斗地图相关
        /// <summary>
        /// 插入地图信息
        /// </summary>
        /// <param name="maps">地图信息</param>
        /// <returns></returns>
        public async Task<Results> InsertBattleMapInfomation(List<Model_BattleMap> maps)
        {
            var count = await dbInstance.InsertAllAsync(maps);
            if (count <= 0)
                return Results.NewError(message: "データ挿入失敗");
            return Results.NewSuccess();
        }
        /// <summary>
        /// 查询战斗地图
        /// </summary>
        /// <returns></returns>
        public async Task<Result<List<Model_BattleMap>>> QBattleMapInfomation()
        {
            var returnResult = new Result<List<Model_BattleMap>>();
            var q = $"select * from {typeof(Model_BattleMap).Name}";
            var jk = await dbInstance.QueryAsync<Model_BattleMap>(q);
            if (jk == null)
                return returnResult.Error(message: "データいません");
            return returnResult.Success(data: jk);
        }
        #endregion

        #region 怪物相关
        /// <summary>
        /// 更新地图的怪物信息
        /// </summary>
        /// <param name="pkId">地图Id</param>
        /// <param name="monsets">怪物列表</param>
        /// <returns></returns>
        public async Task<Results> UpdateMonsterInfomation(int pkId, List<Model_Monster> monsets)
        {
            var jk = await dbInstance.FindAsync<Model_BattleMap>(pkId);
            if (jk == null)
                return Results.NewError(message: "地図のデータがいません");
            jk.LivedMonster = monsets.ToJson();
            var dict = new Dictionary<string, object>();
            dict.Add("LivedMonster", monsets.ToJson());
            var count = await dbInstance.UpdateAsync<Model_BattleMap>(pkId, dict);
            if (count <= 0)
                return Results.NewError(message: "データ更新失敗");

            return Results.NewSuccess();
        }
        /// <summary>
        /// 获取生活在该地图的怪物
        /// </summary>
        /// <param name="pkId"></param>
        /// <returns></returns>
        public async Task<Result<List<Model_Monster>>> QBattleMapMonsterInfomation(int pkId)
        {
            var result = new Result<List<Model_Monster>>();
            var jk = await dbInstance.GetAsync<Model_BattleMap>(pkId);
            if (jk == null)
                return result.Error(message: "地図のデータがいません");
            var returnResult = jk.LivedMonster.ToObject<List<Model_Monster>>();
            if (returnResult == null || !returnResult.Any())
                return result.Error(message: "データいません");
            return result.Success(data: returnResult);
        }

        #endregion

        #region 查询历史记录相关
        /// <summary>
        /// 插入历史搜索记录
        /// </summary>
        /// <param name="historyType">类型</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<Results> InsertSearchHistoryInfo(InputHistoryType historyType, string message)
        {
            var result = new Results();
            var info = new Model_SearchHistory();
            info.HistoryType = historyType;
            info.Message = message;
            info.UpdateTime = DateTime.Now;

            var count = await dbInstance.InsertAsync(info);
            if (count <= 0)
            {
                return result.Error(message: "插入数据失败");
            }

            return result.Success();
        }

        /// <summary>
        /// 更新使用时间
        /// </summary>
        /// <param name="pkId">主键</param>
        /// <returns></returns>
        public async Task<Results> UpdateSearchHistoryInfo(int pkId)
        {
            var dict = new Dictionary<string, object>();
            dict.Add("UpdateTime", DateTime.Now);
            var count = await dbInstance.UpdateAsync<Model_SearchHistory>(pkId, dict);
            if (count > 0)
            {
                return Results.NewSuccess(true);
            }
            else
            {
                return Results.NewError(message: "更新失败");
            }
        }

        /// <summary>
        /// 查询历史信息
        /// </summary>
        /// <param name="historyType">历史信息类型</param>
        /// <param name="message">输入信息</param>
        /// <returns></returns>
        public async Task<Result<List<Model_SearchHistory>>> QSearchHistoryInfo(InputHistoryType historyType, string message)
        {
            var result = new Result<List<Model_SearchHistory>>();

            //查询语句
            var q = $"select * from {typeof(Model_SearchHistory).Name} where HistoryType = ? and  Message like ? ";

            var list = await dbInstance.QueryAsync<Model_SearchHistory>(q, historyType, $"%{message}%");

            return result.Success(list);
        }

        /// <summary>
        /// 删除某个历史记录
        /// </summary>
        /// <param name="pkId"></param>
        /// <returns></returns>
        public async Task<Results> DeleteSearchHistoryInfo(int pkId)
        {
            var result = new Results();

            var count = await dbInstance.DeleteAsync<Model_SearchHistory>(pkId);
            if (count <= 0)
            {
                return result.Error(message: "删除数据失败");
            }

            return result.Success(message: "删除数据成功");
        }

        #endregion
        #region 下载文件相关
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="url">请求网络地址</param>
        /// <param name="tempFilePath">临时本地地址</param>
        /// <param name="filePath">本地文件</param>
        /// <returns></returns>
        public async Task<Results> InsertDownLoadFileInfo(string url, string tempFilePath, string filePath, string downloadTag = "")
        {
            var result = new Results();

            if (string.IsNullOrWhiteSpace(url))
            {
                return result.Error();
            }

            var oldInfo = await dbInstance.FindAsync<DownLoadFileInfo>(url);
            if (oldInfo != null)
            {
                return result.Success();
            }

            var info = new DownLoadFileInfo
            {
                FileUrl = url,
                TempLocalPath = tempFilePath,
                LocalPath = filePath,
                DownLoadTag = downloadTag
            };

            var count = await dbInstance.InsertAsync(info);
            if (count <= 0)
            {
                return result.Error(message: "插入数据失败 ");
            }

            return result.Success();
        }

        /// <summary>
        /// 更新进度
        /// </summary>
        /// <param name="url"></param>
        /// <param name="breakPoints"></param>
        /// <returns></returns>
        public async Task<Results> UpdateDownLoadFileInfo(string url, long breakPoints)
        {
            var result = new Results();

            if (string.IsNullOrWhiteSpace(url))
            {
                return result.Error();
            }
            var dict = new Dictionary<string, object>();
            dict.Add("Now", breakPoints);

            var count = await dbInstance.UpdateAsync<DownLoadFileInfo>(url, dict);
            if (count <= 0)
            {
                return result.Error(message: "更新进度失败");
            }

            return result.Success();
        }

        /// <summary>
        /// 更新总大小
        /// </summary>
        /// <param name="url"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public async Task<Results> UpdateDownLoadFileInfoToLen(string url, long len)
        {
            var result = new Results();

            if (string.IsNullOrWhiteSpace(url))
            {
                return result.Error();
            }
            var dict = new Dictionary<string, object>();
            dict.Add("Length", len);
            dict.Add("DownLoadState", DownLoadState.Downloading);

            var count = await dbInstance.UpdateAsync<DownLoadFileInfo>(url, dict);
            if (count <= 0)
            {
                return result.Error(message: "更新总大小失败");
            }

            return result.Success();
        }

        /// <summary>
        /// 更新总大小
        /// </summary>
        /// <param name="url"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public async Task<Results> UpdateDownLoadFileInfoState(string url, DownLoadState state)
        {
            var result = new Results();

            if (string.IsNullOrWhiteSpace(url))
            {
                return result.Error();
            }
            var dict = new Dictionary<string, object>();
            dict.Add("DownLoadState", state);

            var count = await dbInstance.UpdateAsync<DownLoadFileInfo>(url, dict);
            if (count <= 0)
            {
                return result.Error(message: "更新总大小失败");
            }

            return result.Success();
        }


        /// <summary>
        /// 获取数据失败
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Result<DownLoadFileInfo>> GetDownLoadFileInfo(string url)
        {
            var result = new Result<DownLoadFileInfo>();
            if (string.IsNullOrWhiteSpace(url))
            {
                return result.Error();
            }

            var info = await dbInstance.FindAsync<DownLoadFileInfo>(url);
            if (info == null)
            {
                return result.Error(message: "获取数据失败");
            }

            return result.Success(info);
        }
        #endregion

        #region 生成随机字符串帐号
        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum = true, bool useLow = true, bool useUpp = true, bool useSpe = true, string custom = "")
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }
        #endregion
    }
}