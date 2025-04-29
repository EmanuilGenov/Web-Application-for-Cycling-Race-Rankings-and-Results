using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CyclingRaces.Data;
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
                // Ensure roles exist
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                EnsureRolesAsync(roleManager).Wait();

                if (context.Cyclists.Any())
                {
                    return; // DB has been seeded
                }

                // Seed Teams
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
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Green Trail", Country = "Australia", Manager = "Sophie Wright" },
                };
                context.Teams.AddRange(teams);

                // Seed Organisers
                var organisers = new List<Organiser>
                {
                    new Organiser { Id = Guid.NewGuid().ToString(), Name = "Global Cycling Org", Email = "info@gco.org", PasswordHash = "hash1" },
                    new Organiser { Id = Guid.NewGuid().ToString(), Name = "Pedal Masters", Email = "contact@pedalmasters.org", PasswordHash = "hash2" },
                    new Organiser { Id = Guid.NewGuid().ToString(), Name = "Ride Europe", Email = "events@rideeurope.com", PasswordHash = "hash3" },
                    new Organiser { Id = Guid.NewGuid().ToString(), Name = "Asia Spin League", Email = "admin@asl.asia", PasswordHash = "hash4" },
                    new Organiser { Id = Guid.NewGuid().ToString(), Name = "Velocity Events", Email = "support@velocity.events", PasswordHash = "hash5" },
                    new Organiser { Id = Guid.NewGuid().ToString(), Name = "Downhill Racing", Email = "team@downhill.org", PasswordHash = "hash6" },
                    new Organiser { Id = Guid.NewGuid().ToString(), Name = "Tour Cyclers", Email = "info@tourcyclers.com", PasswordHash = "hash7" },
                    new Organiser { Id = Guid.NewGuid().ToString(), Name = "Urban Wheelers", Email = "urban@wheelers.com", PasswordHash = "hash8" },
                    new Organiser { Id = Guid.NewGuid().ToString(), Name = "Mountain Push", Email = "hello@mountainpush.net", PasswordHash = "hash9" },
                    new Organiser { Id = Guid.NewGuid().ToString(), Name = "World Spin Org", Email = "contact@worldspin.org", PasswordHash = "hash10" },
                };
                context.Organisers.AddRange(organisers);

                var teamIds = teams.Select(t => t.Id).ToList();

                var cyclists = new List<Cyclist>
                {
                    new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "john.doe", Email = "john.doe@example.com", PasswordHash = "hash1", Nationality = "USA", DateOfBirth = new DateTime(1995, 5, 12), TeamId = teamIds[0] },
                    new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "emma.taylor", Email = "emma.t@example.com", PasswordHash = "hash2", Nationality = "UK", DateOfBirth = new DateTime(1993, 3, 22), TeamId = teamIds[1] },
                    new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "luc.bernard", Email = "luc.b@example.com", PasswordHash = "hash3", Nationality = "France", DateOfBirth = new DateTime(1990, 8, 10), TeamId = teamIds[2] },
                    new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "ali.farooq", Email = "ali.f@example.com", PasswordHash = "hash4", Nationality = "UAE", DateOfBirth = new DateTime(1998, 12, 1), TeamId = teamIds[3] },
                    new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "nils.hansen", Email = "nils.h@example.com", PasswordHash = "hash5", Nationality = "Norway", DateOfBirth = new DateTime(1992, 7, 14), TeamId = teamIds[4] },
                    new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "maria.gomez", Email = "maria.g@example.com", PasswordHash = "hash6", Nationality = "Colombia", DateOfBirth = new DateTime(1994, 6, 30), TeamId = teamIds[5] },
                    new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "hiro.yamada", Email = "hiro.y@example.com", PasswordHash = "hash7", Nationality = "Japan", DateOfBirth = new DateTime(1991, 9, 5), TeamId = teamIds[6] },
                    new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "oliver.lee", Email = "oliver.l@example.com", PasswordHash = "hash8", Nationality = "Canada", DateOfBirth = new DateTime(1996, 2, 18), TeamId = teamIds[7] },
                    new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "giulia.bianchi", Email = "giulia.b@example.com", PasswordHash = "hash9", Nationality = "Italy", DateOfBirth = new DateTime(1993, 4, 28), TeamId = teamIds[8] },
                    new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "jack.brown", Email = "jack.b@example.com", PasswordHash = "hash10", Nationality = "Australia", DateOfBirth = new DateTime(1997, 11, 11), TeamId = teamIds[9] },
                };
                context.Cyclists.AddRange(cyclists);

                var cyclistIds = cyclists.Select(c => c.Id).ToList();
                var organiserIds = organisers.Select(o => o.Id).ToList();

                // Seed Races
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

                var raceIds = races.Select(r => r.Id).ToList();

                // Seed Results
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

                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                SeedAdminUserAsync(userManager).Wait();

                context.SaveChanges();
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager)
        {
            string adminEmail = "admin@cyclingraces.com";
            string adminPassword = "Admin@123"; // Use a strong password

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
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
