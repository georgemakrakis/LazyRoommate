using LazyRoommate.MenuItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using LazyRoommate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LazyRoommate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPage1Master : ContentPage
    {
        public Task Dialogs { get; private set; }
        private List<Menu> masterPageItems;

        public MasterDetailPage1Master()
        {
            InitializeComponent();

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
                var conf = await DisplayAlert("Delete Account", "Are you sure you want to delete your account? If you proceed all data you have will be deleted!", "Yes", "No");
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

        class MasterDetailPage1MasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterDetailPage1MenuItem> MenuItems { get; set; }

            public MasterDetailPage1MasterViewModel()
            {
                MenuItems = new ObservableCollection<MasterDetailPage1MenuItem>(new[]
                {
                    new MasterDetailPage1MenuItem { Id = 0, Title = "Page 1" },
                    new MasterDetailPage1MenuItem { Id = 1, Title = "Page 2" },
                    new MasterDetailPage1MenuItem { Id = 2, Title = "Page 3" },
                    new MasterDetailPage1MenuItem { Id = 3, Title = "Page 4" },
                    new MasterDetailPage1MenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}