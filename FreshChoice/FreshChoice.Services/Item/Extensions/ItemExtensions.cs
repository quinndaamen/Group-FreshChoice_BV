using FreshChoice.Services.Item.Models;

namespace FreshChoice.Services.Item.Extensions;

public static class ItemExtensions
{
    public static ItemModel ToModel(this Data.Entities.Item entity)
    {
        return new ItemModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Price = entity.Price,
            Quantity = entity.Quantity,
            Category =  entity.Category,
        };
    }

    public static Data.Entities.Item ToEntity(this ItemModel model)
    {
        return new Data.Entities.Item
        {
            Id = model.Id,
            Name = model.Name,
            Price = model.Price,
            Quantity = model.Quantity,
            Category =  model.Category,
        };
    }
}
