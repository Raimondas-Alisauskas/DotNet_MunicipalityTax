namespace MunicipalityTax.Services.Tests.IntegrTests
{
    using System.Linq;
    using AutoMapper;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Services.Mappers;
    using MunicipalityTax.Services.Services;
    using MunicipalityTax.Services.Tests.Helpers;
    using Xunit;

    // todo: add unitests
    public class TaxRatesServiceTests : IClassFixture<ShareTestDatabaseFixture>
    {
        private readonly TaxRatesService sService;

        public TaxRatesServiceTests(ShareTestDatabaseFixture fixture)
        {
            var context = fixture.CreateContext();
            var mapper = new MapperConfiguration(
                 c => c.AddProfile<AutomapProfile>())
                 .CreateMapper();
            this.sService = new TaxRatesService(context, mapper);
        }

        [Theory]
        [InlineData("TestMunicipality", 2016, 1, 1, 0.1)]
        [InlineData("TestMunicipality", 2015, 12, 28, 0.2)]
        [InlineData("TestMunicipality", 2016, 1, 3, 0.2)]
        [InlineData("TestMunicipality", 2016, 1, 4, 0.3)]
        [InlineData("TestMunicipality", 2016, 2, 1, 0.4)]
        public void ReadMunicipalTaxRatesAtGivenDay_UnitTests_ExpectedBehavior1(string municipalityName, int year, int month, int day, decimal tax)
        {
            // Arrange
            var request = new TaxRateRequest
            {
                MunicipalityName = municipalityName,
                Year = year,
                Month = month,
                Day = day,
            };

            // Act
            var expected = tax;
            var result = this.sService.ReadMunicipalTaxRatesAtGivenDay(request).FirstOrDefault().Tax;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("TestMunicipality", 2015, 12, 27, true)]
        [InlineData("TestMunicipality", 2017, 1, 1, true)]
        [InlineData("Test", 2016, 1, 1, true)]
        public void ReadMunicipalTaxRatesAtGivenDay_UnitTests_EmptyResults(string municipalityName, int year, int month, int day, bool empty)
        {
            // Arrange
            var request = new TaxRateRequest
            {
                MunicipalityName = municipalityName,
                Year = year,
                Month = month,
                Day = day,
            };

            // Act
            var expected = empty;
            var result = !this.sService.ReadMunicipalTaxRatesAtGivenDay(request).Any();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
