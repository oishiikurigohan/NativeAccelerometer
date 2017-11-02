using Android.Hardware;
using Android.Content;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeAccelerometer.Droid.AndroidAccelerometer))]

namespace NativeAccelerometer.Droid
{
    public class AndroidAccelerometer : Java.Lang.Object, INativeAccelerometer, ISensorEventListener
    {
        private static readonly object syncLock = new object();
        private SensorManager sensorManager;
        private double prevTimeStamp;

        public string MobileDeviceName { get; private set; }
        public event AccelerometerEventHandler AccelerationReceived;

        public AndroidAccelerometer()
        {
            sensorManager = (SensorManager)Forms.Context.GetSystemService(Context.SensorService);
            MobileDeviceName = Android.OS.Build.Manufacturer + " " + Android.OS.Build.Model;
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
            sensorManager.UnregisterListener(this);
            prevTimeStamp = 0;
        }

        // センサの値を取得したとき
        public void OnSensorChanged(SensorEvent e)
        {
            // センサの取得間隔を取得(sec)
            double nowTimeStamp = e.Timestamp;
            double interval = (prevTimeStamp != 0) ? (nowTimeStamp - prevTimeStamp) / 1000000000 : 0;
            prevTimeStamp = nowTimeStamp;

            // センサの値をセットしイベントを投げる
            AccelerationReceived(this, new AccelerometerEventArgs {
                X = e.Values[0],
                Y = e.Values[1],
                Z = e.Values[2],
                Interval = interval });
        }

        // センサの値を取得する頻度が変わったとき
        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy) { }
    }
}