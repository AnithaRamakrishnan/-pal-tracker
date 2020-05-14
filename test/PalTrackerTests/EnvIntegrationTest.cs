using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PalTrackerTests
{
    [Collection("Integration")]
    public class EnvIntegrationTest
    {
        private HttpClient _testClient;

        public EnvIntegrationTest()
        {
            Environment.SetEnvironmentVariable("PORT", "123");
            Environment.SetEnvironmentVariable("MEMORY_LIMIT", "512M");
            Environment.SetEnvironmentVariable("CF_INSTANCE_INDEX", "1");
            Environment.SetEnvironmentVariable("CF_INSTANCE_ADDR", "127.0.0.1");
        }

        [Fact]
        public async Task ReturnsCloudFoundryEnv()
        {
            _testClient = await IntegrationTestServer.GetHttpClient();
            var response = await _testClient.GetAsync("/env");
            response.EnsureSuccessStatusCode();

            var expectedResponse =
                @"{""port"":""123"",""memoryLimit"":""512M"",""cfInstanceIndex"":""1"",""cfInstanceAddr"":""127.0.0.1""}";
            var actualResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal(expectedResponse, actualResponse);
        }
    }
}