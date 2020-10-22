using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges()
        {
            var addedTargetApps = ChangeTracker
                    .Entries()
                        .Where(e => e.Entity is TargetApp && (
                                e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var item in addedTargetApps)
            {
                if (item.State == EntityState.Added)
                {
                    ((TargetApp)item.Entity).CreatedDate = DateTime.UtcNow;
                }
                else if (item.State == EntityState.Modified)
                {
                    ((TargetApp)item.Entity).ModifiedDate = DateTime.UtcNow;
                }
            }

            
            return base.SaveChanges();
        }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SingularizeTableNames(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void SingularizeTableNames(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.Name.StartsWith("Microsoft.AspNetCore.Identity."))
                {
                    continue;
                }
                entityType.SetTableName(entityType.DisplayName());
            }
        }

        public DbSet<TargetApp> TargetApps { get; set; }
        public DbSet<LogData> LogDatas { get; set; }
    }



    
}
