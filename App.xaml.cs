using QuestPDF.Infrastructure;

namespace bislerium
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            QuestPDF.Settings.License = LicenseType.Community;
        }
    }
}
