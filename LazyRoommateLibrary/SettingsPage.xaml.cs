using LazyRoommate.Models;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LazyRoommate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        public async void DelAccountClicked(object sender, EventArgs e)
        {
            var UserTable = App.client.GetTable<UsersTable>();
            var userItem = await UserTable.Where(x => (x.Email == App.Email)).ToListAsync();           
            var user = userItem.FirstOrDefault();
            
            await UserTable.DeleteAsync(user);

            await DisplayAlert("User Deleted", "User "+App.Email+" has been deleted from app","Ok");

            App.Email = string.Empty;
            App.ProfileImage = string.Empty;
            App.ProfileName = string.Empty;
            await Navigation.PushAsync(new LoginPage(), true);

            //This just came up just for security-reverse engineering reasons i think...
            Navigation.RemovePage(this);
        }
    }
}