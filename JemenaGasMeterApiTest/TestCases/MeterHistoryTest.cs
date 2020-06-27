using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using JemenaGasMeter.ApiIntegrationTest;
using System.Text;
using JemenaGasMeter.WebApi.Repository;

namespace JemenaGasMeter.ApiIntegrationTest.TestCases
{
    public class MeterHistoryTest
    {
        private TestDriver _driver;

        public MeterHistoryTest()
        {
            _driver = new TestDriver();
        }

        // To Test: get all meterhistories
        [Theory]
        [InlineData("GET")]
        public async Task MeterHistoryAllTestAsync(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/meterhistories");
        
            // Act
            var response = await _driver.Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // To Test: get meterhistories by ID
        // [Theory]
        // [InlineData("GET")]
        // public async Task MeterHistoryByIdTestAsync(string method)
        // {
        //     // Arrange
        //     var request = new HttpRequestMessage(new HttpMethod(method), "/api/meterhistories/2");

        //     // Act
        //     var response = await _driver.Client.SendAsync(request);

        //     // Assert
        //     response.EnsureSuccessStatusCode();
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        // }

        // To Test: Save the single meter transaction into Meter history table database
        [Theory]
        [InlineData("POST")]
        public async Task MeterPickupBulkTestAsync(string content)
        {
            // Arrange
            var uri = "/api/MeterHistories/pickup";
            content = "{\"mirn\":[\"IF0002\",\"IF0003\"],\"payRollID\":\"1001\",\"meterStatus\":2,\"location\":\"D0001\",\"transactionDate\":\"2020-05-16T10:08:59.377Z\"}";
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _driver.Client.PostAsync(uri, httpContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // To Test: Save the single meter transaction into Meter history table database
        [Theory]
        [InlineData("POST")]
        public async Task MeterReturnSingleTestAsync(string content)
        {
             // Arrange
             var uri = "/api/meterhistories/return";
            content = "{\"MIRN\":[\"IF0004\"],\"PayRollID\":\"1001\",\"MeterStatus\":3,\"meterCondition\":[1],\"Location\":\"W0003\",\"TransactionDate\":\"2020-05-15T15:04:48.395Z\"}";
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _driver.Client.PostAsync(uri, httpContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // To Test: Save the single meter transaction into Meter history table database
        [Theory]
        [InlineData("POST")]
        public async Task MeterReturnBulkTestAsync(string content)
        {
            // Arrange
            var uri = "/api/meterhistories/return";
            content = "{\"MIRN\":[\"IF0005\",\"IF0006\"],\"PayRollID\":\"1001\",\"MeterStatus\":3,\"meterCondition\":[1,3],\"Location\":\"D0001\",\"TransactionDate\":\"2020-05-15T15:04:48.395Z\"}";
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");

            // Act
            var response = await _driver.Client.PostAsync(uri, httpContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
