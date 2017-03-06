using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace LazyRoommate
{
    public partial class MainPage : MasterDetailPage
    {
        private MobileServiceUser user { get; set; }
       
        public MainPage()
        {
            InitializeComponent();

            //making UWP back button visible
            //App.currentView = SystemNavigationManager.GetForCurrentView();
            //App.currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
 
        }

        private void Button1_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage();
            IsPresented = false;
        }
        private void Button2_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage();
            IsPresented = false;
        }
        private void Button3_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage();
            IsPresented = false;
        }
    }
}
