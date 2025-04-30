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
        private readonly UserManager<ApplicationUser> _userManager;

        public RacesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Races
        public async Task<IActionResult> Index(string sortOrder, string raceTypeFilter, string locationFilter, string searchString)
        {
            sortOrder = sortOrder?.ToLower() ?? "";

            ViewData["NameSortParm"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["LocationSortParm"] = sortOrder == "location" ? "location_desc" : "location";
            ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["TypeSortParm"] = sortOrder == "type" ? "type_desc" : "type";
            ViewData["DistanceSortParm"] = sortOrder == "distance" ? "distance_desc" : "distance";
            ViewData["OrganiserSortParm"] = sortOrder == "organiser" ? "organiser_desc" : "organiser";

            ViewData["CurrentSort"] = sortOrder;

            var races = _context.Races.Include(r => r.Organiser).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                races = races.Where(r => r.Name.Contains(searchString) || r.Organiser.UserName.Contains(searchString));
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
                "location" => races.OrderBy(r => r.Location),
                "location_desc" => races.OrderByDescending(r => r.Location),
                "date" => races.OrderBy(r => r.Date),
                "date_desc" => races.OrderByDescending(r => r.Date),
                "type" => races.OrderBy(r => r.Type),
                "type_desc" => races.OrderByDescending(r => r.Type),
                "distance" => races.OrderBy(r => r.Distance),
                "distance_desc" => races.OrderByDescending(r => r.Distance),
                "organiser" => races.OrderBy(r => r.Organiser.UserName),
                "organiser_desc" => races.OrderByDescending(r => r.Organiser.UserName),
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
        [Authorize(Roles = "Admin,Organiser")]
        public IActionResult Create()
        {
            ViewData["OrganiserId"] = new SelectList(_context.Organisers, "Id", "Name");
            return View();
        }

        // POST: Races/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Organiser")]
        public async Task<IActionResult> Create([Bind("Name,Location,Date,Type,Distance,OrganiserId")] Race race)
        {
            if (ModelState.IsValid)
            {
                race.Id = Guid.NewGuid().ToString(); // Auto-generate race ID

                if (!User.IsInRole("Admin"))
                {
                    // Set organiser based on the current logged-in user
                    var user = await _userManager.GetUserAsync(User);
                    var organiser = await _context.Organisers.FirstOrDefaultAsync(o => o.Id == user.Id);

                    if (organiser == null)
                    {
                        ModelState.AddModelError("", "Current user is not linked to an organiser account.");
                        return View(race);
                    }

                    race.OrganiserId = organiser.Id;
                }

                _context.Add(race);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (User.IsInRole("Admin"))
            {
                ViewData["OrganiserId"] = new SelectList(_context.Organisers, "Id", "Name", race.OrganiserId);
            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Cyclist")]
        public async Task<IActionResult> RegisterForRace(string raceId)
        {
            var user = await _userManager.GetUserAsync(User);

            var participation = new Result
            {
                Id = Guid.NewGuid().ToString(),
                CyclistId = user.Id,
                RaceId = raceId,
                IsVolunteer = false
            };

            _context.Results.Add(participation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Cyclist")]
        public async Task<IActionResult> RegisterAsVolunteer(string raceId)
        {
            var user = await _userManager.GetUserAsync(User);

            var participation = new Result
            {
                Id = Guid.NewGuid().ToString(),
                CyclistId = user.Id,
                RaceId = raceId,
                IsVolunteer = true
            };

            _context.Results.Add(participation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
