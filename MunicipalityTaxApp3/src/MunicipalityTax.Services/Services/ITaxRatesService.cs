namespace MunicipalityTax.Services.Services
{
    using System;
    using System.Collections.Generic;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Contracts.Out;

    public interface ITaxRatesService
    {
        IEnumerable<TaxRateDto> ReadMunicipalTaxRatesAtGivenDay(Guid municipalityId, TaxRateRequest request);
    }
}