
using LazyRoommate.Models;
using System;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms.Xaml;

namespace LazyRoommate
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateTasksPage
    {
        public CreateTasksPage()
        {
            InitializeComponent();
        }

        private async void OkClicked(object sender, System.EventArgs e)
        {
            var userInfo = await App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

            var UserTable = App.client.GetTable<UsersTable>();
            var userItem = await UserTable.Where(x => (x.Email == userInfo.Email)).ToListAsync();
            var user = userItem.FirstOrDefault();

            if (user.RoomName == null)
            {
                await DisplayAlert("Task", "You have to belong in a room to create task.", "Ok");
            }
            else
            {
                try
                {           
                    var TaskTable = App.client.GetTable<TasksTable>();
                    await TaskTable.InsertAsync(new TasksTable {TaskName = TaskName.Text, TaskDescription = TaskDesc.Text, RoomName = user.RoomName, Done = false, Confirmed = false });
                    await DisplayAlert("Task", "New task added!!!", "Ok");               
                    await Navigation.PushAsync(new MainPage());             
               
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }

            

        }
    }


}
