﻿using LazyRoommate.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace LazyRoommate.DataFactoryModel
{
    public static class DataFactory
    {
        public static IList<TasksTable> Tasks { get; private set; }
        public static MobileServiceCollection<TasksTable, TasksTable> UserTasks { get; set; }
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
                new TasksTable
                {
                    id="12",
                    TaskName="name",
                    TaskDescription="Descr",
                    RoomName="George's",
                    Done=true,
                    Confirmed=true
                },
                new TasksTable
                {
                    id="13",
                    TaskName="name2",
                    TaskDescription="Descr2",
                    RoomName="Argyris",
                    Done=true,
                    Confirmed=true
                }
            };
        }
        public static async Task Init()
        {

            //var userInfo = await App.client.InvokeApiAsync<UserInfo>("UserInfo", HttpMethod.Get, null);

            //userinfo request is for Federetion Login
            var UserTable = App.client.GetTable<UsersTable>();
            var userItem = await UserTable.Where(x => (x.Email == App.UserName)).ToListAsync();
            var user = userItem.FirstOrDefault();

            var TaskTable = App.client.GetTable<TasksTable>();
            UserTasks = await TaskTable.Where(x => (x.RoomName == user.RoomName)).ToCollectionAsync();
            //Everything that contains at least the current date and all that are un-done(un-confirmed)
            //var task = TaskItem;

        }

    }
}