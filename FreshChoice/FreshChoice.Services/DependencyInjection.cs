using FreshChoice.Services.EmployeeManagement;
using FreshChoice.Services.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FreshChoice.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(
        this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        services.AddIdentityServices();
        services.AddEmployeeManagementServices();
        
        return services;
    }
}