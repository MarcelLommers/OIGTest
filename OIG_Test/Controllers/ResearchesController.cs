using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OIG_Test.DBInfra;
using OIG_Test.DBInfra.Interfaces;
using OIG_Test.Models;

namespace OIG_Test.Controllers
{
    public class ResearchesController : Controller
    {
        private readonly DataContext _context;

        public ResearchesController(DataContext context)
        {
            _context = context;
        }

        // GET: Researches
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString)
        {
            // Start with ViewBag variables, setting search / sorting params.
            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.StartDateSortParm = (String.IsNullOrEmpty(sortOrder) || sortOrder == "startDate") ? "startDate_desc" : "startDate";
            ViewBag.EndDateSortParm = sortOrder == "endDate" ? "endDate_desc" : "endDate";

            // If no new searchString was submitted, maintain the currentFilter.
            if (searchString == null)
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            // Retrieve all researches in the database.
            var researches = from r in _context.Research
                             select r;

            // Filter based on search string.
            if (!String.IsNullOrEmpty(searchString))
            {
                researches = researches.Where(r => r.Name.Contains(searchString));
            }

            // Order based on sortOrder.
            switch (sortOrder)
            {
                case "name":
                    researches = researches.OrderBy(r => r.Name);
                    break;
                case "name_desc":
                    researches = researches.OrderByDescending(r => r.Name);
                    break;
                case "startDate_desc":
                    researches = researches.OrderByDescending(r => r.StartDate).ThenBy(r => r.EndDate);
                    break;
                case "endDate":
                    researches = researches.OrderBy(r => r.EndDate);
                    break;
                case "endDate_desc":
                    researches = researches.OrderByDescending(r => r.EndDate);
                    break;
                case "startDate":
                default:
                    researches = researches.OrderBy(r => r.StartDate).ThenBy(r => r.EndDate);
                    break;
            }

            // Return remaining researches as a list.
            return View(await researches.ToListAsync());
        }

        // GET: Researches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var research = await _context.Research
                .FirstOrDefaultAsync(m => m.ResearchId == id);
            if (research == null)
            {
                return NotFound();
            }

            return View(research);
        }

        // GET: Researches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Researches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResearchId,Name,StartDate,EndDate")] Research research)
        {
            // Technically this should now be caught by ModelState.IsValid, considering cleanup.
            TimeSpan researchDuration = research.EndDate - research.StartDate;

            Boolean startsInFuture = DateTime.Now < research.StartDate;

            // research is in the future and has a minimum duration of 1 hour
            if (startsInFuture &&
                researchDuration.TotalHours >= 1 &&
                ModelState.IsValid)
            {
                _context.Add(research);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(research);
        }

        // GET: Researches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var research = await _context.Research.FindAsync(id);
            if (research == null)
            {
                return NotFound();
            }
            return View(research);
        }

        // POST: Researches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResearchId,Name,StartDate,EndDate")] Research research)
        {
            if (id != research.ResearchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(research);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResearchExists(research.ResearchId))
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
            return View(research);
        }

        // GET: Researches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var research = await _context.Research
                .FirstOrDefaultAsync(m => m.ResearchId == id);
            if (research == null)
            {
                return NotFound();
            }

            return View(research);
        }

        // POST: Researches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var research = await _context.Research.FindAsync(id);
            _context.Research.Remove(research);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Researches/Stop/5
        public async Task<IActionResult> Stop(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var research = await _context.Research
                .FirstOrDefaultAsync(m => m.ResearchId == id);
            if (research == null)
            {
                return NotFound();
            }

            return View(research);
        }

        // POST: Researches/Stop/5
        [HttpPost, ActionName("Stop")]
        [ValidateAntiForgeryToken]
        // This could technically be an edit, normally this would be discussed before implementing.
        public async Task<IActionResult> StopConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Find target research and set the endDate to now.
            var research = await _context.Research
                .FirstOrDefaultAsync(m => m.ResearchId == id);
            research.EndDate = DateTime.Now;

            try
            {
                _context.Update(research);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResearchExists(research.ResearchId))
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

        // GET: Researches/Entry/5
        public async Task<IActionResult> Entry(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var research = await _context.Research.FindAsync(id);
            if (research == null)
            {
                return NotFound();
            }
            return View(research);
        }

        // POST: Researches/Entry/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EntrySubmitted(int? id)
        {
            // while this should never get called currently, we redirect to index to be certain
            //if (id == null)
            //{
            //    return NotFound();
            //}

            // Here we would confirm the validity of the submitted entry.
            // Out of scope for current assessment.

            return RedirectToAction(nameof(Index));
        }

        private bool ResearchExists(int id)
        {
            return _context.Research.Any(e => e.ResearchId == id);
        }

        // Check for minimum research duration of 1 hour.
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyResearchDuration(DateTime startDate, DateTime endDate)
        {
            TimeSpan researchDuration = endDate - startDate;

            if (researchDuration.TotalHours < 1)
            {
                return Json($"Research cannot have a shorter duration than 1 hour.");
            }

            return Json(true);
        }
    }
}
