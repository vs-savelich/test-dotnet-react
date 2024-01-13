namespace TestDotnetReact.Server.Model;

public class Tenant
{
    public Guid Id { get; set; }
    // TODO: name + country must be unique
    public string Name { get; set; }
    public string Country { get; set; }

    public List<Portfolio> Portfolios { get; } = [];
}
