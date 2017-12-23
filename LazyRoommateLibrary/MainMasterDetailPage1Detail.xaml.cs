using LazyRoommate.DataFactoryModel;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamForms.Controls;

namespace LazyRoommate
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPage1Detail : ContentPage
    {
        private string date;
        public MasterDetailPage1Detail()
        {
            InitializeComponent();

            Calendar.SelectedDate = DateTime.Today;
            date = Calendar.SelectedDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            LoadList(date);

            //MasterDetailPage1.DetailNavPage
            //NavigationPage.SetHasNavigationBar(this, true);
            //NavigationPage.SetHasBackButton(this, false);


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
                    }));
                    break;
            }
        }
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
            catch (InvalidOperationException ex)
            {
                timelineListView.ItemsSource = null;
                retry.IsVisible = true;
                retry.Text = "Please select a date to see the tasks";
                list.IsRefreshing = false;
            }


        }
        private async void timelineListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var item = (TasksTable)e.Item;
                var TaskTable = App.client.GetTable<TasksTable>();
                var taskItem = await TaskTable.Where(x => (x.TaskName == item.TaskName) &&  (x.id == item.id)).ToListAsync();
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