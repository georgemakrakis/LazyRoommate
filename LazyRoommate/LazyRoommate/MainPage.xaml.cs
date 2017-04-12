using Acr.UserDialogs;
using LazyRoommate.DataFactoryModel;
using LazyRoommate.MenuItems;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LazyRoommate
{
    public partial class MainPage : MasterDetailPage
    {
        private MobileServiceUser user { get; set; }
        public Task Dialogs { get; private set; }
        private List<Menu> masterPageItems;
        public MainPage()
        {            
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);


            
            //Initializing the Hamburger menu

            masterPageItems = new List<Menu>
            {
                new Menu
                {
                    Title = "Add Task",
                    //IconSource = "",
                    //TargetType = typeof(UserDialogs)
                },
                new Menu
                {
                    Title = "Join Room",
                    //IconSource = "",
                    //TargetType = typeof( )
                },
                new Menu
                {
                    Title = "Create Room",
                    //IconSource = "",
                    //TargetType = typeof( )
                }
            };
            menu.ItemsSource = masterPageItems;

            BindingContext = DataFactory.Classes;
        }
        private async void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var userInfo = await App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

            //Getting the source of the item selected on menu, so we could use is later
            var item = (Menu)e.SelectedItem;

            //Here we have everything that our Dialog contains            
            var prmt = new PromptConfig();            
            prmt.OkText = "Ok";
            prmt.CancelText = "Cancel";
            prmt.IsCancellable = true;
            if (item.Title.Equals("Add Task"))
            {
                prmt.Title = "Add Task";
            }
            else if (item.Title.Equals("Join Room"))
            {
                prmt.Title = "Join Room";
            }
            else if (item.Title.Equals("Create Room"))
            {
                prmt.Title = "Create Room";
            }                        
                       
            var result=await UserDialogs.Instance.PromptAsync(prmt);
            if(result.Ok)
            {
                try
                {
                    var table = App.client.GetTable<TasksTable>();
                    await table.InsertAsync(new TasksTable { id = "1", TaskName = result.Value, RoomName = "1", Done = false, Confirmed = false });
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            }
                       
        }        
        private void timelineListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            timelineListView.SelectedItem = null;
        }

        private void OnLabelClicked()
        {

        }

        private void Button1_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage();
            IsPresented = false;
        }
        private void Button2_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage();
            IsPresented = false;
        }
        private void Button3_Clicked(object sender, System.EventArgs e)
        {
            Detail = new NavigationPage();
            IsPresented = false;
        }

        private void profile_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ProfilePage(), true);
        }
    }
}
