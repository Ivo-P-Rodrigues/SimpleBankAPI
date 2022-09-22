using Microsoft.EntityFrameworkCore;

namespace SimpleBankAPI.Core.Models
{
    public partial class SimpleBankAPIContext : DbContext
    {

        public SimpleBankAPIContext(DbContextOptions options) : base(options)
        {
        }
        
        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Movement> Movements { get; set; } = null!;
        public virtual DbSet<Transfer> Transfers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Session> Sessions { get; set; } = null!;
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Balance).HasColumnName("balance");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Currency)
                    .HasMaxLength(3)
                    .HasColumnName("currency")
                    .HasDefaultValueSql("'EUR'::bpchar")
                    .IsFixedLength();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_user_fkey");
            });

            modelBuilder.Entity<Movement>(entity =>
            {
                entity.Property(e => e.MovementId).HasColumnName("movement_id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Movements)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("movements_accounts_fkey");
            });

            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.Property(e => e.TransferId).HasColumnName("transfer_id");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.FromAccountId).HasColumnName("from_account_id");

                entity.Property(e => e.RequestDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("request_date")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ToAccountId).HasColumnName("to_account_id");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "unique_users_email")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "unique_users_username")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email)
                    .HasMaxLength(320)
                    .HasColumnName("email");

                entity.Property(e => e.Fullname).HasColumnName("fullname");

                entity.Property(e => e.PasswordChangedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("password_changed_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Salt).HasColumnName("salt");

                entity.Property(e => e.Username).HasColumnName("username");
            });
            

            modelBuilder.Entity<Session>(entity =>
            {
                entity.Property(e => e.SessionId)
                    .HasColumnName("session_id")
                    .HasDefaultValueSql("(gen_random_uuid())::text");

                entity.Property(e => e.AccessToken).HasColumnName("access_token");

                entity.Property(e => e.AccessTokenExpiresAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("access_token_expires_at");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created_at");

                entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");

                entity.Property(e => e.RefreshTokenExpiresAt)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("refresh_token_expires_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sessions_users_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


    }
}
