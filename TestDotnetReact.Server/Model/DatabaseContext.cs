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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>()
            .HasIndex("Name", "Country")
            .IsUnique();
        modelBuilder.Entity<Tenant>()
            .Property(t => t.Name)
            .HasMaxLength(Tenant.NameMaxLength)
            .IsRequired();
        modelBuilder.Entity<Tenant>()
            .Property(t => t.Country)
            .HasMaxLength(Tenant.CountryMaxLength)
            .IsRequired();

        modelBuilder.Entity<Portfolio>()
            .HasIndex("Name", "TenantId")
            .IsUnique();
        modelBuilder.Entity<Portfolio>()
            .Property(t => t.Name)
            .HasMaxLength(Portfolio.NameMaxLength)
            .IsRequired();

        modelBuilder.Entity<Plant>()
            .HasIndex("Name", "PortfolioId")
            .IsUnique();
        modelBuilder.Entity<Plant>()
            .Property(t => t.Name)
            .HasMaxLength(Plant.NameMaxLength)
            .IsRequired();
    }
}
