using LazyRoommate.Managers;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Popups;

namespace LazyRoommate.UWP
{
    public sealed partial class MainPage : IAuthenticate
    {
        // Define a authenticated user.
        private MobileServiceUser user { get; set; }

        public async Task<bool> AuthenticateFacebook()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.Facebook);
                if (user != null)
                {
                    var userInfo = await LazyRoommate.App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);
                    message = string.Format("you are now signed-in as {0}.", userInfo.Name);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            MessageDialog mg = new MessageDialog(message);
            await mg.ShowAsync();

            return success;
        }
        public async Task<bool> AuthenticateGoogle()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Google login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.Google);
                if (user != null)
                {
                    var userInfo = await LazyRoommate.App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);
                    message = string.Format("you are now signed-in as {0}.", userInfo.Name);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            MessageDialog mg = new MessageDialog(message);
            await mg.ShowAsync();

            return success;
        }
        public async Task<bool> AuthenticateTwitter()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Twitter login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.Twitter);
                if (user != null)
                {
                    var userInfo = await LazyRoommate.App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);
                    message = string.Format("you are now signed-in as {0}.", userInfo.Name);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            MessageDialog mg = new MessageDialog(message);
            await mg.ShowAsync();

            return success;
        }
        public MainPage()
        {
            this.InitializeComponent();
            // Initialize the authenticator before loading the app.
            LazyRoommate.App.Init(this);
            LoadApplication(new LazyRoommate.App());
            
        }
    }
}
