using MFASeekerApp.ViewModel;

namespace MFASeekerApp.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(BaseViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }

    }

}
