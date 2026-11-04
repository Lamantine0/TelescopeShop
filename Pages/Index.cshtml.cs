using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TelescopeShop.Data;
using TelescopeShop.Models;

namespace TelescopeShop.Pages;

public class IndexModel : PageModel
{
    private readonly TelescopeDbContext _context;

    public IndexModel(TelescopeDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Telescope> Telescopes { get; private set; } = [];

    public IReadOnlyList<string> Manufacturers { get; private set; } = [];

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Manufacturer { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? ApertureMin { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? ApertureMax { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? FocalMin { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? FocalMax { get; set; }

    /// <summary>null — все, true — только с линзой, false — только без неё.</summary>
    [BindProperty(SupportsGet = true)]
    public bool? Barlow { get; set; }

    public bool HasSearch => !string.IsNullOrWhiteSpace(Search);

    public bool HasManufacturer => !string.IsNullOrWhiteSpace(Manufacturer);

    public bool HasFilters => HasSearch || HasManufacturer
        || ApertureMin.HasValue || ApertureMax.HasValue
        || FocalMin.HasValue || FocalMax.HasValue
        || Barlow.HasValue;

    /// <summary>Число активных фильтров (диапазон считается одним фильтром).</summary>
    public int ActiveFilterCount
    {
        get
        {
            var count = 0;
            if (HasSearch) count++;
            if (HasManufacturer) count++;
            if (ApertureMin.HasValue || ApertureMax.HasValue) count++;
            if (FocalMin.HasValue || FocalMax.HasValue) count++;
            if (Barlow.HasValue) count++;
            return count;
        }
    }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Manufacturers = await _context.Telescopes
            .AsNoTracking()
            .Select(t => t.Manufacturer)
            .Distinct()
            .OrderBy(m => m)
            .ToListAsync(cancellationToken);

        var query = _context.Telescopes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            var term = Search.Trim();
            query = query.Where(t => t.Manufacturer.Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(Manufacturer))
        {
            var manufacturer = Manufacturer.Trim();
            query = query.Where(t => t.Manufacturer == manufacturer);
        }

        if (ApertureMin.HasValue)
        {
            var min = ApertureMin.Value;
            query = query.Where(t => t.Aperture >= min);
        }

        if (ApertureMax.HasValue)
        {
            var max = ApertureMax.Value;
            query = query.Where(t => t.Aperture <= max);
        }

        if (FocalMin.HasValue)
        {
            var min = FocalMin.Value;
            query = query.Where(t => t.FocalLength >= min);
        }

        if (FocalMax.HasValue)
        {
            var max = FocalMax.Value;
            query = query.Where(t => t.FocalLength <= max);
        }

        if (Barlow.HasValue)
        {
            var withBarlow = Barlow.Value;
            query = query.Where(t => t.BarlowLensIncluded == withBarlow);
        }

        Telescopes = await query
            .OrderBy(t => t.Id)
            .ToListAsync(cancellationToken);
    }
}
