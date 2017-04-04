using System;
using System.Collections.Generic;
using System.Linq;
namespace SignalRService
{
    public class SystemManager
    {

        public static BaseMessage CreateMessage(object data)
        {
            int code = (int)MessageCode.Success;
            string message = MessageName.Success;
            return createMessage(code, message, data);
        }

        public static BaseMessage CreateMessage(int code, string message, object data)
        {
            return createMessage(code, message, data);
        }
        private static BaseMessage createMessage(int code, string message, object data)
        {
            return new BaseMessage
            {
                Code = code,
                Message = message,
                Data = data
            };
        }
    }

    public class BaseMessage
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public enum MessageCode
    {
        Success = 1,
        Info = 2,
        Warning = 3,
        Error = 9
    }

    static class MessageName
    {
        public const string Success = "data success";
        public const string Info = "data info";
        public const string Warning = "warning";
        public const string Error = "System error";
    }

    public class BasePriceboardContent
    {
        public int Type { get; set; }
        public object Data { get; set; }
    }

    public enum PriceboardMessageType
    {
        StockInfo = 1,
        ExchangeInfo = 2
    }
}