using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace User.Api.Message
{

    public class UserMessage : IUserMessage
    {
        IConfiguration _configuration;
        private Task _lastTask;
        private List<Microsoft.Azure.ServiceBus.Message> _messages;

        public UserMessage(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Utilizar esse SendMessageAsync
        /// </summary>
        /// <param name="message"></param>
        public async void SendMessagesAsync(Microsoft.Azure.ServiceBus.Message message)
        {
            var connectionString = _configuration["serviceBus:connectionString"];
            var queueClient = new QueueClient(connectionString, "UserChanged");
            int tries = 0;
            
            while (true)
            {
                if ((tries > 10))
                    break;
                
                await queueClient.SendAsync(message);
            }
            await queueClient.CloseAsync();
        }

        /// <summary>
        /// Metodo para testar
        /// </summary>
        public async void SendMessagesAsync()
        {
            if (_lastTask != null && !_lastTask.IsCompleted)
                return;

            var connectionString = _configuration["connectionStrings:connectionString"];

            var queueClient = new QueueClient(connectionString, "UserChanged");

            _lastTask = SendAsync(queueClient);

            await _lastTask;

            var closeTask = queueClient.CloseAsync();

            await closeTask;

            HandleException(closeTask);
        }

        private async Task SendAsync(QueueClient queueClient)
        {
            int tries = 0;
            Microsoft.Azure.ServiceBus.Message message;
            while (true)
            {
                if (_messages.Count <= 0)
                    break;
                lock (_messages)
                {
                    message = _messages.FirstOrDefault();
                }
                var sendTask = queueClient.SendAsync(message);
                await sendTask;

                var success = HandleException(sendTask);

                if (!success)
                    Thread.Sleep(10000 * (tries < 60 ? tries++ : tries));
                else
                    _messages.Remove(message);
            }
        }

        private bool HandleException(Task closeTask)
        {
            try
            {
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}