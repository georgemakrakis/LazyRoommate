using LazyRoommate.Managers;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.Facebook, "lazyroommate.azurewebsites.net");
                if (user != null)
                {
                    var userInfo = await LazyRoommate.App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

                    var table2 = LazyRoommate.App.client.GetTable<UsersTable>();
                    var userItem = await table2.Where(x => (x.Email == userInfo.Email)).ToListAsync();
                    var first = userItem.FirstOrDefault();

                    if (first != null)
                    {
                        message = string.Format("Already signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                        LazyRoommate.App.Email = userInfo.Email;
                        LazyRoommate.App.ProfileName = userInfo.Name;
                        LazyRoommate.App.ProfileImage = userInfo.ImageUri;
                        success = true;
                    }
                    else
                    {
                        //inserting logged in user into database
                        var table = LazyRoommate.App.client.GetTable<UsersTable>();
                        await table.InsertAsync(new UsersTable { id = userInfo.Id, Email = userInfo.Email, Name = userInfo.Name, ImageUri = userInfo.ImageUri });

                        message = string.Format("you are now signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                        LazyRoommate.App.Email = userInfo.Email;
                        LazyRoommate.App.ProfileName = userInfo.Name;
                        LazyRoommate.App.ProfileImage = userInfo.ImageUri;
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
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.Google, "lazyroommate.azurewebsites.net");
                if (user != null)
                {
                    var userInfo = await LazyRoommate.App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

                    var table2 = LazyRoommate.App.client.GetTable<UsersTable>();
                    var userItem = await table2.Where(x => (x.Email == userInfo.Email)).ToListAsync();
                    var first = userItem.FirstOrDefault();

                    if (first != null)
                    {
                        message = string.Format("Already signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                        LazyRoommate.App.Email = userInfo.Email;
                        LazyRoommate.App.ProfileName = userInfo.Name;
                        LazyRoommate.App.ProfileImage = userInfo.ImageUri;
                        success = true;
                    }
                    else
                    {
                        //inserting logged in user into database
                        var table = LazyRoommate.App.client.GetTable<UsersTable>();
                        await table.InsertAsync(new UsersTable { id = userInfo.Id, Email = userInfo.Email, Name = userInfo.Name, ImageUri = userInfo.ImageUri });

                        message = string.Format("you are now signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                        LazyRoommate.App.Email = userInfo.Email;
                        LazyRoommate.App.ProfileName = userInfo.Name;
                        LazyRoommate.App.ProfileImage = userInfo.ImageUri;
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
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.Twitter, "lazyroommate.azurewebsites.net");
                if (user != null)
                {
                    var userInfo = await LazyRoommate.App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

                    var table2 = LazyRoommate.App.client.GetTable<UsersTable>();
                    var userItem = await table2.Where(x => (x.Email == userInfo.Email)).ToListAsync();
                    var first = userItem.FirstOrDefault();

                    if (first != null)
                    {
                        message = string.Format("Already signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                        LazyRoommate.App.Email = userInfo.Email;
                        LazyRoommate.App.ProfileName = userInfo.Name;
                        LazyRoommate.App.ProfileImage = userInfo.ImageUri;
                        success = true;
                    }
                    else
                    {
                        //inserting logged in user into database
                        var table = LazyRoommate.App.client.GetTable<UsersTable>();
                        await table.InsertAsync(new UsersTable { id = userInfo.Id, Email = userInfo.Email, Name = userInfo.Name, ImageUri = userInfo.ImageUri });

                        message = string.Format("you are now signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
                        LazyRoommate.App.Email = userInfo.Email;
                        LazyRoommate.App.ProfileName = userInfo.Name;
                        LazyRoommate.App.ProfileImage = userInfo.ImageUri;
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
