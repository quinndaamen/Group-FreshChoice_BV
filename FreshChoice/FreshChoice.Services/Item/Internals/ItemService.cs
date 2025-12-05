using FreshChoice.Data;
using FreshChoice.Services.Item.Contracts;
using FreshChoice.Services.Item.Extensions;
using FreshChoice.Services.Item.Models;
using Microsoft.EntityFrameworkCore;

namespace FreshChoice.Services.Item.Internals
{
    public class ItemService : IItemService
    {
        private readonly ApplicationDbContext _context;

        public ItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all items
        public async Task<List<ItemModel>> GetAllAsync()
        {
            var entities = await _context.Items.ToListAsync();
            return entities.Select(e => e.ToModel()).ToList();
        }

        // Get single item by Id
        public async Task<ItemModel> GetByIdAsync(long id)
        {
            var entity = await _context.Items.FindAsync(id);
            return entity?.ToModel(); // null if not found
        }

        // Create new item
        public async Task<ItemModel> CreateAsync(ItemModel model)
        {
            var entity = model.ToEntity();
            _context.Items.Add(entity);
            await _context.SaveChangesAsync();
            return entity.ToModel();
        }

        // Update existing item
        public async Task<ItemModel> UpdateAsync(ItemModel model)
        {
            var entity = await _context.Items.FindAsync(model.Id);
            if (entity == null) return null;

            entity.Name = model.Name;
            entity.Price = model.Price;
            entity.Quantity = model.Quantity;
            entity.Category = model.Category;

            _context.Items.Update(entity);
            await _context.SaveChangesAsync();
            return entity.ToModel();
        }

        // Delete item
        public async Task DeleteAsync(long id)
        {
            var entity = await _context.Items.FindAsync(id);
            if (entity == null) return;

            _context.Items.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}