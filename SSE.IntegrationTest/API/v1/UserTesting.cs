using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SSE.Common.Api.v1.Requests.User;
using SSE.IntegrationTest.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;

namespace SSE.IntegrationTest.API.v1
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class UserTesting
    {
        private readonly HttpClient _httpClient;
        private string token = string.Empty;
        private readonly string BASE_URL = "/api/v" + (int)API_VERSION.v1 + "/users/";
        private readonly string SIGNIN_URL = "signin";
        private readonly string CONFIG_URL = "config";
        private readonly string COMPANY_URL = "companies";
        private readonly string UNIT_URL = "units";
        private readonly string STORE_URL = "stores";
        private readonly string LANG_CHANGE_URL = "lang";
        private readonly string STORE_CACHE_URL = "stores/cached";

        public UserTesting()
        {
            TokenService tokenService = TokenService.Instance;
            token = tokenService.GetToken();

            _httpClient = new TestClientService().Client;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        [Theory, Priority(0)]
        [InlineData("00", "DUNGDD", "123@123aBcd")]
        public async Task SigninTesting(string hostId, string userName, string passWord)
        {
            Console.WriteLine("First");
            // Arrange
            string url = string.Concat(BASE_URL, SIGNIN_URL);

            SigninRequest signinRequest = new SigninRequest
            {
                HostId = hostId,
                UserName = userName,
                Password = passWord
            };

            string json = JsonConvert.SerializeObject(signinRequest);

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = httpContent;
            var response = await _httpClient.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);

            JObject respObject = JObject.Parse(content);
            Assert.Equal(StatusCodes.Status200OK, respObject.GetValue("statusCode"));
        }

        [Theory(Skip = "ConfigTesting"), Priority(2)]
        [InlineData("00")]
        public async Task ConfigTesting(string companyId)
        {
            // Arrange
            string url = string.Concat(BASE_URL, CONFIG_URL);

            ConfigRequest configRequest = new ConfigRequest
            {
                CompanyId = companyId
            };

            string json = JsonConvert.SerializeObject(configRequest);

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = httpContent;
            var response = await _httpClient.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);

            JObject respObject = JObject.Parse(content);
            Assert.Equal(StatusCodes.Status200OK, respObject.GetValue("statusCode"));
        }

        [Fact, Priority(1)]
        public async Task GetCompaniesTesting()
        {
            Console.WriteLine("Second");

            // Arrange
            string url = string.Concat(BASE_URL, COMPANY_URL);

            // Act
            var response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);

            JObject respObject = JObject.Parse(content);
            Assert.Equal(StatusCodes.Status200OK, respObject.GetValue("statusCode"));
        }

        [Fact, Priority(3)]
        public async Task GetUnitsTesting()
        {
            // Arrange
            string url = string.Concat(BASE_URL, UNIT_URL);

            // Act
            var response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);

            JObject respObject = JObject.Parse(content);
            Assert.Equal(StatusCodes.Status200OK, respObject.GetValue("statusCode"));
        }

        [Theory, Priority(4)]
        [InlineData("CTY")]
        public async Task GeStoresTesting(string unitId)
        {
            // Arrange
            string url = string.Concat(BASE_URL, STORE_URL, "?unitId=", unitId);

            // Act
            var response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);

            JObject respObject = JObject.Parse(content);
            Assert.Equal(StatusCodes.Status200OK, respObject.GetValue("statusCode"));
        }

        [Theory, Priority(6)]
        [InlineData("e")]
        [InlineData("v")]
        public async Task UpdateLangTesting(string lang)
        {
            // Arrange
            string url = string.Concat(BASE_URL, LANG_CHANGE_URL);

            UpdateLangRequest configRequest = new UpdateLangRequest
            {
                Lang = lang
            };

            string json = JsonConvert.SerializeObject(configRequest);

            // Act
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            request.Content = httpContent;
            var response = await _httpClient.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);

            JObject respObject = JObject.Parse(content);
            Assert.Equal(StatusCodes.Status200OK, respObject.GetValue("statusCode"));
        }

        [Theory, Priority(5)]
        [InlineData("CH04")]
        public async Task SetCacheStoreTesting(string storeId)
        {
            // Arrange
            string url = string.Concat(BASE_URL, STORE_CACHE_URL, "?storeId=", storeId);

            // Act
            var response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);

            JObject respObject = JObject.Parse(content);
            Assert.Equal(StatusCodes.Status200OK, respObject.GetValue("statusCode"));
        }
    }
}