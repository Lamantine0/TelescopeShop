using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TelescopeShop_.Data;
using TelescopeShop_.Models;

namespace TelescopeShop_.Pages;

public class IndexModel : PageModel
{
    private readonly TelescopeDbContext _context;

    public IndexModel(TelescopeDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Telescope> Telescopes { get; private set; } = [];

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public bool HasSearch => !string.IsNullOrWhiteSpace(Search);

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        var query = _context.Telescopes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            var term = Search.Trim();
            query = query.Where(t => t.Manufacturer.Contains(term));
        }

        Telescopes = await query
            .OrderBy(t => t.Id)
            .ToListAsync(cancellationToken);
    }
}
