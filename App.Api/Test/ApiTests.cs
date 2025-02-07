using Xunit;

namespace App.Api.Test
{
    public class ApiTests
    {
        private readonly HttpClient _client = new HttpClient();

        [Fact]
        public async Task SendMultipleRequestsAsync()
        {
            var baseUrl = "https://localhost:7264/api/Products"; // API URL
            var response = await _client.GetAsync(baseUrl);
            var content = await response.Content.ReadAsStringAsync(); // JSON içeriği al

            Console.WriteLine($"Status Code: {response.StatusCode}");
            Console.WriteLine($"Response Body: {content}");

            Assert.True(response.IsSuccessStatusCode, "API response failed!");
        }
    }
}
