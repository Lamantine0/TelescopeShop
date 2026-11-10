using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TelescopeShop.Data;
using TelescopeShop.Models;

namespace TelescopeShop.Tests;

internal sealed class TestDatabase : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<TelescopeDbContext> _options;

    public TestDatabase()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<TelescopeDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = CreateContext();
        context.Database.EnsureCreated();
    }

    public TelescopeDbContext CreateContext()
    {
        return new TelescopeDbContext(_options);
    }

    public void AddTelescope(Telescope telescope)
    {
        using var context = CreateContext();
        context.Telescopes.Add(telescope);
        context.SaveChanges();
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
