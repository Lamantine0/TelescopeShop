using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TelescopeShop.Data;
using TelescopeShop.Models;

namespace TelescopeShop.Pages;

public class TelescopeModel : PageModel
{
    private static readonly string[] ImageExtensions = [".jpg", ".jpeg", ".png", ".webp"];

    private readonly TelescopeDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public TelescopeModel(TelescopeDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
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

        // Галерея по конвенции: wwwroot/AstroPhoto/{id}/* (папки нет — пусто).
        Gallery = _environment.WebRootFileProvider
            .GetDirectoryContents($"/AstroPhoto/{Telescope.Id}")
            .Where(f => !f.IsDirectory && ImageExtensions.Contains(Path.GetExtension(f.Name), StringComparer.OrdinalIgnoreCase))
            .OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
            .Select(f => $"~/AstroPhoto/{Telescope.Id}/{f.Name}")
            .ToList();

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
