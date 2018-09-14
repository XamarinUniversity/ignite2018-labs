using MyCircle.Services;
using Xamarin.Forms;

namespace MyCircle
{
    public partial class App : Application
    {
        public static IAsyncMessageRepository Repository = new AzureMessageRepository();
            //new InMemoryMessageRepository();

        public App ()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.Accent,
                BarTextColor = Device.RuntimePlatform == Device.iOS ? Color.White : Color.Default
            };
        }

        protected override async void OnStart ()
        {
            string userName = UserInfo.UserName;
            if (string.IsNullOrWhiteSpace(userName))
            {
                // Force a logout
                await LoginService.LogoutAsync();
            }
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }
    }
}
