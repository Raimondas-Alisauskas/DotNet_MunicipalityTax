namespace MunicipalityTax.Persistence.DbContexts
{
    using Microsoft.EntityFrameworkCore;
    using MunicipalityTax.Domain.Entities;

    public class MtxDbContext : DbContext
    {
        public DbSet<Municipality> Municipality { get; set; }

        public DbSet<TaxSchedule> TaxSchedule { get; set; }

        public MtxDbContext(DbContextOptions<MtxDbContext> options)
            : base(options)
        {
        }
    }
}
