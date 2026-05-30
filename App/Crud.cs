using Microsoft.EntityFrameworkCore;

namespace cs_practice_11;

public class Crud(AppDbContext db)
{
    // C
    public async Task<User> CreateUser(CancellationToken ct = default)
    {
        var user = new User();
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
        
        return user;
    }

    public async Task AddNoteToUser(int userId, string name, CancellationToken ct = default)
    {
        var note = new Note
        {
            UserId = userId,
            Name = name,
            CreatedAt = DateTime.UtcNow
        };
        db.Notes.Add(note);
        await db.SaveChangesAsync(ct);
        
    }

    // R
    public async Task<List<User>> ReadUsers(string search, CancellationToken ct = default)
    {
        IQueryable<User> query = db.Users.Include(u => u.Notes);
        
        query = query.Where(u => u.Notes.Any(n => n.Name.Contains(search)));
        
        return await query.ToListAsync(ct);
    }

    public async Task<List<User>> ReadUsers(int id, CancellationToken ct = default)
    {
        IQueryable<User> query = db.Users.Include(u => u.Notes);
        
        query = query.Where(u => u.Id == id);
        return await query.ToListAsync(ct);
    }

    public async Task<List<Note>> ReadNotes(string search, CancellationToken ct = default)
    {
        IQueryable<Note> query = db.Notes;
        
        query = query.Where(n => n.Name.Contains(search));
        
        return await query.ToListAsync(ct);
    }
    
    // U
    public async Task<Note> Update(Note note, string name, CancellationToken ct = default)
    {
        note.Name = name;
        note.CreatedAt = DateTime.UtcNow;
        db.Notes.Update(note);
        await db.SaveChangesAsync(ct);

        return note;
    }
    
    // D
    public async Task DeleteNote(Note? note, CancellationToken ct = default)
    {
        if (note != null) db.Notes.Remove(note);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteUser(User? user, CancellationToken ct = default)
    {
        if (user != null) db.Users.Remove(user);
        await db.SaveChangesAsync(ct);
    }
}