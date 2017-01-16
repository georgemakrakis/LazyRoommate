using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using LazyRoommate.Managers;
using LazyRoommate.Models;
using System.Net.Http;

namespace LazyRoommate.Droid
{
    [Activity(Label = "LazyRoommate", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        // Define a authenticated user.
        private MobileServiceUser user;

        public async Task<bool> AuthenticateFacebook()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(this,MobileServiceAuthenticationProvider.Facebook);
                if (user != null)
                {
                    var userInfo = await LazyRoommate.App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);
                    message = string.Format("you are now signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                    success = true;

                    //inserting logged in user into database
                    var table = LazyRoommate.App.client.GetTable<UsersTable>();
                    await table.InsertAsync(new UsersTable { id = userInfo.Id, Email = userInfo.Email, Name = userInfo.Name, ImageUri = userInfo.ImageUri });
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

            return success;
        }

        public async Task<bool> AuthenticateGoogle()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(this, MobileServiceAuthenticationProvider.Google);
                if (user != null)
                {
                    var userInfo = await LazyRoommate.App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);
                    message = string.Format("you are now signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                    success = true;

                    //inserting logged in user into database
                    var table = LazyRoommate.App.client.GetTable<UsersTable>();
                    await table.InsertAsync(new UsersTable { id = userInfo.Id, Email = userInfo.Email, Name = userInfo.Name, ImageUri = userInfo.ImageUri });
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

            return success;
        }

        public async Task<bool> AuthenticateTwitter()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Facebook login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(this, MobileServiceAuthenticationProvider.Twitter);
                if (user != null)
                {
                    var userInfo = await LazyRoommate.App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);
                    message = string.Format("you are now signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                    success = true;

                    //inserting logged in user into database
                    var table = LazyRoommate.App.client.GetTable<UsersTable>();
                    await table.InsertAsync(new UsersTable { id = userInfo.Id, Email = userInfo.Email, Name = userInfo.Name, ImageUri = userInfo.ImageUri });
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.Create().Show();

            return success;
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            // Initialize the authenticator before loading the app.
            App.Init((IAuthenticate)this);
            LoadApplication(new App());
        }
    }
}

