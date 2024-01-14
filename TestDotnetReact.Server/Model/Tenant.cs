namespace TestDotnetReact.Server.Model;

public class Tenant
{
    public const int NameMaxLength = 200;
    public const int CountryMaxLength = 200;

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }

    public List<Portfolio> Portfolios { get; } = [];
}
