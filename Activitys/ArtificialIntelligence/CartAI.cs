//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Android.Provider;
//using IMAS.Utils.Files;
//using IMAS_AI.P_Chan;
//using System.Threading.Tasks;
//using IMAS_AI;

//namespace IdoMaster_GensouWorld.Activitys.ArtificialIntelligence
//{
//    [Activity(Label = "CartAI", Theme = "@style/Theme.PublicTheme")]
//    public class CartAI : BaseActivity
//    {
//        private Button bt_id;
//        private Button bt_bank;
//        private Button bt_tab_j;
//        private Button bt_tab_e;
//        public override int A_GetContentViewId()
//        {
//            return Resource.Layout.activity_ai_cart_konw;
//        }

//        public override void B_BeforeInitView()
//        {
//        }

//        public override void C_InitView()
//        {
//            bt_id = FindViewById<Button>(Resource.Id.bt_id_card);
//            bt_bank = FindViewById<Button>(Resource.Id.bt_bankcard);
//            bt_tab_j = FindViewById<Button>(Resource.Id.bt_tab_j);
//            bt_tab_e = FindViewById<Button>(Resource.Id.bt_tab_e);
//        }

//        public override void D_BindEvent()
//        {
//            bt_bank.Click += OnClickListener;
//            bt_id.Click += OnClickListener;
//            bt_tab_j.Click += OnClickListener;
//            bt_tab_e.Click += OnClickListener;
//        }

//        public override void E_InitData()
//        {

//        }

//        public override void F_OnClickListener(View v, EventArgs e)
//        {
//            switch (v.Id)
//            {
//                case Resource.Id.bt_id_card:
//                    //识别身份证
//                    ToTakePhoto(0x001);
//                    break;
//                case Resource.Id.bt_bankcard:
//                    //识别银行卡
//                    ToTakePhoto(0x002);
//                    break;
//                case Resource.Id.bt_tab_j:
//                    //识别表格,并返回json
//                    ToTakePhoto(0x003);
//                    break;
//                case Resource.Id.bt_tab_e:
//                    //识别表格,并返回excel
//                    ToTakePhoto(0x004);
//                    break;
//            }
//        }

//        public override void G_OnAdapterItemClickListener(AdapterView.ItemClickEventArgs e)
//        {

//        }

//        #region 前往拍照
//        /// <summary>
//        /// 照片绝对路径
//        /// </summary>
//        private string PicLastPath = "";
//        private void ToTakePhoto(int type)
//        {
//            var intent = new Intent(MediaStore.ActionImageCapture);
//            intent.PutExtra(MediaStore.ExtraVideoQuality, 1);
//            //  PicLastPath = GetPhotopath();
//            var outPhoto = new Java.IO.File(GetPhotopath());
//            var uri = Android.Net.Uri.FromFile(outPhoto);
//            //获取拍照后未压缩的原图片,并保存在uri路径中
//            intent.PutExtra(MediaStore.ExtraOutput, uri);
//            StartActivityForResult(intent, type);
//        }
//        /// <summary>
//        /// 获取照片路径
//        /// </summary>
//        /// <returns></returns>
//        private string GetPhotopath()
//        {
//            //文件夹路径
//            var cachePath = $"{FilePathManager.GetInstance().GetPrivateRootDirPath()}/YurishiTempPic/";
//            //检测并创建目录
//            FilePathManager.GetInstance().CreateDir(cachePath);
//            //照片全路径
//            var fileAbPath = System.IO.Path.Combine(cachePath, "temp.png"); //$"{cachePath}" + GetSystemDate();
//            var fileAbPath_s = System.IO.Path.Combine(cachePath, "temp_s.jpg"); //$"{cachePath}" + GetSystemDate();
//            PicLastPath = fileAbPath;
//            return fileAbPath_s;
//        }

