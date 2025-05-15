using FamilyResortManager.Data.DataBase.Models;
using FamilyResortManager.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FamilyResortManager.Data.DataBase;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Staff> Staff { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<CleaningTask> CleaningTasks { get; set; }
    public DbSet<AuditLog> AuditLog { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<TaskEntry> TaskEntries { get; set; }
    public DbSet<TaskLogEntry> TaskLogEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}