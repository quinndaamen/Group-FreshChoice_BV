using FreshChoice.Services.Item.Contracts;
using FreshChoice.Services.Item.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreshChoice.Web.Controllers
{
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        // LIST all items
        public async Task<IActionResult> Index()
        {
            var items = await _itemService.GetAllAsync();
            return View(items);
        }

        // VIEW single item
        public async Task<IActionResult> Details(long id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _itemService.CreateAsync(model);
            return RedirectToAction("Index");
        }

        // EDIT (GET)
        public async Task<IActionResult> Edit(long id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null) return NotFound();

            return View(item);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ItemModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _itemService.UpdateAsync(model);
            return RedirectToAction("Index");
        }

        // DELETE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id)
        {
            await _itemService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
