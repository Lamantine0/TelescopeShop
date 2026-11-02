using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TelescopeShop.Data;
using TelescopeShop.Models;

namespace TelescopeShop.Pages;

public class TelescopeModel : PageModel
{
    private readonly TelescopeDbContext _context;

    public TelescopeModel(TelescopeDbContext context)
    {
        _context = context;
    }

    public Telescope? Telescope { get; private set; }

    public IReadOnlyList<Telescope> Related { get; private set; } = [];

    public IReadOnlyList<string> Gallery { get; private set; } = [];

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Telescope = await _context.Telescopes
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (Telescope is null)
        {
            return NotFound();
        }

        Gallery = Telescope.Id == 1
            ? new[]
            {
                "~/AstroPhoto/Celestron Astro 130.jpg",
                "~/AstroPhoto/Celestron Astro 130.png"
            }
            : Array.Empty<string>();

        // Похожие: той же апертуры или того же производителя, кроме текущего.
        Related = await _context.Telescopes
            .AsNoTracking()
            .Where(t => t.Id != id)
            .Where(t => t.Aperture == Telescope.Aperture || t.Manufacturer == Telescope.Manufacturer)
            .OrderBy(t => t.Id)
            .Take(4)
            .ToListAsync(cancellationToken);

        return Page();
    }
}
