namespace MunicipalityTax.Contracts.In
{
    using System;

    public class TaxScheduleDto : TaxScheduleCreateDto
    {
        public Guid Id { get; set; }
    }
}
