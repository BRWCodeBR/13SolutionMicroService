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
        public static async void SendNewIdMessage(string guid)
        {
            var _iUMsg = new UserMessage(FacialService.Configuration);
            var msg = new Microsoft.Azure.ServiceBus.Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = Encoding.ASCII.GetBytes("Novo Cadastro: " + guid.ToString())
            };

            _iUMsg.SendMessagesAsync(msg);
        }

        public static async void SendPersistedIdMessage(string guid)
        {
            var _iUMsg = new UserMessage(FacialService.Configuration);
            var msg = new Microsoft.Azure.ServiceBus.Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = Encoding.ASCII.GetBytes("Cadastro Persistido: " + guid.ToString())
            };

            _iUMsg.SendMessagesAsync(msg);
        }
    }
}
