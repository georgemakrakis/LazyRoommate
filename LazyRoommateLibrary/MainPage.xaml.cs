using Acr.UserDialogs;
using LazyRoommate.DataFactoryModel;
using LazyRoommate.MenuItems;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamForms.Controls;

namespace LazyRoommate
{
    public partial class MainPage : MasterDetailPage
    {
        public Task Dialogs { get; private set; }
        private List<Menu> masterPageItems;


        public async void LoadList(string date)
        {
            try
            {
                await DataFactory.Init(date);
                ActivityIndicator.IsRunning = false;
                ActivityIndicator.IsVisible = false;
                timelineListView.ItemsSource = DataFactory.UserTasks;
                retry.IsVisible = false;
                Calendar.ForceRedraw();
            }
            catch (HttpRequestException ex)
            {
                retry.IsVisible = true;
                retry.Text = "An Network error occured.\nPlease check network connectivity.";
                ActivityIndicator.IsRunning = false;
                ActivityIndicator.IsVisible = false;
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                retry.IsVisible = true;
                retry.Text = "A service related issue occured. Please contact admin.";
                ActivityIndicator.IsRunning = false;
                ActivityIndicator.IsVisible = false;
            }
            catch (Exception ex)
            {
                retry.IsVisible = true;
                retry.Text = "An other error occured please try again.";
                ActivityIndicator.IsRunning = false;
                ActivityIndicator.IsVisible = false;
            }
        }
        public void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            try
            {
                var date = Calendar.SelectedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                //refreshing logic here
                LoadList(date);
                //make sure to end the refresh state
                list.IsRefreshing = false;
            }
            catch(InvalidOperationException ex)
            {
                timelineListView.ItemsSource = null;
                retry.IsVisible = true;
                retry.Text = "Please select a date to see the tasks";
                list.IsRefreshing = false;
            }
            
           
        }



        public MainPage()
        {
            InitializeComponent();

            Calendar.SelectedDate = DateTime.Today;
            var date = Calendar.SelectedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            LoadList(date);

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, false);

            //Lack of having an PullToRefresh Action in windows made use use the toolbar to refresh the list
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    
                    break;
                case Device.Android:
                    
