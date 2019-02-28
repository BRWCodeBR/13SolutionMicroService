using System;
using System.Net.Http;
using Polly;
using Polly.Registry;

namespace User.Api.Polly
{
    public static class PolyRegistryExtensions
    {
        public static IPolicyRegistry<string> AddBasicRetryPolicy(this IPolicyRegistry<string> policyRegistry)
        {
            var retryPolicy = Policy
                .Handle<Exception>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryCount => TimeSpan.FromSeconds(Math.Pow(2, retryCount)), (result, timeSpan, retryCount, context) =>
                {
                    if (!context.TryGetLogger(out var logger)) return;

                    context.TryGetValue("url", out var url);

                })
                .WithPolicyKey(PolicyNames.BasicRetry);

            policyRegistry.Add(PolicyNames.BasicRetry, retryPolicy);

            return policyRegistry;
        }
    }
}