using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Infrastructure.Persistence.Configurations
{
    public class MovementConfiguration : IEntityTypeConfiguration<Movement>
    {
        public void Configure(EntityTypeBuilder<Movement> builder)
        {
            builder.Property(e => e.Id).HasColumnName("movement_id");

            builder.Property(e => e.AccountId).HasColumnName("account_id");

            builder.Property(e => e.Amount).HasColumnName("amount");

            builder.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(d => d.Account)
                .WithMany(p => p.Movements)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movements_accounts_fkey");
        }
    }
}
