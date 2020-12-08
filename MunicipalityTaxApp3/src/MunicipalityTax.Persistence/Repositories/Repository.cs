namespace MunicipalityTax.Persistence.Repositories
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using MunicipalityTax.Domain.Entities;
    using MunicipalityTax.Persistence.DbContexts;

    public class Repository<T> : IRepository<T>
        where T : BaseEntity
    {
        private readonly MtxDbContext mtxDbContext;
        private readonly DbSet<T> table;

        public Repository(MtxDbContext mtxDbContext)
        {
            this.mtxDbContext = mtxDbContext;
            this.table = mtxDbContext.Set<T>();
        }

        public IEnumerable<T> ReadAll()
        {
            return this.table
                    .AsNoTracking();
        }

        public T Read(Guid id)
        {
            return this.table.Find(id);
        }

        public void Create(T entity)
        {
            this.table.Add(entity);
        }

        public void CreateAll(IEnumerable<T> entities)
        {
            this.table.AddRange(entities);
        }

        public void Update(Guid id, T entity)
        {
            T entityToUpdate = this.table.Find(id);

            if (entityToUpdate != null)
            {
                entity.Id = id;
                this.mtxDbContext.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            }
        }

        public void Delete(Guid id)
        {
            T entity = this.table.Find(id);

            if (entity != null)
            {
                this.table.Remove(entity);
            }
        }

        public int Save()
        {
            return this.mtxDbContext.SaveChanges();
        }
    }
}
