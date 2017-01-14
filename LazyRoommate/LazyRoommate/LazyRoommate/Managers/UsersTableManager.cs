using Microsoft.WindowsAzure.MobileServices;

namespace LazyRoommate.Managers
{
    public partial class UsersTableManager
    {
        static UsersTableManager defaultInstance = new UsersTableManager();
        private IMobileServiceTable<Models.UsersTable> UsersTable;
        private UsersTableManager()
        {
            UsersTable = App.client.GetTable<Models.UsersTable>();

        }
        public static UsersTableManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }
        public MobileServiceClient CurrentClient
        {
            get { return App.client; }
        }
        
    }
}
