namespace NativeAccelerometer
{
    public interface INativeAccelerometer
    {
        string MobileDeviceName { get; }
        string LinearAccelerationSensorName { get; }
        double Length { get; }
        void Start();
        void Stop();
        void Clear();
    }
}
