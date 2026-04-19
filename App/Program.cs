using cs_practice_11;

await using var db = new AppDbContext();
await db.Database.EnsureCreatedAsync();