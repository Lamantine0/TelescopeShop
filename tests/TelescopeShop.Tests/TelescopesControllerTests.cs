using Microsoft.AspNetCore.Mvc;
using TelescopeShop.Controllers;
using TelescopeShop.DTOs;

namespace TelescopeShop.Tests;

public class TelescopesControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsSeededTelescopes()
    {
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var controller = new TelescopesController(context);

        var result = await controller.GetAll(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var telescopes = Assert.IsAssignableFrom<IEnumerable<TelescopeDto>>(ok.Value).ToList();
        Assert.Equal(10, telescopes.Count);
    }

    [Fact]
    public async Task GetById_ReturnsTelescope()
    {
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var controller = new TelescopesController(context);

        var result = await controller.GetById(1, CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var telescope = Assert.IsType<TelescopeDto>(ok.Value);
        Assert.Equal(1, telescope.Id);
        Assert.Equal("Celestron", telescope.Manufacturer);
    }

    [Fact]
    public async Task GetById_ReturnsNotFoundForMissingTelescope()
    {
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var controller = new TelescopesController(context);

        var result = await controller.GetById(999, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_AddsTelescopeAndReturnsCreated()
    {
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var controller = new TelescopesController(context);

        var result = await controller.Create(CreateDto(), CancellationToken.None);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        var telescope = Assert.IsType<TelescopeDto>(created.Value);
        Assert.True(telescope.Id > 0);

        using var verifyContext = db.CreateContext();
        var stored = await verifyContext.Telescopes.FindAsync(telescope.Id);
        Assert.NotNull(stored);
        Assert.Equal("Test Manufacturer", stored.Manufacturer);
        Assert.Equal(12345m, stored.Price);
    }

    [Fact]
    public async Task Update_ChangesTelescope()
    {
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var controller = new TelescopesController(context);

        var dto = CreateDto();
        var result = await controller.Update(1, dto, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);

        using var verifyContext = db.CreateContext();
        var stored = await verifyContext.Telescopes.FindAsync(1);
        Assert.NotNull(stored);
        Assert.Equal(dto.Manufacturer, stored.Manufacturer);
        Assert.Equal(dto.Price, stored.Price);
    }

    [Fact]
    public async Task Update_ReturnsNotFoundForMissingTelescope()
    {
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var controller = new TelescopesController(context);

        var result = await controller.Update(999, CreateDto(), CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_RemovesTelescope()
    {
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var controller = new TelescopesController(context);

        var result = await controller.Delete(1, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);

        using var verifyContext = db.CreateContext();
        Assert.Null(await verifyContext.Telescopes.FindAsync(1));
    }

    [Fact]
    public async Task Delete_ReturnsNotFoundForMissingTelescope()
    {
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var controller = new TelescopesController(context);

        var result = await controller.Delete(999, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    private static TelescopeDto CreateDto()
    {
        return new TelescopeDto
        {
            Manufacturer = "Test Manufacturer",
            TripodHeight = 1200,
            Aperture = 150,
            Seeker = "Test finder",
            BarlowLensIncluded = true,
            MaximumUsefulMagnification = 300,
            FocalLength = 1200,
            Description = "Test description",
            Price = 12345m
        };
    }
}
