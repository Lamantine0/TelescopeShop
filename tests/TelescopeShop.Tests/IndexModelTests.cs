using TelescopeShop.Models;
using TelescopeShop.Pages;

namespace TelescopeShop.Tests;

public class IndexModelTests
{
    private static async Task<IndexModel> RunAsync(TestDatabase db, Action<IndexModel>? configure = null)
    {
        using var context = db.CreateContext();
        var model = new IndexModel(context);

        configure?.Invoke(model);
        await model.OnGetAsync(CancellationToken.None);
        return model;
    }

    [Fact]
    public async Task ManufacturerFilter_MatchesSubstringCaseInsensitively()
    {
        using var db = new TestDatabase();

        var model = await RunAsync(db, m => m.Manufacturer = "sky");

        Assert.NotEmpty(model.Telescopes);
        Assert.All(model.Telescopes, t => Assert.Equal("Sky-Watcher", t.Manufacturer));
    }

    [Fact]
    public async Task SearchFilter_MatchesManufacturer()
    {
        using var db = new TestDatabase();

        var model = await RunAsync(db, m => m.Search = "orion");

        Assert.Equal(2, model.Telescopes.Count);
        Assert.All(model.Telescopes, t => Assert.Equal("Orion", t.Manufacturer));
    }

    [Fact]
    public async Task ApertureRange_FiltersBothBounds()
    {
        using var db = new TestDatabase();

        var model = await RunAsync(db, m =>
        {
            m.ApertureMin = 150;
            m.ApertureMax = 210;
        });

        Assert.NotEmpty(model.Telescopes);
        Assert.All(model.Telescopes, t =>
        {
            Assert.InRange(t.Aperture, 150, 210);
        });
    }

    [Fact]
    public async Task FocalRange_FiltersBothBounds()
    {
        using var db = new TestDatabase();

        var model = await RunAsync(db, m =>
        {
            m.FocalMin = 1000;
            m.FocalMax = 1500;
        });

        Assert.NotEmpty(model.Telescopes);
        Assert.All(model.Telescopes, t =>
        {
            Assert.InRange(t.FocalLength, 1000, 1500);
        });
    }

    [Fact]
    public async Task BarlowFilter_True_ReturnsOnlyTelescopesWithBarlow()
    {
        using var db = new TestDatabase();

        var model = await RunAsync(db, m => m.Barlow = true);

        Assert.NotEmpty(model.Telescopes);
        Assert.All(model.Telescopes, t => Assert.True(t.BarlowLensIncluded));
    }

    [Fact]
    public async Task SortPriceAscending_PutsZeroPriceLast()
    {
        using var db = new TestDatabase();
        db.AddTelescope(CreateTelescope(100, price: 0));

        var model = await RunAsync(db, m => m.Sort = "price-asc");

        Assert.Equal(0m, model.Telescopes[^1].Price);

        var paidPrices = model.Telescopes
            .Where(t => t.Price > 0)
            .Select(t => t.Price)
            .ToList();

        Assert.Equal(paidPrices.OrderBy(p => p), paidPrices);
    }

    [Fact]
    public async Task SortPriceDescending_PutsZeroPriceLast()
    {
        using var db = new TestDatabase();
        db.AddTelescope(CreateTelescope(100, price: 0));

        var model = await RunAsync(db, m => m.Sort = "price-desc");

        Assert.Equal(129990m, model.Telescopes[0].Price);
        Assert.Equal(0m, model.Telescopes[^1].Price);

        var paidPrices = model.Telescopes
            .Where(t => t.Price > 0)
            .Select(t => t.Price)
            .ToList();

        Assert.Equal(paidPrices.OrderByDescending(p => p), paidPrices);
    }

    [Fact]
    public async Task SortApertureDescending_ReturnsLargestFirst()
    {
        using var db = new TestDatabase();

        var model = await RunAsync(db, m => m.Sort = "aperture-desc");

        Assert.Equal(254, model.Telescopes[0].Aperture);
        Assert.Equal(70, model.Telescopes[^1].Aperture);
    }

    [Fact]
    public async Task ActiveFilters_BuildRemoveUrlsPreservingOtherParameters()
    {
        using var db = new TestDatabase();

        var model = await RunAsync(db, m =>
        {
            m.Search = "Celestron";
            m.Manufacturer = "Celestron";
            m.ApertureMin = 100;
            m.Barlow = false;
            m.Sort = "price-desc";
        });

        Assert.Equal(4, model.ActiveFilters.Count);

        var manufacturerChip = model.ActiveFilters.Single(f => f.Label.StartsWith("Фирма:"));
        Assert.DoesNotContain("manufacturer=", manufacturerChip.RemoveUrl);
        Assert.Contains("search=Celestron", manufacturerChip.RemoveUrl);
        Assert.Contains("apertureMin=100", manufacturerChip.RemoveUrl);
        Assert.Contains("barlow=false", manufacturerChip.RemoveUrl);
        Assert.Contains("sort=price-desc", manufacturerChip.RemoveUrl);

        Assert.Equal("/?sort=price-desc", model.ClearFiltersUrl);
    }

    [Fact]
    public async Task NoFilters_ProducesNoChipsAndNoClearUrl()
    {
        using var db = new TestDatabase();

        var model = await RunAsync(db);

        Assert.Empty(model.ActiveFilters);
        Assert.Null(model.ClearFiltersUrl);
    }

    private static Telescope CreateTelescope(int id, decimal price)
    {
        return new Telescope
        {
            Id = id,
            Manufacturer = "Test",
            TripodHeight = 1000,
            Aperture = 100,
            Seeker = "Test",
            BarlowLensIncluded = false,
            MaximumUsefulMagnification = 200,
            FocalLength = 1000,
            Description = "Test",
            Price = price
        };
    }
}
