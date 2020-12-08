namespace MunicipalityTax.Services.Services
{
    using System.Collections.Generic;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Contracts.Out;

    public interface ITaxRatesService
    {
        IEnumerable<TaxRateDto> ReadMunicipalTaxRatesAtGivenDay(TaxRateRequest request);
    }
}