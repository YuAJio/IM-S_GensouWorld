using IMAS.LocalDBManager.Models;
using SQLite;
using System;
using System.Threading.Tasks;

namespace IMAS.Tips.Logic.LocalDBManager
{
    public class IMAS_ProAppDB : SQLiteAsyncConnection, IDisposable
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbPath"></param>
        /// <param name="userId"></param>
        /// <param name="autoCreateDb"></param>
        internal IMAS_ProAppDB(string dbPath, long userId, bool autoCreateDb = true)
            : base(dbPath)
        {
            //_xmUserId = userId;

        }

        /// <summary>
        /// 初始化方法
        /// </summary>
        public void Init()
        {
            Task.Run(() =>
            {
                CreateTabls();
            }).ContinueWith(t =>
            {
                if (t.Exception != null)
                {
                    Console.WriteLine("创建数据库表异常：" + t.Exception.Message);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// 创建数据库库表
        /// </summary>
        /// <returns></returns>
        private async void CreateTabls()
        {
            //   await CreateTablesAsync(CreateFlags.ImplicitPK, typeof(DownLoadFileInfo), typeof(UploadFileSceneInfo), typeof(SceneInfo), typeof(SearchHistoryInfo), typeof(PersonalCommonRecord), typeof(ShoppingCartDetail), typeof(FunMenuItem), typeof(FunMenuItemBase), typeof(MessageBase));
            await CreateTablesAsync(CreateFlags.ImplicitPK,
                typeof(Model_ProducerInfo),
                typeof(Model_SearchHistory),
                typeof(Model_TodaySellGoods),
                typeof(Model_PlayerControllCharacter),
                typeof(Model_BattleMap)
                );

        }

        /// <summary>
        /// 重建数据
        /// </summary>
        public void RebuildDb()
        {
            //关闭数据库
            CloseConnection();
            //创建数据库
            CreateTabls();
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void CloseConnection()
        {
            ResetPool();
            //SQLiteConnectionPool.Shared.Reset();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            CloseConnection();
        }

    }
}
