using System;
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

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, false);

        }    
       
        public async void LoginClick(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
            {
                if (sender.Equals(FBButton))
                {
                    authenticated = await App.Authenticator.AuthenticateFacebook();                    
                }
                else if (sender.Equals(GoogleButton))
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
            if (authenticated)
            {
                //await DisplayAlert("Alert", "Authentication Successful", "OK");
                await Navigation.PushAsync(new MainPage());


            }
        }



    }


}
