using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace LazyRoommate
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        public async void SignUpClick(object sender, EventArgs e)
        {

            //this method works for android and UWP link : https://developer.xamarin.com/guides/xamarin-forms/user-interface/navigation/modal/
            await this.Navigation.PushModalAsync(new SignUpPage());

        }
    }
}