                    break;
                case Device.Windows:
                    ToolbarItems.Add(new ToolbarItem("Refresh", "refresh.png", () =>
                    {                       
                        LoadList(date);
                        //SubHeader.Text = day_selected;
                    }));
                    break;
            }

            //SizeChanged += ChangeCalendarSize;

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
                    Title = "Delete Account",
                    Icon = "delete_account.png"
                },
                new Menu
                {
                    Title = "Help",
                    Icon = "help_32.png"
                }
            };
            menu.ItemsSource = masterPageItems;

            //BindingContext = DataFactory.Tasks;           


            // Connecting context of this page to the our View Model class
            //BindingContext = new TasksViewModel();
        }

        private void ChangeCalendarSize(object sender, EventArgs e)
        {
            //Calendar.WidthRequest = Math.Max(Width, 400);
            //Calendar.HeightRequest = Math.Max(Height, 100);

            Calendar.WidthRequest = Math.Max(Width, 400);
            Calendar.HeightRequest = Math.Min(Height, 330);            
        }


        private async void OnMenuItemSelected(object sender, ItemTappedEventArgs e)
        {
            //var userInfo = await App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

            //Getting the source of the item selected on menu, so we could use is later
            var item = (Menu)e.Item;
            PromptConfig prmt = null;

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
                App.RoomName = string.Empty;
                await Navigation.PushAsync(new LoginPage(), true);

                //This just came up just for security-reverse engineering reasons i think...
                //Navigation.RemovePage(this);
            }
            else if (item.Title.Equals("Delete Account"))
            {
                var conf = await DisplayAlert("Delete Account", "Are you sure you want to delete your account? If you proceed all data you have will be deleted!", "Yes","No");
                if (conf)
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

                        await UserTable.DeleteAsync(user);

                        await DisplayAlert("User Deleted", "User " + App.Email + " has been deleted from app", "Ok");
                    }
                    //These are for admins
                    else
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

                        //At the end we remove user from admins table and users table
                        await roomsAdmins.DeleteAsync(admin);
                        await UserTable.DeleteAsync(user);

                        await DisplayAlert("User Deleted", "User " + App.Email + " has been deleted from app", "Ok");
                    }
                                    
                    App.Email = string.Empty;
                    App.ProfileImage = string.Empty;
                    App.ProfileName = string.Empty;
                    await Navigation.PushAsync(new LoginPage(), true);

                    //This just came up just for security-reverse engineering reasons i think...
                    Navigation.RemovePage(this);
                }
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
                            else if (roomsAdminsItem.Count == 0)
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
                            if (roomItem != null)
                            {
                                user.RoomName = result.Value;

                                await UserTable.UpdateAsync(user);
                                //promt message successful join

                                await DisplayAlert("Join Room", "Joining Room Succeed!!", "Ok");
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
            try
            {
                var item = (TasksTable)e.Item;
                var TaskTable = App.client.GetTable<TasksTable>();
                var taskItem = await TaskTable.Where(x => (x.TaskName == item.TaskName)).ToListAsync();
                var task = taskItem.FirstOrDefault();
                var answer = false;

                // code for user's role and alert message's attributes                
                int role = 0;// (1) done, (2) undone, (3) conf, (4) unconf
                string buttontxt = string.Empty; // user's action based on task's state

                if (task.DoneBy.Equals(App.Email) && !task.ConfirmedBy.Equals(App.Email))
                {
                    role = 2;
                    answer = await DisplayAlert(item.TaskName, item.TaskDescription + "\n\nDone by: " + task.DoneBy + "\nConfirmed by:" + task.ConfirmedBy, "Undone", "Cancel");
                }
                else if (task.ConfirmedBy.Equals(App.Email))
                {
                    role = 4;
                    answer = await DisplayAlert(item.TaskName, item.TaskDescription + "\n\nDone by: " + task.DoneBy + "\nConfirmed by:" + task.ConfirmedBy, "UnConfirm", "Cancel");
                }
                else if (task.DoneBy.Equals(string.Empty))
                {
                    role = 1;
                    answer = await DisplayAlert(item.TaskName, item.TaskDescription + "\n\nDone by: " + task.DoneBy + "\nConfirmed by:" + task.ConfirmedBy, "Done", "Cancel");
                }
                else if (task.ConfirmedBy.Equals(string.Empty) && !task.DoneBy.Equals(App.Email) && !task.DoneBy.Equals(string.Empty))
                {
                    role = 3;
                    answer = await DisplayAlert(item.TaskName, item.TaskDescription + "\n\nDone by: " + task.DoneBy + "\nConfirmed by:" + task.ConfirmedBy, "Confirm", "Cancel");
                }

                // action button 
                if (answer)
                {
                    switch (role)
                    {
                        case 1:
                            task.DoneBy = App.Email;
                            await TaskTable.UpdateAsync(task);
                            break;
                        case 2:
                            task.DoneBy = string.Empty;
                            task.ConfirmedBy = string.Empty;
                            await TaskTable.UpdateAsync(task);
                            break;
                        case 3:
                            task.ConfirmedBy = App.Email;
                            await TaskTable.UpdateAsync(task);
                            break;
                        case 4:
                            task.ConfirmedBy = string.Empty;
                            await TaskTable.UpdateAsync(task);
                            break;

                    }
                    // refres list with updated tasks 
                    var date = Calendar.SelectedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    await DataFactory.Init(date);
                    timelineListView.ItemsSource = DataFactory.UserTasks;
                    retry.IsVisible = false;

                }               
                ((ListView)sender).SelectedItem = null;
            }
            catch (Exception ex)
            {
                retry.IsVisible = true;
                retry.Text = "Changes can not be applied cause of network releated issue.";
            }
        }

        
        private void Calendar_OnDateClicked(object sender, DateTimeEventArgs e)
        {
           
            //Get the slected value from calendar
            var date = Calendar.SelectedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            LoadList(date);
            
        }
    }
}
