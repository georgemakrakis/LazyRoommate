using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.UI.Popups;
using LazyRoommate.Managers;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using XamForms.Controls.Windows;

namespace LazyRoommate.UWP
{
    public sealed partial class MainPage : IAuthenticate
    {
        // Define a authenticated user.
        private MobileServiceUser user { get; set; }
        static bool success;
        // Check if the token is available within the password vault
        private PasswordCredential acct;

        public async Task<bool> AuthenticateFacebook()
        {

            var message = string.Empty;
            try
            {
                // Check if the token is available within the password vault
                PasswordCredential acct;
                try
                {
                    acct = new PasswordVault().FindAllByResource("facebook").FirstOrDefault();
                }
                catch (Exception e)
                {
                    acct = null;
                }

                if (acct != null)
                {
                    var token = new PasswordVault().Retrieve("facebook", acct.UserName).Password;
                    if (token != null && token.Length > 0 && !IsTokenExpired(token))
                    {
                        LazyRoommate.App.client.CurrentUser = new MobileServiceUser(acct.UserName);
                        LazyRoommate.App.client.CurrentUser.MobileServiceAuthenticationToken = token;

                        LazyRoommate.App.Token = token;
                        LazyRoommate.App.AccountUsername = acct.UserName;

                        message = await GetSetLazyData();
                        // Display the success or failure message.
                        MessageDialog mg1 = new MessageDialog(message);
                        await mg1.ShowAsync();

                        return success;
                    }
                }


                // Sign in with Facebook login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.Facebook, "lazyroommateservice.azurewebsites.net");
                if (user != null)
                {
                    message = await GetSetLazyData();
                }

                // Store the token in the password vault
                new PasswordVault().Add(new PasswordCredential("facebook",
                    LazyRoommate.App.client.CurrentUser.UserId,
                    LazyRoommate.App.client.CurrentUser.MobileServiceAuthenticationToken));
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

        public async Task<string> GetSetLazyData()
        {
            var userInfo = await LazyRoommate.App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

            var table2 = LazyRoommate.App.client.GetTable<UsersTable>();
            var userItem = await table2.Where(x => (x.Email == userInfo.Email)).ToListAsync();
            var first = userItem.FirstOrDefault();

            if (first != null)
            {

                LazyRoommate.App.Email = userInfo.Email;
                LazyRoommate.App.ProfileName = userInfo.Name;
                LazyRoommate.App.ProfileImage = userInfo.ImageUri;
                LazyRoommate.App.RoomName = first.RoomName;
                success = true;
                return string.Format("Already signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
            }

            //inserting logged in user into database
            var table = LazyRoommate.App.client.GetTable<UsersTable>();
            await table.InsertAsync(new UsersTable { id = userInfo.Id, Email = userInfo.Email, Name = userInfo.Name, ImageUri = userInfo.ImageUri });


            LazyRoommate.App.Email = userInfo.Email;
            LazyRoommate.App.ProfileName = userInfo.Name;
            LazyRoommate.App.ProfileImage = userInfo.ImageUri;
            LazyRoommate.App.RoomName = string.Empty;
            success = true;
            return string.Format("you are now signed-in as {0}. \nEmail {1}. \nId {2}", userInfo.Name, userInfo.Email, userInfo.Id);
        }
        public async Task<bool> AuthenticateGoogle()
        {
            var message = string.Empty;
            try
            {
                // Check if the token is available within the password vault
                PasswordCredential acct;
                try
                {
                    acct = new PasswordVault().FindAllByResource("google").FirstOrDefault();
                }
                catch (Exception e)
                {
                    acct = null;
                }

                if (acct != null)
                {
                    var token = new PasswordVault().Retrieve("google", acct.UserName).Password;
                    if (token != null && token.Length > 0 && !IsTokenExpired(token))
                    {
                        LazyRoommate.App.client.CurrentUser = new MobileServiceUser(acct.UserName);
                        LazyRoommate.App.client.CurrentUser.MobileServiceAuthenticationToken = token;

                        LazyRoommate.App.Token = token;
                        LazyRoommate.App.AccountUsername = acct.UserName;

                        message = await GetSetLazyData();
                        // Display the success or failure message.
                        MessageDialog mg1 = new MessageDialog(message);
                        await mg1.ShowAsync();

                        return success;
                    }
                }


                // Sign in with Facebook login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.Google, "lazyroommateservice.azurewebsites.net");
                if (user != null)
                {
                    message = await GetSetLazyData();
                }

                // Store the token in the password vault
                new PasswordVault().Add(new PasswordCredential("google",
                    LazyRoommate.App.client.CurrentUser.UserId,
                    LazyRoommate.App.client.CurrentUser.MobileServiceAuthenticationToken));
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
            var message = string.Empty;
            try
            {
                // Check if the token is available within the password vault
                PasswordCredential acct;
                try
                {
                    acct = new PasswordVault().FindAllByResource("twitter").FirstOrDefault();
                }
                catch (Exception e)
                {
                    acct = null;
                }

                if (acct != null)
                {
                    var token = new PasswordVault().Retrieve("twitter", acct.UserName).Password;
                    if (token != null && token.Length > 0 && !IsTokenExpired(token))
                    {
                        LazyRoommate.App.client.CurrentUser = new MobileServiceUser(acct.UserName);
                        LazyRoommate.App.client.CurrentUser.MobileServiceAuthenticationToken = token;

                        LazyRoommate.App.Token = token;
                        LazyRoommate.App.AccountUsername = acct.UserName;

                        message = await GetSetLazyData();
                        // Display the success or failure message.
                        MessageDialog mg1 = new MessageDialog(message);
                        await mg1.ShowAsync();

                        return success;
                    }
                }


                // Sign in with Facebook login using a server-managed flow.
                user = await UsersTableManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.Twitter, "lazyroommateservice.azurewebsites.net");
                if (user != null)
                {
                    message = await GetSetLazyData();
                }

                // Store the token in the password vault
                new PasswordVault().Add(new PasswordCredential("twitter",
                    LazyRoommate.App.client.CurrentUser.UserId,
                    LazyRoommate.App.client.CurrentUser.MobileServiceAuthenticationToken));
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

        public async Task<bool> LogoutAsync()
        {
            await UsersTableManager.DefaultManager.CurrentClient.LogoutAsync();

            //Deleting the stored tokens                      
            try
            {
                var account1 = new PasswordVault().Retrieve("facebook", LazyRoommate.App.AccountUsername);
                var vault1 = new PasswordVault();
                vault1.Remove(new PasswordCredential(
                    "facebook", LazyRoommate.App.AccountUsername, account1.Password));               
            }
            catch (Exception e)
            {                
                try
                {
                    var account2 = new PasswordVault().Retrieve("google", LazyRoommate.App.AccountUsername);
                    var vault2 = new PasswordVault();
                    vault2.Remove(new PasswordCredential(
                        "google", LazyRoommate.App.AccountUsername, account2.Password));                   
                }
                catch (Exception exception)
                {
                    try
                    {
                        var account3 = new PasswordVault().Retrieve("twitter", LazyRoommate.App.AccountUsername);
                        var vault3 = new PasswordVault();
                        vault3.Remove(new PasswordCredential(
                            "twitter", LazyRoommate.App.AccountUsername, account3.Password));
                    }
                    catch (Exception e1)
                    {                        
                        throw;
                    }                                                         
                    throw;
                }

                throw;
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

        public MainPage()
        {
            InitializeComponent();
            // Initialize the authenticator before loading the app.
            LazyRoommate.App.Init(this);
            Calendar.Init();
            LoadApplication(new LazyRoommate.App());

            //LazyRoommate.App.client.CurrentUser = new MobileServiceUser();
            //LazyRoommate.App.client.CurrentUser.MobileServiceAuthenticationToken = LazyRoommate.App.Token;
            string token = null;
            string token2 = null;
            string token3 = null;
            try
            {
                token = new PasswordVault().Retrieve("facebook", LazyRoommate.App.AccountUsername).Password;
                token2 = new PasswordVault().Retrieve("google", LazyRoommate.App.AccountUsername).Password;
                token3 = new PasswordVault().Retrieve("twitter", LazyRoommate.App.AccountUsername).Password;
            }
            catch (Exception ex)
            {

            }

            if (!string.IsNullOrEmpty(token) && !IsTokenExpired(token))
            {
                LazyRoommate.App.client.CurrentUser = new MobileServiceUser(LazyRoommate.App.AccountUsername);
                LazyRoommate.App.client.CurrentUser.MobileServiceAuthenticationToken = token;

            }
            else if (!string.IsNullOrEmpty(token2) && !IsTokenExpired(token2))
            {
                LazyRoommate.App.client.CurrentUser = new MobileServiceUser(LazyRoommate.App.AccountUsername);
                LazyRoommate.App.client.CurrentUser.MobileServiceAuthenticationToken = token2;

            }
            else if (!string.IsNullOrEmpty(token3) && !IsTokenExpired(token3))
            {
                LazyRoommate.App.client.CurrentUser = new MobileServiceUser(LazyRoommate.App.AccountUsername);
                LazyRoommate.App.client.CurrentUser.MobileServiceAuthenticationToken = token3;

            }


        }
    }
}
