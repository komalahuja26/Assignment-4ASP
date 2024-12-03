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
    [Authorize] // making all actions private so that it is only available to authenticated users
    public class HealthSuggestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HealthSuggestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HealthSuggestions
        [AllowAnonymous] // Allowing anonymous access to Index
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HealthSuggestion.Include(h => h.PeriodEntry);
            return View("Index", await applicationDbContext.ToListAsync()); 
        }

        // GET: HealthSuggestions/Details/5
        [AllowAnonymous] // Allowing anonymous access to Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthSuggestion = await _context.HealthSuggestion
                .Include(h => h.PeriodEntry)
                .FirstOrDefaultAsync(m => m.HealthSuggestionId == id);

            if (healthSuggestion == null)
            {
                return NotFound();
            }

            return View("Details", healthSuggestion); 
        }

        // GET: HealthSuggestions/Create
        public IActionResult Create()
        {
            ViewData["PeriodEntryId"] = new SelectList(_context.PeriodEntry, "PeriodEntryId", "Name");
            return View("Create"); 
        }

        // POST: HealthSuggestions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HealthSuggestionId,PeriodEntryId,WaterIntake,HealthyFoods")] HealthSuggestion healthSuggestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(healthSuggestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PeriodEntryId"] = new SelectList(_context.PeriodEntry, "PeriodEntryId", "Name", healthSuggestion.PeriodEntryId);
            return View("Create", healthSuggestion); 
        }

        // GET: HealthSuggestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthSuggestion = await _context.HealthSuggestion.FindAsync(id);
            if (healthSuggestion == null)
            {
                return NotFound();
            }

            ViewData["PeriodEntryId"] = new SelectList(_context.PeriodEntry, "PeriodEntryId", "Name", healthSuggestion.PeriodEntryId);
            return View("Edit", healthSuggestion); 
        }

        // POST: HealthSuggestions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HealthSuggestionId,PeriodEntryId,WaterIntake,HealthyFoods")] HealthSuggestion healthSuggestion)
        {
            if (id != healthSuggestion.HealthSuggestionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(healthSuggestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HealthSuggestionExists(healthSuggestion.HealthSuggestionId))
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

            ViewData["PeriodEntryId"] = new SelectList(_context.PeriodEntry, "PeriodEntryId", "Name", healthSuggestion.PeriodEntryId);
            return View("Edit", healthSuggestion); 
        }

        // GET: HealthSuggestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthSuggestion = await _context.HealthSuggestion
                .Include(h => h.PeriodEntry)
                .FirstOrDefaultAsync(m => m.HealthSuggestionId == id);

            if (healthSuggestion == null)
            {
                return NotFound();
            }

            return View("Delete", healthSuggestion); 
        }

        // POST: HealthSuggestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var healthSuggestion = await _context.HealthSuggestion.FindAsync(id);
            if (healthSuggestion != null)
            {
                _context.HealthSuggestion.Remove(healthSuggestion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HealthSuggestionExists(int id)
        {
            return _context.HealthSuggestion.Any(e => e.HealthSuggestionId == id);
        }
    }
}
