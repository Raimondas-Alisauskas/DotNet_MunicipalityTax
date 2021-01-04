namespace MunicipalityTax.Services.Tests.IntegrTests
{
    using System;
    using System.Linq;
    using AutoMapper;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Domain.Entities;
    using MunicipalityTax.Persistence.Repositories;
    using MunicipalityTax.Services.Mappers;
    using MunicipalityTax.Services.Services;
    using MunicipalityTax.Services.Tests.Helpers;
    using Xunit;

    public class TaxRatesServiceTests : IClassFixture<ShareTestDatabaseFixture>
    {
        private readonly TaxRatesService sService;

        public TaxRatesServiceTests(ShareTestDatabaseFixture fixture)
        {
            var context = fixture.CreateContext();
            var mapper = new MapperConfiguration(
                 c => c.AddProfile<AutomapProfile>())
                 .CreateMapper();

            var repo = new Repository<TaxSchedule>(context);
            this.sService = new TaxRatesService(repo, mapper);
        }

        [Theory]
        [InlineData("2016-01-01", 0.1)]
        [InlineData("2015-12-28", 0.2)]
        [InlineData("2016-01-03", 0.2)]
        [InlineData("2016-01-04", 0.3)]
        [InlineData("2016-03-01", 0.4)]
        public void ReadMunicipalTaxRatesAtGivenDay_UnitTests_ExpectedBehavior1(string date, decimal tax)
        {
            // Arrange
            var municipalityId = Guid.Parse("7ebced2b-e2f9-45e0-bf75-111111111100");
            var request = new TaxRateRequest
            {
                Date = DateTime.Parse(date),
            };

            // Act
            var expected = tax;
            var result = this.sService.ReadMunicipalTaxRatesAtGivenDay(municipalityId, request).FirstOrDefault().Tax;

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("7ebced2b-e2f9-45e0-bf75-111111111100", "2015-12-27", true)]
        [InlineData("7ebced2b-e2f9-45e0-bf75-111111111100", "2017-01-01", true)]
        [InlineData("7ebced2b-e2f9-45e0-bf75-111111111101", "2016-01-01", true)]
        public void ReadMunicipalTaxRatesAtGivenDay_UnitTests_EmptyResults(string guid, string date, bool empty)
        {
            // Arrange
            var municipalityId = Guid.Parse(guid);
            var request = new TaxRateRequest
            {
                Date = DateTime.Parse(date),
            };

            // Act
            var expected = empty;
            var result = !this.sService.ReadMunicipalTaxRatesAtGivenDay(municipalityId, request).Any();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
