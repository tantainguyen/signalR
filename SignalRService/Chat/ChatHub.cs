using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using System.Diagnostics;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace SignalRService
{
    [HubName("chat")]
    public class ChatHub : Hub
    {
        #region variable
        public static IHubContext hubContext;
        private static List<ClientModel> clients = new List<ClientModel>();
        #endregion variable
        #region override function
        public override Task OnConnected()
        {

            string channelId = Context.QueryString["channel"];
            string connectionId = Context.ConnectionId;

            if (string.IsNullOrEmpty(channelId))
                channelId = "vst-chat"; //default channel

            if (clients.Count(x => x.ConnectionId == connectionId) == 0)
            {
                clients.Add(new ClientModel { ConnectionId = connectionId, ChannelId = channelId.ToLower() });
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string channelId = Context.QueryString["channel"];
            string connectionId = Context.ConnectionId;
            try
            {
                ClientModel client = null;
                if (string.IsNullOrEmpty(channelId))
                    client = clients.Single(x => x.ConnectionId == connectionId);
                else
                    client = clients.Single(x => x.ConnectionId == connectionId && x.ChannelId == channelId);
                if (client != null)
                    clients.Remove(client);
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
        public void Send(string name, string message)
        {
            Send(null, name, message);
        }

        public void Talk(string name, string message)
        {
            Send(null, name, message);
        }
        private void Send(string channel, string name, string message)
        {
            if (string.IsNullOrEmpty(channel))
            {
                Clients.All.receive(name, message);
            }
            else
            {
                foreach (var client in clients.Where(x => x.ChannelId == channel.ToLower()))
                {
                    try
                    {
                        Clients.Client(client.ConnectionId).receive(name, message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public static void SendAllContext(string name, string message)
        {
            SendContext(null, name, message);
        }
        public static void SendChannelContext(string channel, string name, string message)
        {
            SendContext(channel, name, message);
        }

        private static void SendContext(string channel, string name, string message)
        {
            if (string.IsNullOrEmpty(channel))
            {
                hubContext.Clients.All.receive(name, message);
            }
            else
            {
                foreach (var client in clients.Where(x => x.ChannelId == channel.ToLower()))
                {
                    try
                    {
                        hubContext.Clients.Client(client.ConnectionId).receive(name, message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        #endregion public function
    }
}