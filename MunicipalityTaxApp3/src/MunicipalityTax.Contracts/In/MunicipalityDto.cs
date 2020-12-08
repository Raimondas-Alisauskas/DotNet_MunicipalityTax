namespace MunicipalityTax.Contracts.In
{
    using System;
    using System.ComponentModel;

    // todo: Add validation tests
    public class MunicipalityDto : MunicipalityCreateDto
    {
        [DefaultValue("7ebced2b-e2f9-45e0-bf75-111111111100")]
        public Guid Id { get; set; }
    }
}
