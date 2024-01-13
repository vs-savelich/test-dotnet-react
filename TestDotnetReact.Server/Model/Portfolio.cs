namespace TestDotnetReact.Server.Model;

public class Portfolio
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; }

    public List<Plant> Plants { get; } = [];
}
