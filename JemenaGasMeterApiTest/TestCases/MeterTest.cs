using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using JemenaGasMeter.ApiIntegrationTest;
using JemenaGasMeter.WebApi.DbModels;
using System;

namespace JemenaGasMeter.ApiIntegrationTest.TestCases
{
    public class MeterTest
    {
        private TestDriver _driver;
        private Meter _mdriver;

        public MeterTest()
        {
            _driver = new TestDriver();

            var Meter = new Meter();
            Meter.MIRN = "IF0001";
            Meter.MeterType = MeterType.Domestic;
            Meter.MeterStatus = MeterStatus.Inhouse;
            Meter.MeterCondition = MeterCondition.Active;
            Meter.ExpriyDate = DateTime.UtcNow.AddDays(5);
        }

        // To Test: get all meters
        [Theory]
        [InlineData("GET")]
        public async Task MeterAllTestAsync(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/meters");
        
            // Act
            var response = await _driver.Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // To Test: get meters by ID
        [Theory]
        [InlineData("GET")]
        public async Task MeterByIdTestAsync(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/meters/IF0001");

            // Act
            var response = await _driver.Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // To Test: get meters by ID
        // [Theory]
        // [InlineData("GET")]
        // public async Task MeterIsAvailableTestAsync(string method)
        // {
        //     // Arrange
        //     var request = new HttpRequestMessage(new HttpMethod(method), "/api/meters/status/IF0001");

        //     // Act
        //     var response = await _driver.Client.SendAsync(request);

        //     // Assert
        //     var responseCode = response.EnsureSuccessStatusCode();
        //     if(responseCode.StatusCode.Equals(HttpStatusCode.OK))
        //     {
        //         Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //     }
        // }
    }
}
