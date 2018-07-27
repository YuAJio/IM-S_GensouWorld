using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IdoMaster_GensouWorld.Utils;
using Com.Nostra13.Universalimageloader.Core;
using IMAS.LocalDBManager.Models;
using Java.Lang;
using CustomControl;
using IdoMaster_GensouWorld.Activitys.ProductionPage;

namespace IdoMaster_GensouWorld.Activitys.Opening
{
    /// <summary>
    /// 序章
    /// </summary>
    [Activity(Label = "Prologue_Activity", Theme = "@style/Theme.PublicThemePlus")]
    public class Prologue_Activity : BaseActivity
    {
        #region UI控件
        private ImageView iv_bg;
        private ImageView iv_character;
        private TextView tv_name;
        private GalTextView tv_talk_Content;
        #endregion

        #region 聊天内容
        /// <summary>
        /// 聊天内容
        /// </summary>
        private List<Model_ChatContent> list_Content = new List<Model_ChatContent>();
        private int Chat_Position;
        private long Chat_Last_Id = -1;
        private const long Chat_Speed = 2000;
        #endregion

        #region Handler
        private MyHandler mHandler;
        private class MyHandler : Handler
        {
            public Action<Message> handlerAction;
            public override void HandleMessage(Message msg)
            {
                handlerAction?.Invoke(msg);
            }
        }
        private void RunHandlerAction(Message msg)
        {
            switch (msg.What)
            {
                case IMAS_Constants.EVENT_CHAT_GO_ON://消息往下走
                    if (Chat_Position >= list_Content.Count)
                    {//达到了消息的最大值
                        mHandler.RemoveCallbacks(runable_Alpha);
                        StartActivity(new Intent(this, typeof(ProductionHomeActivity)));
                        this.Finish();
                        return;
                    }
                    var chat = list_Content[Chat_Position];
                    Chat_Position++;
                    tv_name.Text = chat.From_Name;
                    tv_talk_Content.SetTextContent(chat.Content);
                    switch (chat.EventFlag)
                    {
                        case 0x001:
                            ImageLoader.Instance.DisplayImage(IMAS_Constants.BackGround_Pic_Building_Out, iv_bg, ImageLoaderHelper.BackGroundImageOption());
                            break;
                        case 0x002:
                            iv_character.SetImageResource(0);
                            Chat_Last_Id = chat.From_Id;
                            break;
                    }
                    if (Chat_Last_Id != chat.From_Id)
                    {
                        switch (chat.From_Id)
                        {
                            case IMAS_Constants.AMAMI_HARUKA_ID:
                                iv_character.SetImageResource(Resource.Mipmap.daily_haruka_one);
                                break;
                            case IMAS_Constants.KISARAGI_CHIHAYA_ID:
                                iv_character.SetImageResource(Resource.Mipmap.daily_chihaya_one);
                                break;
                            case IMAS_Constants.Producer_Name_ID:
                                iv_character.SetImageResource(0);
                                break;
                        }
                    }
                    Chat_Last_Id = chat.From_Id;
                    //  mHandler.PostDelayed(runable_Alpha, Chat_Speed);
                    break;
            }
        }
        private MyRunable runable_Alpha;
        /// <summary>
        /// 子线程Runnable
        /// </summary>
        public class MyRunable : Java.Lang.Object, IRunnable
        {
            private Handler mHandwel;
            private int mWhat;

            public MyRunable(Handler mHandwel, int what)
            {
                this.mHandwel = mHandwel;
                this.mWhat = what;
            }
            public void Run()
            {
                Message msg = new Message();
                msg.What = mWhat;
                mHandwel.SendMessage(msg);
            }
        }
        #endregion

        public override int A_GetContentViewId()
        {
            return Resource.Layout.activity_prologue_main;
        }

