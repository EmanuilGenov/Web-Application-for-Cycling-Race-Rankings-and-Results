using Xunit;
using Microsoft.EntityFrameworkCore;
using CyclingRaces.Controllers;
using CyclingRaces.Data;
using CyclingRaces.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Moq;

public class RacesControllerTests
{
    private ApplicationDbContext GetDbContextWithSampleRaces()
    {
        var context = TestHelpers.GetEmptyDbContext();

        var organiser = new ApplicationUser { Id = "user1", UserName = "organiser1" };
        context.Users.Add(organiser);

        context.Races.AddRange(
            new Race { Id = "1", Name = "Alpha Race", Location = "Paris", Date = DateTime.Today.AddDays(1), Type = "Road", Distance = 100, OrganiserId = "user1" },
            new Race { Id = "2", Name = "Beta Race", Location = "London", Date = DateTime.Today, Type = "Track", Distance = 150, OrganiserId = "user1" }
        );

        context.SaveChanges();
        return context;
    }

    private RacesController GetController(ApplicationDbContext context)
    {
        var userManager = MockUserManager();
        return new RacesController(context, userManager.Object);
    }

    private Mock<UserManager<ApplicationUser>> MockUserManager()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        return new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
    }

    [Fact]
    public async Task Index_ReturnsAllRaces()
    {
        var context = GetDbContextWithSampleRaces();
        var controller = GetController(context);

        var result = await controller.Index(null, null, null, null) as ViewResult;
        var model = result.Model as List<Race>;

        Assert.Equal(2, model.Count);
    }

    [Fact]
    public async Task Index_FiltersByLocation()
    {
        var context = GetDbContextWithSampleRaces();
        var controller = GetController(context);

        var result = await controller.Index(null, null, "Paris", null) as ViewResult;
        var model = result.Model as List<Race>;

        Assert.Single(model);
        Assert.Equal("Paris", model.First().Location);
    }

    [Fact]
    public async Task Index_SortsByNameDescending()
    {
        var context = GetDbContextWithSampleRaces();
        var controller = GetController(context);

        var result = await controller.Index("name_desc", null, null, null) as ViewResult;
        var model = result.Model as List<Race>;

        Assert.Equal("Beta Race", model.First().Name);
    }

    [Fact]
    public async Task Details_ReturnsRace_WhenIdIsValid()
    {
        var context = GetDbContextWithSampleRaces();
        var controller = GetController(context);

        var result = await controller.Details("1") as ViewResult;
        var model = result.Model as Race;

        Assert.NotNull(model);
        Assert.Equal("Alpha Race", model.Name);
    }

    [Fact]
    public async Task Details_ReturnsNotFound_WhenIdIsNull()
    {
        var controller = GetController(TestHelpers.GetEmptyDbContext());
        var result = await controller.Details(null);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_ReturnsNotFound_WhenRaceNotFound()
    {
        var controller = GetController(TestHelpers.GetEmptyDbContext());
        var result = await controller.Details("nonexistent");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteConfirmed_RemovesRace()
    {
        var context = GetDbContextWithSampleRaces();
        var controller = GetController(context);

        var result = await controller.DeleteConfirmed("1");

        Assert.Equal(1, context.Races.Count());
        Assert.DoesNotContain(context.Races, r => r.Id == "1");
    }

    [Fact]
    public async Task DeleteConfirmed_IgnoresInvalidId()
    {
        var context = GetDbContextWithSampleRaces();
        var controller = GetController(context);

        var result = await controller.DeleteConfirmed("invalid");

        Assert.Equal(2, context.Races.Count());
    }

    [Fact]
    public async Task Create_Post_RedirectsOnSuccess()
    {
        var context = TestHelpers.GetEmptyDbContext();
        var controller = GetController(context);

        var race = new Race
        {
            Name = "New Race",
            Location = "Berlin",
            Type = "Road",
            Date = DateTime.Today,
            Distance = 50,
            OrganiserId = "someOrg"
        };

        controller.ModelState.Clear();
        var result = await controller.Create(race) as RedirectToActionResult;

        Assert.Equal("Index", result.ActionName);
        Assert.Single(context.Races);
    }

    [Fact]
    public async Task Create_Post_ReturnsViewIfModelInvalid()
    {
        var context = TestHelpers.GetEmptyDbContext();
        var controller = GetController(context);

        controller.ModelState.AddModelError("Name", "Required");

        var result = await controller.Create(new Race()) as ViewResult;
        Assert.IsType<ViewResult>(result);
    }
}
