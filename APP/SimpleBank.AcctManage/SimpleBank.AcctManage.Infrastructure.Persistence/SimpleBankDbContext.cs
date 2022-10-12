using Microsoft.EntityFrameworkCore;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Infrastructure.Persistence
{
    public class SimpleBankDbContext : DbContext
    {
        public SimpleBankDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Movement> Movements { get; set; } = null!;
        public virtual DbSet<Transfer> Transfers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "0.0.1");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SimpleBankDbContext).Assembly);
        }



    }
}