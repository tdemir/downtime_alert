using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebUI.Health
{
    public class HealthCheckApi : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            //TODO: Implement your own healthcheck logic here
            var isHealthy = true;
            if (isHealthy)
            {
                var dict = new Dictionary<string, object>();
                dict.Add("Version", "v1");
                return Task.FromResult(HealthCheckResult.Healthy("Healthy API", data: dict));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("Unhealthy API"));
        }

    }
}
