using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CyclingRaces.Data;
using CyclingRaces.Data.Models;
using Microsoft.AspNetCore.Authorization;
using CyclingRaces.Data.ViewModels;

namespace CyclingRaces.Controllers
{
    [Authorize]
    public class HistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HistoryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            // Check if user is a Cyclist
            var cyclist = await _context.Cyclists
                .Include(c => c.Results)
                    .ThenInclude(p => p.Race)
                .FirstOrDefaultAsync(c => c.Id == userId);

            // Check if user is an Organiser
            var organiser = await _context.Organisers
                .Include(o => o.Races)
                .FirstOrDefaultAsync(o => o.Id == userId);

            var viewModel = new HistoryViewModel
            {
                StageResults = cyclist?.Results ?? new List<Result>(),
                CreatedRaces = organiser?.Races ?? new List<Race>()
            };

            return View(viewModel);
        }
    }
}