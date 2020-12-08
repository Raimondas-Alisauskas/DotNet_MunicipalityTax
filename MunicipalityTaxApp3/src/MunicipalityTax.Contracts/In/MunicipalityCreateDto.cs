namespace MunicipalityTax.Contracts.In
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    // todo: Add validation tests
    public class MunicipalityCreateDto : IDto
    {
        [Required]
        [StringLength(100, ErrorMessage = "Municipality name must be not more than 100 characters length")]
        [DefaultValue("TestMunicipality")]
        public string MunicipalityName { get; set; }
    }
}
