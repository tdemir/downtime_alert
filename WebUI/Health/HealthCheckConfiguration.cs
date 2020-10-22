using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebUI.Health
{
    public class HealthCheckConfiguration
    {
        public static void ConfigureHealthChecks(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                //that's to the method you created 
                ResponseWriter = WriteHealthCheckResponse
            });
        }

        public static void RegisterHealthCheckSettings(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<HealthCheckApi>("Api");


            services.AddScoped<HealthCheckApi>();
        }

        private static Task WriteHealthCheckResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application / json";


            var item = new
            {
                Status = result.Status.ToString(),
                TotalDurationInMilliseconds = result.TotalDuration.TotalMilliseconds.ToString(),
                Results = result.Entries.Select(pair => new Dictionary<string, object>() {
                    {pair.Key, new
                    {
                        Status = pair.Value.Status.ToString(),
                        Description = pair.Value.Description,
                        DurationInMilliseconds = pair.Value.Duration.TotalMilliseconds.ToString(),
                        Data = pair.Value.Data.Select(p => new Dictionary<string,object>(){
                            {p.Key, p.Value }
                        })
                    } }
                })
            };


            return httpContext.Response.WriteAsync(item.Serialize());
        }
    }
}
