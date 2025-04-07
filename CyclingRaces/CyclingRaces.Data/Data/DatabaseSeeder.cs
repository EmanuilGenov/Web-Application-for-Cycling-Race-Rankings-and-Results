using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CyclingRaces.Data;
using CyclingRaces.Data.Models;
using Microsoft.EntityFrameworkCore;

public class SeedDataService
{
	private readonly ApplicationDbContext _context;

	public SeedDataService(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task SeedDataAsync()
	{
		if (_context.Cyclists.Any()) return; // Prevent duplicate seeding

		// Seed Teams
		var teams = new List<Team>
		{
			new Team { Id = Guid.NewGuid().ToString(), Name = "Thunder Riders", Country = "Velandia", Manager = "Jonas Flint" },
			new Team { Id = Guid.NewGuid().ToString(), Name = "Iron Pedals", Country = "Crestoria", Manager = "Lena Voss" },
			new Team { Id = Guid.NewGuid().ToString(), Name = "Storm Breakers", Country = "Zephyria", Manager = "Marco Diaz" }
		};
		_context.Teams.AddRange(teams);
		await _context.SaveChangesAsync();

		// Seed Cyclists
		var cyclists = new List<Cyclist>
		{
			new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "Arlen Tavis", Email = "arlen@cycling.com", PasswordHash = "HASHED_PASSWORD", DateOfBirth = new DateTime(1995, 6, 15), Nationality = "Velandian", TeamId = teams[0].Id },
			new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "Bren Loric", Email = "bren@cycling.com", PasswordHash = "HASHED_PASSWORD", DateOfBirth = new DateTime(1998, 9, 3), Nationality = "Crestorian", TeamId = teams[1].Id },
			new Cyclist { Id = Guid.NewGuid().ToString(), UserName = "Calla Vey", Email = "calla@cycling.com", PasswordHash = "HASHED_PASSWORD", DateOfBirth = new DateTime(2001, 4, 22), Nationality = "Zephyrian", TeamId = teams[2].Id }
		};
		_context.Cyclists.AddRange(cyclists);
		await _context.SaveChangesAsync();

		// Seed Organisers
		var organisers = new List<Organiser>
		{
			new Organiser { Name = "Elias Norwood", Email = "elias@races.com", PasswordHash = "HASHED_PASSWORD" },
			new Organiser { Name = "Faye Hollis", Email = "faye@races.com", PasswordHash = "HASHED_PASSWORD" }
		};
		_context.Organisers.AddRange(organisers);
		await _context.SaveChangesAsync();

		// Seed Races
		var races = new List<Race>
		{
			new Race { Id = Guid.NewGuid().ToString(), Name = "Skyline Dash", Location = "Velandia City", Date = DateTime.Today.AddDays(10), Type = "Road", Distance = 120.5, OrganiserName = organisers[0].Name },
			new Race { Id = Guid.NewGuid().ToString(), Name = "Sunset Circuit", Location = "Crestoria Hills", Date = DateTime.Today.AddDays(20), Type = "Track", Distance = 85.2, OrganiserName = organisers[1].Name }
		};
		_context.Races.AddRange(races);
		await _context.SaveChangesAsync();

		// Seed Stages
		var stages = new List<Stage>
		{
			new Stage { Id = Guid.NewGuid().ToString(), RaceId = races[0].Id, StageNumber = 1, Distance = 60.2, StartLocation = "Downtown", EndLocation = "Summit Peak" },
			new Stage { Id = Guid.NewGuid().ToString(), RaceId = races[0].Id, StageNumber = 2, Distance = 60.3, StartLocation = "Summit Peak", EndLocation = "City Center" }
		};
		_context.Stages.AddRange(stages);
		await _context.SaveChangesAsync();

		// Seed Participations
		var participations = new List<Participation>
		{
			new Participation { Id = Guid.NewGuid().ToString(), RaceId = races[0].Id, CyclistId = cyclists[0].Id, OverallTime = TimeSpan.FromHours(3.5), OverallRank = 1 },
			new Participation { Id = Guid.NewGuid().ToString(), RaceId = races[0].Id, CyclistId = cyclists[1].Id, OverallTime = TimeSpan.FromHours(3.7), OverallRank = 2 }
		};
		_context.Participations.AddRange(participations);
		await _context.SaveChangesAsync();

		// Seed Results
		var results = new List<Result>
		{
			new Result { Id = Guid.NewGuid().ToString(), StageId = stages[0].Id, CyclistId = cyclists[0].Id, Time = TimeSpan.FromMinutes(90), Rank = 1 },
			new Result { Id = Guid.NewGuid().ToString(), StageId = stages[0].Id, CyclistId = cyclists[1].Id, Time = TimeSpan.FromMinutes(95), Rank = 2 }
		};
		_context.Results.AddRange(results);
		await _context.SaveChangesAsync();
	}
}
