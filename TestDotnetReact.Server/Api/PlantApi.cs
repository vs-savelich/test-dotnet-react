using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TestDotnetReact.Server.Model;

namespace TestDotnetReact.Server.Api;

public sealed record CreatePlantRequest : IValidatable
{
    public string Name { get; set; }
}

public sealed record PlantDto
{
    public Guid Id { get; set; }
    public Guid PortfolioId { get; set; }
    public string Name { get; set; }
}

public static class PlantApi
{
    public static RouteGroupBuilder MapPlantApi(this RouteGroupBuilder parent)
    {
        var group = parent.MapGroup("/plant");

        group.MapGet("/{portfolioId:guid}", GetPlantAsync);
        group.MapPost("/{portfolioId:guid}", CreatePlantAsync);
        group.MapDelete("/{portfolioId:guid}/{id:guid}", DeletePlantAsync);

        return parent;
    }

    private static async Task<List<PlantDto>> GetPlantAsync(DatabaseContext ctx, Guid portfolioId, int page = 0, int pageSize = 20) =>
        await ctx.Plants
            .Where(p => p.PortfolioId == portfolioId)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(e => e.MapToDto())
            .ToListAsync();

    private static async Task<IResult> CreatePlantAsync(DatabaseContext ctx, Guid portfolioId, CreatePlantRequest request)
    {
        var portfolio = await ctx.FindAsync<Portfolio>(portfolioId);
        if (portfolio is null) return TypedResults.NotFound();
        var plant = request.MapToEntity(portfolioId);
        await ctx.AddAsync(plant);
        await ctx.SaveChangesAsync();

        return TypedResults.Created($"{plant.Id}", plant.MapToDto());
    }

    private static async Task<IResult> DeletePlantAsync(DatabaseContext ctx, Guid portfolioId, Guid id)
    {
        var plant = await ctx.Plants.SingleOrDefaultAsync(p => p.PortfolioId == portfolioId && p.Id == id);
        if (plant is null) return TypedResults.NotFound();

        ctx.Remove(plant);
        await ctx.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    private static Plant MapToEntity(this CreatePlantRequest request, Guid portfolioId) =>
        new()
        {
            Id = Guid.NewGuid(),
            PortfolioId = portfolioId,
            Name = request.Name
        };

    private static PlantDto MapToDto(this Plant entity) =>
        new()
        {
            Id = entity.Id,
            PortfolioId = entity.PortfolioId,
            Name = entity.Name
        };
}

public sealed class CreatePlantRequestValidator : AbstractValidator<CreatePlantRequest>
{
    public CreatePlantRequestValidator()
    {
        RuleFor(r => r.Name).NotEmpty().MaximumLength(Plant.NameMaxLength);
    }
}