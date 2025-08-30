using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using SSE_Server;

namespace SSE.IntegrationTest.Services
{
    public sealed class TestServerService
    {
        private static TestServerService serverService = null;
        public TestServer Server { get; private set; }

        public static TestServerService Instance
        {
            get
            {
                if (serverService == null)
                {
                    serverService = new TestServerService();
                }
                return serverService;
            }
        }

        private TestServerService()
        {
            var _builder = new WebHostBuilder()
                    .UseStartup<Startup>()
                    .UseEnvironment("Development")
                    .ConfigureAppConfiguration(builder =>
                    {
                        builder.AddJsonFile("appsettings.json");
                    });

            Server = new TestServer(_builder);
        }
    }
}