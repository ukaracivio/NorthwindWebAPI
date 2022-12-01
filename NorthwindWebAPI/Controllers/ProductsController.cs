using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NorthwindWebAPI.Models;
using System.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace NorthwindWebAPI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly NorthwindContext _context;

        public ProductsController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            // LINQ devreye giriyor
            var northwindContext = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier);

            return View(await northwindContext.ToListAsync());

            //var products = from p in _context.Products
            //               join c in _context.Categories on p.CategoryId equals c.CategoryId
            //               join s in _context.Suppliers on p.SupplierId equals s.SupplierId
            //               select new
            //               {
            //                   ProductName = p.ProductName,
            //                   QuantityPerUnit = p.QuantityPerUnit,
            //                   UnitPrice = p.UnitPrice,
            //                   UnitsInStock = p.UnitsInStock,
            //                   UnitsOnOrder = p.UnitsOnOrder,
            //                   ReorderLevel = p.ReorderLevel,
            //                   Discontinued = p.Discontinued,
            //                   Category = p.CategoryId,
            //                   CategoryName = c.CategoryName,
            //                   Supplier = p.SupplierId,
            //                   SupplierName = s.CompanyName
            //               };

            //return View(products);
        }

        [NonAction]
        private dynamic ToCategoriesSelectList(DbSet<Category> categories, string valueField, string textField)
        {
            // Öncelikle bir liste yaratıyorum.
            List<SelectListItem> list = new List<SelectListItem>();

            //parametre olara gelen categories tablosu bastan sona okunarak bir listeye alınıyor.
            foreach (var item in categories)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.CategoryName,
                    Value = item.CategoryId.ToString()
                });
            }

            return new SelectList(list, "Value", "Text");

        }

        [NonAction]
        private dynamic ToSuppliersSelectList(DbSet<Supplier> suppliers, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (var item in suppliers)
            {
                list.Add(new SelectListItem()
                {
                    Value = item.SupplierId.ToString(),
                    Text = item.CompanyName

                });
            }

            return new SelectList(list, "Value", "Text");

        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }



            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var categories = _context.Categories.ToList();
            if (categories != null)
            {
                ViewBag.CategoryList = ToCategoriesSelectList(_context.Categories, "CategoryId", "CategoryName");
            }

            var suppliers = _context.Suppliers.ToList();
            if (suppliers != null)
            {
                ViewBag.SupplierList = ToSuppliersSelectList(_context.Suppliers, "SupplierId", "CompanyName");
            }

            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierId", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
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
            if (_context.Products == null)
            {
                return Problem("Entity set 'NorthwindContext.Products'  is null.");
            }
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
          return _context.Products.Any(e => e.ProductId == id);
        }

    }
}
