using System.Collections.Generic;
using System;
using Android.Hardware;
using Android.Content;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(NativeAccelerometer.Droid.AndroidAccelerometer))]

namespace NativeAccelerometer.Droid
{
    public class AndroidAccelerometer : Java.Lang.Object, INativeAccelerometer, ISensorEventListener
    {
        private static readonly object syncLock = new object();
        private SensorManager sensorManager;
        private double prevTimeStamp;
        private double prevVelocity;
        private double x, y, z;

        public string LinearAccelerationSensorName { get; private set; }
        public string MobileDeviceName { get; private set; }
        public double Length { get; private set; }

        public AndroidAccelerometer()
        {
            sensorManager = (SensorManager)Forms.Context.GetSystemService(Context.SensorService);
            MobileDeviceName = Android.OS.Build.Manufacturer + " " + Android.OS.Build.Model;
            IList<Sensor> sensors = sensorManager.GetSensorList(SensorType.LinearAcceleration);
            foreach (var sensor in sensors) LinearAccelerationSensorName += ( sensor.Name + " : " + sensor.Vendor + " ");
        }

        // 計測開始
        public void Start()
        {
            sensorManager.RegisterListener(this, sensorManager.GetDefaultSensor(SensorType.LinearAcceleration), SensorDelay.Fastest);
        }

        // 計測終了
        public void Stop()
        {
            // 必要のない時はセンサーを無効にしないとバッテリーが消耗するので注意。
            // 画面が消灯してもセンサーは自動的に無効になりません。
            sensorManager.UnregisterListener(this);
        }

        // クリア
        public void Clear()
        {
            Stop();
            prevTimeStamp = 0;
            prevVelocity = 0;
            Length = 0;
            x = 0;
            y = 0;
            z = 0;
        }

        // センサの値を取得したとき
        public void OnSensorChanged(SensorEvent e)
        {
            lock (syncLock)
            {
                double nowTimeStamp = e.Timestamp;
                double time = 0;

                // センサの取得間隔(sec)
                if (prevTimeStamp != 0) time = (nowTimeStamp - prevTimeStamp) / 1000000000;
                prevTimeStamp = nowTimeStamp;

                // 検出値が閾値以下の場合は切り捨て
                const double threshold = 0.02;
                double sensorValueX = ( Math.Abs(e.Values[0]) > threshold ) ? e.Values[0] : 0;
                double sensorValueY = ( Math.Abs(e.Values[1]) > threshold ) ? e.Values[1] : 0;
                double sensorValueZ = ( Math.Abs(e.Values[2]) > threshold ) ? e.Values[2] : 0;

                // ローパスフィルタでノイズ成分を除去
                const double alpha = 0.8;
                x = x * alpha + sensorValueX * (1 - alpha);
                y = y * alpha + sensorValueY * (1 - alpha);
                z = z * alpha + sensorValueZ * (1 - alpha);

                // 3軸の加速度を合成 |a|=√{(x^2)+(y^2)+(z^2)}
                double acceleration = Math.Sqrt(x * x + y * y + z * z);

                // 合成加速度とセンサの取得間隔から速度を算出 v=v0+at
                double velocity = prevVelocity + acceleration * time;
                prevVelocity = velocity;

                // 速度から距離を算出 d=vt
                double distance = velocity * time;

                // 移動距離の積算
                Length += distance;

                //System.Diagnostics.Debug.WriteLine(e.Values[0] + "," + e.Values[1] + "," + e.Values[2] + "," + x + "," + y + "," + z + "," + acceleration + "," + velocity);
            }
        }

        // センサの値を取得する頻度が変わったとき
        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
        }
    }
}