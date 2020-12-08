namespace MunicipalityTax.Services.Tests.Helpers
{
    using System;
    using System.Data.Common;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using MunicipalityTax.Persistence.DbContexts;

    public class ShareTestDatabaseFixture : IDisposable
    {
        private static readonly object Lock = new object();

        private static bool databaseInitialized;

        public ShareTestDatabaseFixture()
        {
            this.Connection = new SqlConnection(@"Server=(localdb)\mssqllocaldb;Database=MTX3Test;ConnectRetryCount=0");

            this.Seed();

            this.Connection.Open();
        }

        public DbConnection Connection { get; }

        public MtxDbContext CreateContext(DbTransaction transaction = null)
        {
            var context = new MtxDbContext(new DbContextOptionsBuilder<MtxDbContext>().UseSqlServer(this.Connection).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
        {
            lock (Lock)
            {
                if (!databaseInitialized)
                {
                    using (var context = this.CreateContext())
                    {
                        context.Database.EnsureDeleted();

                        context.Database.Migrate();
                    }

                    databaseInitialized = true;
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            this.Connection.Dispose();
        }
    }
}
