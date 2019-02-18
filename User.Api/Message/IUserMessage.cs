using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Message
{
    public interface IUserMessage
    {
        void SendMessagesAsync();

        void SendMessagesAsync(Microsoft.Azure.ServiceBus.Message message);

    }
}
