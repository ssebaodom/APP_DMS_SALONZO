using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SSE.Common.Api.v1.Requests.Home;
using SSE.IntegrationTest.Services;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SSE.IntegrationTest.API.v1
{
    public class HomeTesting
    {
        private readonly HttpClient _httpClient;
        private string token = string.Empty;
        private readonly string BASE_URL = "/api/v" + (int)API_VERSION.v1 + "/home/";
        private readonly string FILTER_TIME_URL = "filter-times";
        private readonly string REPORT_CATEGORIES_URL = "reports";
        private readonly string REPORT_DATA_URL = "reports";

        public HomeTesting()
        {
            TokenService tokenService = TokenService.Instance;
            token = tokenService.GetToken();

            _httpClient = new TestClientService().Client;
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }

        [Fact]
        public async Task GetFilterTimeTesting()
        {
            // Arrange
            string url = string.Concat(BASE_URL, FILTER_TIME_URL);

            // Act
            var response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);

            JObject respObject = JObject.Parse(content);
            Assert.Equal(StatusCodes.Status200OK, respObject.GetValue("statusCode"));
        }

        [Fact]
        public async Task GetReportCategoriesTesting()
        {
            // Arrange
            string url = string.Concat(BASE_URL, REPORT_CATEGORIES_URL);

            // Act
            var response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);

            JObject respObject = JObject.Parse(content);
            Assert.Equal(StatusCodes.Status200OK, respObject.GetValue("statusCode"));
        }

        [Fact]
        public async Task GetDefaultDataTesting()
        {
            // Arrange
            string url = string.Concat(BASE_URL, REPORT_DATA_URL);

            // Act
            var response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(content);

            JObject respObject = JObject.Parse(content);
            Assert.Equal(StatusCodes.Status200OK, respObject.GetValue("statusCode"));
        }

        [Theory]
        [InlineData("C002", "013", "CTY")]
        public async Task GetReportDataTesting(string reportId, string timeId, string unitId)
        {
            // Arrange
            string url = string.Concat(BASE_URL, REPORT_DATA_URL);

            //Discount getReportDataRequest = new Discount
            //{
            //    report_id = reportId,
            //    time_id = timeId,
            //    unit_id = unitId
            //};

            //string json = JsonConvert.SerializeObject(getReportDataRequest);

            // Act
            //var request = new HttpRequestMessage(HttpMethod.Post, url);
            //HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            //request.Content = httpContent;
            //var response = await _httpClient.SendAsync(request);
            //string content = await response.Content.ReadAsStringAsync();

            //// Assert
            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Assert.NotNull(content);

            //JObject respObject = JObject.Parse(content);
            //Assert.Equal(StatusCodes.Status200OK, respObject.GetValue("statusCode"));
        }
    }
}