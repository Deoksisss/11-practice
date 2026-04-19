using cs_practice_11;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace App.Tests;

// Так как в задании не написано сделать тесты, я сделал их нейронкой (я их не читал)

public class CrudTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _db;
    private readonly Crud _crud;

    public CrudTests()
    {
        // 1. Создаем соединение с SQLite в памяти
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // 2. Настраиваем DbContext на использование этого соединения
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new AppDbContext();

        // 3. Создаем схему таблиц (обязательно для SQLite)
        _db.Database.EnsureCreated();

        _crud = new Crud(_db);
    }

    [Fact]
    public async Task CreateUser_ShouldWorkInSqlite()
    {
        // Act
        var user = await _crud.CreateUser();

        // Assert
        Assert.True(user.Id > 0);
    }

    [Fact]
    public async Task AddNoteToUser_ShouldPersistData()
    {
        // Arrange
        var user = await _crud.CreateUser();

        // Act
        await _crud.AddNoteToUser(user.Id, "Сходить за хлебом");

        // Assert
        var note = await _db.Notes.FirstOrDefaultAsync(n => n.UserId == user.Id);
        Assert.NotNull(note);
        Assert.Equal("Сходить за хлебом", note.Name);
    }

    [Fact]
    public async Task ReadUsers_WithSearch_ShouldReturnMatches()
    {
        // Arrange
        var user = await _crud.CreateUser();
        await _crud.AddNoteToUser(user.Id, "Купить молоко");
        await _crud.AddNoteToUser(user.Id, "Продать гараж");

        // Act
        var result = await _crud.ReadUsers("молоко");

        // Assert
        Assert.Contains(result[0].Notes, n => n.Name.Contains("молоко"));
    }

    [Fact]
    public async Task Update_ShouldChangeNoteInDb()
    {
        // Arrange
        var user = await _crud.CreateUser();
        await _crud.AddNoteToUser(user.Id, "Старое имя");
        var note = await _db.Notes.FirstAsync();

        // Act
        await _crud.Update(note, "Новое имя");

        // Assert
        var updated = await _db.Notes.FirstAsync();
        Assert.Equal("Новое имя", updated.Name);
    }

    [Fact]
    public async Task DeleteUser_ShouldRemoveUser()
    {
        // Arrange
        var user = await _crud.CreateUser();
        var userId = user.Id;

        // Act
        await _crud.DeleteUser(user);

        // Assert
        var deleted = await _crud.ReadUsers(userId);
        Assert.Empty(deleted);
    }

    // Очистка ресурсов после каждого теста
    public void Dispose()
    {
        _db.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}