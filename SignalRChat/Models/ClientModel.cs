using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRChat.Models
{
    public class ClientModel
    {
        #region Properties        
        public string ChannelId { get; set; }
        public string ConnectionId { get; set; }
        #endregion Properties
    }
}