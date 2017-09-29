using Acr.UserDialogs;
using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LazyRoommate.DataFactoryModel
{
    public static class DataFactory
    {
        public static IList<TasksTable> Tasks { get; private set; }
        public static ICollection<TasksTable> UserTasks = new ObservableCollection<TasksTable>();

        private static DateTime TodayAt(int hour, int minute)
        {
            return new DateTime(DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                hour, minute, 0);
        }        
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

                var TaskTable = App.client.GetTable<TasksTable>();
                var TempTasks = await TaskTable.Where(x => (x.RoomName == user.RoomName)).ToCollectionAsync();
                foreach (var x in TempTasks)
                {
                    if ((x.StartDate.Month.ToString() == date.Substring(0, 1) || (x.StartDate.Month.ToString()== date.Substring(0,2))) &&                    
                        (x.StartDate.Day.ToString() == date.Substring(2, 2) || x.StartDate.Day.ToString() == date.Substring(3, 1)) &&
                        x.StartDate.Year.ToString() == date.Substring(5, 4))
                    {
                      Debug.WriteLine(x.StartDate.Year.ToString());
                      Debug.WriteLine(date.Substring(5, 4));
                       UserTasks.Add(x);
                    }

                }
                
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