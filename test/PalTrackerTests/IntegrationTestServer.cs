using Microsoft.AspNetCore.TestHost;
using PalTracker;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Threading.Tasks;

namespace PalTrackerTests
{
    public static class IntegrationTestServer
    {
        //public static TestServer Start() =>
        //    new TestServer(Program.CreateHostBuilder(GetHttpClient()));

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
              });


            // build and start the IHost
            var host = await hostBuilder.StartAsync();

            return host.GetTestClient();
        }
    }
}