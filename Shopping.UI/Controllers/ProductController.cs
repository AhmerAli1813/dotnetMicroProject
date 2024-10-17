using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.UI.Data;
using Shopping.UI.Models;

namespace Shopping.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShoppingUIContext _context;

        public ProductController(ShoppingUIContext context)
        {
            _context = context;
        }

        // GET: ProductDtoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductDto.ToListAsync());
        }

        // GET: ProductDtoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productDto = await _context.ProductDto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productDto == null)
            {
                return NotFound();
            }

            return View(productDto);
        }

        // GET: ProductDtoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductDtoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageUrl,Price,CategoryId,CreatedAt,UpdateAt")] ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productDto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productDto);
        }

        // GET: ProductDtoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productDto = await _context.ProductDto.FindAsync(id);
            if (productDto == null)
            {
                return NotFound();
            }
            return View(productDto);
        }

        // POST: ProductDtoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageUrl,Price,CategoryId,CreatedAt,UpdateAt")] ProductDto productDto)
        {
            if (id != productDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productDto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductDtoExists(productDto.Id))
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
            return View(productDto);
        }

        // GET: ProductDtoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productDto = await _context.ProductDto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productDto == null)
            {
                return NotFound();
            }

            return View(productDto);
        }

        // POST: ProductDtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productDto = await _context.ProductDto.FindAsync(id);
            if (productDto != null)
            {
                _context.ProductDto.Remove(productDto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductDtoExists(int id)
        {
            return _context.ProductDto.Any(e => e.Id == id);
        }
    }
}
