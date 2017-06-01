namespace LazyRoommate.Models
{
    public class TasksTable
    {
        public string id { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string RoomName { get; set; }
        public bool Done { get; set; }
        public bool Confirmed { get; set; }
    }
}
