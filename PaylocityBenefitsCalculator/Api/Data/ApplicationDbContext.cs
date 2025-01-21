using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Dependent> Dependents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Salary).HasColumnType("decimal(10,2)");

                entity.HasMany(e => e.Dependents)
                    .WithOne(d => d.Employee)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Dependent>(entity =>
            {
                entity.ToTable("Dependents");
                entity.HasKey(d => d.Id);
                entity.Property(d => d.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(d => d.LastName).HasMaxLength(100).IsRequired();
            });
        }
    }
}