//        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
//        {
//            base.OnActivityResult(requestCode, resultCode, data);
//            if (resultCode == Result.Ok)
//            {
//                switch (requestCode)
//                {
//                    case 0x001:
//                        HttpIDCard(PicLastPath);
//                        break;
//                    case 0x002:
//                        HttpBankCard(PicLastPath);
//                        break;
//                    case 0x003:
//                        HttpTab(PicLastPath, TextDuscernType.TabType_Json);
//                        break;
//                    case 0x004:
//                        HttpTab(PicLastPath, TextDuscernType.TabType_Excel);
//                        break;
//                }
//            }
//        }
//        #endregion
//        #region Http相关
//        /// <summary>
//        /// 检测IDCard
//        /// </summary>
//        private void HttpIDCard(string path)
//        {
//            ShowWaitDiaLog("IDカード認証中…");
//            Task.Run(() =>
//            {
//                var result = PChan_Text_Discern.GetInstance().IDCard(path);
//                return result;
//            }).ContinueWith(t =>
//            {
//                HideWaitDiaLog();
//                if (t.Exception != null)
//                {
//                    return;
//                }
//                ShowSureConfim(null, t.Result.ToString(), null);

//            }, TaskScheduler.FromCurrentSynchronizationContext());
//        }
//        /// <summary>
//        /// 检测银行卡
//        /// </summary>
//        private void HttpBankCard(string path)
//        {
//            ShowWaitDiaLog("銀行カード認証中…");
//            Task.Run(() =>
//            {
//                var result = PChan_Text_Discern.GetInstance().BankCard(path);
//                return result;
//            }).ContinueWith(t =>
//            {
//                HideWaitDiaLog();
//                if (t.Exception != null)
//                {
//                    return;
//                }
//                ShowSureConfim(null, t.Result.ToString(), null);

//            }, TaskScheduler.FromCurrentSynchronizationContext());
//        }
//        private string tab_Id = "";
//        private Dictionary<string, object> option;
//        /// <summary>
//        /// 认证表格
//        /// </summary>
//        /// <param name="path"></param>
//        /// <param name="type"></param>
//        private void HttpTab(string path, TextDuscernType type)
//        {
//            ShowWaitDiaLog("テーブル認証中…");

//            Task.Run(() =>
//            {
//                var jk = new M_RealRecognitionResult();
//                var result = PChan_Text_Discern.GetInstance().TabDiscernBegin(path);
//                if (result != null)
//                {
//                    var id = result.result.Where(r => r != null).FirstOrDefault();
//                    if (id != null)
//                    {
//                        tab_Id = id.request_id;
//                        option = new Dictionary<string, object>();
//                        switch (type)
//                        {
//                            case TextDuscernType.TabType_Json:
//                                option.Add("result_type", "json");
//                                break;
//                            case TextDuscernType.TabType_Excel:
//                                option.Add("result_type", "excel");
//                                break;
//                        }
//                        jk = PChan_Text_Discern.GetInstance().TabDiscern(tab_Id, option);
//                    }
//                    else
//                    {
//                        return new M_RealRecognitionResult() { error_msg = "请求识别错误:请求Id未找到" };
//                    }
//                }
//                else
//                {
//                    return new M_RealRecognitionResult() { error_msg = "请求识别超时" };
//                }
//                return jk;
//            }).ContinueWith(t =>
//            {
//                HideWaitDiaLog();
//                if (t.Exception != null)
//                {
//                    return;
//                }
//                if (t.Result.result.ret_code != 3)
//                {
//                    HttpGetTabDiscern();
//                }
//                else
//                {
//                    ShowSureConfim(null, t.Result.result.result_data.ToString(), null);
//                }
//            }, TaskScheduler.FromCurrentSynchronizationContext());

//        }
//        private void HttpGetTabDiscern()
//        {
//            Task.Run(() =>
//            {
//                var result = PChan_Text_Discern.GetInstance().TabDiscern(tab_Id, option);
//                return result;
//            }).ContinueWith(t =>
//            {
//                if (t.Exception != null)
//                {
//                    return;
//                }
//                if (t.Result.result.ret_code != 3)
//                {
//                    HttpGetTabDiscern();
//                }
//            }, TaskScheduler.FromCurrentSynchronizationContext()); ;
//        }

//        #endregion
//    }
//}