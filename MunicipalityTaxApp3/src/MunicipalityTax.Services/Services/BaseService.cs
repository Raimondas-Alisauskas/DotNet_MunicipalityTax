namespace MunicipalityTax.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoMapper;
    using MunicipalityTax.Contracts.In;
    using MunicipalityTax.Domain.Entities;
    using MunicipalityTax.Persistence.Repositories;

    public class BaseService<Tdto, Tcreate, Tentity> : IBaseService<Tdto, Tcreate, Tentity>
        where Tdto : IDto
        where Tcreate : IDto
        where Tentity : BaseEntity
    {
        private readonly IRepository<Tentity> repository;
        private readonly IMapper mapper;

        public BaseService(IRepository<Tentity> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public BaseService()
        {
        }

        public virtual IEnumerable<Tdto> ReadAll()
        {
            var entities = this.repository.ReadAll();

            return this.mapper.Map<IEnumerable<Tdto>>(source: entities);
        }

        public virtual Tdto Read(Guid id)
        {
            var entity = this.repository.Read(id);

            return this.mapper.Map<Tdto>(source: entity);
        }

        public virtual Guid Create(Tcreate dto)
        {
            var entity = this.mapper.Map<Tentity>(source: dto);
            this.repository.Create(entity);
            this.repository.Save();

            return entity.Id;
        }

        public virtual IEnumerable<Guid> CreateAll(IEnumerable<Tcreate> dtos)
        {
            var entities = this.mapper.Map<IEnumerable<Tentity>>(source: dtos);
            this.repository.CreateAll(entities);
            this.repository.Save();

            return entities.Select(e => e.Id);
        }

        public virtual int Update(Guid id, Tcreate dto)
        {
            var entity = this.mapper.Map<Tentity>(source: dto);
            this.repository.Update(id, entity);

            return this.repository.Save();
        }

        public virtual int Delete(Guid id)
        {
            this.repository.Delete(id);

            return this.repository.Save();
        }

        public bool HasFields(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldsArr = fields.Split(',');

            foreach (var field in fieldsArr)
            {
                var propertyName = field.Trim();

                var propertyInfo = typeof(Tdto)
                    .GetProperty(
                        propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
