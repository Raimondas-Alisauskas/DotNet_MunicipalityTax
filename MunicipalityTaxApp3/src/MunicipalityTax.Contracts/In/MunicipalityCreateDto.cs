namespace MunicipalityTax.Contracts.In
{
    using System.ComponentModel.DataAnnotations;

    // todo: Add validation tests
    public class MunicipalityCreateDto : IDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Municipality name must be not more than 100 characters length")]
        public string MunicipalityName { get; set; }
    }
}
