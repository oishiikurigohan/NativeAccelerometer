using System;

namespace NativeAccelerometer
{
    // 加速度センサの値が更新されたときのイベントのパラメータ
    public class AccelerometerEventArgs : EventArgs
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double Interval { get; set; }
    }

    // 加速度センサの値が更新されたときのイベントハンドラ
    public delegate void AccelerometerEventHandler(object sender, AccelerometerEventArgs args);

    public interface INativeAccelerometer
    {
        string MobileDeviceName { get; }
        event AccelerometerEventHandler AccelerationReceived;
        void Start();
        void Stop();
    }
}
