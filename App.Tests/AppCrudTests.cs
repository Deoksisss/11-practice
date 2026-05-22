using cs_practice_11;
using Microsoft.EntityFrameworkCore;

namespace App.Tests;

public static class NotesCrudTests
{

    static async Task<AppDbContext> InitializeDb()
    {
        AppDbContext db = new AppDbContext();
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
        return db;
    }
    // C
    [Fact]
    public static async Task UserCreated_Success()
    {
        var db = await InitializeDb();
        
        var createdUser = await Crud.CreateUser();
        
        Assert.NotNull(createdUser);
        Assert.True(createdUser.Id > 0);
    }
    [Fact]
    public async Task NoteCreated_Success()
    {
        var db = await InitializeDb();
        var createdUser = await Crud.CreateUser();

        await Crud.AddNoteToUser(createdUser.Id, "My first note");
        var note = await db.Notes.FirstOrDefaultAsync(n => n.Id == createdUser.Id);
        
        Assert.NotNull(note);
        Assert.Equal("My first note", note.Name);
        Assert.Equal(createdUser.Id, note.UserId);
    }    
    // R
    [Fact]
    public static async Task ReadByName_Success()
    {
        string name = "My first note";
        var db = await InitializeDb();
        await Crud.Create(name);
        
        var notes = await Crud.Read("My");
        
        Assert.Equal(notes.First().Name, name);
    }

    [Fact]
    public static async Task ReadByID_Success()
    {
        var name = "My first note";
        var db = await InitializeDb();
        await Crud.Create(name);
        
        var note = await Crud.Read(1);
        
        Assert.NotNull(note);
        Assert.Equal(note.Name, name);
        
    }

    [Fact]
    public static async Task ReadById_OutOfRange_Null()
    {
        var db = await InitializeDb();
        
        var note = await Crud.Read(1);
        Assert.Null(note);
    }
    
    [Fact]
    public static async Task Update_Success()
    {
        var db = await InitializeDb();
        var newName = "New note";
        var note = await Crud.Create("Old Note");
        
        await Crud.Update(note, newName);
        
        Assert.Equal(newName, note.Name);
    }

    [Fact]
    public static async Task Delete_Success()
    {
        var db = await InitializeDb();
        var note = await Crud.Create("My trash note");
        var noteId = note.Id;
        
        await Crud.Delete(note);
        
        Assert.Null(await Crud.Read(noteId));
    }
}