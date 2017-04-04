using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRAPIService.Core
{
    public class User
    {
        public string ConnectionID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }

    public class Room
    {
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        public List<User> Users { get; set; }

    }
}