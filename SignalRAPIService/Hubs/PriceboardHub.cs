using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using SignalRAPIService.Core;

namespace SignalRAPIService.Hubs
{
    [HubName("priceboard")]
    public class PriceboardHub : Hub
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
                channel = "vst-priceboard"; //default channel

            Groups.Add(connectionId, channel.ToLower());

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string channel = Context.QueryString["channel"];
            string connectionId = Context.ConnectionId;
            try
            {
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
        }

        #endregion public function

        #region private function

        #endregion private function
    }
}