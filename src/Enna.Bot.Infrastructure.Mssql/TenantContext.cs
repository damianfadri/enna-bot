using Enna.Streamers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Enna.Bot.Infrastructure.Mssql
{
    public class TenantContext : DbContext
    {
        public DbSet<Tenant<ulong>> Tenants { get; set; }

        public TenantContext(DbContextOptions<TenantContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Tenant<ulong>>();
        }
    }
}
