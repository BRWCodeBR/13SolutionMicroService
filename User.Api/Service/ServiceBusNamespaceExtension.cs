using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ServiceBus.Fluent;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.Api.Service
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceBusNamespaceExtension
    {
        /// <summary>
        /// GetServiceBusNamespace
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceBusNamespace GetServiceBusNamespace(this IConfiguration configuration)
        {
            //ref: https://tomasherceg.com/blog/post/azure-servicebus-in-net-core-managing-topics-queues-and-subscriptions-from-the-code

            var config = configuration.GetSection("serviceBus").Get<ServiceBusConfiguration>();

            var credentials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(config.ClientId, config.ClientSecret,
                        config.TenantId, AzureEnvironment.AzureGlobalCloud);

            var serviceBusManager = ServiceBusManager.Authenticate(credentials, config.SubscriptionId);
            return serviceBusManager.Namespaces.GetByResourceGroup(config.ResourceGroup, config.NamespaceName);
        }
    }
}
