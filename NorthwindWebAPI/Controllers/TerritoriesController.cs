using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NorthwindWebAPI.Models;

namespace NorthwindWebAPI.Controllers
{
    public class TerritoriesController : Controller
    {
        private readonly NorthwindContext _context;

        public TerritoriesController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: Territories
        public async Task<IActionResult> Index()
        {
            var northwindContext = _context.Territories
                                .Include(t => t.Region)
                                .OrderBy(t=> t.Region.RegionDescription)
                                .ThenBy(t => t.TerritoryDescription);


            return View(await northwindContext.ToListAsync());
        }

        // Territory sayfalarında kullanılacak select list hazırlanması
        private dynamic ToRegionsSelectList(DbSet<Region> regions,string valueField,string TextField)
        {
            List<SelectListItem> regionlist = new List<SelectListItem>(); // region tanımlarını tutacak liste....

            foreach (var item in regions)
            {
                regionlist.Add(new SelectListItem()
                {
                    Value = item.RegionId.ToString(),
                    Text = item.RegionDescription
                });

            }

            regionlist.Insert(0, new SelectListItem { Value = "0", Text = "--- Lütfen Bölge Seçiniz ---" });

            return new SelectList(regionlist, "Value", "Text");
        }


        // GET: Territories/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Territories == null)
            {
                return NotFound();
            }

            var territory = await _context.Territories
                .Include(t => t.Region)
                .FirstOrDefaultAsync(m => m.TerritoryId == id);
            if (territory == null)
            {
                return NotFound();
            }

            return View(territory);
        }

        // GET: Territories/Create
        public IActionResult Create()
        {
            var regions = _context.Regions.ToList();

            if (regions != null)
            {
                ViewBag.RegionList = ToRegionsSelectList(_context.Regions, "RegionId", "RegionDescription");

            }


            ViewData["RegionId"] = new SelectList(_context.Regions, "RegionId", "RegionId");
            
            return View();
        }

        // POST: Territories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TerritoryId,TerritoryDescription,RegionId")] Territory territory)
        {
            ViewData["RegionId"] = new SelectList(_context.Regions, "RegionId", "RegionId", territory.RegionId);

            var errors = ModelState.Values.SelectMany(v => v.Errors);


            if (ModelState.IsValid)
            {
                _context.Add(territory);

                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(territory);
        }

        // GET: Territories/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var regions = _context.Regions.ToList();

            if (regions != null)
            {
                ViewBag.RegionList = ToRegionsSelectList(_context.Regions, "RegionId", "RegionDescription");

            }

            if (id == null || _context.Territories == null)
            {
                return NotFound();
            }

            var territory = await _context.Territories.FindAsync(id);

            if (territory == null)
            {
                return NotFound();
            }
            
            ViewData["RegionId"] = new SelectList(_context.Regions, "RegionId", "RegionId", territory.RegionId);
            
            return View(territory);
        }

        // POST: Territories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("TerritoryId,TerritoryDescription,RegionId")] Territory territory)
        {
            if (id != territory.TerritoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(territory);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerritoryExists(territory.TerritoryId))
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
            ViewData["RegionId"] = new SelectList(_context.Regions, "RegionId", "RegionId", territory.RegionId);
            return View(territory);
        }

        // GET: Territories/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Territories == null)
            {
                return NotFound();
            }

            var territory = await _context.Territories
                .Include(t => t.Region)
                .FirstOrDefaultAsync(m => m.TerritoryId == id);
            if (territory == null)
            {
                return NotFound();
            }

            return View(territory);
        }

        // POST: Territories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Territories == null)
            {
                return Problem("Entity set 'NorthwindContext.Territories'  is null.");
            }
            var territory = await _context.Territories.FindAsync(id);
            if (territory != null)
            {
                _context.Territories.Remove(territory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TerritoryExists(string id)
        {
          return _context.Territories.Any(e => e.TerritoryId == id);
        }
    }
}
