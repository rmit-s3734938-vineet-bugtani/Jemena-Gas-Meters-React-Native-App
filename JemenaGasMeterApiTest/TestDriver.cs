using System.Net.Http;
using JemenaGasMeter.WebApi;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using System;

namespace JemenaGasMeter.ApiIntegrationTest
{
    public class TestDriver
    {
       // private HttpClient _client;
        public HttpClient Client { get; }
        public TestDriver()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            var server = new TestServer(
                new WebHostBuilder().UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((builderContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })

             .UseStartup<Startup>()
                );
            //_client = server.CreateClient();
            Client = server.CreateClient();
        }

    }
}
