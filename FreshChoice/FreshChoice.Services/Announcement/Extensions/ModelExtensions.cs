using FreshChoice.Data.Entities;
using FreshChoice.Services.Announcement.Models;

namespace FreshChoice.Services.Announcement.Extensions
{
    public static class ModelExtensions
    {
        public static AnnouncementModel ToModel(this Data.Entities.Announcement announcement) =>
            new()
            {
                Id = announcement.Id,
                Name = announcement.Name,
                Date = announcement.Date,
                Text = announcement.Text
            };

        public static Data.Entities.Announcement ToEntity(this AnnouncementModel model) =>
            new()
            {
                Id = model.Id,
                Name = model.Name,
                Date = model.Date,
                Text = model.Text
            };
    }
}