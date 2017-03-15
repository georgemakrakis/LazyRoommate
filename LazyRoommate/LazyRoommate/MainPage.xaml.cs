using LazyRoommate.MenuItems;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LazyRoommate
{
    public partial class MainPage : MasterDetailPage
    {
        private MobileServiceUser user { get; set; }
        
        public MainPage()
        {            
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, true);

            //Initializing the Hamburger menu

            var masterPageItems = new List<Menu>
            {
                new Menu
                {
                    Title = "Add Task",
                    //IconSource = ""
                    //TargetType = typeof( )
                },
                new Menu
                {
                    Title = "Join Room",
                    //IconSource = ""
                    //TargetType = typeof( )
                },
                new Menu
                {
                    Title = "Create Room",
                    //IconSource = ""
                    //TargetType = typeof( )
                }
            };
            menu.ItemsSource = masterPageItems;
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
