using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using System.Text.Json;

namespace Dima.Api.Endpoints.Categories;

public class UpdateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
       => app.MapPut("/{id}", HandleAsync)
             .WithName("Categories: Update")
             .WithSummary("Atualiza uma categoria")
             .WithDescription("Atualiza uma categoria")
             .WithOrder(2)
             .Produces<Response<Category?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        long id,
        ICategoryHandler handler,
        UpdateCategoryRequest request)
    {
        Console.WriteLine($"Recebido: {JsonSerializer.Serialize(request)}");

        if (request == null || id != request.Id)
        {
            return TypedResults.BadRequest("Requisição inválida.");
        }

        request.UserId = user.Identity?.Name ?? string.Empty;
        request.Id = id;

        var result = await handler.UpdateAsync(request);


        return result.IsSuccess
            ? TypedResults.Ok(result.Data)
            : TypedResults.BadRequest(result.Data);
    }
}
