using Microsoft.Extensions.Logging;
using Polly;

namespace User.Api.Polly
{
    public static class PollyContextExtensions
    {
        public static bool TryGetLogger(this Context context, out ILogger logger)
        {
            if (context.TryGetValue(PolicyContextItems.Logger, out var loggerObject) && loggerObject is ILogger theLogger)
            {
                logger = theLogger;
                return true;
            }

            logger = null;
            return false;
        }
    }
}