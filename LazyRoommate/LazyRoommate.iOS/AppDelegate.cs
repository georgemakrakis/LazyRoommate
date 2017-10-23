using Foundation;
using LazyRoommate.Managers;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UIKit;

namespace LazyRoommate.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IAuthenticate
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
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController, MobileServiceAuthenticationProvider.Facebook, "lazyroommateservice.azurewebsites.net");
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
            UIAlertView avAlert = new UIAlertView("Sign-in result", message, null, "OK", null);
            avAlert.Show();
            return success;
        }
        public async Task<bool> AuthenticateGoogle()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Google login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController, MobileServiceAuthenticationProvider.Google, "lazyroommateservice.azurewebsites.net");
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
            // Display the success or failure message.
            UIAlertView avAlert = new UIAlertView("Sign-in result", message, null, "OK", null);
            avAlert.Show();

            return success;
        }
        public async Task<bool> AuthenticateTwitter()
        {
            var success = false;
            var message = string.Empty;
            try
            {
                // Sign in with Twitter login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController, MobileServiceAuthenticationProvider.Twitter, "lazyroommateservice.azurewebsites.net");
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
            UIAlertView avAlert = new UIAlertView("Sign-in result", message, null, "OK", null);
            avAlert.Show();

            return success;
        }
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return UsersTableManager.DefaultManager.CurrentClient.ResumeWithURL(url);
        }
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            XamForms.Controls.iOS.Calendar.Init();
            LoadApplication(new App());
            App.Init(this);
            return base.FinishedLaunching(app, options);
        }
    }
}
