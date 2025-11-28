using Essentials.Results;
using FreshChoice.Data;
using FreshChoice.Data.Entities;
using FreshChoice.Services.Announcement.Contracts;
using FreshChoice.Services.Announcement.Extensions;
using FreshChoice.Services.Announcement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FreshChoice.Services.Announcement.Internals;

internal class AnnouncementService : IAnnouncementService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AnnouncementService> _logger;

    public AnnouncementService(ApplicationDbContext context, ILogger<AnnouncementService> logger)
    {
        _context = context;
        _logger = logger;
    }
    

    // Interface method implementation
    public async Task<IEnumerable<AnnouncementModel>> GetAllAnnouncementAsync() =>
        await _context
            .Announcements
            .AsNoTracking()
            .Select(x => x.ToModel())
            .ToListAsync();

    public Task<AnnouncementModel?> GetAnnouncementByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<AnnouncementModel?> GetAnnouncementByIdAsync(long announcementId) =>
        await _context
            .Announcements
            .Where(x => x.Id == announcementId)
            .Select(x => x.ToModel())
            .FirstOrDefaultAsync();

    public async Task<MutationResult> UpdateAnnouncementAsync(AnnouncementModel announcement)
    {
        try
        {
            var entity = await _context.Announcements.FindAsync(announcement.Id);
            if (entity == null)
                return MutationResult.ResultFrom(null, "AnnouncementNotFound");

            entity.Name = announcement.Name;
            entity.Date = announcement.Date;
            entity.Text = announcement.Text;

            await _context.SaveChangesAsync();
            return MutationResult.ResultFrom(entity, "AnnouncementUpdated");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating announcement");
            return MutationResult.ResultFrom(e);
        }
    }

    public async Task<MutationResult> CreateAnnouncementAsync(AnnouncementModel announcement)
    {
        try
        {
            var entity = new Data.Entities.Announcement
            {
                Name = announcement.Name,
                Date = announcement.Date,
                Text = announcement.Text
            };

            _context.Announcements.Add(entity);
            await _context.SaveChangesAsync();

            return MutationResult.ResultFrom(entity, "AnnouncementCreated");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating announcement");
            return MutationResult.ResultFrom(e);
        }
    }

    public async Task<StandardResult> DeleteAnnouncementAsync(Guid announcementId)
    {
        try
        {
            var entity = await _context.Announcements.FindAsync(announcementId);
            if (entity == null)
                return StandardResult.UnsuccessfulResult("AnnouncementNotFound");

            _context.Announcements.Remove(entity);
            await _context.SaveChangesAsync();

            return StandardResult.SuccessfulResult();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting announcement");
            return StandardResult.UnsuccessfulResult("ErrorDeletingAnnouncement");
        }
    }
}
