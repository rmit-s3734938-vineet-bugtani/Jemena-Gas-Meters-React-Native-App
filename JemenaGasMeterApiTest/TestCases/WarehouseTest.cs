using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using JemenaGasMeter.ApiIntegrationTest;

namespace JemenaGasMeter.ApiIntegrationTest.TestCases
{
    public class WarehouseTest
    {
        private TestDriver _driver;

        public WarehouseTest()
        {
            _driver = new TestDriver();
        }

        // To Test: get all warehouses
        [Theory]
        [InlineData("GET")]
        public async Task WarehouseAllTestAsync(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/warehouses");
        
            // Act
            var response = await _driver.Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // To Test: get warehouses by ID
        [Theory]
        [InlineData("GET")]
        public async Task WarehouseByIdTestAsync(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/warehouses/W0001");

            // Act
            var response = await _driver.Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
