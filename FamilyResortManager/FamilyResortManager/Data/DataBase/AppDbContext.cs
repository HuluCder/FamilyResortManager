using FamilyResortManager.Data.DataBase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FamilyResortManager.Data.DataBase;

public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Models.Client> Clients { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Staff> Staff { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<CleaningTask> CleaningTasks { get; set; }
    public DbSet<AuditLog> AuditLog { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // дополнительные конфигурации по необходимости
    }
}