using Microsoft.EntityFrameworkCore;

namespace cs_practice_11;

public class Crud
{
    // C
    public static async Task<Note> Create(string name, CancellationToken ct = default)
    {
        await using var db = new NoteDbContext();
        var note = new Note
        {
            Name = name,
            CreatedAt = DateTime.UtcNow
        };
        
        db.Notes.Add(note);
        await db.SaveChangesAsync(ct);
        return note;
    }

    // R
    public static async Task<List<Note>> Read(string search, CancellationToken ct = default)
    {
        await using var db = new NoteDbContext();
        return await db.Notes.Where(x => EF.Functions.Like(x.Name, $"%search%")).ToListAsync(ct);
    }

    public static async Task<Note?> Read(int id, CancellationToken ct = default)
    {
        await using var db = new NoteDbContext();
        return await db.Notes.FirstOrDefaultAsync(x => x.Id == id, ct);
    }
    
    // U
    public static async Task Update(Note note, string name, CancellationToken ct = default)
    {
        await using var db = new NoteDbContext();
        note.Name = name;
        note.CreatedAt = DateTime.UtcNow;
        db.Notes.Update(note);
        await db.SaveChangesAsync(ct);
    }
    
    // D
    public static async Task Delete(Note note, CancellationToken ct = default)
    {
        await using var db = new NoteDbContext();
        db.Notes.Remove(note);
        await db.SaveChangesAsync(ct);
    }
}