using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LazyRoommate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();

            LoadInfo();

            //ProfileName.Text = App.client.CurrentUser;           
        }
        public void LoadInfo()
        {
            //Taking the already logged-in user info and putting them into the proper xaml elements
            //var userInfo = await App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

            ProfileName.Text = App.ProfileName;
            Email.Text = App.Email;
            ProfileImage.Source = App.ProfileImage;

            //These will be added later

            /*TasksDone.Text = null;
            AllTasks.Text = null;
            RoomID.Text = userInfo.RoomName;
            Roomates.Text = null;*/          
        }
    }
}
