using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Auth;

namespace LazyRoommate.Helpers
{
    public class AuthStore 
    {
        private static string TokenKeyName = "token";

        public static void CacheAuthToken(MobileServiceUser user)
        {
            var account = new Account(user.UserId);
            account.Properties.Add(TokenKeyName, user.MobileServiceAuthenticationToken);
            AccountStore.Create().Save(account, App.AppName);

            Debug.WriteLine($"Cached auth token: {user.MobileServiceAuthenticationToken}");
        }

        public static MobileServiceUser GetUserFromCache()
        {
            var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();

            if (account == null)
            {
                return null;
            }

            var token = account.Properties[TokenKeyName];
            Debug.WriteLine($"Retrieved token from account store: {token}");

            return new MobileServiceUser(account.Username)
            {
                MobileServiceAuthenticationToken = token
            };
        }

        public static void DeleteTokenCache()
        {            
            var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create().Delete(account, App.AppName);
            }
        }        
    }
}
