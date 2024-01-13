using Microsoft.EntityFrameworkCore;

namespace TestDotnetReact.Server.Model;

public class DatabaseContext : DbContext
{
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Plant> Plants { get; set; }

    public string DbPath { get; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "test.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite($"Data Source={DbPath}");
}
