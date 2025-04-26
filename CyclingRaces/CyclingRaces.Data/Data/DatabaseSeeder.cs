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
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Thunder Riders", Country = "Velandia", Manager = "Jonas Flint" },
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Iron Pedals", Country = "Crestoria", Manager = "Lena Voss" },
                    new Team { Id = Guid.NewGuid().ToString(), Name = "Storm Breakers", Country = "Zephyria", Manager = "Marco Diaz" }
                };
                context.Teams.AddRange(teams);
                context.SaveChanges();

                // Seed Cyclists
                var cyclists = new List<Cyclist>
                {
                    new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "Arlen Tavis", Email = "arlen@cycling.com", PasswordHash = "HASHED_PASSWORD", DateOfBirth = new DateTime(1995, 6, 15), Nationality = "Velandian", TeamId = teams[0].Id },
                    new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "Bren Loric", Email = "bren@cycling.com", PasswordHash = "HASHED_PASSWORD", DateOfBirth = new DateTime(1998, 9, 3), Nationality = "Crestorian", TeamId = teams[1].Id },
                    new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "Calla Vey", Email = "calla@cycling.com", PasswordHash = "HASHED_PASSWORD", DateOfBirth = new DateTime(2001, 4, 22), Nationality = "Zephyrian", TeamId = teams[2].Id }
                };
                context.Cyclists.AddRange(cyclists);
                context.SaveChanges();

                // Seed Organisers
                var organisers = new List<Organiser>
                {
                    new Organiser { Id = Guid.NewGuid().ToString(), Name = "Elias Norwood", Email = "elias@races.com", PasswordHash = "HASHED_PASSWORD" },
                    new Organiser { Id = Guid.NewGuid().ToString(), Name = "Faye Hollis", Email = "faye@races.com", PasswordHash = "HASHED_PASSWORD" }
                };
                context.Organisers.AddRange(organisers);
                context.SaveChanges();

                // Seed Races
                var races = new List<Race>
                {
                    new Race { Id = Guid.NewGuid().ToString(), Name = "Skyline Dash", Location = "Velandia City", Date = DateTime.Today.AddDays(10), Type = "Road", Distance = 120.5, OrganiserId = organisers[0].Id },
                    new Race { Id = Guid.NewGuid().ToString(), Name = "Sunset Circuit", Location = "Crestoria Hills", Date = DateTime.Today.AddDays(20), Type = "Track", Distance = 85.2, OrganiserId = organisers[1].Id }
                };
                context.Races.AddRange(races);
                context.SaveChanges();

                // Seed Stages
                var stages = new List<Stage>
                {
                    new Stage { Id = Guid.NewGuid().ToString(), RaceId = races[0].Id, StageNumber = 1, Distance = 60.2, StartLocation = "Downtown", EndLocation = "Summit Peak" },
                    new Stage { Id = Guid.NewGuid().ToString(), RaceId = races[0].Id, StageNumber = 2, Distance = 60.3, StartLocation = "Summit Peak", EndLocation = "City Center" }
                };
                context.Stages.AddRange(stages);
                context.SaveChanges();

                // Seed Participations
                var participations = new List<Participation>
                {
                    new Participation { Id = Guid.NewGuid().ToString(), RaceId = races[0].Id, CyclistId = cyclists[0].Id, OverallTime = TimeSpan.FromHours(3.5), OverallRank = 1 },
                    new Participation { Id = Guid.NewGuid().ToString(), RaceId = races[0].Id, CyclistId = cyclists[1].Id, OverallTime = TimeSpan.FromHours(3.7), OverallRank = 2 }
                };
                context.Participations.AddRange(participations);
                context.SaveChanges();

                // Seed Results
                var results = new List<Result>
                {
                    new Result { Id = Guid.NewGuid().ToString(), StageId = stages[0].Id, CyclistId = cyclists[0].Id, Time = TimeSpan.FromMinutes(90), Rank = 1 },
                    new Result { Id = Guid.NewGuid().ToString(), StageId = stages[0].Id, CyclistId = cyclists[1].Id, Time = TimeSpan.FromMinutes(95), Rank = 2 }
                };
                context.Results.AddRange(results);
                context.SaveChanges();
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