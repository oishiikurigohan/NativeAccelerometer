using System;
using Xamarin.Forms;

namespace NativeAccelerometer
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            INativeAccelerometer accelerometer = DependencyService.Get<INativeAccelerometer>();
            this.LabelDeviceName.Text = accelerometer.MobileDeviceName;
            this.LabelSensorName.Text = accelerometer.LinearAccelerationSensorName;

            this.ButtonStart.Clicked += (sender, e) => {
                accelerometer.Clear();
                accelerometer.Start();
            };

            this.ButtonStop.Clicked += (sender, e) => {
                accelerometer.Stop();
                this.LabelLength.Text = Math.Round(accelerometer.Length, 2, MidpointRounding.AwayFromZero) + "m";
            };

            this.ButtonClear.Clicked += (sender, e) => {
                accelerometer.Clear();
                this.LabelLength.Text = "0.00m";
            };
        }
    }
}
