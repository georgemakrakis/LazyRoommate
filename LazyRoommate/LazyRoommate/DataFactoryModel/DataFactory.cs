using LazyRoommate.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LazyRoommate.DataFactoryModel
{
    public static class DataFactory
    {
        public static IList<TasksTable> Tasks { get; private set; }

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
                    id=12,
                    TaskName="name",
                    TaskDescription="Descr",
                    RoomName="George's",
                    Done=true,
                    Confirmed=true
                },
                new TasksTable
                {
                    id=13,
                    TaskName="name2",
                    TaskDescription="Descr2",
                    RoomName="Argyris",
                    Done=true,
                    Confirmed=true
                }
            };
        }
    }
}
