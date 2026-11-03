using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TelescopeShop.Data;
using TelescopeShop.Models;

namespace TelescopeShop.Pages;

public class CheckoutModel : PageModel
{
    private readonly TelescopeDbContext _context;

    public CheckoutModel(TelescopeDbContext context)
    {
        _context = context;
    }

    public Telescope? Telescope { get; private set; }

    public bool OrderAccepted { get; private set; }

    [BindProperty]
    [Required(ErrorMessage = "Укажите имя.")]
    public string CustomerName { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Укажите телефон.")]
    public string Phone { get; set; } = string.Empty;

    [BindProperty]
    public string? Comment { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        Telescope = await _context.Telescopes
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        return Telescope is null ? NotFound() : Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        Telescope = await _context.Telescopes
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (Telescope is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Демо-заглушка: заказ никуда не сохраняется.
        OrderAccepted = true;
        return Page();
    }
}
