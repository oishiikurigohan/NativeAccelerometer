using CoreMotion;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NativeAccelerometer.iOS.iOSAccelerometer))]

namespace NativeAccelerometer.iOS
{
    public class iOSAccelerometer : INativeAccelerometer
    {
        private CMMotionManager motionManager;
        private double prevTimeStamp;

        public string MobileDeviceName { get; private set; }
        public event AccelerometerEventHandler AccelerationReceived;

        public iOSAccelerometer()
        {
            motionManager = new CMMotionManager();
            MobileDeviceName = UIDevice.CurrentDevice.SystemName + " " + UIDevice.CurrentDevice.SystemVersion + " " + UIDevice.CurrentDevice.Model;
        }

        public void Start()
        {
            if (motionManager.AccelerometerAvailable)
            {
                motionManager.AccelerometerUpdateInterval = 0.005;
                motionManager.StartAccelerometerUpdates( NSOperationQueue.CurrentQueue, (data, error) =>
                {
                    // センサの取得間隔を取得(sec)
                    // CMAccelerometerData.Timestamp : デバイスが起動してからの秒単位の時間
                    double nowTimeStamp = data.Timestamp;
                    double interval = (prevTimeStamp != 0) ? (nowTimeStamp - prevTimeStamp) : 0;
                    prevTimeStamp = nowTimeStamp;

                    AccelerationReceived(this, new AccelerometerEventArgs {
                        X = data.Acceleration.X,
                        Y = data.Acceleration.Y,
                        Z = data.Acceleration.Z,
                        Interval = interval });
                });
            }
        }

        public void Stop()
        {
            motionManager.StopAccelerometerUpdates();
            prevTimeStamp = 0;
        }
    }
}