using Microsoft.EntityFrameworkCore;
using TelescopeShop_.Models;

namespace TelescopeShop_.Data;

public class TelescopeDbContext : DbContext
{
    public TelescopeDbContext(DbContextOptions<TelescopeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Telescope> Telescopes => Set<Telescope>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Telescope>().HasData(
            new Telescope { Id = 1, Manufacturer = "Celestron", TripodHeight = 1200, Aperture = 130, Seeker = "Red dot finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 260, FocalLength = 650 },
            new Telescope { Id = 2, Manufacturer = "Sky-Watcher", TripodHeight = 1000, Aperture = 130, Seeker = "Red dot finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 260, FocalLength = 650 },
            new Telescope { Id = 3, Manufacturer = "Orion", TripodHeight = 1200, Aperture = 203, Seeker = "EZ Finder II", BarlowLensIncluded = false, MaximumUsefulMagnification = 406, FocalLength = 1200 },
            new Telescope { Id = 4, Manufacturer = "Meade", TripodHeight = 1300, Aperture = 203, Seeker = "8x50 finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 406, FocalLength = 2032 },
            new Telescope { Id = 5, Manufacturer = "Celestron", TripodHeight = 1200, Aperture = 150, Seeker = "StarPointer red dot finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 300, FocalLength = 1500 },
            new Telescope { Id = 6, Manufacturer = "Sky-Watcher", TripodHeight = 1250, Aperture = 200, Seeker = "8x50 finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 400, FocalLength = 1200 },
            new Telescope { Id = 7, Manufacturer = "Celestron", TripodHeight = 1000, Aperture = 70, Seeker = "5x24 finder", BarlowLensIncluded = true, MaximumUsefulMagnification = 140, FocalLength = 700 },
            new Telescope { Id = 8, Manufacturer = "Orion", TripodHeight = 900, Aperture = 114, Seeker = "EZ Finder II", BarlowLensIncluded = false, MaximumUsefulMagnification = 228, FocalLength = 450 },
            new Telescope { Id = 9, Manufacturer = "GSO", TripodHeight = 1300, Aperture = 254, Seeker = "8x50 finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 508, FocalLength = 1250 },
            new Telescope { Id = 10, Manufacturer = "Celestron", TripodHeight = 1100, Aperture = 102, Seeker = "StarPointer red dot finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 204, FocalLength = 1325 }
        );
    }
}
