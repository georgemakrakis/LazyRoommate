using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public async void LoginClick(object sender, EventArgs e)
        {
            
            if (App.Authenticator != null)
            {
                authenticated = await App.Authenticator.Authenticate();
            }
            //for testing
            else
            {
                await DisplayAlert("Alert", "Authenticator X", "OK");
            }
            if (authenticated == true)
            {
                await DisplayAlert("Alert", "Authentication Successful", "OK");
            }
        }

        public async void SignUpClick(object sender, EventArgs e)
        {

            //this method works for android and UWP link : https://developer.xamarin.com/guides/xamarin-forms/user-interface/navigation/modal/
            await this.Navigation.PushModalAsync(new SignUpPage());

        }
    }
}
