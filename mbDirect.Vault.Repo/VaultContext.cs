using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using mbDirect.Vault.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mbDirect.Vault.Models.Data;

namespace mbDirect.Vault.Repo
{
    public class VaultContext : DbContext
    {
        #region constructors
        public VaultContext() : base()
        {

        }
        public VaultContext(DbContextOptions<VaultContext> options) : base(options)
        {

        }
        #endregion

        public DbSet<Account> Accounts { get; set; }
        public DbSet<InstrumentType> InstrumentTypes { get; set; }
        public DbSet<AccountStatus> AccountStatuses { get; set; }

        public DbSet<Key> Keys { get; set; }
        public DbSet<TransitCredential> TransitCredentials {get; set;}
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Gateway> Gateways { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=mbDirectVault;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountStatus>(e =>
            {
                e.HasKey(s => s.Code);
                e.Property(p => p.LastMaintDate).HasDefaultValueSql("getdate()");
                e.Property(p => p.Code).HasMaxLength(10).HasColumnType("varchar");
                e.Property(p => p.Name).HasMaxLength(20).HasColumnType("varchar");
            });

            modelBuilder.Entity<InstrumentType>(e => {
                e.HasKey(s => s.Code);
                e.Property(p => p.LastMaintDate).HasDefaultValueSql("getdate()");
                e.Property(p => p.Code).HasMaxLength(10).HasColumnType("varchar");
                e.Property(p => p.Name).HasMaxLength(30).HasColumnType("varchar");
                e.HasData(
                    new InstrumentType { Code = "credit", Name = "Credit Card"},
                    new InstrumentType { Code = "debit", Name = "Debit Card" }
                );
            });

            modelBuilder.Entity<Account>(e => {
                e.HasOne<InstrumentType>().WithMany().HasForeignKey(e=>e.InstrumentTypeCode).HasConstraintName("FK_Account_InstrumentType");
                e.HasOne<AccountStatus>().WithMany().HasForeignKey(e=>e.AccountStatusCode).HasConstraintName("FK_Account_Status");
                e.Property(s => s.AddDateTime).HasDefaultValueSql("getdate()");
                e.Property(s => s.StatusDateTime).HasDefaultValueSql("getdate()");
                e.Property(p => p.AccountStatusCode).HasMaxLength(10).HasColumnType("varchar");
                e.Property(p => p.BillingZipCode).HasMaxLength(5).HasColumnType("varchar");
                e.Property(p => p.Expiration).HasMaxLength(4).HasColumnType("varchar");
                e.Property(p => p.InstrumentTypeCode).HasMaxLength(10).HasColumnType("varchar");
                e.Property(p => p.OwnerName).HasMaxLength(60).HasColumnType("varchar");
                e.Property(p => p.RoutingNumber).HasMaxLength(9).HasColumnType("varchar");
                e.Property(p => p.Number).HasMaxLength(20).HasColumnType("varchar").IsRequired(true);
            });

            modelBuilder.Entity<Key>(e => {
                e.HasKey(s => s.KeyId);
                e.Property(p => p.KeyValue).HasMaxLength(100).HasColumnType("varchar").IsRequired(true);
                e.Property(p => p.Vector).HasMaxLength(100).HasColumnType("varchar");
                e.Property(p => p.LastMaintDate).HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<TransitCredential>(e => {
                e.HasKey(s => s.Number);
                e.Property(p => p.UserId).HasMaxLength(60).HasColumnType("varchar").IsRequired(true);
                e.Property(p => p.PasswordEncrypted).HasMaxLength(100).HasColumnType("varchar");
                e.Property(p => p.PasswordKeyId).IsRequired(true);
                e.HasOne<Key>().WithMany().HasForeignKey(f => f.PasswordKeyId).HasConstraintName("FK_TransitCredential_PasswordKey");
                e.Property(p => p.LastMaintDate).HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<Merchant>(e => {
                e.HasKey(m=>m.MerchantId);
                e.Property(p => p.DeviceId).HasMaxLength(20).HasColumnType("varchar");
                e.HasOne<TransitCredential>().WithMany();
                e.Property(p => p.TransitTransactionKey).HasMaxLength(30).HasColumnType("varchar");
                e.Property(p => p.LastMaintDate).HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<Gateway>(e => {
                e.HasKey(m => m.Id);
                e.Property(p => p.Description).HasMaxLength(50).HasColumnType("varchar");
                e.Property(p => p.EndPoint).HasMaxLength(100).HasColumnType("varchar").IsRequired(true);
                e.Property(p => p.LastMaintDate).HasDefaultValueSql("getdate()");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
