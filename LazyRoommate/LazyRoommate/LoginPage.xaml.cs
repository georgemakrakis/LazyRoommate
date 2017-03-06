
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
            Image Fb_Image = new Image { Aspect = Aspect.AspectFit };
            Fb_Image.Source = Device.OnPlatform
           (
                iOS:ImageSource.FromFile("Assets/social_connect-facebook.png"),
                Android: ImageSource.FromFile("Assets/social_connect-facebook.png"),
                WinPhone: ImageSource.FromFile("Assets/social_connect-facebook.png")
            );
            //FBButton.Image;

            InitializeComponent();
        }

        public async void LoginClick(object sender, EventArgs e)
        {
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
                //await DisplayAlert("Alert", "Authentication Successful", "OK");
                await Navigation.PushAsync(new MainPage(),true);
                
            }
        }
    }
}
