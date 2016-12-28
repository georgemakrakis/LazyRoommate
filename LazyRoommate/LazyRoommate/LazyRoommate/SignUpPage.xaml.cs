using System;

using Xamarin.Forms;

namespace LazyRoommate
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }
        public async void RegisterClick (object sender,EventArgs e)
        {

            //code for a VERY simple entry in mobile app's DB
            if (passwordEntry1.Text.Equals(passwordEntry2.Text))
            {
                var table = App.client.GetTable<Models.UsersTable>();
                await table.InsertAsync(new Models.UsersTable { UserName = usernameEntry.Text,Password = passwordEntry2.Text });
            }
            else
            {
               await DisplayAlert("Alert","Passwors don't match!","OK");
            }
        }
    }
}
