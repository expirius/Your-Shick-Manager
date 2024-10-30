using MFASeekerApp.ViewModel;

namespace MFASeekerApp.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(UserSession vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }

    }

}
