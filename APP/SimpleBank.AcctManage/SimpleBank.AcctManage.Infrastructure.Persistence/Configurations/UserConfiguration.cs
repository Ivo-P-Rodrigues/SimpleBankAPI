using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(e => e.Email, "unique_users_email")
                .IsUnique();

            builder.HasIndex(e => e.Username, "unique_users_username")
                .IsUnique();

            builder.Property(e => e.Id)
                .HasColumnName("user_id")
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at")
                .HasDefaultValueSql("clock_timestamp()");

            builder.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");

            builder.Property(e => e.Fullname)
                .HasColumnName("fullname");

            builder.Property(e => e.PasswordChangedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("password_changed_at")
                .HasDefaultValueSql("clock_timestamp()");

            builder.Property(e => e.Salt)
                .HasColumnName("salt");

            builder.Property(e => e.Username)
                .HasColumnName("username");

            builder.ToTable("Users", "SB-operational");
            
        }
    }
}
