using Acr.UserDialogs;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace LazyRoommate.DataFactoryModel
{
    public class TasksTableCopy
    {
        public string id { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string RoomName { get; set; }
        public string DoneBy { get; set; }
        public string ConfirmedBy { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public double DaysLeft { get; set; }
    }
    public static class DataFactory
    {
        public static IList<TasksTable> Tasks { get; private set; }
        public static ICollection<TasksTableCopy> UserTasks = new ObservableCollection<TasksTableCopy>();
        static DataFactory()
        {

            Tasks = new ObservableCollection<TasksTable>
            {
                //new TasksTable
                //{
                //    id="12",
                //    TaskName="name",
                //    TaskDescription="Descr",
                //    RoomName="George's",
                //    DoneBy="",
                //    ConfirmedBy="",

                //},
                //new TasksTable
                //{
                //    id="13",
                //    TaskName="name2",
                //    TaskDescription="Descr2",
                //    RoomName="Argyris",
                //    DoneBy="",
                //    ConfirmedBy="",

                //}
            };
        }
        public static async Task Init(string date)
        {

            //var userInfo = await App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

            //userinfo request is for Federetion Login
            try
            {
                var UserTable = App.client.GetTable<UsersTable>();
                var userItem = await UserTable.Where(x => (x.Email == App.Email)).ToListAsync();
                var user = userItem.FirstOrDefault();

                //LazyRoommate.App.client.CurrentUser = new MobileServiceUser(App.AccountUsername);
                //LazyRoommate.App.client.CurrentUser.MobileServiceAuthenticationToken = App.Token;

                var TaskTable = App.client.GetTable<TasksTable>();
                var usrtaks = await TaskTable.Where(x => (x.RoomName == user.RoomName)).ToCollectionAsync();
                UserTasks.Clear();
                foreach (var x in usrtaks)
                {
                    TasksTableCopy taskCopy = new TasksTableCopy()
                    {
                        id = x.id,
                        TaskName = x.TaskName,
                        TaskDescription = x.TaskDescription,
                        RoomName = x.RoomName,
                        DoneBy = x.DoneBy,
                        ConfirmedBy = x.ConfirmedBy,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate
                    };
                    if (DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(x.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(x.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                    {
                        //These if here are changeing a variable so we can show a message to users for the state of the task
                        if (x.ConfirmedBy != string.Empty)
                        {
                            taskCopy.Status = "Confirmed";
                        }
                        else if (x.DoneBy != string.Empty)
                        {
                            taskCopy.Status = "Done";
                        }

                        taskCopy.DaysLeft = (DateTime.ParseExact(x.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) -
                                      DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture)).TotalDays;
                        UserTasks.Add(taskCopy);
                    }
                }

                //HttpClient client = new HttpClient
                //{
                //    BaseAddress = new Uri("https://lazyroommateservice.azurewebsites.net")
                //};
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(
                //    new MediaTypeWithQualityHeaderValue("application/json"));

                //HttpResponseMessage response = await client.GetAsync(client.BaseAddress + "tables/TasksTables/TasksBetween?roomname=" + App.RoomName + "&date=" + date);
                //if (response.IsSuccessStatusCode)
                //{
                //    List<TasksTable> model;
                //    var resultJSON = await response.Content.ReadAsStringAsync();
                //    model = JsonConvert.DeserializeObject<List<TasksTable>>(resultJSON);
                //}

                //var result = await App.client.InvokeApiAsync<TasksTable>("/tables/TasksTable/TasksBetween", System.Net.Http.HttpMethod.Get, null);



                //These must be in the query above
                //foreach (var x in TempTasks)
                //{
                //    if ((x.StartDate.Month.ToString() == date.Substring(0, 1) || (x.StartDate.Month.ToString() == date.Substring(0, 2))) &&
                //        (x.StartDate.Day.ToString() == date.Substring(2, 2) || x.StartDate.Day.ToString() == date.Substring(3, 1)) &&
                //        x.StartDate.Year.ToString() == date.Substring(5, 4))
                //    {
                //        //Debug.WriteLine(x.StartDate.Year.ToString());
                //        //Debug.WriteLine(date.Substring(5, 4));
                //        var tempDate = x.StartDate.Date;
                //        x.StartDate=(tempDate.ToString("d"));

                //        UserTasks.Add(x);
                //    }

                //}

                //foreach (var task in UserTasks)
                //{
                //    if (task.Done.Equals(App.Email))
                //    {
                //        if ()
                //        {

                //        }
                //    }
                //}

            }
            catch (HttpRequestException ex)
            {
                //AlertConfig t_config = new AlertConfig();
                //t_config.SetOkText("OK");
                //t_config.SetMessage("An Network error occured. Please check network connectivity.");
                //t_config.SetTitle("Error");
                //await UserDialogs.Instance.AlertAsync(t_config);

                //await Init();    
                throw ex;
            }
            catch (MobileServiceInvalidOperationException ex)
            {
                //AlertConfig t_config = new AlertConfig();
                //t_config.SetOkText("OK");
                //t_config.SetMessage("A service related issue occured. Please contact admin.");
                //t_config.SetTitle("Error");
                //await UserDialogs.Instance.AlertAsync(t_config);

                //await Init();                
                throw ex;
            }
            catch (Exception ex)
            {
                //AlertConfig t_config = new AlertConfig();
                //t_config.SetOkText("OK");
                //t_config.SetMessage(ex.ToString());
                //t_config.SetTitle("Error");
                //await UserDialogs.Instance.AlertAsync(t_config);
                throw ex;

                //await Init();

            }
            //Everything that contains at least the current date and all that are un-done(un-confirmed)
            //var task = TaskItem;

        }

    }
}