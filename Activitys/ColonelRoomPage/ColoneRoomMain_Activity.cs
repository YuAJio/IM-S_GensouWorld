using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Baidu.Aip.Ocr;
using Com.Nostra13.Universalimageloader.Core;
using IdoMaster_GensouWorld.Activitys.BattlePage;
using IdoMaster_GensouWorld.Utils;
using IMAS.BaiduAI;
using IMAS.BaiduAI.Identification_Text;
using IMAS.CupCake.Extensions;
using IMAS.LocalDBManager.Models;
using IMAS.Tips.Logic.LocalDBManager;
using IMAS.Utils.Files;
using IMAS.Utils.Sp;
using Karan.Churi;
using Newtonsoft.Json;

namespace IdoMaster_GensouWorld.Activitys.ColonelRoomPage
{
    /// <summary>
    /// 制作人自室
    /// </summary>
    [Activity(Label = "ColoneRoomMain_Activity", Theme = "@style/Theme.PublicTheme",
        ConfigurationChanges =
     Android.Content.PM.ConfigChanges.Orientation |
     Android.Content.PM.ConfigChanges.ScreenSize |
      Android.Content.PM.ConfigChanges.Keyboard |
      Android.Content.PM.ConfigChanges.KeyboardHidden), /*IntentFilter(actions:new string[] { "android.media.action.CAMERA" })*/]
    public class ColoneRoomMain_Activity : BaseActivity
    {
        #region UI控件
        /// <summary>
        /// 休息
        /// </summary>
        private TextView tv_qk;
        /// <summary>
        /// 改变背景
        /// </summary>
        private TextView tv_change;
        /// <summary>
        /// 改变Id
        /// </summary>
        private TextView tv_change_id;
        /// <summary>
        /// 离开
        /// </summary>
        private TextView tv_leave;
        /// <summary>
        /// 背景图
        /// </summary>
        private ImageView iv_background;
        #endregion

