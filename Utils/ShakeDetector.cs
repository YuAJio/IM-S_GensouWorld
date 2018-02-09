using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace IdoMaster_GensouWorld.Utils
{

    /// <summary>
    /// 检测手机摇晃类
    /// </summary>
    public class ShakeDetector : Java.Lang.Object, ISensorEventListener
    {
        /// <summary>
        /// 检测的时间间隔
        /// </summary>
        private const int UPDATE_INTERVAL = 100;
        /// <summary>
        /// 上次检测的时间
        /// </summary>
        private long lastUpdateTime;

        /// <summary>
        /// 上一次检测是,加速度在x/y/z方向上的分量,用于和当前加速度比较求差
        /// </summary>
        private float lastX, lastY, lastZ;

        private Context mContext;

        private SensorManager sensorManager;

        private List<IOnShakeListener> mListeners;

        public int shakeThreshold = 72;

        public ShakeDetector(Context context)
        {
            mContext = context;
            sensorManager = (SensorManager)Application.Context.GetSystemService(Context.SensorService);
            mListeners = new List<IOnShakeListener>();
        }

        /// <summary>
        /// 启动摇晃检测
        /// </summary>
        public void Start()
        {
            if (sensorManager == null)
            {
                throw new ArgumentNullException();
            }
            var sensor = sensorManager.GetDefaultSensor(SensorType.Accelerometer);
            if (sensor == null)
            {
                throw new ArgumentNullException();
            }
            bool success = sensorManager.RegisterListener(this, sensor, SensorDelay.Ui);
            if (!success)
            {
                throw new Exception(message: "レジスト失敗,シェイクができません");
            }
        }

        /// <summary>
        /// 停止摇晃检测
        /// </summary>
        public void Stop()
        {
            if (sensorManager != null)
            {
                sensorManager.UnregisterListener(this);
            }
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {

        }

        public void OnSensorChanged(SensorEvent e)
        {
            var type = e.Sensor.Type;
            if (type == SensorType.Accelerometer)
            {
                var currentTime = Java.Lang.JavaSystem.CurrentTimeMillis();
                var diffTime = currentTime - lastUpdateTime;
                if (diffTime < UPDATE_INTERVAL)
                {
                    return;
                }
                lastUpdateTime = currentTime;
                var x = e.Values[0];
                var y = e.Values[1];
                var z = e.Values[2];

                var deltaX = x - lastX;
                var deltaY = x - lastY;
                var deltaZ = x - lastZ;

                lastX = x;
                lastY = y;
                lastZ = z;

                var delta = Java.Lang.Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
                Console.WriteLine($"=========================={delta}==========================");
                if (delta > shakeThreshold)
                {
                    this.NotifyListeners();
                }
            }
        }

        /// <summary>
        /// 通知所有注册过的Activity
        /// </summary>
        private void NotifyListeners()
        {
            foreach (var item in mListeners)
            {
                item.OnShake();
            }
        }

        #region 接口相关
        /// <summary>
        /// 注册监听摇晃的接口
        /// </summary>
        /// <param name="listener"></param>
        public void RegisterOnShakeListener(IOnShakeListener listener)
        {
            if (mListeners.Contains(listener))
            {
                return;
            }
            mListeners.Add(listener);
        }
        /// <summary>
        /// 解绑接口
        /// </summary>
        /// <param name="listener"></param>
        public void UnRegisterOnShakeListener(IOnShakeListener listener)
        {
            mListeners.Remove(listener);
        }

        public interface IOnShakeListener
        {
            /// <summary>
            /// 手机摇晃时触发
            /// </summary>
            void OnShake();
        }
        #endregion



    }
}