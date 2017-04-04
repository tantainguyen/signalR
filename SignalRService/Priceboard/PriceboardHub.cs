using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace SignalRService
{
    [HubName("priceboard")]
    public class PriceboardHub : Hub
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
                channelId = "vst-pb"; //default channel

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
        public void SendAll(string message)
        {
            Send(null, message);
        }
        public void SendChannel(string channel, string message)
        {
            Send(channel, message);
        }

        private void Send(string channel, string message)
        {
            if (string.IsNullOrEmpty(channel))
            {
                Clients.All.receive(message);
            }
            else
            {
                foreach (var client in clients.Where(x => x.ChannelId == channel.ToLower()))
                {
                    try
                    {
                        Clients.Client(client.ConnectionId).receive(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public static void SendAllContext(string message)
        {
            SendContext(null, message);
        }
        public static void SendChannelContext(string channel, string message)
        {
            SendContext(channel, message);
        }

        private static void SendContext(string channel, string message)
        {
            if (string.IsNullOrEmpty(channel))
            {
                hubContext.Clients.All.receive(message);
            }
            else
            {
                foreach (var client in clients.Where(x => x.ChannelId == channel.ToLower()))
                {
                    try
                    {
                        hubContext.Clients.Client(client.ConnectionId).receive(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        #endregion public function

        #region private function

        private BaseMessage GetResultMessage()
        {
            List<BasePriceboardContent> list = new List<BasePriceboardContent>();
            list.Add(new BasePriceboardContent { Type = 1, Data = "ACB new info" });
            list.Add(new BasePriceboardContent { Type = 1, Data = "VCB new info" });
            list.Add(new BasePriceboardContent { Type = 2, Data = "Update HSX status" });

            return SystemManager.CreateMessage(list);
        }

        #endregion private function
    }
}