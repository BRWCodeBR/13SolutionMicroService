using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using User.Api.ViewModels;

namespace User.Api.Message
{
    public class UserReceive : IUserReceive
    {
        const string QueuePath = "UserChanged";
        static ITopicClient _queueClient;
        private static string _storeId;
        private static List<Task> PendingCompleteTasks;
        private static int count;
        private static IConfiguration _configuration;

        public UserReceive()
        {
            _configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile(Directory.GetCurrentDirectory() + "/appsettings.json")
            .Build(); ;
        }
        

        public async void ReceiveMessages()
        {
            var subscriptionClient = new SubscriptionClient( new ServiceBusConnectionStringBuilder(_configuration["serviceBus:connectionString"].ToString()), "UserChanged");

            //by default a 1=1 rule is added when subscription is created, so we need to remove it
            await subscriptionClient.RemoveRuleAsync("$Default");

            await subscriptionClient.AddRuleAsync(new RuleDescription
            {
                Filter = new CorrelationFilter { Label = _storeId },
                Name = "filter-store"
            });

            var mo = new MessageHandlerOptions(ExceptionHandle) { AutoComplete = true };

            subscriptionClient.RegisterMessageHandler(Handle, mo);

            Console.ReadLine();
        }

        private static Task Handle(Microsoft.Azure.ServiceBus.Message message, CancellationToken arg2)
        {
            Console.WriteLine($"message Label: {message}");
            var productChangesString = Encoding.UTF8.GetString(message.Body);

            Console.WriteLine("Message Received");
            Console.WriteLine(productChangesString);

            //Thread.Sleep(40000);

            return Task.CompletedTask;
        }

        private static Task ExceptionHandle(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Message handler encountered an exception {arg.Exception}.");
            var context = arg.ExceptionReceivedContext;
            Console.WriteLine($"- Endpoint: {context.Endpoint}, Path: {context.EntityPath}, Action: {context.Action}");
            return Task.CompletedTask;
        }


    }
}