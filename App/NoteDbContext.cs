using Microsoft.EntityFrameworkCore;

namespace cs_practice_11;

public class NoteDbContext : DbContext
{
    public DbSet<Note> Notes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=notes.db");
    }
}