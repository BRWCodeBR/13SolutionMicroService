using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Api.Message;

namespace User.Api.Service
{
    public static class MessageService
    {
        private static UserMessage _iUMsg = new UserMessage(FacialService.Configuration);
        public static async void SendNewIdMessage(string faceGuid, string codUser)
        {            
            var msg = new Microsoft.Azure.ServiceBus.Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = Encoding.ASCII.GetBytes("{\"AreaRestrictionsSet\": false, \"UserId\":" + codUser + "}")
            };

            _iUMsg.SendMessagesAsync(msg);
        }

        public static async void SendPersistedIdMessage(string guid)
        {            
            var msg = new Microsoft.Azure.ServiceBus.Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = Encoding.ASCII.GetBytes("Cadastro Persistido: " + guid.ToString())
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
