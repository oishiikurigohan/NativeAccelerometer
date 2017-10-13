using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(NativeAccelerometer.iOS.iOSAccelerometer))]

namespace NativeAccelerometer.iOS
{
    public class iOSAccelerometer : INativeAccelerometer
    {
        public string LinearAccelerationSensorName { get; private set; }
        public string MobileDeviceName { get; private set; }
        public double Length { get; private set; }

        public iOSAccelerometer()
        {
            MobileDeviceName = UIDevice.CurrentDevice.SystemName + " " + UIDevice.CurrentDevice.SystemVersion + " " + UIDevice.CurrentDevice.Model;
            LinearAccelerationSensorName = "";
            Length = 0;
        }

        public void Start() { }
        public void Stop() { }
        public void Clear() { }
    }
}