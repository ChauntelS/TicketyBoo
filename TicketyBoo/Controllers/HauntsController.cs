using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketyBoo.Data;
using TicketyBoo.Models;

namespace TicketyBoo.Controllers
{
    [Authorize]
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

                return RedirectToAction(nameof(Index),"Home");
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,Organizer,Date,CategoryId,ImagePath,FormFile")] Haunt haunt)
        {
            if (id != haunt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingHaunt = await _context.Haunt.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);

                    if (existingHaunt == null)
                    {
                        return NotFound();
                    }

                    // If a new image file is uploaded
                    if (haunt.FormFile != null)
                    {
                        // Step 1: Create a unique filename
                        string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(haunt.FormFile.FileName);

                        // Step 2: Determine file paths
                        string photosPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos");
                        string newFilePath = Path.Combine(photosPath, newFileName);
                        string oldFilePath = Path.Combine(photosPath, existingHaunt.ImagePath ?? "");

                        // Step 3: Upload the new file
                        using (var stream = new FileStream(newFilePath, FileMode.Create))
                        {
                            await haunt.FormFile.CopyToAsync(stream);
                        }

                        // Step 4: Delete the old file (if it exists)
                        if (!string.IsNullOrEmpty(existingHaunt.ImagePath) && System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }

                        // Step 5: Update the ImagePath in DB
                        haunt.ImagePath = newFileName;
                    }
                    else
                    {
                        // Keep the existing image if no new one was uploaded
                        haunt.ImagePath = existingHaunt.ImagePath;
                    }

                    // Step 6: Update record in database
                    _context.Update(haunt);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index), "Home");
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
            return RedirectToAction(nameof(Index),"Home");
        }

        private bool HauntExists(int id)
        {
            return _context.Haunt.Any(e => e.Id == id);
        }
    }
}