        private Model_ProducerInfo info_Producer;
        private AzPermissionManager permissionManager;
        private List<string> list_Permission;

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
                var jk = bundler.GetString("Info");
                info_Producer = jk.ToObject<Model_ProducerInfo>();
            }
        }

        public override void C_InitView()
        {
            tv_qk = FindViewById<TextView>(Resource.Id.tv_rest);
            tv_change = FindViewById<TextView>(Resource.Id.tv_change_background);
            tv_change_id = FindViewById<TextView>(Resource.Id.tv_change_info);
            tv_leave = FindViewById<TextView>(Resource.Id.tv_leave);
            iv_background = FindViewById<ImageView>(Resource.Id.iv_background);
        }

        public override void D_BindEvent()
        {
            tv_qk.Click += OnClickListener;
            tv_change.Click += OnClickListener;
            tv_change_id.Click += OnClickListener;
            tv_leave.Click += OnClickListener;
        }

        public override void E_InitData()
        {
            list_Permission = new List<string>()
            {
                Manifest.Permission.Camera,
                Manifest.Permission.ReadExternalStorage,
                Manifest.Permission.WriteExternalStorage,
            };
            permissionManager = new AzPermissionManager(list_Permission);

            permissionManager.CheckAndRequestPermissions(this);
            var bgPath = LoadBackGroundPicPath();
            if (!string.IsNullOrEmpty(bgPath))
            {
                ImageLoader.Instance.DisplayImage(bgPath, iv_background, ImageLoaderHelper.BackGroundImageOption());
            }
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
                case Resource.Id.tv_change_background:
                    {
                        //改变庭院背景
                        ShowConfim("自室寫眞選択", "どちらにするの？",
                        (j, k) =>
                        {
                            //if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                            //{
                            //    var checkpermisi = ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera);
                            //    ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.Camera }, 222);
                            //}
                            //else

                            if (IsPermissionGranted(Manifest.Permission.Camera))
                                //从相机选择
                                ToTakePhoto();
                            else
                                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.Camera }, 10020);

                        },
                        (j, k) =>
                        {
                            //从手机选择
                            Intent intent;

                            if (Build.VERSION.SdkInt < BuildVersionCodes.Kitkat)
                            {
                                intent = new Intent(Intent.ActionGetContent);
                                intent.SetType("image/*");
                            }
                            else
                            {
                                intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                            }
                            this.StartActivityForResult(intent, IMAS_Constants.OnAlbumSelectKey);
                        }
                        , "カメラがら", "写真集がら"
                      );
                    }
                    break;
                case Resource.Id.tv_change_info:
                    {
                        //改变身份Id
                        ShowConfim("IDCard選択", "どちらにするの？",
                     (j, k) =>
                     {
                         //从相机选择
                         ToTakeIdCardPic();
                     },
                     (j, k) =>
                     {
                         //从手机选择
                         Intent intent;

                         if (Build.VERSION.SdkInt < BuildVersionCodes.Kitkat)
                         {
                             intent = new Intent(Intent.ActionGetContent);
                             intent.SetType("image/*");
                         }
                         else
                         {
                             intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                         }
                         this.StartActivityForResult(intent, IMAS_Constants.OnSelectAIdCardPictrueKey);
                     }
                     , "カメラがら", "写真集がら"
                   );

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

        /// <summary>
        /// 读取背景图片地址
        /// </summary>
        /// <param name="path"></param>
        private string LoadBackGroundPicPath()
        {
            return AndroidPreferenceProvider.GetInstance().GetString(IMAS_Constants.SpRoomBackGroundPathKey);
        }

        /// <summary>
        /// 保存背景图片地址
        /// </summary>
        /// <param name="path"></param>
        private void SaveBackGroundPicPath(string path)
        {
            isRefresh = true;
            AndroidPreferenceProvider.GetInstance().PutString(IMAS_Constants.SpRoomBackGroundPathKey, path);
        }

        /// <summary>
        /// 返回上一个页面
        /// </summary>
        private void BackToTheFutrue()
        {
            this.Finish();
        }

        #region 拍照流程
        /// <summary>
        /// 照片临时绝对路径
        /// </summary>
        private string PicLastPathTemp = "";
        /// <summary>
        /// 照片绝对路径
        /// </summary>
        private string PicLastPath = "";

        private void ToTakePhoto()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            intent.PutExtra(MediaStore.ExtraVideoQuality, 1);
            PicLastPath = GetPhotopath();
            PicLastPathTemp = GetPhotopathTemp();
            Java.IO.File outPhoto = new Java.IO.File(PicLastPathTemp);
            var uri = Android.Net.Uri.FromFile(outPhoto);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                uri = FileProvider.GetUriForFile(this, PackageName + ".fileprovider", outPhoto);

            //获取拍照后未压缩的原图片,并保存在uri路径中
            intent.PutExtra(MediaStore.ExtraOutput, uri);
            this.StartActivityForResult(intent, IMAS_Constants.OnTakeAPictrueKey);
        }

        /// <summary>
        /// 获取临时文件
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        private string GetPhotopathTemp()
        {
            //文件夹路径
            var cachePath = $"{FilePathManager.GetInstance().GetPrivateRootDirPath()}/PicFile/";
            //检测并创建目录
            FilePathManager.GetInstance().CreateDir(cachePath);
            //照片全路径
            var fileAbPath = System.IO.Path.Combine(cachePath, "temp.jpg"); //$"{cachePath}" + GetSystemDate();
            return fileAbPath;
        }

        /// <summary>
        /// 获取照片路径
        /// </summary>
        /// <returns></returns>
        private string GetPhotopath()
        {
            //文件夹路径
            var cachePath = $"{FilePathManager.GetInstance().GetPrivateRootDirPath()}/PicFile/";
            //检测并创建目录
            FilePathManager.GetInstance().CreateDir(cachePath);
            //照片全路径
            var fileAbPath = System.IO.Path.Combine(cachePath, Guid.NewGuid().ToString("N") + ".jpg"); //$"{cachePath}" + GetSystemDate();
            return fileAbPath;
        }

        /// <summary>
        /// 保存拍摄的照片
        /// </summary>
        private void SaveNewPic()
        {
            try
            {
                {//处理图片，测试是否出现内存溢出OOM,，测试压缩比例尺，质量压缩
                    var newOpts = new Android.Graphics.BitmapFactory.Options();
                    Bitmap bmp = null;
                    int scaleFactor = 1;
                    using (FileStream ins = File.OpenRead(PicLastPathTemp))
                    {
                        var tempBmp = BitmapFactory.DecodeStream(ins, new Rect(), newOpts);
                        var oldW = newOpts.OutWidth;
                        var oldH = newOpts.OutHeight;
                        scaleFactor = oldW > oldH ? oldW / 1600 : oldH / 1600;
                        tempBmp.Recycle();
                    }
                    using (FileStream ins = File.OpenRead(PicLastPathTemp))
                    {
                        newOpts.InSampleSize = scaleFactor;
                        bmp = BitmapFactory.DecodeStream(ins, new Rect(), newOpts);
                    }

                    using (FileStream fs = new FileStream(PicLastPath, FileMode.CreateNew))
                    {
                        bmp.Compress(Bitmap.CompressFormat.Jpeg, 50, fs);//质量压缩方法,压缩质量0~100 (100代表不压缩),把压缩后的数据存放到baos
                    }
                    //回收
                    bmp.Recycle();
                    //删除临时文件
                    File.Delete(PicLastPathTemp);

                }
            }
            catch (Exception e)
            {
                ShowMsgLong("エラー！" + e.Message);
            }
        }
        #endregion

        #region 识别身份证

        /// <summary>
        /// 识别身份证拍取图片
        /// </summary>
        private void ToTakeIdCardPic()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            intent.PutExtra(MediaStore.ExtraVideoQuality, 1);
            PicLastPath = GetPhotopath();
            PicLastPathTemp = GetPhotopathTemp();
            Java.IO.File outPhoto = new Java.IO.File(PicLastPathTemp);
            var uri = Android.Net.Uri.FromFile(outPhoto);
            //获取拍照后未压缩的原图片,并保存在uri路径中
            intent.PutExtra(MediaStore.ExtraOutput, uri);
            this.StartActivityForResult(intent, IMAS_Constants.OnTakeAIdCardPictrueKey);
        }

        private void ToTakeHttpResult(string path)
        {

            ShowWaitDiaLog("しばらくお待ちください", false);
            Task.Run(() =>
            {
                //SaveNewPic();
                var result = BaiduTextRecognitionUtils.GetKagemusha().IdCard(path, 1);
                return result;
            }).ContinueWith(t =>
            {
                HideWaitDiaLog();
                if (t.Exception != null)
                {
                    return;
                }
                var obj = t.Result;
                var errorCode = obj.GetInt32("error_code");
                if (errorCode <= 0)
                {
                    var jk = JsonConvert.DeserializeObject<AIM_IDCard.Words_result>(obj.GetString("words_result"));
                    //var jk1 = obj.GetObject<List<AIM_IDCard.Words_result>>("words_result");
                    var js = $"名前：{jk.姓名.Words}\nID：{jk.公民身份号码.Words}\n住所：{jk.住址.Words}";
                    ShowBigToast(js);
                }
                else
                {
                    ShowMsgLong("エラー:" + errorCode);
                    return;
                }

                //var jk = t.Result.ToObject<AIM_IDCard>();
                //var jc = 1 + 2;
            }, TaskScheduler.FromCurrentSynchronizationContext());
            //var cachePath = $"{FilePathManager.GetInstance().GetPrivateRootDirPath()}/PicFile/";
            //var jk = Directory.GetFiles(cachePath);
        }
        #endregion

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                var imagePath = "";
                switch (requestCode)
                {
                    #region 设置背景
                    case IMAS_Constants.OnAlbumSelectKey:
                        {//选择相册
                            var jk = FilePathManager.GetInstance().GetRealFilePath(this, data.Data);
                            imagePath = "file://" + jk;
                            SaveBackGroundPicPath(imagePath);
                            ImageLoader.Instance.DisplayImage(imagePath, iv_background, ImageLoaderHelper.BackGroundImageOption());

                        }
                        break;
                    case IMAS_Constants.OnTakeAPictrueKey:
                        {//拍个照片
                            ShowWaitDiaLog("しばらくお待ちください", false);
                            Task.Run(() =>
                            {
                                SaveNewPic();
                            }).ContinueWith(t =>
                            {
                                HideWaitDiaLog();
                                imagePath = "file://" + PicLastPath;
                                SaveBackGroundPicPath(imagePath);
                                ImageLoader.Instance.DisplayImage(imagePath, iv_background, ImageLoaderHelper.BackGroundImageOption());
                            }, TaskScheduler.FromCurrentSynchronizationContext());
                        }
                        break;
                    #endregion

                    #region 识别身份证
                    case IMAS_Constants.OnSelectAIdCardPictrueKey:
                        {
                            var jk = FilePathManager.GetInstance().GetRealFilePath(this, data.Data);
                            ToTakeHttpResult(jk);
                        }
                        break;
                    case IMAS_Constants.OnTakeAIdCardPictrueKey:
                        {//验证身份证
                            ToTakeHttpResult(PicLastPathTemp);
                        }
                        break;
                        #endregion

                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        /// <summary>
        /// 返回是否刷新
        /// </summary>
        private bool isRefresh = false;
        /// <summary>
        /// 重写Activity结束
        /// </summary>
        public override void Finish()
        {
            if (isRefresh)
            {
                isRefresh = false;
                this.SetResult(Result.Ok);
            }
            base.Finish();
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
            var p_Id = AndroidPreferenceProvider.GetInstance().GetInt(IMAS_Constants.SpProducerInfoIdKey);
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



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            permissionManager.CheckResult(requestCode, permissions, grantResults);

            var granted = permissionManager.Status[0].Granted;
            var denied = permissionManager.Status[0].Denied;
        }
    }

}