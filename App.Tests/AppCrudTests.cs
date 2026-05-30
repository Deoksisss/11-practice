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
    public static async Task SearchUserByName_ShouldReturnUsersWithMatchingNotes()
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

    [Fact]
    public static async Task SearchUserByUserId_ShouldReturnUsersWithMatchingNotes()
    {
        using var db = await InitializeDb();
        var crud = new Crud(db);
        var user1 = await crud.CreateUser();
        await crud.AddNoteToUser(user1.Id, "Search note by ID");
        
        var search = await crud.ReadUsers(user1.Id);
        
        Assert.NotNull(search);
        Assert.Single(search);
    }
    
    // U
    [Fact]
    public static async Task UpdateNote_ShouldUpdateNote()
    {
        using var db = await InitializeDb();
        var crud = new Crud(db);
        var user1 = await crud.CreateUser();
        await crud.AddNoteToUser(user1.Id, "It Should be updated");
        var oldNote = await crud.ReadNotes("It Should be updated");
        var newNote = await crud.Update(oldNote[0], "Updated Note");
        
        Assert.NotNull(newNote);
        Assert.Equal("Updated Note", newNote.Name);
    }
    
    // D
    [Fact]
    public static async Task DeleteNote_ShouldDeleteNote()
    {
        var db = await InitializeDb();
        var crud = new Crud(db);
        var user1 = await crud.CreateUser();
        await crud.AddNoteToUser(user1.Id, "It should be deleted");
        var oldNotes = await crud.ReadNotes("It should be deleted");

        await crud.DeleteNote(oldNotes[0]);
        var deletedNotes = await crud.ReadNotes("It should be deleted");
        
        Assert.Empty(deletedNotes);
        
        
    }
}