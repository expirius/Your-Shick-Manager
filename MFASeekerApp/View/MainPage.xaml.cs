using MFASeeker.ViewModel;

namespace MFASeeker.View
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();

            BindingContext = vm;
        }

    }

}
