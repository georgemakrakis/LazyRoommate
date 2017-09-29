using System;

namespace LazyRoommate.Models
{
    public class TasksTable
    {
        public string id { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string RoomName { get; set; }
        public string DoneBy { get; set; }
        public string ConfirmedBy { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
