namespace MunicipalityTax.Contracts.In
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class TaxRateRequest // todo: Add validation tests
    {
        [Required]
        [DefaultValue("TestMunicipality")]
        public string MunicipalityName { get; set; }

        [Required]
        [DefaultValue("2016")]
        public int Year { get; set; }

        [Required]
        [DefaultValue("8")]
        public int Month { get; set; }

        [Required]
        [DefaultValue("1")]
        public int Day { get; set; }
    }
}
