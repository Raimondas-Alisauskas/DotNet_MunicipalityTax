namespace MunicipalityTax.Services.Services
{
    using System.Collections.Generic;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Domain.Entities;

    public interface IFileService : IBaseService<TaxScheduleDto, TaxScheduleCreateDto, TaxSchedule>
    {
        public IEnumerable<TaxScheduleCreateDto> GetTaxScheduleCreateDtos(byte[] file);
    }
}
