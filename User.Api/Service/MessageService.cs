using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Api.Helpers;
using User.Api.Message;
using User.Api.Models;

namespace User.Api.Service
{
    public static class MessageService
    {
        //private readonly UserStaticContext _db;

        //public MessageService()
        //{
        //    _db = new UserStaticContext();
        //}

        private static UserMessage _iUMsg = new UserMessage(FacialService.Configuration);
        public static async void SendNewIdMessage(UserFood user)
        {
            var msg = new Microsoft.Azure.ServiceBus.Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = Encoding.ASCII.GetBytes("{\"AreaRestrictionsSet\": false, \"UserId\":" + user.codUserFood + "}")
            };

            _iUMsg.SendMessagesAsync(msg);
        }

        public static async void SendPersistedIdMessage(UserFood user)
        {
            var msg = new Microsoft.Azure.ServiceBus.Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(user.ToServiceModel()))
            };

            _iUMsg.SendMessagesAsync(msg);
        }

        /// <summary>
        /// Envia a mensagem com status de processing:true
        /// </summary>
        public static async void SendProcessingMessage(string processingGuid)
        {
            var msg = new Microsoft.Azure.ServiceBus.Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = Encoding.ASCII.GetBytes("{\"Processing\":true,\"UserId\":}")
            };

            _iUMsg.SendMessagesAsync(msg);
        }
    }
}
