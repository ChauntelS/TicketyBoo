using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketyBoo.Data;
using TicketyBoo.Models;

namespace TicketyBoo.Controllers
{
    public class HauntsController : Controller
    {
        private readonly TicketyBooContext _context;

        public HauntsController(TicketyBooContext context)
        {
            _context = context;
        }

        // GET: Haunts
        public async Task<IActionResult> Index()
        {
            var ticketyBooContext = _context.Haunt.Include(h => h.Category);
            return View(await ticketyBooContext.ToListAsync());
        }


        // GET: Haunts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var haunt = await _context.Haunt
                .Include(h => h.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (haunt == null)
            {
                return NotFound();
            }

            return View(haunt); //passes the haunt object if it is not null
        }

        // GET: Haunts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Title");
            return View();
        }

        // POST: Haunts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Location,Organizer,Date,CategoryId,ImagePath,FormFile")] Haunt haunt)
        {
            // Initialize values
            haunt.CreateDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                //
                // Step 1: save the file (optionally)
                //
                if (haunt.FormFile != null)
                {
                    // Create a unique filename using a Guid          
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(haunt.FormFile.FileName); // f81d4fae-7dec-11d0-a765-00a0c91e6bf6.jpg

                    // Initialize the filename in photo record
                    haunt.ImagePath = filename;

                    // Get the file path to save the file. Use Path.Combine to handle diffferent OS
                    string saveFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", filename);

                    // Save file
                    using (FileStream fileStream = new FileStream(saveFilePath, FileMode.Create))
                    {
                        await haunt.FormFile.CopyToAsync(fileStream);
                    }
                }

                //
                // Step 2: save record in database
                //

                _context.Add(haunt);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }


            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Title", haunt.CategoryId);
            return View(haunt);
        }

        // GET: Haunts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var haunt = await _context.Haunt.FindAsync(id);
            if (haunt == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Title", haunt.CategoryId);

            return View(haunt);
        }

        // POST: Haunts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,Organizer,Date,CategoryId,FormFile")] Haunt haunt)
        {
            if (id != haunt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //
                // step 1: save the file
                //
                if (haunt.FormFile != null)
                {

                    // determine new filename

                    // set the new filename in the db record

                    // upload the new file

                    // delete the old file
                }

                //
                // step 2: save in database
                //

                try
                {
                    _context.Update(haunt);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HauntExists(haunt.Id))
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

            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Title", haunt.CategoryId);

            return View(haunt);
        }

        // GET: Haunts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var haunt = await _context.Haunt
                .Include(h => h.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (haunt == null)
            {
                return NotFound();
            }

            return View(haunt);
        }

        // POST: Haunts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var haunt = await _context.Haunt.FindAsync(id);
            if (haunt != null)
            {
                _context.Haunt.Remove(haunt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HauntExists(int id)
        {
            return _context.Haunt.Any(e => e.Id == id);
        }
    }
}
