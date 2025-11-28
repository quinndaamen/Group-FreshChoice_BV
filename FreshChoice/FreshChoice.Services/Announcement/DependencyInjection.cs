using FreshChoice.Services.Announcement.Contracts;
using FreshChoice.Services.Announcement.Internals;
using FreshChoice.Services.Item.Contracts;
using FreshChoice.Services.Item.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace FreshChoice.Services.announcement;

public static class DependencyInjection
{
    public static IServiceCollection AddAnnouncementServices(this IServiceCollection services)
    {
        services.AddScoped<IAnnouncementService, AnnouncementService>();
        
        return services;
    }
}