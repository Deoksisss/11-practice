using Microsoft.EntityFrameworkCore;

namespace cs_practice_11;

public class AppDbContext : DbContext
{
    public DbSet<Note> Notes { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasMany(u => u.Notes).WithOne().HasForeignKey(n => n.UserId);
    }
}