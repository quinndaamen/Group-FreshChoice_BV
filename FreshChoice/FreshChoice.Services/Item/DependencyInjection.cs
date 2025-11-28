using FreshChoice.Services.Item.Contracts;
using FreshChoice.Services.Item.Internals;
using Microsoft.Extensions.DependencyInjection;

namespace FreshChoice.Services.Item;

public static class DependencyInjection
{
    public static IServiceCollection AddItemServices(this IServiceCollection services)
    {
        services.AddScoped<IItemService, ItemService>();
        
        return services;
    }
}