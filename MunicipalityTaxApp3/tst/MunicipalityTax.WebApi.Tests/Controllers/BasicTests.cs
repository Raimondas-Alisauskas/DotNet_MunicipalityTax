namespace MunicipalityTax.WebApi.Tests
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    public class BasicTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public BasicTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Theory]
        [InlineData("/", "text/html; charset=utf-8")]
        [InlineData("/swagger", "text/html; charset=utf-8")]
        [InlineData("/api/v1/Municipalities", "application/json; charset=utf-8")]
        [InlineData("/api/v1/Municipalities/7ebced2b-e2f9-45e0-bf75-111111111100/TaxSchedules", "application/json; charset=utf-8")]
        [InlineData("/api/v1/TaxRates?MunicipalityName=TestMunicipality&Date=2016-01-01", "application/json; charset=utf-8")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url, string contentType)
        {
            // Arrange
            var client = this.factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(contentType,  response.Content.Headers.ContentType.ToString());
        }
    }
}
