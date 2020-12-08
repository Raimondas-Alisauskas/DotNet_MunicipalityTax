namespace MunicipalityTax.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TaxSchedule : BaseEntity
    {
        [Required]
        public ScheduleType ScheduleType { get; set; }

        [Required]
        public DateTime TaxStartDate { get; set; }

        [Required]
        public decimal Tax { get; set; }

        [Required]
        public Guid MunicipalityId { get; set; }

        [ForeignKey("MunicipalityId")]
        public Municipality Municipality { get; set; }
    }
}
