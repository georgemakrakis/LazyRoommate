using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Xamarin.Forms;

namespace LazyRoommate
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            //making UWP back button visible
            App.currentView = SystemNavigationManager.GetForCurrentView();
            App.currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            
        }
    }
}
