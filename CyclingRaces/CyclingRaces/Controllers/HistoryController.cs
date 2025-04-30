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
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge(); // Or RedirectToAction("Login", "Account");
            }

            var viewModel = new HistoryViewModel();

            // Check if user is in the "Cyclist" role
            if (await _userManager.IsInRoleAsync(user, "Cyclist"))
            {
                viewModel.StageResults = await _context.Results
                    .Include(r => r.Race)
                    .Where(r => r.CyclistId == user.Id)
                    .ToListAsync();
            }

            // Check if user is in the "Organiser" role
            if (await _userManager.IsInRoleAsync(user, "Organiser"))
            {
                viewModel.CreatedRaces = await _context.Races
                    .Where(r => r.OrganiserId == user.Id)
                    .ToListAsync();
            }

            return View(viewModel);
        }
    }
}
