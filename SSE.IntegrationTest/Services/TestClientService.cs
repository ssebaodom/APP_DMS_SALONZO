using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;

namespace SSE.IntegrationTest.Services
{
    public class TestClientService : IDisposable
    {
        public HttpClient Client { get; private set; }

        public TestClientService()
        {
            TestServer server = TestServerService.Instance.Server;
            Client = server.CreateClient();
            Client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml+json");
        }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}