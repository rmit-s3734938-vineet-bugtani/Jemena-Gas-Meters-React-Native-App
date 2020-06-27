using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using JemenaGasMeter.ApiIntegrationTest;

namespace JemenaGasMeter.ApiIntegrationTest.TestCases
{
    public class DepotTest
    {
        private TestDriver _driver;

        public DepotTest()
        {
            _driver = new TestDriver();
        }

        // To Test: get all depots
        [Theory]
        [InlineData("GET")]
        public async Task DepotAllTestAsync(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/depots");
        
            // Act
            var response = await _driver.Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // To Test: get depots by ID
        [Theory]
        [InlineData("GET")]
        public async Task DepotByIdTestAsync(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/depots/D0001");

            // Act
            var response = await _driver.Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
