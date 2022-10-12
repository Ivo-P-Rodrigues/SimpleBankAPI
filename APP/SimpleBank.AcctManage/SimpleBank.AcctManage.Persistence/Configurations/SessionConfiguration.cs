using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleBank.AcctManage.Domain;

namespace SimpleBank.AcctManage.Persistence.Configurations
{
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("session_id")
                .HasDefaultValueSql("(gen_random_uuid())::text");

            builder.Property(e => e.AccessToken).HasColumnName("access_token");

            builder.Property(e => e.AccessTokenExpiresAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("access_token_expires_at");

            builder.Property(e => e.Active).HasColumnName("active");

            builder.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");

            builder.Property(e => e.RefreshToken).HasColumnName("refresh_token");

            builder.Property(e => e.RefreshTokenExpiresAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("refresh_token_expires_at");

            builder.Property(e => e.UserId).HasColumnName("user_id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Sessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sessions_users_fkey");
        }
    }
}
