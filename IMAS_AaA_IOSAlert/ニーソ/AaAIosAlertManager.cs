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
//using Com.Bigkoo.Alertview;
//using Java.Lang;
//using Android.Views.InputMethods;

//namespace IMAS_AaA_IOSAlert.ニーソ
//{
//    public class AaAIosAlertManager : IOnItemClickListener, IOnDismissListener
//    {
//        private EditText et_Name;
//        private AlertView aaA_alertView;

//        public Action<Java.Lang.Object, int> ItemClick;

//        private Context context;
//        private static AaAIosAlertManager _Instance;

//        public static AaAIosAlertManager GetInstance()
//        {
//            if (_Instance != null)
//            {
//                _Instance = new AaAIosAlertManager();
//            }
//            return _Instance;
//        }
//        public AaAIosAlertManager() { }
//        public AaAIosAlertManager(Context context)
//        {
//            this.context = context;
//        }

//        public void AllThingsGone()
//        {
//            aaA_alertView?.Dismiss();
//        }

//        public void ShowConfirm()
//        {
//            aaA_alertView = new AlertView("タイトル", "貴様！見でいるな！", "キャンセル", new string[] { "ナニコレ" }, null, context, AlertView.Style.Alert, this);
//            aaA_alertView.Show();
//        }

//        #region  单项点击事件
//        public void OnItemClick(Java.Lang.Object p0, int p1)
//        {
//            if (p1 == 0)
//            {
//                aaA_alertView?.Dismiss();
//            }
//            else
//            {
//                ItemClick.Invoke(p0, p1);
//            }
//        }
//        #endregion

//        #region 提示框消失事件
//        public IntPtr Handle
//        {
//            get
//            {
//                return new IntPtr(0);
//            }
//        }

//        public void Dispose()
//        {

//        }

//        public void OnDismiss(Java.Lang.Object p0)
//        {

//        }
//        #endregion

//    }
//}