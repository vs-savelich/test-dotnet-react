using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TestDotnetReact.Server.Model;

namespace TestDotnetReact.Server.Api;

public sealed record CreateOrUpdateTenantRequest : IValidatable
{
    public string Name { get; set; }
    public string Country { get; set; }
}

public sealed record TenantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Country { get; set; }
}

public static class TenantApi
{
    public static RouteGroupBuilder MapTenantApi(this RouteGroupBuilder parent)
    {
        var group = parent.MapGroup("/tenant");

        group.MapGet("/", GetTenantsAsync);
        group.MapPost("/", CreateTenantAsync);
        group.MapPut("/{id:guid}", UpdateTenantAsync);
        group.MapDelete("/{id:guid}", DeleteTenantAsync);

        return parent;
    }

    private static async Task<List<TenantDto>> GetTenantsAsync(DatabaseContext ctx, int page = 0, int pageSize = 20) =>
        await ctx.Tenants
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(e => e.MapToDto())
            .ToListAsync();

    private static async Task<Created<TenantDto>> CreateTenantAsync(DatabaseContext ctx, CreateOrUpdateTenantRequest request)
    {
        var tenant = request.MapToEntity();
        await ctx.AddAsync(tenant);
        await ctx.SaveChangesAsync();

        return TypedResults.Created($"{tenant.Id}", tenant.MapToDto());
    }

    private static async Task<IResult> UpdateTenantAsync(DatabaseContext ctx, Guid id, CreateOrUpdateTenantRequest request)
    {
        var tenant = await ctx.FindAsync<Tenant>(id);
        if (tenant is null) return TypedResults.NotFound();

        request.MapToEntity(tenant);
        ctx.Update(tenant);
        await ctx.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    private static async Task<IResult> DeleteTenantAsync(DatabaseContext ctx, Guid id)
    {
        var tenant = await ctx.FindAsync<Tenant>(id);
        if (tenant is null) return TypedResults.NotFound();

        ctx.Remove(tenant);
        await ctx.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    private static Tenant MapToEntity(this CreateOrUpdateTenantRequest request)
    {
        var tenant = new Tenant { Id = Guid.NewGuid() };
        request.MapToEntity(tenant);
        return tenant;
    }

    private static void MapToEntity(this CreateOrUpdateTenantRequest request, Tenant entity)
    {
        entity.Name = request.Name;
        entity.Country = request.Country;
    }

    private static TenantDto MapToDto(this Tenant entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Country = entity.Country
        };
}

public sealed class CreateOrUpdateTenantRequestValidator : AbstractValidator<CreateOrUpdateTenantRequest>
{
    public CreateOrUpdateTenantRequestValidator(DatabaseContext database)
    {
        RuleFor(r => r.Name).NotEmpty().MaximumLength(Tenant.NameMaxLength);
        RuleFor(r => r.Country).NotEmpty().MaximumLength(Tenant.CountryMaxLength);
        RuleFor(r => r).CustomAsync(async (request, ctx, cancellationToken) =>
        {
            var exists = await database.Tenants.AnyAsync(t => t.Name == request.Name && t.Country == request.Country, cancellationToken);

            if (exists)
                ctx.AddFailure(string.Empty, "Name and Country must be unique.");
        });
    }
}
