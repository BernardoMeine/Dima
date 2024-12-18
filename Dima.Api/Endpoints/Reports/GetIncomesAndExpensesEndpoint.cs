using System.Security.Claims;
using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;

namespace Dima.Api.Endpoints.Reports;

public class GetIncomesAndExpensesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/incomes-expenses", HandleAsync)
            .WithName("Reports: Get Incomes and Expenses")
            .WithSummary("Seleciona a renda e despesa do usuário")
            .WithDescription("Seleciona a renda e despesa do usuário")
            .Produces<Response<List<IncomesAndExpenses>?>>();
    
    private static async Task<IResult> HandleAsync(
        ClaimsPrincipal user,
        IReportHandler handler)
    {
        var request = new GetIncomesAndExpensesRequest()
        {
            UserId = user.Identity?.Name ?? string.Empty
        };

        var result = await handler.GetIncomesAndExpensesReportAsync(request);

        if (result is not null)
        {
            return TypedResults.Ok(result);
        }

        return TypedResults.NotFound("Não foi possível mostrar a renda e a despesa");
    }
}