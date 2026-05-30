using Kiemtragiuaki.Data;
using Kiemtragiuaki.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kiemtragiuaki.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ComputerStoreContext _context;

        public ProductsController(ComputerStoreContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category);
            return View(await products.OrderByDescending(p => p.Id).ToListAsync());
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands.Where(b => b.Status == "active" || string.IsNullOrEmpty(b.Status)), "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => c.Status == "active" || string.IsNullOrEmpty(c.Status)), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Sku,Name,Slug,CategoryId,BrandId,Price,CostPrice,DiscountPercent,Quantity,MainImage,ShortDescription,FullDescription,Status,Featured")] Product product)
        {
            if (ModelState.IsValid)
            {
                // Generate slug if empty
                if (string.IsNullOrEmpty(product.Slug))
                {
                    product.Slug = product.Name.ToLower().Replace(" ", "-");
                }
                product.CreatedAt = DateTime.Now;
                product.UpdatedAt = DateTime.Now;
                
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands.Where(b => b.Status == "active" || string.IsNullOrEmpty(b.Status)), "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => c.Status == "active" || string.IsNullOrEmpty(c.Status)), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands.Where(b => b.Status == "active" || string.IsNullOrEmpty(b.Status)), "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => c.Status == "active" || string.IsNullOrEmpty(c.Status)), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Sku,Name,Slug,CategoryId,BrandId,Price,CostPrice,DiscountPercent,Quantity,MainImage,ShortDescription,FullDescription,Status,Featured,Views,CreatedAt")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.UpdatedAt = DateTime.Now;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands.Where(b => b.Status == "active" || string.IsNullOrEmpty(b.Status)), "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(c => c.Status == "active" || string.IsNullOrEmpty(c.Status)), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
