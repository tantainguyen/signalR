using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRAPIService.Hubs
{
    public class NewsHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}