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

        public static string Email
        {
            get { return AppSettings.GetValueOrDefault(nameof(Email), string.Empty); }
            set { AppSettings.AddOrUpdateValue(nameof(Email), value); }
        }
        public static string ProfileName
        {
            get { return AppSettings.GetValueOrDefault(nameof(ProfileName), string.Empty); }
            set { AppSettings.AddOrUpdateValue(nameof(ProfileName), value); }
        }
        public static string ProfileImage
        {
            get { return AppSettings.GetValueOrDefault(nameof(ProfileImage), string.Empty); }
            set { AppSettings.AddOrUpdateValue(nameof(ProfileImage), value); }
        }
        public static string RoomName
        {
            get { return AppSettings.GetValueOrDefault(nameof(RoomName), string.Empty); }
            set { AppSettings.AddOrUpdateValue(nameof(RoomName), value); }
        }

        public static void Init(IAuthenticate authenticator)
        {           
            Authenticator = authenticator;
        }

        public App()
        {
            // The root page of your application
            InitializeComponent();
            if (App.Email != string.Empty)
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {               
                MainPage = new NavigationPage(new LoginPage());               
            }
          
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
