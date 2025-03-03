using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MFASeekerApp.ViewModel;

namespace MFASeekerApp.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(BaseViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
            _ = RefreshToastAsync();
        }
        public async Task RefreshToastAsync()
        {
            string text = "This is a Toast";
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;

            var toast = Toast.Make(text, duration, fontSize);
            while (true)
            {
                await toast.Show();
                await Task.Delay(2000);
            }
        }
    }

}
