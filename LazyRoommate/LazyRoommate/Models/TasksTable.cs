namespace LazyRoommate.Models
{
    public class TasksTable
    {
        public int id { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string RoomName { get; set; }
        public bool Done { get; set; }
        public bool Confirmed { get; set; }
    }
}
