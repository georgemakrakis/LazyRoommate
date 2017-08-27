using Acr.UserDialogs;
using LazyRoommate.DataFactoryModel;
using LazyRoommate.MenuItems;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LazyRoommate
{
    public partial class MainPage : MasterDetailPage
    {
        private MobileServiceUser user { get; set; }
        public Task Dialogs { get; private set; }
        private List<Menu> masterPageItems;
        public async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            //refreshing logic here
            await DataFactory.Init();
            timelineListView.ItemsSource = DataFactory.UserTasks;
            //make sure to end the refresh state
            list.IsRefreshing = false;
        }



        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, false);
         


            //Initializing the Hamburger menu

            masterPageItems = new List<Menu>
            {
               new Menu
                {
                    Title = "Add Task",
                    Icon="add_task.png",                                       
                    //TargetType = typeof(UserDialogs)
                },
                new Menu
                {
                    Title = "Join Room",
                    Icon = "join_room_32.png"
                    //TargetType = typeof( )
                },
                new Menu
                {
                    Title = "Create Room",
                      Icon = "create_room_32.png"                   
                    //TargetType = typeof( )
                },
                new Menu
                {
                    Title = "Leave Room",
                   Icon = "leave_room_32.png"                   
                    //TargetType = typeof( )
                }
            };
            menu.ItemsSource = masterPageItems;

            //BindingContext = DataFactory.Tasks;
                    
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DataFactory.Init();
                ActivityIndicator.IsRunning = false;
                ActivityIndicator.IsVisible = false;
                timelineListView.ItemsSource = DataFactory.UserTasks;
                                     
            });

           

            // Connecting context of this page to the our View Model class
            //BindingContext = new TasksViewModel();
        }        

        private async void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //var userInfo = await App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

            //Getting the source of the item selected on menu, so we could use is later
            var item = (Menu)e.SelectedItem;
            PromptConfig prmt=null;

            if (item.Title.Equals("Add Task"))
            {
                await Navigation.PushAsync(new CreateTasksPage());
            }
            else if (item.Title.Equals("Join Room"))
            {
                //Here we have everything that our Dialog contains            
                prmt = new PromptConfig();
                prmt.OkText = "Ok";
                prmt.CancelText = "Cancel";
                prmt.IsCancellable = true;

                prmt.Title = "Join Room";
            }
            else if (item.Title.Equals("Create Room"))
            {
                //Here we have everything that our Dialog contains            
                prmt = new PromptConfig();
                prmt.OkText = "Ok";
                prmt.CancelText = "Cancel";
                prmt.IsCancellable = true;

                prmt.Title = "Create Room";
            }
            else if (item.Title.Equals("Leave Room"))
            {
                var answer = await DisplayAlert("Leave Room", "Would you like to leave this room?", "Yes", "No");
                Debug.WriteLine("Answer: " + answer);
                if (answer)
                {
                    var UserTable = App.client.GetTable<UsersTable>();
                    var userItem = await UserTable.Where(x => (x.Email == App.Email)).ToListAsync();
                    var user = userItem.FirstOrDefault();

                    user.RoomName = null;

                    await UserTable.UpdateAsync(user);

                    await DisplayAlert("Leave Room", "You left your room","Ok");
                }
            }
            if (prmt != null)
            {
                var result = await UserDialogs.Instance.PromptAsync(prmt);

                if (result.Ok)
                {
                    //userInfo = await App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

                    var UserTable = App.client.GetTable<UsersTable>();
                    var userItem = await UserTable.Where(x => (x.Email == App.Email)).ToListAsync();
                    var user = userItem.FirstOrDefault();


                    if (item.Title.Equals("Create Room"))
                    {
                        try
                        {
                            //Also checking if the room name already exists and alert user
                            var roomItem = await UserTable.Where(x => (x.RoomName == result.Value)).ToListAsync();
                            if (roomItem != null)
                            {
                                //This is the way to update user's record with a new room value                                              
                                user.RoomName = result.Value;
                                await UserTable.UpdateAsync(user);

                               await DisplayAlert("Create Room","Room created succesfuly!!","Ok");                              

                            }
                            else
                            {
                              await  DisplayAlert("Create Room","Room name already exists!!", "Ok");
                            }

                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex);
                        }
                    }
                    else if (item.Title.Equals("Join Room"))
                    {
                        try
                        {
                            //Function for getting id of room and alter user's no2 record
                            //user.RoomName = result.Value;

                            var roomItem = await UserTable.Where(x => (x.RoomName == result.Value)).ToListAsync();
                            if(roomItem!=null)
                            {
                                user.RoomName = result.Value;

                                await UserTable.UpdateAsync(user);
                                //promt message successful join

                               await DisplayAlert("Join Room","Joining Room Succed!!","Ok");
                            }
                            else
                            {
                                //promt message unable to find
                                await DisplayAlert("Join Room", "Room name does not exist, please try again!", "Ok");
                            }




                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex);

                        }
                    }

                }
            }
        }
        private void timelineListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            timelineListView.SelectedItem = null;
        }

        private void profile_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ProfilePage(), true);
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            App.Email = string.Empty;
            App.ProfileImage = string.Empty;
            App.ProfileName = string.Empty;          
            await Navigation.PushModalAsync(new LoginPage(), true);

            //This just came up just for security-reverse engineering reasons i think...
            Navigation.RemovePage(this);
        }

        private void LoadingList(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void Settings_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new SettingsPage());
        }
    }
}
