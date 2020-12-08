namespace MunicipalityTax.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Domain.Entities;

    public interface ITaxScheduleService : IBaseService<TaxScheduleDto, TaxScheduleCreateDto, TaxSchedule>
    {
        bool IsMunicipalityExist(Guid municipalityId);

        IEnumerable<TaxScheduleDto> ReadAllByMunicipalityId(Guid municipalityId);

        IEnumerable<ExpandoObject> ReadWithParameters(TaxScheduleRequest request);
    }
}