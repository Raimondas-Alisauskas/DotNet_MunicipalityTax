namespace MunicipalityTax.Services.Services
{
    using System.Collections.Generic;
    using System.Dynamic;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Domain.Entities;

    public interface IMunicipalityService : IBaseService<MunicipalityDto, MunicipalityCreateDto, Municipality>
    {
        IEnumerable<ExpandoObject> ReadWithParameters(MunicipalityRequest request);
    }
}
