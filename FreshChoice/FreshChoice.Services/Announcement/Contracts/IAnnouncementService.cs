using Essentials.Results;
using FreshChoice.Data.Entities;
using FreshChoice.Services.Announcement.Models;
using FreshChoice.Services.EmployeeManagement.Models;

namespace FreshChoice.Services.Announcement.Contracts;

public interface IAnnouncementService
{
    Task<IEnumerable<AnnouncementModel>> GetAllAnnouncementAsync();
    Task<AnnouncementModel?> GetAnnouncementByIdAsync(Guid id);
    Task<MutationResult> UpdateAnnouncementAsync(AnnouncementModel model);
    Task<MutationResult> CreateAnnouncementAsync(AnnouncementModel model);
    Task<StandardResult> DeleteAnnouncementAsync(Guid id);
}
