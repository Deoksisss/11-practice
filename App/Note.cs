namespace cs_practice_11;

public class Note
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required DateTimeOffset CreatedAt { get; set; }
}