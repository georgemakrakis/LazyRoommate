
using Acr.UserDialogs;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LazyRoommate
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateTasksPage 
    {
        public CreateTasksPage()
        {
            InitializeComponent();
            //Enabling button only after entry
            TaskName.TextChanged += EnableSaveItemButton;
        }

        private void EnableSaveItemButton(object sender, TextChangedEventArgs e)
        {
            AddBtn.IsEnabled = true;
        }

        private async void OkClicked(object sender, System.EventArgs e)
        {
            //var userInfo = await App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);
            try
            {
                var UserTable = App.client.GetTable<UsersTable>();
                var userItem = await UserTable.Where(x => (x.Email == App.Email)).ToListAsync();
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
                        await TaskTable.InsertAsync(new TasksTable
                        {
                            TaskName = TaskName.Text,
                            TaskDescription = TaskDesc.Text,
                            RoomName = user.RoomName,
                            DoneBy = "",
                            ConfirmedBy = "",
                            StartDate= StartDate.Date,
                            EndDate= EndDate.Date,
                        });
                        await DisplayAlert("Task", "New task added!!!", "Ok");               
                        await Navigation.PushAsync(new MainPage());             
               
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                AlertConfig t_config = new AlertConfig();
                t_config.SetOkText("OK");
                t_config.SetMessage("An Network error occured. Please check network connectivity.");
                t_config.SetTitle("Error");
                await UserDialogs.Instance.AlertAsync(t_config);                
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                AlertConfig t_config = new AlertConfig();
                t_config.SetOkText("OK");
                t_config.SetMessage("A service related issue occured. Please contact admin.");
                t_config.SetTitle("Error");
                await UserDialogs.Instance.AlertAsync(t_config);               
            }
            catch (Exception ex)
            {
                AlertConfig t_config = new AlertConfig();
                t_config.SetOkText("OK");
                t_config.SetMessage(ex.ToString());
                t_config.SetTitle("Error");
                await UserDialogs.Instance.AlertAsync(t_config);
            }




        }
    }


}
