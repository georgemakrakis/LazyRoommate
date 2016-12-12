using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Xamarin.Forms;

namespace LazyRoommate
{
    public partial class LoginPage : ContentPage
    {
        // Track whether the user has authenticated.
        private bool authenticated = false;

        public LoginPage()
        {
            InitializeComponent();

            //making UWP back button non-visible
            App.currentView = SystemNavigationManager.GetForCurrentView();
            App.currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        public async void LoginClick(object sender, EventArgs e)
        {

            //var button = GoogleButton;
            if (App.Authenticator != null)
            {
                if(sender.Equals(FBButton))
                {
                    authenticated = await App.Authenticator.AuthenticateFacebook();
                }
                else if(sender.Equals(GoogleButton))
                {
                    authenticated = await App.Authenticator.AuthenticateGoogle();
                }
                else if (sender.Equals(TwitterButton))
                {
                    authenticated = await App.Authenticator.AuthenticateTwitter();
                }
            }
            //for testing
            else
            {
                await DisplayAlert("Alert", "Authenticator X", "OK");
            }
            if (authenticated == true)
            {
                await DisplayAlert("Alert", "Authentication Successful", "OK");
                await Navigation.PushModalAsync(new MainPage());
            }
        }
    }
}
