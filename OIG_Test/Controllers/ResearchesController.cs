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
        private readonly IResearchAccess _researches;

        public ResearchesController(IResearchAccess researchAccess, DataContext context)
        {
            _context = context;
            _researches = researchAccess;
        }

        // GET: Researches
        public async Task<IActionResult> Index()
        {
            return View(await _context.Research.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("ResearchId,Name,startDate,endDate")] Research research)
        {
            TimeSpan researchDuration = research.endDate - research.startDate ;

            Boolean startsInFuture = DateTime.Now < research.startDate;

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
        public async Task<IActionResult> Edit(int id, [Bind("ResearchId,Name,startDate,endDate")] Research research)
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

        private bool ResearchExists(int id)
        {
            return _context.Research.Any(e => e.ResearchId == id);
        }
    }
}
