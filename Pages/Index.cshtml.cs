using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TelescopeShop.Data;
using TelescopeShop.Models;

namespace TelescopeShop.Pages;

public sealed record ActiveFilter(string Label, string RemoveUrl);

public class IndexModel : PageModel
{
    private readonly TelescopeDbContext _context;

    public IndexModel(TelescopeDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Telescope> Telescopes { get; private set; } = [];

    public IReadOnlyList<string> Manufacturers { get; private set; } = [];

    public IReadOnlyList<string> TelescopeNames { get; private set; } = [];

    public IReadOnlyList<ActiveFilter> ActiveFilters { get; private set; } = [];

    public string? ClearFiltersUrl { get; private set; }

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

    [BindProperty(SupportsGet = true)]
    public string? Sort { get; set; }

    public bool HasSearch => !string.IsNullOrWhiteSpace(Search);

    public bool HasManufacturer => !string.IsNullOrWhiteSpace(Manufacturer);

    public bool HasSort => !string.IsNullOrWhiteSpace(Sort) && Sort != "default";

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

        var telescopeNames = await _context.Telescopes
            .AsNoTracking()
            .Select(t => new { t.Manufacturer, t.Aperture, t.FocalLength })
            .Distinct()
            .ToListAsync(cancellationToken);

        TelescopeNames = telescopeNames
            .Select(t => $"{t.Manufacturer} {t.Aperture} мм · f/{t.FocalLength} мм")
            .OrderBy(name => name)
            .ToList();

        var query = _context.Telescopes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(Manufacturer))
        {
            var manufacturer = Manufacturer.Trim().ToLower();
            query = query.Where(t => t.Manufacturer.ToLower().Contains(manufacturer));
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

        var telescopes = await query.ToListAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(Search))
        {
            var term = Search.Trim().ToLowerInvariant();
            telescopes = telescopes
                .Where(t => t.FullName.ToLowerInvariant().Contains(term))
                .ToList();
        }

        Telescopes = Sort switch
        {
            "price-asc" => telescopes
                .OrderBy(t => t.Price == 0)
                .ThenBy(t => t.Price)
                .ThenBy(t => t.Id)
                .ToList(),
            "price-desc" => telescopes
                .OrderBy(t => t.Price == 0)
                .ThenByDescending(t => t.Price)
                .ThenBy(t => t.Id)
                .ToList(),
            "aperture-desc" => telescopes
                .OrderByDescending(t => t.Aperture)
                .ThenBy(t => t.Id)
                .ToList(),
            "aperture-asc" => telescopes
                .OrderBy(t => t.Aperture)
                .ThenBy(t => t.Id)
                .ToList(),
            _ => telescopes.OrderBy(t => t.Id).ToList()
        };

        BuildActiveFilters();
    }

    private void BuildActiveFilters()
    {
        var filters = new List<ActiveFilter>();

        if (HasSearch)
        {
            filters.Add(new($"Поиск: «{Search!.Trim()}»", BuildFilterUrl("search")));
        }

        if (HasManufacturer)
        {
            filters.Add(new($"Фирма: {Manufacturer!.Trim()}", BuildFilterUrl("manufacturer")));
        }

        if (ApertureMin.HasValue || ApertureMax.HasValue)
        {
            filters.Add(new($"Апертура: {FormatRange(ApertureMin, ApertureMax)}", BuildFilterUrl("aperture")));
        }

        if (FocalMin.HasValue || FocalMax.HasValue)
        {
            filters.Add(new($"Фокусное: {FormatRange(FocalMin, FocalMax)}", BuildFilterUrl("focal")));
        }

        if (Barlow.HasValue)
        {
            filters.Add(new(
                Barlow.Value ? "Только с линзой Барлоу" : "Только без линзы Барлоу",
                BuildFilterUrl("barlow")));
        }

        ActiveFilters = filters;
        ClearFiltersUrl = HasFilters ? BuildFilterUrl("all") : null;
    }

    private static string FormatRange(int? min, int? max)
    {
        if (min.HasValue && max.HasValue)
        {
            return $"{min}–{max} мм";
        }

        if (min.HasValue)
        {
            return $"от {min} мм";
        }

        return $"до {max} мм";
    }

    private string BuildFilterUrl(string? remove)
    {
        var values = new Dictionary<string, string?>();

        if (remove != "all")
        {
            if (remove != "search")
            {
                AddValue(values, "search", Search);
            }

            if (remove != "manufacturer")
            {
                AddValue(values, "manufacturer", Manufacturer);
            }

            if (remove != "aperture")
            {
                AddValue(values, "apertureMin", ApertureMin);
                AddValue(values, "apertureMax", ApertureMax);
            }

            if (remove != "focal")
            {
                AddValue(values, "focalMin", FocalMin);
                AddValue(values, "focalMax", FocalMax);
            }

            if (remove != "barlow" && Barlow.HasValue)
            {
                AddValue(values, "barlow", Barlow.Value ? "true" : "false");
            }
        }

        AddValue(values, "sort", Sort);

        var query = string.Join(
            "&",
            values.Select(pair =>
                $"{Uri.EscapeDataString(pair.Key)}={Uri.EscapeDataString(pair.Value!)}"));

        return string.IsNullOrEmpty(query) ? "/" : $"/?{query}";
    }

    private static void AddValue(Dictionary<string, string?> values, string key, object? value)
    {
        var text = Convert.ToString(value, CultureInfo.InvariantCulture);

        if (!string.IsNullOrWhiteSpace(text))
        {
            values[key] = text;
        }
    }
}
