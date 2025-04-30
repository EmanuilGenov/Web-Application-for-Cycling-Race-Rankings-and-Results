using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CyclingRaces.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CyclingRaces.Data.Data
{
    public class DatabaseSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                EnsureRolesAsync(roleManager).Wait();

                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                if (context.Teams.Any())
                    return; // Already seeded

                var teams = new List<Team>
                {
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Speed Hawks", Country = "USA", Manager = "Alice Johnson" },
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Pedal Kings", Country = "UK", Manager = "Robert Smith" },
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Alpine Racers", Country = "France", Manager = "Marie Dubois" },
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Desert Cyclones", Country = "UAE", Manager = "Khalid Mansour" },
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Nordic Blitz", Country = "Norway", Manager = "Lars Johansen" },
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Andean Riders", Country = "Colombia", Manager = "Carlos Rivera" },
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Sakura Speed", Country = "Japan", Manager = "Akira Tanaka" },
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Crimson Wheels", Country = "Canada", Manager = "Emily Carter" },
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Velox Tigers", Country = "Italy", Manager = "Giovanni Rossi" },
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Green Trail", Country = "Australia", Manager = "Sophie Wright" }
                };
                context.Teams.AddRange(teams);
                context.SaveChanges();

                var teamIds = teams.Select(t => t.Id).ToList();

                var organisers = new List<Organiser>
                {
                    new Organiser { UserName = "GlobalCyclingOrg", Email = "info@gco.org", Nationality = "USA", DateOfBirth = new DateTime(1980, 1, 1) },
                    new Organiser { UserName = "PedalMasters", Email = "contact@pedalmasters.org", Nationality = "UK", DateOfBirth = new DateTime(1980, 1, 1) },
                    new Organiser { UserName = "RideEurope", Email = "events@rideeurope.com", Nationality = "France", DateOfBirth = new DateTime(1980, 1, 1) },
                    new Organiser { UserName = "AsiaSpinLeague", Email = "admin@asl.asia", Nationality = "Japan", DateOfBirth = new DateTime(1980, 1, 1) },
                    new Organiser { UserName = "VelocityEvents", Email = "support@velocity.events", Nationality = "UAE", DateOfBirth = new DateTime(1980, 1, 1) },
                    new Organiser { UserName = "DownhillRacing", Email = "team@downhill.org", Nationality = "Canada", DateOfBirth = new DateTime(1980, 1, 1) },
                    new Organiser { UserName = "TourCyclers", Email = "info@tourcyclers.com", Nationality = "Italy", DateOfBirth = new DateTime(1980, 1, 1) },
                    new Organiser { UserName = "UrbanWheelers", Email = "urban@wheelers.com", Nationality = "Norway", DateOfBirth = new DateTime(1980, 1, 1) },
                    new Organiser { UserName = "MountainPush", Email = "hello@mountainpush.net", Nationality = "Colombia", DateOfBirth = new DateTime(1980, 1, 1) },
                    new Organiser { UserName = "WorldSpinOrg", Email = "contact@worldspin.org", Nationality = "Australia", DateOfBirth = new DateTime(1980, 1, 1) }
                };

                foreach (var organiser in organisers)
                {
                    organiser.EmailConfirmed = true;
                    var result = userManager.CreateAsync(organiser, "Organiser@123").Result;
                    if (result.Succeeded)
                        userManager.AddToRoleAsync(organiser, "Organiser").Wait();
                    else
                        throw new Exception($"Failed to create organiser {organiser.UserName}: " +
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                var cyclists = new List<Cyclist>
                {
                    new Cyclist { UserName = "john.doe", Email = "john.doe@example.com", Nationality = "USA", DateOfBirth = new DateTime(1995, 5, 12), TeamId = teamIds[0] },
                    new Cyclist { UserName = "emma.taylor", Email = "emma.t@example.com", Nationality = "UK", DateOfBirth = new DateTime(1993, 3, 22), TeamId = teamIds[1] },
                    new Cyclist { UserName = "luc.bernard", Email = "luc.b@example.com", Nationality = "France", DateOfBirth = new DateTime(1990, 8, 10), TeamId = teamIds[2] },
                    new Cyclist { UserName = "ali.farooq", Email = "ali.f@example.com", Nationality = "UAE", DateOfBirth = new DateTime(1998, 12, 1), TeamId = teamIds[3] },
                    new Cyclist { UserName = "nils.hansen", Email = "nils.h@example.com", Nationality = "Norway", DateOfBirth = new DateTime(1992, 7, 14), TeamId = teamIds[4] }
                };

                foreach (var cyclist in cyclists)
                {
                    cyclist.EmailConfirmed = true;
                    var result = userManager.CreateAsync(cyclist, "Cyclist@123").Result;
                    if (result.Succeeded)
                        userManager.AddToRoleAsync(cyclist, "Cyclist").Wait();
                    else
                        throw new Exception($"Failed to create cyclist {cyclist.UserName}: " +
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                context.SaveChanges();

                var organiserIds = context.Users.Where(u => u is Organiser).Select(u => u.Id).ToList();

                var races = new List<Race>();
                for (int i = 0; i < 10; i++)
                {
                    races.Add(new Race
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = $"Race {i + 1}",
                        Location = $"City {i + 1}",
                        Date = DateTime.Today.AddDays(-i * 7),
                        Type = (i % 2 == 0) ? "Road" : "Mountain",
                        Distance = 100 + i * 10,
                        OrganiserId = organiserIds[i % organiserIds.Count]
                    });
                }
                context.Races.AddRange(races);
                context.SaveChanges();

                var raceIds = races.Select(r => r.Id).ToList();
                var cyclistIds = context.Users.Where(u => u is Cyclist).Select(u => u.Id).ToList();

                var results = new List<Result>();
                int rank = 1;
                foreach (var raceId in raceIds)
                {
                    foreach (var cyclistId in cyclistIds.Take(5))
                    {
                        results.Add(new Result
                        {
                            Id = Guid.NewGuid().ToString(),
                            CyclistId = cyclistId,
                            RaceId = raceId,
                            Time = TimeSpan.FromHours(2 + rank * 0.1),
                            Rank = rank++,
                            IsVolunteer = rank % 5 == 0
                        });
                    }
                }
                context.Results.AddRange(results);
                context.SaveChanges();

                SeedAdminUserAsync(userManager).Wait();
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            string adminEmail = "admin@cyclingraces.com";
            string adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Nationality = "Bulgarian",
                    DateOfBirth = new DateTime(2007, 1, 23)
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }

        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = new[] { "Cyclist", "Organiser", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
