namespace FreshChoice.Services.Announcement.Models
{
    public class AnnouncementModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}