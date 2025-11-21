namespace FreshChoice.Data.Entities;

public class Announcement : Entity
{
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Text { get; set; } = string.Empty;
}