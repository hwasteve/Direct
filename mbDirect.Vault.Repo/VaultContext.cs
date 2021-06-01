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
        public VaultContext(): base()
        {

        }
        public VaultContext(DbContextOptions<VaultContext> options): base(options)
        {

        }
        #endregion

        public DbSet<Account> Accounts { get; set; }
        public DbSet<InstrumentType> InstrumentTypes { get; set; }
        public DbSet<AccountStatus> AccountStatuses { get; set; }
       
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
            });

            modelBuilder.Entity<InstrumentType>(e => {
                e.HasKey(s => s.Code);
                e.Property(p => p.LastMaintDate).HasDefaultValueSql("getdate()");
                e.HasData(
                    new InstrumentType { Code = "credit", Name = "Credit Card"},
                    new InstrumentType { Code = "debit", Name = "Debit Card" }
                );
            });

            modelBuilder.Entity<Account>(e => {
                e.HasOne<InstrumentType>();
                e.HasOne < AccountStatus>();
                e.Property(s => s.AddDateTime).HasDefaultValueSql("getdate()");
                e.Property(s => s.StatusDateTime).HasDefaultValueSql("getdate()");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
