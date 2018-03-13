
using Acr.UserDialogs;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Globalization;
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
            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);

            StartDate.MinimumDate = DateTime.Today;

            //ToolbarItems.Add(new ToolbarItem("Back", "back_button.png", () =>
            //{
            //    Application.Current.MainPage = new MasterDetailPage1
            //    {
            //        Master = new MasterDetailPage1Master(),
            //        Detail = new NavigationPage(new MasterDetailPage1Detail())
            //        {
            //            BarBackgroundColor = Color.FromHex("#FFA000"),
            //            BarTextColor = Color.White
            //        }
            //    };
            //}));


            //Enabling button only after entry
            TaskName.TextChanged += EnableSaveItemButton;
        }

        private void EnableSaveItemButton(object sender, TextChangedEventArgs e)
        {
            AddBtn.IsEnabled = !e.NewTextValue.Equals("");
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
                            // DoneBy is not empty for notifications purpose, it empties on serivce
                            DoneBy = App.Email,
                            ConfirmedBy = string.Empty,
                            StartDate= StartDate.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                            EndDate = EndDate.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                        });
                        await DisplayAlert("Task", "New task added!!!", "Ok");
                        Application.Current.MainPage = new MasterDetailPage1
                        {
                            Master = new MasterDetailPage1Master(),
                            Detail = new NavigationPage(new MasterDetailPage1Detail())
                            {
                                BarBackgroundColor = Color.FromHex("#FFA000"),
                                BarTextColor = Color.White
                            }
                        };

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
