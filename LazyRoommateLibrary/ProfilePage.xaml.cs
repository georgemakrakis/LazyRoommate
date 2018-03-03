using System;
using System.Diagnostics;
using System.Linq;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LazyRoommate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);
            LoadInfo();
        }


        public async void LoadInfo()
        {
            //Taking the already logged-in user info and putting them into the proper xaml elements
            //var userInfo = await App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);
            var UserTable = App.client.GetTable<UsersTable>();
            var TasksTable = App.client.GetTable<TasksTable>();

            var userItem = await UserTable.Where(x => (x.Email == App.Email)).ToListAsync();
            var user = userItem.FirstOrDefault();


            var allTasks = await TasksTable.Where(x => (x.RoomName == App.RoomName)).ToListAsync();//This might change to:( x.DoneBy==App.Email)
            var completedTasks = await TasksTable.Where(x => (x.DoneBy == App.Email) && (x.ConfirmedBy != string.Empty)).ToListAsync();

            ProfileName.Text = App.ProfileName;
            Email.Text = App.Email;
            ProfileImage.Source = App.ProfileImage;
            RoomName.Text = user.RoomName;
            AllTasks.Text = allTasks.Count.ToString();
            TasksCompleted.Text = completedTasks.Count.ToString();

            Debug.WriteLine(user.RoomName+"+++++++++++++++++++");

            if (!string.IsNullOrEmpty(user.RoomName))
            {
                var roomateItem = await UserTable.Where(x => (x.RoomName == user.RoomName) && (x.Email != App.Email))
                    .ToListAsync();

                roomateItem.ForEach(x =>
                {
                    if (x.Email != App.Email)
                        Roomates.Text += "\n" + "Name: " + x.Name + "\nEmail: " + x.Email + "\n";
                });
            }            
        }
    }
}
