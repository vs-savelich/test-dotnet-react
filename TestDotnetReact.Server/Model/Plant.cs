namespace TestDotnetReact.Server.Model;

public class Plant
{
    public const int NameMaxLength = 200;

    public Guid Id { get; set; }
    public Guid PortfolioId { get; set; }
    public string Name { get; set; }
}
