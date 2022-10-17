using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimpleBank.AcctManage.Core.Domain;
using System.Reflection.Emit;

namespace SimpleBank.AcctManage.Infrastructure.Persistence.Configurations
{
    public class UserTonkenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("user_token_id")
                .HasDefaultValueSql("uuid_generate_v4()");

            builder.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

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

            builder.Ignore(i => i.Active);
            
            builder.Ignore(i => i.Refresh);

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.ToTable("UserTokens", "SB-auth");
        }


    }
}
