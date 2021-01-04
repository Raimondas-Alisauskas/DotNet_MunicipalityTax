namespace MunicipalityTax.Contracts.In
{
    using System;
    using MunicipalityTax.Domain.Entities;

    public class TaxScheduleCreateFileDto
    {
        public Guid MunicipalityId { get; set; }

        public ScheduleType ScheduleType { get; set; }

        public DateTime TaxStartDate { get; set; }

        public decimal Tax { get; set; }

        public TaxScheduleCreateFileDto(string municipalityId, string scheduleType, string taxStartDate, string tax)
        {
            this.MunicipalityId = Guid.Parse(municipalityId);
            this.ScheduleType = (ScheduleType)Enum.Parse(typeof(ScheduleType), scheduleType, true);
            this.TaxStartDate = DateTime.Parse(taxStartDate);
            this.Tax = decimal.Parse(tax);
        }
    }
}
