using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        private readonly IConfiguration _configuration;
        private readonly TicketyBooContext _context;
        private readonly BlobContainerClient _containerClient;

        //Constructor
        public HauntsController(IConfiguration configuration, TicketyBooContext context)
        {
            _context = context;
            
            _configuration = configuration;

            //Setup blob container client
            var connectionString = _configuration.GetConnectionString("AzureStorage");
            var containerName = "tickety-boo-uploads";
            _containerClient = new BlobContainerClient(connectionString, containerName);
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

                    //
                    //Upload file to Azure Blob Storage
                    //

                    // store the file to upload in fileUpload
                    IFormFile fileUpload = haunt.FormFile;

                    // create a unique filename for the blob
                    string blobName = Guid.NewGuid().ToString() + "_" + fileUpload.FileName;

                    var blobClient = _containerClient.GetBlobClient(blobName);

                    using (var stream = fileUpload.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = fileUpload.ContentType });
                    }

                    string blobURL = blobClient.Uri.ToString();

                    // assgin the blob URL to the record to save in Db
                    haunt.ImagePath = blobURL;

                }
                else 
                {
                    //if no image is uploaded 
                    haunt.ImagePath = "https://nscc0239497storageblob.blob.core.windows.net/tickety-boo-uploads/BooGhost.png";
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
                        // Create a unique name for the new blob
                        IFormFile fileUpload = haunt.FormFile;
                        string blobName = Guid.NewGuid().ToString() + "_" + fileUpload.FileName;

                        var blobClient = _containerClient.GetBlobClient(blobName);

                        // Upload to Azure Blob Storage
                        using (var stream = fileUpload.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = fileUpload.ContentType });
                        }
                        // Save the new blob URL
                        string blobURL = blobClient.Uri.ToString();
                        haunt.ImagePath = blobURL;

                        // Delete the old blob if it exists and isn't the default image
                        if (!string.IsNullOrEmpty(existingHaunt.ImagePath) &&
                            existingHaunt.ImagePath != "https://nscc0239497storageblob.blob.core.windows.net/tickety-boo-uploads/BooGhost.png")
                        {
                            try
                            {
                                var oldBlobUri = new Uri(existingHaunt.ImagePath);
                                string oldBlobName = Path.GetFileName(oldBlobUri.LocalPath);
                                var oldBlobClient = _containerClient.GetBlobClient(oldBlobName);
                                await oldBlobClient.DeleteIfExistsAsync();
                            }
                            catch
                            {
                               
                            }
                        }
                    }
                    else
                    {
                        // Keep existing image if no new one uploaded
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
