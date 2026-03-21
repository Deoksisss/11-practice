using cs_practice_11;

await using var db = new NoteDbContext();
await db.Database.EnsureCreatedAsync();