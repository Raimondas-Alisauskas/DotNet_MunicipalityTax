namespace MunicipalityTax.Persistence.DbContexts
{
    using System;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Municipality>().HasData(
                new Municipality()
                {
                    Id = Guid.Parse("7ebced2b-e2f9-45e0-bf75-111111111100"),
                    MunicipalityName = "TestMunicipality",
                });
            modelBuilder.Entity<TaxSchedule>().HasData(
                new TaxSchedule()
                {
                    Id = Guid.Parse("7ebced2b-e2f9-45e0-bf75-111111111113"),
                    ScheduleType = ScheduleType.Daily,
                    TaxStartDate = DateTime.Parse("2016-01-01"),
                    TaxEndDate = DateTime.Parse("2016-01-01"),
                    Tax = 0.1m,
                    MunicipalityId = Guid.Parse("7ebced2b-e2f9-45e0-bf75-111111111100"),
                },
                new TaxSchedule()
                {
                    Id = Guid.Parse("7ebced2b-e2f9-45e0-bf75-111111111111"),
                    ScheduleType = ScheduleType.Weekly,
                    TaxStartDate = DateTime.Parse("2015-12-28"),
                    TaxEndDate = DateTime.Parse("2016-01-03"),
                    Tax = 0.2m,
                    MunicipalityId = Guid.Parse("7ebced2b-e2f9-45e0-bf75-111111111100"),
                },
                new TaxSchedule()
                {
                    Id = Guid.Parse("7ebced2b-e2f9-45e0-bf75-111111111112"),
                    ScheduleType = ScheduleType.Monthly,
                    TaxStartDate = DateTime.Parse("2016-01-01"),
                    TaxEndDate = DateTime.Parse("2016-01-31"),
                    Tax = 0.3m,
                    MunicipalityId = Guid.Parse("7ebced2b-e2f9-45e0-bf75-111111111100"),
                },
                new TaxSchedule()
                {
                    Id = Guid.Parse("7ebced2b-e2f9-45e0-bf75-111111111114"),
                    ScheduleType = ScheduleType.Yearly,
                    TaxStartDate = DateTime.Parse("2016-01-01"),
                    TaxEndDate = DateTime.Parse("2016-12-31"),
                    Tax = 0.4m,
                    MunicipalityId = Guid.Parse("7ebced2b-e2f9-45e0-bf75-111111111100"),
                },
                new TaxSchedule()
                {
                    Id = Guid.Parse("7ebced2b-e2f9-45e0-bf75-111111111115"),
                    ScheduleType = ScheduleType.Monthly,
                    TaxStartDate = DateTime.Parse("2016-02-01"),
                    TaxEndDate = DateTime.Parse("2016-02-29"),
                    Tax = 0.5m,
                    MunicipalityId = Guid.Parse("7ebced2b-e2f9-45e0-bf75-111111111100"),
                });
        }
    }
}
