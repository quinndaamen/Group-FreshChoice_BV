using FreshChoice.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreshChoice.Data;

public class ApplicationDbContext : IdentityDbContext<Employee, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Item> Items { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<EmployeeShift> EmployeeShifts { get; set; }
    public DbSet<Announcement> Announcements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EmployeeShift>()
            .HasKey(es => new { es.EmployeeId, es.ShiftId });

        modelBuilder.Entity<EmployeeShift>()
            .HasOne(es => es.Employee)
            .WithMany(e => e.EmployeeShifts)
            .HasForeignKey(es => es.EmployeeId);

        modelBuilder.Entity<EmployeeShift>()
            .HasOne(es => es.Shift)
            .WithMany(s => s.EmployeeShifts)
            .HasForeignKey(es => es.ShiftId);
    }
}