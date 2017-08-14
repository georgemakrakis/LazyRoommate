using Microsoft.WindowsAzure.MobileServices;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace LazyRoommate
{
    //implementing an authentication interface
    public interface IAuthenticate
    {
        Task<bool> AuthenticateFacebook();
        Task<bool> AuthenticateGoogle();
        Task<bool> AuthenticateTwitter();
    }
    public partial class App : Application
    {
        //initializing the interface with a platform-specific implementation
        public static IAuthenticate Authenticator { get; private set; }     
        //public static SystemNavigationManager currentView=null;
        private static ISettings AppSettings =>
            CrossSettings.Current;

        //UserName will be the email for us
        public static string UserName
        {
            get { return AppSettings.GetValueOrDefault(nameof(UserName), string.Empty); }
            set { AppSettings.AddOrUpdateValue(nameof(UserName), value); }
        }

        public static void Init(IAuthenticate authenticator)
        {           
            Authenticator = authenticator;
        }

        public App()
        {
            // The root page of your application
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage());
        }

        public static MobileServiceClient client = new MobileServiceClient("https://lazyroommate.azurewebsites.net");

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
