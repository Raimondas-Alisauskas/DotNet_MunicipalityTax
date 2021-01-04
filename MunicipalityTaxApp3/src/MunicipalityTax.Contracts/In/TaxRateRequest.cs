namespace MunicipalityTax.Contracts.In
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class TaxRateRequest // todo: Add validation tests
    {
        [Required]
        [DefaultValue("TestMunicipality")]
        public string MunicipalityName { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [DefaultValue("2016-01-01")]
        public DateTime Date { get; set; }
    }
}
