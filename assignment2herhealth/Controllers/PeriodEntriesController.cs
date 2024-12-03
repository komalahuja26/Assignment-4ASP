using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using assignment2herhealth.Data;
using assignment2herhealth.Models;
using Microsoft.AspNetCore.Authorization;

namespace assignment2herhealth.Controllers
{
    [Authorize]//making  all actions private so that it is only available to authenticated users
    public class PeriodEntriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PeriodEntriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PeriodEntries
        [AllowAnonymous] // Allowing anonymous access to Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.PeriodEntry.ToListAsync());
        }

        // GET: PeriodEntries/Details/5
        [AllowAnonymous] // Allowing anonymous access to Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodEntry = await _context.PeriodEntry
                .FirstOrDefaultAsync(m => m.PeriodEntryId == id);
            if (periodEntry == null)
            {
                return NotFound();
            }

            return View(periodEntry);
        }

        // GET: PeriodEntries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PeriodEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PeriodEntryId,Name,Age,PeriodStartDate,NextPredictedPeriodDate")] PeriodEntry periodEntry, IFormFile?Photo)
        {
            if (ModelState.IsValid)
            {// check for photo & upload if exists, then capture new unique file name
                if (Photo != null)
                {
                    var fileName = UploadPhoto(Photo);
                    periodEntry.Photo = fileName;
                }

                _context.Add(periodEntry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(periodEntry);
        }

        // GET: PeriodEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodEntry = await _context.PeriodEntry.FindAsync(id);
            if (periodEntry == null)
            {
                return NotFound();
            }
            return View(periodEntry);
        }

        // POST: PeriodEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PeriodEntryId,Name,Age,PeriodStartDate,NextPredictedPeriodDate")] PeriodEntry periodEntry,IFormFile? Photo,String? CurrentPhoto)
        {
            if (id != periodEntry.PeriodEntryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {// upload photo if any
                    if (Photo != null)
                    {
                        var fileName = UploadPhoto(Photo);
                        periodEntry.Photo = fileName;
                    }
                    else
                    {
                        // keep existing photo if any
                        periodEntry.Photo = CurrentPhoto;
                    }
                    _context.Update(periodEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeriodEntryExists(periodEntry.PeriodEntryId))
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
            return View(periodEntry);
        }

        // GET: PeriodEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var periodEntry = await _context.PeriodEntry
                .FirstOrDefaultAsync(m => m.PeriodEntryId == id);
            if (periodEntry == null)
            {
                return NotFound();
            }

            return View(periodEntry);
        }

        // POST: PeriodEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var periodEntry = await _context.PeriodEntry.FindAsync(id);
            if (periodEntry != null)
            {
                _context.PeriodEntry.Remove(periodEntry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeriodEntryExists(int id)
        {
            return _context.PeriodEntry.Any(e => e.PeriodEntryId == id);
        }
        private static string UploadPhoto(IFormFile? photo)
        {
            // get temp location of uploaded photo
            var filePath = Path.GetTempFileName();

            // use GUID (globally unique identifier) to create unique file name
            // eg. product.jpg => abc123-product.jpg
            var fileName = Guid.NewGuid() + "-" + photo.FileName;

            // set destination path dynamically
            var uploadPath = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\img\\" + fileName;

            // copy the file 
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            // send back new unique file name
            return fileName;
        }
    }
}
