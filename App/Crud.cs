namespace cs_practice_11;

public class Crud
{
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
    
    public static async 
}