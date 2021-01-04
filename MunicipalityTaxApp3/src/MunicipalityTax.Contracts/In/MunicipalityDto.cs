namespace MunicipalityTax.Contracts.In
{
    using System;

    public class MunicipalityDto : MunicipalityCreateDto
    {
        public Guid Id { get; set; }
    }
}
