using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Categories;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Categories: Get By Id")
            .WithSummary("Seleciona uma categoria do usuário pelo Id")
            .WithDescription("Seleciona uma categoria do usuário pelo Id")
            .WithOrder(4)
            .Produces<Category?>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        long id,
        ICategoryHandler handler)
    {
        var request = new GetCategoryByIdRequest
        {
            UserId = user.Identity?.Name ?? string.Empty,
            Id = id
        };

        var category = await handler.GetByIdAsync(request);

        if (category is not null)
        {
            return TypedResults.Ok(category);
        }

        return TypedResults.NotFound("Categoria não encontrada");
    }
}
