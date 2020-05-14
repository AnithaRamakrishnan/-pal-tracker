using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PalTrackerTests
{
    [Collection("Integration")]
    public class WelcomeIntegrationTest
    {
        private HttpClient _testClient;

        public WelcomeIntegrationTest()
        {
            Environment.SetEnvironmentVariable("WELCOME_MESSAGE", "hello from integration test");
        }

        [Fact]
        public async Task ReturnsMessage()
        {
            _testClient = await IntegrationTestServer.GetHttpClient();
            var response = await _testClient.GetAsync("/");
            response.EnsureSuccessStatusCode();

            var expectedResponse = "hello from integration test";
            var actualResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal(expectedResponse, actualResponse);
        }
    }
}