using FreshChoice.Services.announcement;
using FreshChoice.Services.EmployeeManagement;
using FreshChoice.Services.Identity;
using FreshChoice.Services.Item;
using FreshChoice.Services.Shift;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FreshChoice.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddIdentityServices();
        services.AddEmployeeManagementServices();
        services.AddShiftServices();
        services.AddItemServices();
        services.AddAnnouncementServices();
        
        return services;
    }
}