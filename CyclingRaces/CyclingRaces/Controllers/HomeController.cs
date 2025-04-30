using CyclingRaces.Data;
using CyclingRaces.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CyclingRaces.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var featuredRaces = await _context.Races
                .OrderByDescending(r => r.Date)
                .Take(3)
                .ToListAsync();

            return View(featuredRaces);
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
