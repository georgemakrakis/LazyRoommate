using LazyRoommate.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LazyRoommate.ViewModels
{
    public class TasksViewModel 
    {
        public static IList<TasksTable> Items { get; private set; }
        /*public IList<TasksTable> Items
        {
            get { return items; }
            set
            {

                items = value;
                OnPropertyChanged("Items");
            }
        }*/

        public TasksViewModel()
        {
            Items = new ObservableCollection<TasksTable>()
            {
                new TasksTable
                {
                     id =12,
                     TaskName= "TaskName",
                     TaskDescription ="Description",
                     RoomName ="RoomName",
                     Done =true,
                     Confirmed =true
                }
            };
        }        

    }
}
