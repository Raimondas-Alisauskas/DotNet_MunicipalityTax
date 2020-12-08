namespace MunicipalityTax.Services.Services
{
    using System;
    using System.Collections.Generic;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Domain.Entities;

    public interface IBaseService<out Tdto, Tcreate, Tentity>
        where Tdto : IDto
        where Tcreate : IDto
        where Tentity : BaseEntity
    {
        IEnumerable<Tdto> ReadAll();

        Tdto Read(Guid id);

        Guid Create(Tcreate dto);

        IEnumerable<Guid> CreateAll(IEnumerable<Tcreate> dtos);

        int Update(Guid id, Tcreate dto);

        int Delete(Guid id);

        bool HasFields(string fields);
    }
}