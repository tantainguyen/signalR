using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using System.Threading;

namespace SignalRPriceboardService
{
    public class SignalRManager
    {
        private readonly IHubProxy hubProxy;
        private HubConnection hubConnection;
        public SignalRManager() {
            try
            {
                //hubConnection = new HubConnection("http://localhost:62989/signalr");
                hubConnection = new HubConnection("http://localhost:62989/signalr/hubs");
                hubProxy = hubConnection.CreateHubProxy("priceboard");
            
                hubConnection.Start().Wait();
                ProcessMessage();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }       

        private void ProcessMessage()
        {
            string message = "", message2 = "", channel = "",
                    template = "Message send from service at time {0}",
                    template2 = "{0}: status change at time {1}";
            DateTime dt;
            while (true)
            {
                dt = DateTime.Now;

                try
                {
                    message = string.Format(template, dt.ToString("hh:mm:ss"));
                    hubProxy.Invoke("SendAll", message);

                    if (dt.Millisecond % 5 == 0)
                        channel = "HNX";
                    else
                        channel = "HSX";
                    message2 = string.Format(template2, channel, dt.ToString("hh:mm:ss"));

                    hubProxy.Invoke("SendChannel", channel, message2);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Thread.Sleep(new Random().Next(1, 10) * 1000);
            }
        }
    }
}
