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
            new Telescope { Id = 1, Manufacturer = "Celestron", TripodHeight = 1200, Aperture = 130, Seeker = "Red dot finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 260, FocalLength = 650, Description = "Зеркальный телескоп на экваториальной монтировке. Подходит для наблюдений Луны, планет и ярких туманностей. Компактная тренога с регулируемой высотой и плавным ходом по обоим осям." },
            new Telescope { Id = 2, Manufacturer = "Sky-Watcher", TripodHeight = 1000, Aperture = 130, Seeker = "Red dot finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 260, FocalLength = 650, Description = "Настольный телескоп-ньютона в корпусе Гершеля. Благодаря короткому фокусу даёт широкое поле зрения — удобен для обзорных наблюдений Млечного Пути и рассеянных скоплений." },
            new Telescope { Id = 3, Manufacturer = "Orion", TripodHeight = 1200, Aperture = 203, Seeker = "EZ Finder II", BarlowLensIncluded = false, MaximumUsefulMagnification = 406, FocalLength = 1200, Description = "Классический вращающийся добсониан с трубой на лямбда-креплении. Апертура 203 мм собирает достаточно света для наблюдений слабых галактик и глобулярных скоплений." },
            new Telescope { Id = 4, Manufacturer = "Meade", TripodHeight = 1300, Aperture = 203, Seeker = "8x50 finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 406, FocalLength = 2032, Description = "Катадиоптрический телескоп с оптикой ACF на автоматизированной вилковой монтировке. Длинный фокус обеспечивает высокое увеличение — хорош для планетных наблюдений и лунной съёмки." },
            new Telescope { Id = 5, Manufacturer = "Celestron", TripodHeight = 1200, Aperture = 150, Seeker = "StarPointer red dot finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 300, FocalLength = 1500, Description = "Компактный катадиоптрик на одновилковой монтировке с компьютерным наведением. Модель со встроенной базой небесных объектов позволяет быстро найти нужную цель." },
            new Telescope { Id = 6, Manufacturer = "Sky-Watcher", TripodHeight = 1250, Aperture = 200, Seeker = "8x50 finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 400, FocalLength = 1200, Description = "Добсониан с алюминиевой трубой и крепким основанием. Оптимальный выбор для начинающих: большая апертура без сложной настройки и дорогой монтировки." },
            new Telescope { Id = 7, Manufacturer = "Celestron", TripodHeight = 1000, Aperture = 70, Seeker = "5x24 finder", BarlowLensIncluded = true, MaximumUsefulMagnification = 140, FocalLength = 700, Description = "Лёгкий рефрактор на азимутальной треноге — первый телескоп для детей и путешествий. В комплекте линза Барлоу, удваивающая увеличение." },
            new Telescope { Id = 8, Manufacturer = "Orion", TripodHeight = 900, Aperture = 114, Seeker = "EZ Finder II", BarlowLensIncluded = false, MaximumUsefulMagnification = 228, FocalLength = 450, Description = "Настольный телескоп-ньютона на поворотном основании. Короткий фокус даёт широкое поле — подходит для наблюдений с балкона в условиях городской засветки." },
            new Telescope { Id = 9, Manufacturer = "GSO", TripodHeight = 1300, Aperture = 254, Seeker = "8x50 finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 508, FocalLength = 1250, Description = "Мощный добсониан с апертурой 10 дюймов на трёхопорном основании. Серьёзный инструмент для глубокого космоса: туманности, галактики и шаровые скопления." },
            new Telescope { Id = 10, Manufacturer = "Celestron", TripodHeight = 1100, Aperture = 102, Seeker = "StarPointer red dot finder", BarlowLensIncluded = false, MaximumUsefulMagnification = 204, FocalLength = 1325, Description = "Компактный максутов-кассегрейн с Wi-Fi-управлением. Лёгкий корпус и короткая тренога делают его удобным для выездных наблюдений и астрофотографии Луны." }
        );
    }
}
