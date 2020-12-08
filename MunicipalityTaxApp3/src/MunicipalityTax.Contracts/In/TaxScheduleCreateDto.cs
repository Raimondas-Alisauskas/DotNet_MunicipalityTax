namespace MunicipalityTax.Contracts.In
{
    using System;
    using MunicipalityTax.Domain.Entities;

    public class TaxScheduleCreateDto : IDto
    {
        public ScheduleType ScheduleType { get; set; }

        public DateTime TaxStartDate { get; set; }

        public decimal Tax { get; set; }

        public Guid MunicipalityId { get; set; }
    }
}
