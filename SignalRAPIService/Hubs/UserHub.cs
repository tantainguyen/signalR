using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using SignalRAPIService.Core;

namespace SignalRAPIService.Hubs
{
    [HubName("user")]
    public class UserHub : Hub
    {
        #region variable
        static List<Client> clients = new List<Client>();
        #endregion variable

        #region override function
        public override Task OnConnected()
        {
            string channel = Context.QueryString["channel"];
            string connectionId = Context.ConnectionId;

            if (string.IsNullOrEmpty(channel))
                channel = "vst-user"; //default channel

            //if (clients.Count(x => x.ConnectionId == connectionId) == 0)
            //{
            //    clients.Add(new Client { ConnectionId = connectionId, Channel = channel.ToLower() });
            //}

            Groups.Add(connectionId, channel.ToLower());

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string channel = Context.QueryString["channel"];
            string connectionId = Context.ConnectionId;
            try
            {
                /*
                Client client = null;
                if (string.IsNullOrEmpty(channel))
                    client = clients.Single(x => x.ConnectionId == connectionId);
                else
                    client = clients.Single(x => x.ConnectionId == connectionId && x.Channel == channel);
                if (client != null)
                    clients.Remove(client);
                */
                if (!string.IsNullOrEmpty(channel))
                {
                    Groups.Remove(connectionId, channel.ToLower());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }
        #endregion override function

        #region public function
        public void SendAll(string message)
        {
            Clients.All.receive(message);
        }

        public void Notify(string message)
        {
            Clients.All.receive(message);
        }

        public void SendChannel(string channel, string message)
        {
            if (!string.IsNullOrEmpty(channel))
                Clients.Group(channel.ToLower()).receive(message);
            /*
            foreach (var client in clients.Where(x => x.Channel == channel.ToLower()))
            {
                Clients.Client(client.ConnectionId).receive(message);
            }
            */
        }

        #endregion public function

        #region private function

        #endregion private function
    }
}