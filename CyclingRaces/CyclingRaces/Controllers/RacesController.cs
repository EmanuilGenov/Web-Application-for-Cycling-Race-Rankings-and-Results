using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CyclingRaces.Data;
using CyclingRaces.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CyclingRaces.Controllers
{
    public class RacesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RacesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Races
        public async Task<IActionResult> Index(string sortOrder, string raceTypeFilter, string locationFilter, string searchString)
        {
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["LocationSortParm"] = sortOrder == "Location" ? "Location_desc" : "Location";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "Date_desc" : "Date";
            ViewData["TypeSortParm"] = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewData["DistanceSortParm"] = sortOrder == "Distance" ? "Distance_desc" : "Distance";
            ViewData["OrganiserSortParm"] = sortOrder == "Organiser" ? "Organiser_desc" : "Organiser";

            ViewData["CurrentSort"] = sortOrder ?? "";


            var races = _context.Races.Include(r => r.Organiser).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                races = races.Where(r => r.Name.Contains(searchString) || r.Organiser.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(raceTypeFilter))
            {
                races = races.Where(r => r.Type == raceTypeFilter);
            }

            if (!string.IsNullOrEmpty(locationFilter))
            {
                races = races.Where(r => r.Location.Contains(locationFilter));
            }

            races = sortOrder switch
            {
                "name_desc" => races.OrderByDescending(r => r.Name),
                "Location" => races.OrderBy(r => r.Location),
                "location_desc" => races.OrderByDescending(r => r.Location),
                "Date" => races.OrderBy(r => r.Date),
                "date_desc" => races.OrderByDescending(r => r.Date),
                "Type" => races.OrderBy(r => r.Type),
                "type_desc" => races.OrderByDescending(r => r.Type),
                "Distance" => races.OrderBy(r => r.Distance),
                "distance_desc" => races.OrderByDescending(r => r.Distance),
                "Organiser" => races.OrderBy(r => r.Organiser.Name),
                "organiser_desc" => races.OrderByDescending(r => r.Organiser.Name),
                _ => races.OrderBy(r => r.Name),
            };

            ViewData["RaceTypes"] = new SelectList(await _context.Races.Select(r => r.Type).Distinct().ToListAsync(), raceTypeFilter);
            ViewData["Locations"] = new SelectList(await _context.Races.Select(r => r.Location).Distinct().ToListAsync(), locationFilter);
            ViewData["SearchString"] = searchString;

            return View(await races.ToListAsync());
        }

        // GET: Races/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Races
                .Include(r => r.Organiser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (race == null)
            {
                return NotFound();
            }

            return View(race);
        }

        // GET: Races/Create
        [Authorize(Roles ="Admin,Organiser")]
        public IActionResult Create()
        {
            ViewData["OrganiserName"] = new SelectList(_context.Organisers, "Id", "Name");
            return View();
        }

        // POST: Races/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Organiser")]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Date,Type,Distance,OrganiserName")] Race race)
        {
            if (ModelState.IsValid)
            {
                _context.Add(race);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrganiserName"] = new SelectList(_context.Organisers, "Id", "Name", race.OrganiserId);
            return View(race);
        }

        // GET: Races/Edit/5
        [Authorize(Roles = "Admin,Organiser")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Races.FindAsync(id);
            if (race == null)
            {
                return NotFound();
            }
            ViewData["OrganiserName"] = new SelectList(_context.Organisers, "Id", "Name", race.OrganiserId);
            return View(race);
        }

        // POST: Races/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Organiser")]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Location,Date,Type,Distance,OrganiserName")] Race race)
        {
            if (id != race.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(race);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceExists(race.Id))
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
            ViewData["OrganiserName"] = new SelectList(_context.Organisers, "Id", "Id", race.OrganiserId);
            return View(race);
        }

        // GET: Races/Delete/5
        [Authorize(Roles = "Admin,Organiser")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var race = await _context.Races
                .Include(r => r.Organiser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (race == null)
            {
                return NotFound();
            }

            return View(race);
        }

        // POST: Races/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Organiser")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var race = await _context.Races.FindAsync(id);
            if (race != null)
            {
                _context.Races.Remove(race);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RaceExists(string id)
        {
            return _context.Races.Any(e => e.Id == id);
        }
    }
}
