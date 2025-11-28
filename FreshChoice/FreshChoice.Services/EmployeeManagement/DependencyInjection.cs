using FreshChoice.Services.Announcement.Contracts;
using FreshChoice.Services.EmployeeManagement.Contracts;
using FreshChoice.Services.EmployeeManagement.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace FreshChoice.Services.EmployeeManagement;

public static class DependencyInjection
{
    public static IServiceCollection AddEmployeeManagementServices(this IServiceCollection services)
    {
        services.AddScoped<IEmployeeService, EmployeeService>();
        
        return services;
    }
}