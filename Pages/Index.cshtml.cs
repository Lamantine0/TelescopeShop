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

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Telescopes = await _context.Telescopes
            .AsNoTracking()
            .OrderBy(t => t.Id)
            .ToListAsync(cancellationToken);
    }
}
