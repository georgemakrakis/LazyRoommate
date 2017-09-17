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


            var date = DateTime.UtcNow;
            Day1.Text = date.Day + " " + date.DayOfWeek;
            date=date.AddDays(1);
            Day2.Text = date.Day+ " " + (date.DayOfWeek);
            date = date.AddDays(1);
            Day3.Text = date.Day + " " + (date.DayOfWeek);
            date = date.AddDays(1);
            Day4.Text = date.Day + " " + (date.DayOfWeek);
            date = date.AddDays(1);
            Day5.Text = date.Day + " " + (date.DayOfWeek);
            date = date.AddDays(1);
            Day6.Text = date.Day + " " + (date.DayOfWeek);
            date = date.AddDays(1);
            Day7.Text = date.Day + " " + (date.DayOfWeek);

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
                },
                new Menu
                {
                    Title = "Remove Roommate",
                    Icon = "remove_roommate.png"                   
                    //TargetType = typeof( )
                },
                new Menu
                {
                    Title = "Profile",
                    Icon = "user_profile_32.png"
                },
                new Menu
                {
                    Title = "Logout",
                    Icon = "logout_32.png"
                },
                new Menu
                {
                    Title = "Settings",
                    Icon = "settings_32.png"
                },
                new Menu
                {
                    Title = "Help",
                    Icon = "help_32.png"
                }
            };
            menu.ItemsSource = masterPageItems;            

            //BindingContext = DataFactory.Tasks;
                    
            Device.BeginInvokeOnMainThread(async () =>
            {
                //var list = timelineListView.ItemTemplate;
                //list.Values.
                await DataFactory.Init();
                ActivityIndicator.IsRunning = false;
                ActivityIndicator.IsVisible = false;
                timelineListView.ItemsSource = DataFactory.UserTasks;


                                     
            });

           

            // Connecting context of this page to the our View Model class
            //BindingContext = new TasksViewModel();
        }        

        private async void OnMenuItemSelected(object sender, ItemTappedEventArgs  e)
        {
            //var userInfo = await App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

            //Getting the source of the item selected on menu, so we could use is later
            var item = (Menu)e.Item;           
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

                    var roomsAdmins = App.client.GetTable<RoomsAdmins>();
                    var roomsAdminsItem = await roomsAdmins.Where(x => (x.Admin == user.Email)).ToListAsync();
                    var admin = roomsAdminsItem.FirstOrDefault();
                    
                    //These are for regular users   
                    if (admin == null)
                    {                                                               
                        user.RoomName = null;

                        await UserTable.UpdateAsync(user);

                        await DisplayAlert("Leave Room", "You left your room", "Ok");
                    }
                    //These are for admins
                    else
                    {
                        var answer2 = await DisplayAlert("Leave Room", "You are admin of this room. If you proceed all Tasks will be deleted! \n Do you want to continue?", "Yes", "No");
                        if (answer2)
                        {
                            //First we remove all references from user table that points to the specific room
                            var roomRemoveItem = await UserTable.Where(x => (x.RoomName == admin.Room)).ToListAsync();
                            roomRemoveItem.ForEach(async x =>
                            {
                                x.RoomName = null;                                
                                await UserTable.UpdateAsync(x);
                            });

                            //We delete all tasks related to the room
                            var TaskTable = App.client.GetTable<TasksTable>();
                            var tasksItem = await TaskTable.Where(x => (x.RoomName == admin.Room)).ToListAsync();
                            tasksItem.ForEach(async x =>
                            {
                                var taskRemove = tasksItem.FirstOrDefault();
                                await TaskTable.DeleteAsync(taskRemove);
                            });

                            //At the end we remove user from admins table
                            await roomsAdmins.DeleteAsync(admin);
                            await DisplayAlert("Leave Room", "You left your room", "Ok");
                        }
                    }                
                }                   
            }
            else if (item.Title.Equals("Remove Roommate"))
            {
                //Here we have everything that our Dialog contains            
                prmt = new PromptConfig();
                prmt.OkText = "Ok";
                prmt.CancelText = "Cancel";
                prmt.IsCancellable = true;

                prmt.Title = "Enter the email of the user you want to remove:";
            }
            else if (item.Title.Equals("Profile"))
            {
                await Navigation.PushAsync(new ProfilePage(), true);
            }
            else if (item.Title.Equals("Logout"))
            {
                App.Email = string.Empty;
                App.ProfileImage = string.Empty;
                App.ProfileName = string.Empty;
                await Navigation.PushAsync(new LoginPage(), true);

                //This just came up just for security-reverse engineering reasons i think...
                //Navigation.RemovePage(this);
            }
            else if (item.Title.Equals("Settings"))
            {
                await Navigation.PushAsync(new SettingsPage(), true);
            }
            else if (item.Title.Equals("Help"))
            {

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
                            var roomsAdmins = App.client.GetTable<RoomsAdmins>();
                            var roomsAdminsItem = await roomsAdmins.Where(x => (x.Admin == user.Email)).ToListAsync();
                            if (roomsAdminsItem.Count > 0)
                            {
                                var admin = roomsAdminsItem.FirstOrDefault();

                                if (admin.Room != null)
                                {                                    
                                    await DisplayAlert("Create Room", "Cannot create room because you already own a room /n Leave your room and then create another one", "Ok");
                                    
                                }
                            }                           
                            else if(roomsAdminsItem.Count==0)
                            {
                                try
                                {
                                    //Also checking if the room name already exists and alert user
                                    var roomItem = await UserTable.Where(x => (x.RoomName == result.Value))
                                        .ToListAsync();

                                    if (roomItem != null)
                                    {
                                        //This is the way to update user's record with a new room value                                              
                                        user.RoomName = result.Value;
                                        await UserTable.UpdateAsync(user);

                                        //We add the creator in the RoomsAdmins Table                               
                                        await roomsAdmins.InsertAsync(new RoomsAdmins
                                        {
                                            Admin = user.Email,
                                            Room = user.RoomName
                                        });

                                        await DisplayAlert("Create Room", "Room created succesfuly!!", "Ok");

                                    }
                                    else
                                    {
                                        await DisplayAlert("Create Room", "Room name already exists!!", "Ok");
                                    }

                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex);
                                }
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
                                
                               await DisplayAlert("Join Room","Joining Room Succeed!!","Ok");
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
                    else if (item.Title.Equals("Remove Roommate"))
                    {
                        try
                        {
                            var roomsAdmins = App.client.GetTable<RoomsAdmins>();
                            var roomsAdminsItem = await roomsAdmins.Where(x => (x.Admin == user.Email)).ToListAsync();
                            var admin = roomsAdminsItem.FirstOrDefault();

                            if (admin != null)
                            {
                                try
                                {
                                    //Function for getting id of room and alter user's no2 record
                                    //user.RoomName = result.Value;
                                    var answer = await DisplayAlert("Remove Roommate", "Are you sure you want to remove " + result.Text + " from your room?", "Yes", "No");
                                    Debug.WriteLine("Answer: " + answer);
                                    if (answer)
                                    {
                                        var userRemoveItem = await UserTable.Where(x => (x.Email == result.Text)).ToListAsync();
                                        var userRemove = userRemoveItem.FirstOrDefault();

                                        userRemove.RoomName = null;

                                        await UserTable.UpdateAsync(user);

                                        await DisplayAlert("Leave Room", "User: " + result.Text + " has been removed", "Ok");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex);

                                }
                            }
                            else
                            {
                                await DisplayAlert("Remove Roommate", "You are not onwer of this room so you can't remove any other users", "Ok");
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
        private async void timelineListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //timelineListView.SelectedItem = null;
            var item = (TasksTable)e.Item;
            await DisplayAlert(item.TaskName,item.TaskDescription,"Ok");
            ((ListView) sender).SelectedItem = null;
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
            await Navigation.PushAsync(new LoginPage(), true);

            //This just came up just for security-reverse engineering reasons i think...
            //Navigation.RemovePage(this);
        }

        private void LoadingList(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void Settings_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPage(),true);
        }

        private async void DoneConfirm_OnToggled(object sender, ToggledEventArgs e)
        {
            //toogle.ClassId is used to specify the task naem of every list item
            var toogle = (Switch) sender;
            
            //Debug.WriteLine(toogle.ClassId);
            var TaskTable = App.client.GetTable<TasksTable>();
            var taskItem = await TaskTable.Where(x => (x.TaskName == toogle.ClassId)).ToListAsync();
            var task = taskItem.FirstOrDefault();
            
            if (toogle.IsToggled)
            {                
                if (task.Done.Equals(App.Email))
                {
                    await DisplayAlert("Error", "You cannot confirm you own task", "Ok");

                    toogle.IsToggled = false;
                }
                else if (task.Done.Equals(string.Empty))
                {
                    task.Done = App.Email;

                    await TaskTable.UpdateAsync(task);
                }
                else if (!task.Done.Equals(App.Email))
                {
                    task.Confirmed = App.Email;

                    await TaskTable.UpdateAsync(task);
                }
            }
            else
            {
                if (task.Done.Equals(App.Email))
                {
                    task.Done = string.Empty;

                    await TaskTable.UpdateAsync(task);
                }
                else if (task.Done.Equals(string.Empty))
                {
                    //do nothing cause task is empty
                }
                else if (!task.Done.Equals(App.Email))
                {
                    task.Confirmed = string.Empty;

                    await TaskTable.UpdateAsync(task);
                }
            }
        }
    }
}
