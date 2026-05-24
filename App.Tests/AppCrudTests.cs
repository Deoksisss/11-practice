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
        using var db = await InitializeDb();
        var crud = new Crud(db);
        
        var createdUser = await crud.CreateUser();
        
        Assert.NotNull(createdUser);
        Assert.True(createdUser.Id > 0);
    }
    [Fact]
    public static async Task NoteCreated_Success()
    {
        using var db = await InitializeDb();
        var crud = new Crud(db);
        var createdUser = await crud.CreateUser();

        await crud.AddNoteToUser(createdUser.Id, "My first note");
        var note = await db.Notes.FirstOrDefaultAsync(n => n.Id == createdUser.Id);
        
        Assert.NotNull(note);
        Assert.Equal("My first note", note.Name);
        Assert.Equal(createdUser.Id, note.UserId);
    }    
    // R
    [Fact]
    public static async Task SearchNoteByName_ShouldReturnUsersWithMathingNotes()
    {
        using var db = await InitializeDb();
        var crud = new Crud(db);
        var user1 = await crud.CreateUser();
        var user2 = await crud.CreateUser();
        await crud.AddNoteToUser(user1.Id, "Search first note");
        await crud.AddNoteToUser(user2.Id, "Search second note");

        var search = await crud.ReadUsers("Search");
        
        Assert.NotNull(search);
        Assert.Equal(2, search.Count());
        Assert.Equal(user1.Id, search.First().Id);
        Assert.Equal(user2.Id, search.Last().Id);
    }
}