using System;
using System.Linq;
using LazyRoommate.Models;
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
           
            LoadInfo();

            //ProfileName.Text = App.client.CurrentUser;           
        }
        public async void LoadInfo()
        {
            //Taking the already logged-in user info and putting them into the proper xaml elements
            //var userInfo = await App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);
            var UserTable = App.client.GetTable<UsersTable>();
            var TasksTable = App.client.GetTable<TasksTable>();
            var roomateItem = await UserTable.Where(x => (x.RoomName == App.RoomName)&&(x.Email!=App.Email)).ToListAsync();

            var allTasks =await TasksTable.Where(x => (x.RoomName == App.RoomName)).ToListAsync();//This might change to:( x.DoneBy==App.Email)
            var completedTasks =await TasksTable.Where(x => (x.DoneBy==App.Email) && (x.ConfirmedBy!=string.Empty) ).ToListAsync();

            ProfileName.Text = App.ProfileName;
            Email.Text = App.Email;
            ProfileImage.Source = App.ProfileImage;
            RoomName.Text = App.RoomName;
            AllTasks.Text = allTasks.Count.ToString();
            TasksCompleted.Text = completedTasks.Count.ToString();

            roomateItem.ForEach(x =>
            {
                if(x.Email!=App.Email)
                Roomates.Text += "\n" + "Name: " + x.Name + "\nEmail: " + x.Email+"\n";
            });
            //These will be added later

            /*TasksDone.Text = null;
            AllTasks.Text = null;
            RoomID.Text = userInfo.RoomName;
            Roomates.Text = null;*/          
        }
    }
}
