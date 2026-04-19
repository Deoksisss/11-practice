namespace cs_practice_11;

public class User
{
    public int Id { get; set; }
    public List<Note> Notes { get; set; } = new();
}