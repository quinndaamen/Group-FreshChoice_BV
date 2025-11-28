using FreshChoice.Services.Shift.Contracts;
using FreshChoice.Services.Shift.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace FreshChoice.Services.Shift;

public static class DependencyInjection
{
    public static IServiceCollection AddShiftServices(this IServiceCollection services)
    {
        services.AddScoped<IShiftService, ShiftService>();
        
        return services;
    }
}