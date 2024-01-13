using Microsoft.EntityFrameworkCore;
using TestDotnetReact.Server.Model;

namespace TestDotnetReact.Server.Api;

public sealed record CreatePortfolioRequest
{
    public string Name { get; set; }
}

public sealed record PortfolioDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Name { get; set; }
}

public static class PortfolioApi
{
    public static WebApplication MapPortfolioApi(this WebApplication app)
    {
        var group = app.MapGroup("/portfolio");

        group.MapGet("/{tenantId:guid}", GetPortfoliosAsync);
        group.MapPost("/{tenantId:guid}", CreatePortfolioAsync);
        group.MapDelete("/{tenantId:guid}/{id:guid}", DeletePortfolioAsync);
        group.WithOpenApi();

        return app;
    }

    private static async Task<List<PortfolioDto>> GetPortfoliosAsync(DatabaseContext ctx, Guid tenantId, int page = 0, int pageSize = 20) =>
        await ctx.Portfolios
            .Where(p => p.TenantId == tenantId)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(e => e.MapToDto())
            .ToListAsync();

    private static async Task<IResult> CreatePortfolioAsync(DatabaseContext ctx, Guid tenantId, CreatePortfolioRequest request)
    {
        var tenant = await ctx.FindAsync<Tenant>(tenantId);
        if (tenant is null) return TypedResults.NotFound();
        var portfolio = request.MapToEntity(tenantId);
        await ctx.AddAsync(portfolio);
        await ctx.SaveChangesAsync();

        return TypedResults.Created($"{portfolio.Id}", portfolio.MapToDto());
    }

    private static async Task<IResult> DeletePortfolioAsync(DatabaseContext ctx, Guid tenantId, Guid id)
    {
        var portfolio = await ctx.Portfolios.SingleOrDefaultAsync(p => p.TenantId == tenantId && p.Id == id);
        if (portfolio is null) return TypedResults.NotFound();

        ctx.Remove(portfolio);
        await ctx.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    private static Portfolio MapToEntity(this CreatePortfolioRequest request, Guid tenantId) =>
        new()
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Name = request.Name
        };

    private static PortfolioDto MapToDto(this Portfolio entity) =>
        new()
        {
            Id = entity.Id,
            TenantId = entity.TenantId,
            Name = entity.Name
        };
}
