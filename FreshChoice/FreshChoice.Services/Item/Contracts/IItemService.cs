using FreshChoice.Services.Item.Models;

namespace FreshChoice.Services.Item.Contracts;

public interface IItemService
{
    Task<List<ItemModel>> GetAllAsync();
    Task<ItemModel> GetByIdAsync(long id);
    Task<ItemModel> CreateAsync(ItemModel model);
    Task<ItemModel> UpdateAsync(ItemModel model);
    Task DeleteAsync(long id);
}
