using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using SignalRAPIService.Core;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRAPIService.Hubs
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        #region variable
        static List<User> ConnectedUsers = new List<User>();
        static List<Room> Rooms = new List<Room>();
        #endregion variable

        public override Task OnConnected()
        {
            string username = Context.QueryString["username"];
            var connectionId = Context.ConnectionId;
            if (ConnectedUsers.Count(x => x.UserName == username || x.ConnectionID == connectionId) == 0)
            {
                var user = new User { ConnectionID = connectionId, UserName = username };
                ConnectedUsers.Add(user);
                // send to caller
                Clients.Caller.onConnected(connectionId, username, ConnectedUsers, user.UserName + " is connected.");
                // send to all except caller client
                Clients.AllExcept(connectionId).onNewUserConnected(connectionId, username);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string username = Context.QueryString["username"];
            var connectionId = Context.ConnectionId;
            if (ConnectedUsers.Count(x => x.UserName == username || x.ConnectionID == connectionId) > 0)
            {
                var user = ConnectedUsers.First(x => x.ConnectionID == connectionId);

                ConnectedUsers.Remove(user);
                // send to caller
                Clients.Caller.onConnected(connectionId, username, ConnectedUsers, user.UserName + " is disconnected.");
            }

            return base.OnDisconnected(stopCalled);
        }
        public async Task CreateRoom(string roomName)
        {
            var connectionId = Context.ConnectionId;
            var user = GetUser(connectionId);
            object result;
            if (Rooms.Count(x => x.RoomName == roomName) == 0)
            {
                Rooms.Add(new Room { RoomName = roomName, Users = new List<User> { user } });

                await Groups.Add(connectionId, roomName);
                result = GetResult(null, "0", roomName + " is created.");
            }
            else
            {
                result = GetResult(null, "-1", roomName + " is exist. Please choose new rom!");
            }
            Clients.Client(connectionId).receive(result);
        }
        public async Task JoinRoom(string roomName)
        {
            if (Rooms.Count(x => x.RoomName == roomName) > 0)
            {
                var connectionId = Context.ConnectionId;
                var user = GetUser(connectionId);
                Rooms.First(x => x.RoomName == roomName).Users.Add(user);

                await Groups.Add(connectionId, roomName);
                Clients.Group(roomName).receive(GetResult(null, "0", user.UserName + " joined."));
            }
        }
        public async Task LeaveRoom(string roomName)
        {
            if (Rooms.Count(x => x.RoomName == roomName) > 0)
            {
                var connectionId = Context.ConnectionId;
                var user = GetUser(connectionId);
                var room = Rooms.First(x => x.RoomName == roomName);
                room.Users.Remove(user);
                if (room.Users.Count == 0) Rooms.Remove(room);

                await Groups.Remove(connectionId, roomName);
                Clients.Group(roomName).receive(GetResult(null, "0", user.UserName + " leaved."));
            }
        }

        public void Send(string user, string message)
        {
            var connectionId = Context.ConnectionId;
            var fromUser = GetUser(connectionId);
            string[] users = user.Split(',');
            foreach (string s in users)
            {
                sendMessage(connectionId, fromUser, s, message);
            }
        }

        public void SendAll(string message)
        {
            Clients.All.receive(GetResult("System info: " + message, "chat"));
        }

        private void sendMessage(string connectionId, User user, string username, string message)
        {
            if (ConnectedUsers.Count(x => x.UserName == username) > 0)
            {
                var toUser = ConnectedUsers.First(x => x.UserName == username);
                Clients.Client(toUser.ConnectionID).receive(GetResult(user.UserName + ": " + message, "chat"));
            }
            else
            {
                Clients.Caller.showErrorMessage(GetResult("The user is no longer connected.", "-1"));
            }
        }

        private object GetResult(object data, string code = "1", string message = "")
        {
            object result;
            switch (code)
            {
                case "-1": //error
                    result = new { code = code, message = "ERROR::" + message };
                    break;

                case "0": //error
                    if (data == null)
                        result = new { code = code, message = message };
                    else
                        result = new { code = code, message = message, data = data };
                    break;

                default:
                    if (string.IsNullOrEmpty(message))
                        result = new { code = code, data = data };
                    else
                        result = new { code = code, message = message, data = data };
                    break;
            }
            return result;
        }

        private User GetUser(string connectionId)
        {
            if (ConnectedUsers.Count > 0 && ConnectedUsers.Count(x => x.ConnectionID == connectionId) > 0)
                return ConnectedUsers.First(x => x.ConnectionID == connectionId);
            return null;
        }
    }
}