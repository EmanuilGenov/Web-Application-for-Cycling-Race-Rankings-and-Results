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

            var cyclist = await _context.Cyclists
                .Include(o => o.Results)
                .FirstOrDefaultAsync(o => o.UserId == userId);


            var organiser = await _context.Organisers
                .Include(o => o.Races)
                .FirstOrDefaultAsync(o => o.UserId == userId); 

            var viewModel = new HistoryViewModel
            {
                StageResults = cyclist?.Results?.ToList() ?? new List<Result>(),
                CreatedRaces = organiser?.Races?.ToList() ?? new List<Race>()
            };

            return View(viewModel);
        }
    }
}
