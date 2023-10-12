using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DevelopersHub.RealtimeNetworking.Server
{
    public static class Retry
    {

        public static T Do<T>(Func<T> action, TimeSpan retryInterval, int maxAttemptCount, bool throwExceptionOnAttemptsFailed = true)
        {
            var exceptions = new List<Exception>();
            for (int attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                try
                {
                    if (attempted > 0) { Thread.Sleep(retryInterval); }
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (throwExceptionOnAttemptsFailed)
            {
                throw new AggregateException(exceptions);
            }
            else
            {
                return default;
            }
        }

    }
}