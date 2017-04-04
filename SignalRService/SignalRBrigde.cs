using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRService
{
    public class SignalRBrigde
    {
        public void InitHub(string url)
        {
            using (WebApp.Start(url))
            {
                //connect hubs
                ConnectHub();
            }
        }

        private void ConnectHub()
        {
            PriceboardHub.hubContext = GlobalHost.ConnectionManager.GetHubContext<PriceboardHub>();
            ChatHub.hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            //Thread priceboardThread = new Thread(new ThreadStart(PriceboardMessage));
            //// Start the thread
            //priceboardThread.Start();

            //Thread chatThread = new Thread(new ThreadStart(this.ChatMessage));
            //// Start the thread
            //chatThread.Start();

            while (true)
            {
                PriceboardMessage();
                ChatMessage();

                Thread.Sleep(new Random().Next(1, 15) * 1000);
            }
        }

        private void PriceboardMessage()
        {
            //PriceboardHub.hubContext = GlobalHost.ConnectionManager.GetHubContext<PriceboardHub>();            
            string message = "", message2 = "", channel = "",
                    template = "Message send from service at time {0}",
                    template2 = "{0}: status change at time {1}";
            DateTime dt;
            //while (true)
            //{
            dt = DateTime.Now;

            try
            {
                message = string.Format(template, dt.ToString("hh:mm:ss"));
                PriceboardHub.SendAllContext(message);

                if (dt.Millisecond % 2 == 0)
                    channel = "HNX";
                else
                    channel = "HSX";
                message2 = string.Format(template2, channel, dt.ToString("hh:mm:ss"));
                PriceboardHub.SendChannelContext(channel, message2);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Thread.Sleep(new Random().Next(1, 10) * 1000);
            //}
        }

        private void ChatMessage()
        {
            //ChatHub.hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            string message = "",
                    template = "Chat Message send from service at time {0}";
            DateTime dt;
            //while (true)
            //{
            dt = DateTime.Now;

            try
            {
                message = string.Format(template, dt.ToString("hh:mm:ss"));
                ChatHub.SendAllContext("tainguyen", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //Thread.Sleep(new Random().Next(1, 10) * 1000);
            //}
        }
    }
}
