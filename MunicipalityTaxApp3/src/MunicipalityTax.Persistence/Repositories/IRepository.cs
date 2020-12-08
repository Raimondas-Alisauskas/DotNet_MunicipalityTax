namespace MunicipalityTax.Persistence.Repositories
{
    using System;
    using System.Collections.Generic;
    using MunicipalityTax.Domain.Entities;

    public interface IRepository<T>
        where T : BaseEntity
    {
        IEnumerable<T> ReadAll();

        public T Read(Guid id);

        void Create(T entity);

        void CreateAll(IEnumerable<T> entities);

        void Update(Guid id, T entity);

        void Delete(Guid id);

        int Save();
    }
}