using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.AccessControl;
using System;
using CyclingRaces.Data;
using CyclingRaces.Data.Models;

public static class TestHelpers
{
    public static ApplicationDbContext GetEmptyDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Ensures a fresh DB for each test
            .Options;

        return new ApplicationDbContext(options);
    }

    public static ApplicationDbContext GetDbContextWithData()
    {
        var context = GetEmptyDbContext();

        // Seed sample data
        var organiser = new ApplicationUser { Id = "org1", UserName = "organiser1" };
        var race = new Race
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Test Race",
            Location = "Test City",
            Distance = 100,
            Date = DateTime.Today,
            Type = "Test",
            OrganiserId = organiser.Id
        };

        var team = new Team
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Team Rocket",
            Country = "USA",
            Manager = "Ash"
        };

        context.Users.Add(organiser);
        context.Races.Add(race);
        context.Teams.Add(team);
        context.SaveChanges();

        return context;
    }
}