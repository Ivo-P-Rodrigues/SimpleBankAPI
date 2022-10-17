using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleBank.AcctManage.Core.Domain;

namespace SimpleBank.AcctManage.Infrastructure.Persistence.Configurations
{
    public class UserTonkenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("user_token_id")
                .HasDefaultValueSql("(gen_random_uuid())::text");

            builder.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");

            builder.Property(e => e.AccessToken)
                .HasColumnName("access_token");

            builder.Property(e => e.AccessTokenExpiresAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("access_token_expires_at");

            builder.Property(e => e.RefreshToken)
                .HasColumnName("refresh_token");

            builder.Property(e => e.RefreshTokenExpiresAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("refresh_token_expires_at");

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.ToTable("UserTokens", "SB-auth");
        }


    }
}
