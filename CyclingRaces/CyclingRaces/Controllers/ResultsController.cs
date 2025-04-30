using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CyclingRaces.Data;
using CyclingRaces.Data.Models;
using CyclingRaces.Web.ViewModels.Models;
using Microsoft.AspNetCore.Authorization;

namespace CyclingRaces.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResultsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Results
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["RaceSortParm"] = string.IsNullOrEmpty(sortOrder) ? "race_desc" : "";
            ViewData["CyclistSortParm"] = sortOrder == "Cyclist" ? "cyclist_desc" : "Cyclist";
            ViewData["TimeSortParm"] = sortOrder == "Time" ? "time_desc" : "Time";
            ViewData["RankSortParm"] = sortOrder == "Rank" ? "rank_desc" : "Rank";
            ViewData["CurrentFilter"] = searchString;

            var results = await _context.Results
                .Include(r => r.Cyclist)
                .Include(r => r.Race)
                .ToListAsync();

            var raceResults = results.Select(r => new RaceResultViewModel
            {
                Id = r.Id,
                Race = r.Race.Name,
                CyclistName = r.Cyclist.UserName,
                OverallTime = r.Time,
                OverallRank = r.Rank
            });

            if (!string.IsNullOrEmpty(searchString))
            {
                var lowerSearch = searchString.ToLower();
                raceResults = raceResults.Where(r =>
                    r.Race.ToLower().Contains(lowerSearch) ||
                    r.CyclistName.ToLower().Contains(lowerSearch));
            }

            raceResults = sortOrder switch
            {
                "race_desc" => raceResults.OrderByDescending(r => r.Race),
                "Cyclist" => raceResults.OrderBy(r => r.CyclistName),
                "cyclist_desc" => raceResults.OrderByDescending(r => r.CyclistName),
                "Time" => raceResults.OrderBy(r => r.OverallTime),
                "time_desc" => raceResults.OrderByDescending(r => r.OverallTime),
                "Rank" => raceResults.OrderBy(r => r.OverallRank),
                "rank_desc" => raceResults.OrderByDescending(r => r.OverallRank),
                _ => raceResults.OrderBy(r => r.Race)
            };

            return View(raceResults.ToList());
        }



        // GET: Results/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _context.Results
                .Include(r => r.Cyclist)
                .Include(r => r.Race)
                .Where(r => r.Id == id)
                .Select(r => new RaceResultViewModel
                {
                    Id = r.Id,
                    Race = r.Race.Name,
                    CyclistName = r.Cyclist.UserName,
                    OverallTime = r.Time,
                    OverallRank = r.Rank
                })
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // GET: Results/Create
        [Authorize(Roles = "Admin,Organiser")]
        public IActionResult Create()
        {
            ViewData["RaceId"] = new SelectList(_context.Races, "Id", "Name");
            ViewData["CyclistId"] = new SelectList(_context.Cyclists, "Id", "UserName");
            return View();
        }

        // POST: Results/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RaceResultViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new Result
                {
                    Id = Guid.NewGuid().ToString(),
                    RaceId = model.RaceId,
                    CyclistId = model.CyclistId,
                    Time = model.OverallTime,
                    Rank = model.OverallRank
                };
                _context.Results.Add(result);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["RaceId"] = new SelectList(_context.Races, "Id", "Name", model.RaceId);
            ViewData["CyclistId"] = new SelectList(_context.Cyclists, "Id", "UserName", model.CyclistId);
            return View(model);
        }

        // GET: Results/Edit/5
        [Authorize(Roles = "Admin,Organiser")]
        public async Task<IActionResult> Edit(string id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result == null) return NotFound();

            var model = new RaceResultViewModel
            {
                Id = result.Id,
                RaceId = result.RaceId,
                CyclistId = result.CyclistId,
                OverallTime = result.Time,
                OverallRank = result.Rank
            };

            ViewData["RaceId"] = new SelectList(_context.Races, "Id", "Name", result.RaceId);
            ViewData["CyclistId"] = new SelectList(_context.Cyclists, "Id", "UserName", result.CyclistId);
            return View(model);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Organiser")]
        public async Task<IActionResult> Edit(string id, RaceResultViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _context.Results.FindAsync(id);
                    if (result == null) return NotFound();

                    result.RaceId = model.RaceId;
                    result.CyclistId = model.CyclistId;
                    result.Time = model.OverallTime;
                    result.Rank = model.OverallRank;

                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Results.Any(r => r.Id == model.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["RaceId"] = new SelectList(_context.Races, "Id", "Name", model.RaceId);
            ViewData["CyclistId"] = new SelectList(_context.Cyclists, "Id", "UserName", model.CyclistId);
            return View(model);
        }

        // GET: Results/Delete/5
        [Authorize(Roles = "Admin,Organiser")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _context.Results
                .Include(r => r.Race)
                .Include(r => r.Cyclist)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (result == null) return NotFound();

            var model = new RaceResultViewModel
            {
                Id = result.Id,
                Race = result.Race.Name,
                CyclistName = result.Cyclist.UserName,
                OverallTime = result.Time,
                OverallRank = result.Rank
            };

            return View(model);
        }

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Organiser")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result != null)
            {
                _context.Results.Remove(result);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ResultExists(string id)
        {
            return _context.Results.Any(e => e.Id == id);
        }
    }
}
