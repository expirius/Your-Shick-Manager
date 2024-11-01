namespace MFASeekerApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            // Тема по умолчанию DARK
            UserAppTheme = AppTheme.Dark;
            MainPage = new AppShell();
        }
    }
}
