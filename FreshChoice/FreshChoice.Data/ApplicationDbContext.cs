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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EmployeeShift>()
            .HasKey(es => new { es.EmployeeId, es.DepartmentId, es.ShiftId });

        modelBuilder.Entity<EmployeeShift>()
            .HasOne(es => es.Employee)
            .WithMany(e => e.EmployeeShifts)
            .HasForeignKey(es => es.EmployeeId);

        modelBuilder.Entity<EmployeeShift>()
            .HasOne(es => es.Task)
            .WithMany(t => t.EmployeeShifts)
            .HasForeignKey(es => es.DepartmentId);

        modelBuilder.Entity<EmployeeShift>()
            .HasOne(es => es.Shift)
            .WithMany(s => s.EmployeeShifts)
            .HasForeignKey(es => es.ShiftId);
    }

}