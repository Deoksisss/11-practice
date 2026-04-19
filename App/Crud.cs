using Microsoft.EntityFrameworkCore;

namespace cs_practice_11;

public class Crud
{
    private readonly AppDbContext _db = new();
    // C
    public async Task<User> CreateUser(CancellationToken ct = default)
    {
        var user = new User();
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
        
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
        _db.Notes.Add(note);
        await _db.SaveChangesAsync(ct);
    }

    // R
    public async Task<List<User>> ReadUsers(string search, CancellationToken ct = default)
    {
        IQueryable<User> query = _db.Users.Include(u => u.Notes);
        
        query = query.Where(u => u.Notes.Any(n => n.Name.Contains(search)));
        
        return await query.ToListAsync(ct);
    }

    public async Task<List<User>> ReadUsers(int id, CancellationToken ct = default)
    {
        IQueryable<User> query = _db.Users.Include(u => u.Notes);
        
        query = query.Where(u => u.Id == id);
        return await query.ToListAsync(ct);
    }

    public async Task<List<Note>> ReadNotes(string search, CancellationToken ct = default)
    {
        IQueryable<Note> query = _db.Notes;
        
        query = query.Where(n => n.Name.Contains(search));
        
        return await query.ToListAsync(ct);
    }

    public async Task<List<Note>> ReadNotes(int id, CancellationToken ct = default)
    {
        IQueryable<Note> query = _db.Notes;
        
        query = query.Where(n => n.Id == id);
        return await query.ToListAsync(ct);
    }
    
    // U
    public async Task Update(Note note, string name, CancellationToken ct = default)
    {
        note.Name = name;
        note.CreatedAt = DateTime.UtcNow;
        _db.Notes.Update(note);
        await _db.SaveChangesAsync(ct);
    }
    
    // D
    public async Task DeleteNote(Note? note, CancellationToken ct = default)
    {
        if (note != null) _db.Notes.Remove(note);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteUser(User? user, CancellationToken ct = default)
    {
        if (user != null) _db.Users.Remove(user);
        await _db.SaveChangesAsync(ct);
    }
}