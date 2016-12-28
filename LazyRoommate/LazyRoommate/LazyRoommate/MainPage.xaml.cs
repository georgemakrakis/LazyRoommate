using Xamarin.Forms;

namespace LazyRoommate
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            //making UWP back button visible
            //App.currentView = SystemNavigationManager.GetForCurrentView();
            //App.currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
        }
    }
}
