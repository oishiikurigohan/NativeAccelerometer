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
            this.ButtonStart.Clicked += (sender, e) => { accelerometer.Start(); };
            this.ButtonStop.Clicked += (sender, e) => { accelerometer.Stop(); };
            accelerometer.AccelerationReceived += (sender, e) =>
            {
                this.LabelX.Text = e.X.ToString();
                this.LabelY.Text = e.Y.ToString();
                this.LabelZ.Text = e.Z.ToString();
                this.LabelInterval.Text = e.Interval.ToString();
            };
        }
    }
}
