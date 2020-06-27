using Xunit;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using JemenaGasMeter.ApiIntegrationTest;

namespace JemenaGasMeter.ApiIntegrationTest.TestCases
{
    public class UserTest
    {
        private TestDriver _driver;

        public UserTest()
        {
            _driver = new TestDriver();
        }

        // To Test: get all users
        [Theory]
        [InlineData("GET")]
        public async Task UserAllTestAsync(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/users");
        
            // Act
            var response = await _driver.Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // To Test: get users by ID
        [Theory]
        [InlineData("GET")]
        public async Task UserByIdTestAsync(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/users/1001");

            // Act
            var response = await _driver.Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // To Test: Is user exist by pssing payroll ID and PIN
        [Theory]
        [InlineData("GET")]
        public async Task UserExistTestAsync(string method)
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod(method), "/api/users/1/1234");

            // Act
            var response = await _driver.Client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
