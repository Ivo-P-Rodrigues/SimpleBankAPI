using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Infrastructure.Persistence.Configurations
{
    public class AccountDocConfiguration : IEntityTypeConfiguration<AccountDoc>
    {
        public void Configure(EntityTypeBuilder<AccountDoc> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("account_doc_id")
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at")
                .HasDefaultValueSql("clock_timestamp()");

            builder.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");

            builder.Property(e => e.DocType)
                .HasMaxLength(40)
                .HasColumnName("doc_type");

            builder.Property(e => e.Content)
                .HasColumnName("content");

            builder.Property(e => e.AccountId)
                .HasColumnName("account_id");

            builder.HasOne(d => d.Account)
                .WithMany(p => p.Docs)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("accountdocs_accounts_fkey");

            builder.ToTable("AccountDocs", "SB-operational");
            
        }
    }
}
