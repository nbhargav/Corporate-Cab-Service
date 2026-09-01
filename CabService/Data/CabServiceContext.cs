using CabService.Models;
using Microsoft.EntityFrameworkCore;

namespace CabService.Data
{
    public class CabServiceContext : DbContext
    {
        public CabServiceContext(DbContextOptions<CabServiceContext> options) : base(options) { }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<VehicleRequest> VehicleRequests => Set<VehicleRequest>();
        public DbSet<AssignedVehicle> AssignedVehicles => Set<AssignedVehicle>();
        public DbSet<FuelLog> FuelLogs => Set<FuelLog>();
        public DbSet<Feedback> FeedbackEntries => Set<Feedback>();
        public DbSet<VehicleInsurance> VehicleInsurances => Set<VehicleInsurance>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Username)
                .IsUnique();

            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.RegistrationNumber)
                .IsUnique();

            modelBuilder.Entity<VehicleRequest>()
                .HasOne(r => r.Employee)
                .WithMany(e => e.VehicleRequests)
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VehicleRequest>()
                .HasOne(r => r.Vehicle)
                .WithMany(v => v.Requests)
                .HasForeignKey(r => r.VehicleId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<AssignedVehicle>()
                .HasOne(a => a.Request)
                .WithOne(r => r.Assignment)
                .HasForeignKey<AssignedVehicle>(a => a.RequestId);

            modelBuilder.Entity<FuelLog>()
                .HasOne(f => f.Vehicle)
                .WithMany(v => v.FuelLogs)
                .HasForeignKey(f => f.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Vehicle)
                .WithMany(v => v.FeedbackEntries)
                .HasForeignKey(f => f.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VehicleInsurance>()
                .HasOne(i => i.Vehicle)
                .WithOne(v => v.Insurance)
                .HasForeignKey<VehicleInsurance>(i => i.VehicleId);

            // Decimal precision for money/fuel fields
            modelBuilder.Entity<FuelLog>().Property(f => f.Cost).HasPrecision(10, 2);
            modelBuilder.Entity<FuelLog>().Property(f => f.FuelQuantityLiters).HasPrecision(8, 2);
        }
    }
}
