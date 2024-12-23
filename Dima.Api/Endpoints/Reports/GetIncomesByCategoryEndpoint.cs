using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public class GetIncomesByCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/incomes", HandleAsync)
            .WithName("Reports: Get Incomes By Categoru")
            .WithSummary("Seleciona a renda do usuário pela categoria")
            .WithDescription("Seleciona a renda do usuário pela categoria")
            .Produces<Response<List<IncomesByCategory>?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IReportHandler handler)
    {
        var request = new GetIncomesByCategoryRequest()
        {
            UserId = user.Identity?.Name ?? string.Empty
        };

        var result = await handler.GetIncomesByCategoryReportAsync(request);

        if (result is not null)
        {
            return TypedResults.Ok(result);
        }

        return TypedResults.NotFound("Renda da categoria não encontrada");
    }
}