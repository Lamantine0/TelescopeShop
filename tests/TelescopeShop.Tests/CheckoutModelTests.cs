using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TelescopeShop.Pages;

namespace TelescopeShop.Tests;

public class CheckoutModelTests
{
    [Fact]
    public async Task OnGet_ReturnsPageForExistingTelescope()
    {
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var model = CreateModel(context);

        var result = await model.OnGetAsync(1, CancellationToken.None);

        Assert.IsType<PageResult>(result);
        Assert.NotNull(model.Telescope);
    }

    [Fact]
    public async Task OnGet_ReturnsNotFoundForMissingTelescope()
    {
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var model = CreateModel(context);

        var result = await model.OnGetAsync(999, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task OnPost_ValidForm_AcceptsOrder()
    {
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var model = CreateModel(context);
        model.CustomerName = "Иван";
        model.Phone = "+7 999 000-00-00";

        var result = await model.OnPostAsync(1, CancellationToken.None);

        Assert.IsType<PageResult>(result);
        Assert.True(model.OrderAccepted);
    }

    [Fact]
    public async Task OnPost_InvalidForm_DoesNotAcceptOrder()
    {
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var model = CreateModel(context);
        model.PageContext.ModelState.AddModelError(nameof(model.CustomerName), "Укажите имя.");

        var result = await model.OnPostAsync(1, CancellationToken.None);

        Assert.IsType<PageResult>(result);
        Assert.False(model.OrderAccepted);
    }

    [Fact]
    public async Task OnPost_ReturnsNotFoundForMissingTelescope()
    {
        using var db = new TestDatabase();
        using var context = db.CreateContext();
        var model = CreateModel(context);
        model.CustomerName = "Иван";
        model.Phone = "+7 999 000-00-00";

        var result = await model.OnPostAsync(999, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    private static CheckoutModel CreateModel(TelescopeShop.Data.TelescopeDbContext context)
    {
        return new CheckoutModel(context)
        {
            PageContext = new PageContext()
        };
    }
}
