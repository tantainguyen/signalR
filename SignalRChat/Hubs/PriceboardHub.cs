using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using SignalRChat.Models;
using System.Web.Mvc;

namespace SignalRChat.Hubs
{
    [HubName("priceboard")]
    public class PriceboardHub : Hub
    {
        #region variable
        static List<ClientModel> clients = new List<ClientModel>();
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
            Clients.All.receive(message);
        }
        public void SendChannel(string channel, string message)
        {
            foreach (var client in clients.Where(x => x.ChannelId == channel.ToLower()))
            {
                Clients.Client(client.ConnectionId).receive(message);
            }
        }

        //public void SendMessage(JsonResult message)
        //{
        //    Clients.All.receive(message);
        //}
        //public void SendMessageChannel(string channelId, JsonResult message)
        //{
        //    BaseMessage msg = GetResultMessage();
        //    foreach (var client in clients.Where(x => x.ChannelId == channelId))
        //    {
        //        Clients.Client(client.ConnectionId).receive(msg);
        //    }
        //}

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