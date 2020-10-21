namespace CinelAirMiles.Common.Data
{
    using System.Linq;

    using CinelAirMiles.Common.Entities;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Client> Clients { get; set; }

        public DbSet<CreditCardInfo> CreditCardsInfo { get; set; }

        public DbSet<Mile> Miles { get; set; }

        public DbSet<MilesTransaction> MilesTransactions { get; set; }

        public DbSet<ReferrerProgram> ReferrersProgram { get; set; }

        public DbSet<MilesType> MilesTypes { get; set; }

        public DbSet<ProgramTier> ProgramTiers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Disables cascade deleting
            var cascadeFKs = builder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Cascade;
            }

            builder.Entity<ReferrerProgram>()
                .HasKey(c => new { c.ReferredClientId, c.ReferrerClientId });

            builder.Entity<ReferrerProgram>()
                .HasOne(c => c.ReferredClient)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReferrerProgram>()
                .HasOne(c => c.ReferrerClient)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Mile>()
                .HasOne(m => m.MilesType)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
