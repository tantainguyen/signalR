using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRAPIService.Core
{
    public class MsgResult
    {
        public string code { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}