using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Metadata;

namespace TwoFAAPI.Data
{
    public class FADBContext : DbContext
    {
        public FADBContext(DbContextOptions<FADBContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
          
        }

        public DbSet<HrEmp> HrEmp { get; set; }
        public DbSet<HrEmpTwoFABackupcode> HrEmpTwoFABackupcode { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }
    }
}
