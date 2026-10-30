using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelescopeShop_.Data;
using TelescopeShop_.DTOs;
using TelescopeShop_.Models;

namespace TelescopeShop_.Controllers;

[ApiController]
[Route("[controller]")]
public class TelescopesController : ControllerBase
{
    private readonly TelescopeDbContext _context;

    public TelescopesController(TelescopeDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TelescopeDto>>> GetAll(CancellationToken cancellationToken)
    {
        var telescopes = await _context.Telescopes
            .AsNoTracking()
            .Select(t => ToDto(t))
            .ToListAsync(cancellationToken);

        return Ok(telescopes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TelescopeDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var telescope = await _context.Telescopes
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        return telescope is null ? NotFound() : Ok(ToDto(telescope));
    }

    [HttpPost]
    public async Task<ActionResult<TelescopeDto>> Create(TelescopeDto dto, CancellationToken cancellationToken)
    {
        var telescope = new Telescope();
        Apply(dto, telescope);

        _context.Telescopes.Add(telescope);
        await _context.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = telescope.Id }, ToDto(telescope));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TelescopeDto dto, CancellationToken cancellationToken)
    {
        var telescope = await _context.Telescopes
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (telescope is null)
        {
            return NotFound();
        }

        Apply(dto, telescope);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var telescope = await _context.Telescopes
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (telescope is null)
        {
            return NotFound();
        }

        _context.Telescopes.Remove(telescope);
        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    private static void Apply(TelescopeDto dto, Telescope telescope)
    {
        telescope.Manufacturer = dto.Manufacturer;
        telescope.TripodHeight = dto.TripodHeight;
        telescope.Aperture = dto.Aperture;
        telescope.Seeker = dto.Seeker;
        telescope.BarlowLensIncluded = dto.BarlowLensIncluded;
        telescope.MaximumUsefulMagnification = dto.MaximumUsefulMagnification;
        telescope.FocalLength = dto.FocalLength;
    }

    private static TelescopeDto ToDto(Telescope telescope)
    {
        return new TelescopeDto
        {
            Id = telescope.Id,
            Manufacturer = telescope.Manufacturer,
            TripodHeight = telescope.TripodHeight,
            Aperture = telescope.Aperture,
            Seeker = telescope.Seeker,
            BarlowLensIncluded = telescope.BarlowLensIncluded,
            MaximumUsefulMagnification = telescope.MaximumUsefulMagnification,
            FocalLength = telescope.FocalLength
        };
    }
}
