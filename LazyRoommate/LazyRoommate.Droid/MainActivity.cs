using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using LazyRoommate.Managers;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(this, MobileServiceAuthenticationProvider.Facebook, "lazyroommate.azurewebsites.net");
                if (user != null)
                {
                    var userInfo = await LazyRoommate.App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

                    var table2 = LazyRoommate.App.client.GetTable<UsersTable>();
                    var userItem = await table2.Where(x => (x.Email == userInfo.Email)).ToListAsync();
                    var first = userItem.FirstOrDefault();

                    if (first != null)
                    {
                        message = string.Format("Already signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                        App.Email = userInfo.Email;
                        App.ProfileName = userInfo.Name;
                        App.ProfileImage = userInfo.ImageUri;
                        success = true;
                    }
                    else
                    {
                        //inserting logged in user into database
                        var table = LazyRoommate.App.client.GetTable<UsersTable>();
                        await table.InsertAsync(new UsersTable { id = userInfo.Id, Email = userInfo.Email, Name = userInfo.Name, ImageUri = userInfo.ImageUri });

                        message = string.Format("you are now signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                        App.Email = userInfo.Email;
                        App.ProfileName = userInfo.Name;
                        App.ProfileImage = userInfo.ImageUri;
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                success = false;
            }

            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.SetPositiveButton("Ok", (senderAlert, args) =>{});
            builder.Create().Show();
           
            return success;
        }

        public async Task<bool> AuthenticateGoogle()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Google login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(this, MobileServiceAuthenticationProvider.Google, "lazyroommate.azurewebsites.net");
                if (user != null)
                {
                    var userInfo = await LazyRoommate.App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

                    var table2 = LazyRoommate.App.client.GetTable<UsersTable>();
                    var userItem = await table2.Where(x => (x.Email == userInfo.Email)).ToListAsync();
                    var first = userItem.FirstOrDefault();

                    if (first != null)
                    {
                        message = string.Format("Already signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                        App.Email = userInfo.Email;
                        App.ProfileName = userInfo.Name;
                        App.ProfileImage = userInfo.ImageUri;
                        success = true;
                    }
                    else
                    {
                        //inserting logged in user into database
                        var table = LazyRoommate.App.client.GetTable<UsersTable>();
                        await table.InsertAsync(new UsersTable { id = userInfo.Id, Email = userInfo.Email, Name = userInfo.Name, ImageUri = userInfo.ImageUri });

                        message = string.Format("you are now signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                        App.Email = userInfo.Email;
                        App.ProfileName = userInfo.Name;
                        App.ProfileImage = userInfo.ImageUri;
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                success = false;
            }

            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.SetPositiveButton("Ok", (senderAlert, args) => { });
            builder.Create().Show();

            return success;
        }

        public async Task<bool> AuthenticateTwitter()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Twitter login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(this, MobileServiceAuthenticationProvider.Twitter, "lazyroommate.azurewebsites.net");
                if (user != null)
                {
                    var userInfo = await LazyRoommate.App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

                    var table2 = LazyRoommate.App.client.GetTable<UsersTable>();
                    var userItem = await table2.Where(x => (x.Email == userInfo.Email)).ToListAsync();
                    var first = userItem.FirstOrDefault();

                    if (first != null)
                    {
                        message = string.Format("Already signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                        App.Email = userInfo.Email;
                        App.ProfileName = userInfo.Name;
                        App.ProfileImage = userInfo.ImageUri;
                        success = true;
                    }
                    else
                    {
                        //inserting logged in user into database
                        var table = LazyRoommate.App.client.GetTable<UsersTable>();
                        await table.InsertAsync(new UsersTable { id = userInfo.Id, Email = userInfo.Email, Name = userInfo.Name, ImageUri = userInfo.ImageUri });

                        message = string.Format("you are now signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                        App.Email = userInfo.Email;
                        App.ProfileName = userInfo.Name;
                        App.ProfileImage = userInfo.ImageUri;
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                success = false;
            }

            // Display the success or failure message.
            // Display the success or failure message.
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetMessage(message);
            builder.SetTitle("Sign-in result");
            builder.SetPositiveButton("Ok", (senderAlert, args) => { });
            builder.Create().Show();

            return success;
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            UserDialogs.Init(this);
            // Initialize the authenticator before loading the app.
            App.Init((IAuthenticate)this);
            LoadApplication(new App());
        }
    }
}