        public override void B_BeforeInitView()
        {
            #region 初始化对话内容
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.Producer_Name_ID, From_Name = $"{IMAS_Constants.Producer_Name} P", Content = "今天是新任制作人的日子啊,好紧张啊...." });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.Producer_Name_ID, From_Name = $"{IMAS_Constants.Producer_Name} P", Content = "诶!不管了,先到公司再说吧!" });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.AMAMI_HARUKA_ID, From_Name = $"???", Content = "あの……プロデューサー…さん?", EventFlag = 0x002 });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.Producer_Name_ID, From_Name = $"{IMAS_Constants.Producer_Name} P", Content = "ん?かわいいの声だな(振り返る)" });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.AMAMI_HARUKA_ID, From_Name = $"{IMAS_Constants.AMAMI_HARUKA}", Content = "あ！やはり！" });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.AMAMI_HARUKA_ID, From_Name = $"{IMAS_Constants.AMAMI_HARUKA}", Content = "こんにちは(^o^)/！プロデューサーさん！" });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.Producer_Name_ID, From_Name = $"{IMAS_Constants.Producer_Name} P", Content = "pu..puro??额...こんにちは..?" });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.AMAMI_HARUKA_ID, From_Name = $"{IMAS_Constants.AMAMI_HARUKA}", Content = "さぁ！入っろ！" });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.Producer_Name_ID, From_Name = $"{IMAS_Constants.Producer_Name} P", Content = "あ…ちょと…" });
            list_Content.Add(new Model_ChatContent() { From_Id = 0, From_Name = $"", Content = "ゴゴゴゴゴゴゴゴゴゴゴゴ", EventFlag = 0x001 });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.KISARAGI_CHIHAYA_ID, From_Name = $"???", Content = "ん？春香？この方わ？" });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.Producer_Name_ID, From_Name = $"{IMAS_Constants.Producer_Name} P", Content = "あ、どーも、新しのプロデューサーです" });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.KISARAGI_CHIHAYA_ID, From_Name = $"???", Content = "新しの…プロデューサーさん？？" });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.AMAMI_HARUKA_ID, From_Name = $"{IMAS_Constants.AMAMI_HARUKA}", Content = "そだよう千早ちゃん！昨日小鳥さんが言いたちゃない" });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.KISARAGI_CHIHAYA_ID, From_Name = $"{IMAS_Constants.KISARAGI_CHIHAYA}", Content = "は…はじめはし、プロデューサーさん、私の名前わ如月千早です。", });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.Producer_Name_ID, From_Name = $"{IMAS_Constants.Producer_Name} P", Content = "よろしくね、千早～" });
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.AMAMI_HARUKA_ID, From_Name = $"{IMAS_Constants.AMAMI_HARUKA}", Content = "でわさ即、はじめようか～" });
#if DEBUG
            list_Content.Clear();
            list_Content.Add(new Model_ChatContent() { From_Id = IMAS_Constants.Producer_Name_ID, From_Name = $"{IMAS_Constants.Producer_Name} P", Content = "DEBUG模式...." });
#endif
            #endregion
            mHandler = new MyHandler();
            runable_Alpha = new MyRunable(mHandler, IMAS_Constants.EVENT_CHAT_GO_ON);
            mHandler.handlerAction -= RunHandlerAction;
            mHandler.handlerAction += RunHandlerAction;

        }

        public override void C_InitView()
        {
            iv_bg = FindViewById<ImageView>(Resource.Id.iv_bg);
            iv_character = FindViewById<ImageView>(Resource.Id.iv_character);
            tv_name = FindViewById<TextView>(Resource.Id.tv_name);
            tv_talk_Content = FindViewById<GalTextView>(Resource.Id.tv_content);
        }

        public override void D_BindEvent()
        {

        }

        public override void E_InitData()
        {
            tv_talk_Content.SetDelayPlayTime(200);
            ImageLoader.Instance.DisplayImage(IMAS_Constants.BackGround_Pic_Street, iv_bg, ImageLoaderHelper.BackGroundImageOption());
            mHandler.PostDelayed(runable_Alpha, 0);

        }

        public override void F_OnClickListener(View v, EventArgs e)
        {

        }

        public override void G_OnAdapterItemClickListener(View v, AdapterView.ItemClickEventArgs e)
        {

        }

        #region 不让你点返回键
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back || keyCode == Keycode.Home || keyCode == Keycode.Menu)
            {
                return false;
            }
            return base.OnKeyDown(keyCode, e);
        }
        #endregion
        #region 监听屏幕点击
        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                if (!tv_talk_Content.IsSendFinish())
                {
                    tv_talk_Content.SetAllDraw();
                }
                else
                {
                    mHandler.PostDelayed(runable_Alpha, 0);
                }

            }
            return base.OnTouchEvent(e);
        }
        #endregion
    }
}