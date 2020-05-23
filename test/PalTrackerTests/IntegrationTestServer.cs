using Microsoft.AspNetCore.TestHost;
using PalTracker;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading.Tasks;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging;

namespace PalTrackerTests
{
    public static class IntegrationTestServer
    {
        //public static TestServer Start() =>
        //    new TestServer(Program.CreateHostBuilder(new string[] { }));

        public async static Task<HttpClient> GetHttpClient()
        {

            IHostBuilder hostBuilder = new HostBuilder()
              .ConfigureWebHost(webHost =>
              {
                  webHost.UseTestServer();
                  webHost.UseStartup<PalTracker.Startup>();

              })
              .ConfigureAppConfiguration((hostingContext, config) =>
              {
                  config.AddEnvironmentVariables();
                  config.AddCloudFoundry();
              })
              .ConfigureLogging((hostingContext, loggingBuilder) => 
              {
                  //added logging details similar as program.cs
                  loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                  loggingBuilder.AddDynamicConsole();
              });


            // build and start the IHost
            var host = await hostBuilder.StartAsync();

            return host.GetTestClient();
        }
    }
}