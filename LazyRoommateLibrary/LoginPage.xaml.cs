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
            SizeChanged += ChangeLogo;
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, false);        
        }

        void ChangeLogo(object sender, EventArgs e)
        {
            logo.WidthRequest = Math.Min(this.Width, 400);
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
                //await Navigation.PushAsync(new MainPage(), true);

                //Application.Current.MainPage = new MasterDetailPage();
                Application.Current.MainPage = new NavigationPage(new MasterDetailPage1 { Master = new MasterDetailPage1Master(), Detail = new MasterDetailPage1Detail()});
                //Navigation.InsertPageBefore(new MainPage(),this);
                //await Navigation.PopToRootAsync();


            }
        }

      



    }


}
