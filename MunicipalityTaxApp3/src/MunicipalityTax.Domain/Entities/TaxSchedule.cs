namespace MunicipalityTax.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TaxSchedule : BaseEntity
    {
        [Required]
        public ScheduleType ScheduleType { get; set; }

        [Required]
        public DateTime TaxStartDate { get; set; }

        [Required]
        public DateTime TaxEndDate { get; set; }

        [Required]
        public decimal Tax { get; set; }

        [Required]
        public Guid MunicipalityId { get; set; }
    }
}
