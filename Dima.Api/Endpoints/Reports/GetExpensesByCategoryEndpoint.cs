using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public class GetExpensesByCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/expenses", HandleAsync)
            .WithName("Reports: Get Expenses By Category")
            .WithSummary("Seleciona as despesas do usuário pela categoria")
            .WithDescription("Seleciona a despesa do usuário pela categoria")
            .Produces<Response<List<ExpensesByCategory>?>>();

    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IReportHandler handler)
    {
        var request = new GetExpensesByCategoryRequests
        {
            UserId = user.Identity?.Name ?? string.Empty
        };

        var result = await handler.GetExpensesByCategoryReportAsync(request);

        if (result is not null)
        {
            return TypedResults.Ok(result);
        }

        return TypedResults.NotFound("Despesa da categoria não encontrada");
    }
}