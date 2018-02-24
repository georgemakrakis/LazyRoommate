using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Gcm.Client;
using Java.Net;
using LazyRoommate.Managers;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamForms.Controls.Droid;
using Debug = System.Diagnostics.Debug;

namespace LazyRoommate.Droid
{
    [Activity(Label = "LazyRoommate", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity, IAuthenticate
    {
        // Define a authenticated user.
        private MobileServiceUser user;
        static bool success;

        public Context RootView { get; private set; }
        public AccountStore AccountStore { get; private set; }

        public async Task<string> GetSetLazyData()
        {
            var userInfo = await App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

            var table2 = App.client.GetTable<UsersTable>();
            var userItem = await table2.Where(x => (x.Email == userInfo.Email)).ToListAsync();
            var first = userItem.FirstOrDefault();

            if (first != null)
            {

                App.Email = userInfo.Email;
                App.ProfileName = userInfo.Name;
                App.ProfileImage = userInfo.ImageUri;
                App.RoomName = first.RoomName;
                success = true;
                return string.Format("Already signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
            }

            //inserting logged in user into database
            var table = App.client.GetTable<UsersTable>();
            await table.InsertAsync(new UsersTable { id = userInfo.Id, Email = userInfo.Email, Name = userInfo.Name, ImageUri = userInfo.ImageUri });


            App.Email = userInfo.Email;
            App.ProfileName = userInfo.Name;
            App.ProfileImage = userInfo.ImageUri;
            App.RoomName = string.Empty;
            success = true;
            return string.Format("you are now signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
        }

        public async Task<bool> AuthenticateFacebook()
        {
           
            AccountStore = AccountStore.Create();
            var message = string.Empty;
            try
            {
                // Check if the token is available within the password vault
                Account acct;
                try
                {
                    acct = AccountStore.Create().FindAccountsForService("facebook").FirstOrDefault();
                }
                catch (Exception e)
                {
                    acct = null;
                }

                if (acct != null)
                {
                    var token = acct.Properties["Password"];
                    if (!string.IsNullOrEmpty(token) && !IsTokenExpired(token))
                    {
                        App.client.CurrentUser = new MobileServiceUser(acct.Username);
                        App.client.CurrentUser.MobileServiceAuthenticationToken = token;

                        //App.Token = token;
                        //App.AccountUsername = acct.Username;

                        message = await GetSetLazyData();
                        // Display the success or failure message.
                        AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
                        builder1.SetMessage(message);
                        builder1.SetTitle("Sign-in result");
                        builder1.SetPositiveButton("Ok", (senderAlert, args) => { });
                        builder1.Create().Show();

                        return success;
                    }
                }


                // Sign in with Facebook login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(this, MobileServiceAuthenticationProvider.Facebook, "lazyroommateservice.azurewebsites.net");
                if (user != null)
                {
                    message = await GetSetLazyData();
                }

                // Store the token in the password vault               
                Account account = new Account
                {
                    Username = App.client.CurrentUser.UserId
                };
                account.Properties.Add("Password", App.client.CurrentUser.MobileServiceAuthenticationToken);
                AccountStore.Create().Save(account, "facebook");
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


        public async Task<bool> AuthenticateGoogle()
        {
            AccountStore = AccountStore.Create();
            var message = string.Empty;
            try
            {
                // Check if the token is available within the password vault
                Account acct;
                try
                {
                    acct = AccountStore.Create().FindAccountsForService("google").FirstOrDefault();
                }
                catch (Exception e)
                {
                    acct = null;
                }

                if (acct != null)
                {
                    var token = acct.Properties["Password"];
                    if (!string.IsNullOrEmpty(token) && !IsTokenExpired(token))
                    {
                        App.client.CurrentUser = new MobileServiceUser(acct.Username);
                        App.client.CurrentUser.MobileServiceAuthenticationToken = token;

                        //App.Token = token;
                        //App.AccountUsername = acct.Username;

                        message = await GetSetLazyData();
                        // Display the success or failure message.
                        AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
                        builder1.SetMessage(message);
                        builder1.SetTitle("Sign-in result");
                        builder1.SetPositiveButton("Ok", (senderAlert, args) => { });
                        builder1.Create().Show();

                        return success;
                    }
                }


                // Sign in with Google login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(this, MobileServiceAuthenticationProvider.Google, "lazyroommateservice.azurewebsites.net");
                if (user != null)
                {
                    message = await GetSetLazyData();
                }

                // Store the token in the password vault               
                Account account = new Account
                {
                    Username = App.client.CurrentUser.UserId
                };
                account.Properties.Add("Password", App.client.CurrentUser.MobileServiceAuthenticationToken);
                AccountStore.Create().Save(account, "google");
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
            AccountStore = AccountStore.Create();
            var message = string.Empty;
            try
            {
                // Check if the token is available within the password vault
                Account acct;
                try
                {
                    acct = AccountStore.Create().FindAccountsForService("twitter").FirstOrDefault();
                }
                catch (Exception e)
                {
                    acct = null;
                }

                if (acct != null)
                {
                    var token = acct.Properties["Password"];
                    if (!string.IsNullOrEmpty(token) && !IsTokenExpired(token))
                    {
                        App.client.CurrentUser = new MobileServiceUser(acct.Username);
                        App.client.CurrentUser.MobileServiceAuthenticationToken = token;

                        App.Token = token;
                        App.AccountUsername = acct.Username;

                        message = await GetSetLazyData();
                        // Display the success or failure message.
                        AlertDialog.Builder builder1 = new AlertDialog.Builder(this);
                        builder1.SetMessage(message);
                        builder1.SetTitle("Sign-in result");
                        builder1.SetPositiveButton("Ok", (senderAlert, args) => { });
                        builder1.Create().Show();

                        return success;
                    }
                }


                // Sign in with Facebook login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(this, MobileServiceAuthenticationProvider.Twitter, "lazyroommateservice.azurewebsites.net");
                if (user != null)
                {
                    message = await GetSetLazyData();
                }

                // Store the token in the password vault               
                Account account = new Account
                {
                    Username = App.client.CurrentUser.UserId
                };
                account.Properties.Add("Password", App.client.CurrentUser.MobileServiceAuthenticationToken);
                AccountStore.Create().Save(account, "twitter");
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

        public async Task<bool> LogoutAsync()
        {
            Android.Webkit.CookieManager.Instance.RemoveAllCookie();
            await UsersTableManager.DefaultManager.CurrentClient.LogoutAsync();

            //Deleting the stored tokens           
            var account1 = AccountStore.Create().FindAccountsForService("facebook").FirstOrDefault();
            var account2 = AccountStore.Create().FindAccountsForService("google").FirstOrDefault();
            var account3 = AccountStore.Create().FindAccountsForService("twitter").FirstOrDefault();
            if (account1 != null)
            {
                AccountStore.Create().Delete(account1, "facebook");
            }
            else if (account2 != null)
            {
                AccountStore.Create().Delete(account2, "google");
            }
            else if (account3 != null)
            {
                AccountStore.Create().Delete(account3, "twitter");
            }

            return true;
        }

        bool IsTokenExpired(string token)
        {
            // Get just the JWT part of the token (without the signature).
            var jwt = token.Split('.')[1];

            // Undo the URL encoding.
            jwt = jwt.Replace('-', '+').Replace('_', '/');
            switch (jwt.Length % 4)
            {
                case 0: break;
                case 2: jwt += "=="; break;
                case 3: jwt += "="; break;
                default:
                    throw new ArgumentException("The token is not a valid Base64 string.");
            }

            // Convert to a JSON String
            var bytes = Convert.FromBase64String(jwt);
            string jsonString = UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            // Parse as JSON object and get the exp field value,
            // which is the expiration date as a JavaScript primative date.
            JObject jsonObj = JObject.Parse(jsonString);
            var exp = Convert.ToDouble(jsonObj["exp"].ToString());

            // Calculate the expiration by adding the exp value (in seconds) to the
            // base date of 1/1/1970.
            DateTime minTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var expire = minTime.AddSeconds(exp);
            return (expire < DateTime.UtcNow);
        }


        //public override void OnBackPressed()
        //{
        //    Finish();
        //    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        //}



        protected override void OnCreate(Bundle bundle)
        {
            string token = string.Empty;
            string token2 = string.Empty;
            string token3 = string.Empty;            
            try
            {
                token = AccountStore.Create().FindAccountsForService("facebook").FirstOrDefault()?.Properties["Password"];
                token2 = AccountStore.Create().FindAccountsForService("google").FirstOrDefault()?.Properties["Password"];
                token3 = AccountStore.Create().FindAccountsForService("twitter").FirstOrDefault()?.Properties["Password"];
            }
            catch (Exception ex)
            {

            }

            if (!string.IsNullOrEmpty(token) && !IsTokenExpired(token))
            {
                App.client.CurrentUser = new MobileServiceUser(App.AccountUsername);
                App.client.CurrentUser.MobileServiceAuthenticationToken = token;

            }
            else if (!string.IsNullOrEmpty(token2) && !IsTokenExpired(token2))
            {
                App.client.CurrentUser = new MobileServiceUser(App.AccountUsername);
                App.client.CurrentUser.MobileServiceAuthenticationToken = token2;

            }
            else if (!string.IsNullOrEmpty(token3) && !IsTokenExpired(token3))
            {
                App.client.CurrentUser = new MobileServiceUser(App.AccountUsername);
                App.client.CurrentUser.MobileServiceAuthenticationToken = token3;

            }
            else if (string.IsNullOrEmpty(token) && string.IsNullOrEmpty(token2) && string.IsNullOrEmpty(token3))
            {

            }
            // Set the current instance of MainActivity.
            instance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            Calendar.Init();
            UserDialogs.Init(this);
            // Initialize the authenticator before loading the app.
            App.Init(this);
            LoadApplication(new App());

            
            //Push notifications section
            try
            {
                // Check to ensure everything's set up right
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);

                // Register for push notifications
                Debug.WriteLine("Registering...");
                GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);
            }
            catch (MalformedURLException)
            {
                CreateAndShowDialog("There was an error creating the client. Verify the URL.", "Error");
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e.Message, "Error");
            }

        }

        private void CreateAndShowDialog(String message, String title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

        // Create a new instance field for this activity.
        static MainActivity instance;

        // Return the current activity instance.
        public static MainActivity CurrentActivity
        {
            get
            {
                return instance;
            }
        }
    }
}

