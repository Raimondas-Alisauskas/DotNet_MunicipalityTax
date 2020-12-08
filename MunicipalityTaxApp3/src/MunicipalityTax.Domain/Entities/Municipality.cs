namespace MunicipalityTax.Domain.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Municipality : BaseEntity
    {
        [Required]
        public string MunicipalityName { get; set; }

        public ICollection<TaxSchedule> TaxSchedules { get; set; }
            = new HashSet<TaxSchedule>();
    }
}
