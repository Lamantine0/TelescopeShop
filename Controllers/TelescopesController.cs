using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TelescopeShop.Data;
using TelescopeShop.DTOs;
using TelescopeShop.Models;

namespace TelescopeShop.Controllers;

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
            .Select(t => new TelescopeDto
            {
                Id = t.Id,
                Manufacturer = t.Manufacturer,
                TripodHeight = t.TripodHeight,
                Aperture = t.Aperture,
                Seeker = t.Seeker,
                BarlowLensIncluded = t.BarlowLensIncluded,
                MaximumUsefulMagnification = t.MaximumUsefulMagnification,
                FocalLength = t.FocalLength,
                Description = t.Description,
                Price = t.Price
            })
            .ToListAsync(cancellationToken);

        return Ok(telescopes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TelescopeDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var telescope = await _context.Telescopes
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (telescope is null)
        {
            return NotFound();
        }

        return Ok(new TelescopeDto
        {
            Id = telescope.Id,
            Manufacturer = telescope.Manufacturer,
            TripodHeight = telescope.TripodHeight,
            Aperture = telescope.Aperture,
            Seeker = telescope.Seeker,
            BarlowLensIncluded = telescope.BarlowLensIncluded,
            MaximumUsefulMagnification = telescope.MaximumUsefulMagnification,
            FocalLength = telescope.FocalLength,
            Description = telescope.Description,
            Price = telescope.Price
        });
    }

    [HttpPost]
    public async Task<ActionResult<TelescopeDto>> Create(TelescopeDto dto, CancellationToken cancellationToken)
    {
        var telescope = new Telescope
        {
            Manufacturer = dto.Manufacturer,
            TripodHeight = dto.TripodHeight,
            Aperture = dto.Aperture,
            Seeker = dto.Seeker,
            BarlowLensIncluded = dto.BarlowLensIncluded,
            MaximumUsefulMagnification = dto.MaximumUsefulMagnification,
            FocalLength = dto.FocalLength,
            Description = dto.Description,
            Price = dto.Price
        };

        _context.Telescopes.Add(telescope);
        await _context.SaveChangesAsync(cancellationToken);

        dto.Id = telescope.Id;
        return CreatedAtAction(nameof(GetById), new { id = telescope.Id }, dto);
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

        telescope.Manufacturer = dto.Manufacturer;
        telescope.TripodHeight = dto.TripodHeight;
        telescope.Aperture = dto.Aperture;
        telescope.Seeker = dto.Seeker;
        telescope.BarlowLensIncluded = dto.BarlowLensIncluded;
        telescope.MaximumUsefulMagnification = dto.MaximumUsefulMagnification;
        telescope.FocalLength = dto.FocalLength;
        telescope.Description = dto.Description;
        telescope.Price = dto.Price;

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
}
