using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Nostra13.Universalimageloader.Core;
using IdoMaster_GensouWorld.Activitys.BattlePage;
using IdoMaster_GensouWorld.Utils;
using IMAS.CupCake.Extensions;
using IMAS.LocalDBManager.Models;
using IMAS.Tips.Logic.LocalDBManager;
using IMAS.Utils.Files;
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
        /// 改变背景
        /// </summary>
        private TextView tv_change;
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
            tv_leave = FindViewById<TextView>(Resource.Id.tv_leave);
            iv_background = FindViewById<ImageView>(Resource.Id.iv_background);
        }

        public override void D_BindEvent()
        {
            tv_qk.Click += OnClickListener;
            tv_change.Click += OnClickListener;
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
                case Resource.Id.tv_change_background:
                    {
                        //改变庭院背景
                        ShowConfim("自室寫眞選択", "どちらにするの？",
                        (j, k) =>
                        {
                            //从相机选择
                            ToTakePhoto();
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


                        //new AlertView(
                        //    "自室寫眞選択",
                        //    null,
                        //    "キャンセル",
                        //    new string[] { "カメラがら", "寫眞がら" },
                        //    null,
                        //    this,
                        //    AlertView.Style.ActionSheet,
                        //    this
                        //    ).Show();
                        ////new AlertView.Builder().SetContext(this)
                        ////    .SetStyle(AlertView.Style.ActionSheet)
                        ////    .SetTitle("选择操作")
                        ////    .SetMessage(null)
                        ////    .SetCancelText("キャンセル")
                        ////   .SetDestructive("カメラがら", "寫眞がら")
                        ////   .SetOthers(null)
                        ////   .SetOnItemClickListener(this)
                        ////   .Build()
                        ////   .Show();
                    }
                    break;
                case Resource.Id.tv_leave:
                    {//离开
                        this.StartActivityForResult(new Intent(this, typeof(Battle_Map_Select_Activity)), 122);
                        //    BackToTheFutrue();
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

        public void OnItemClick(Object sender, int position)
        {

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
            ShowWaitDiaLog("しばらくお待ちください", false);
            Task.Run(() =>
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
          });

        }
        #endregion

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                var imagePath = "";
                switch (requestCode)
                {
                    case IMAS_Constants.OnAlbumSelectKey:
                        {//选择相册
                            var uri = data.Data.Path;
                            imagePath = "file://" + uri;
                            ImageLoader.Instance.DisplayImage(imagePath, iv_background, ImageLoaderHelper.GeneralImageOption());
                        }
                        break;
                    case IMAS_Constants.OnTakeAPictrueKey:
                        {//拍个照片
                            SaveNewPic();
                            imagePath = PicLastPath;
                            ImageLoader.Instance.DisplayImage(imagePath, iv_background, ImageLoaderHelper.GeneralImageOption());
                        }
                        break;
                }
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public override void Finish()
        {
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


    }

}