using Dima.Api.Data;
using Dima.Core.Enum;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers;

public class ReportHandler(AppDbContext context) : IReportHandler
{
    public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(
        GetIncomesAndExpensesRequest request)
    {
        try
        {
            var data = await context
                .IncomesAndExpenses
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Month)
                .ToListAsync();

            return new Response<List<IncomesAndExpenses>?>(data);
        }
        catch (Exception ex)
        {
            return new Response<List<IncomesAndExpenses>?>(null, 500, ex.Message);
        }
    }

    public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(
        GetExpensesByCategoryRequests request)
    {
        try
        {
            var data = await context
                .ExpensesByCategories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Category)
                .ToListAsync();

            return new Response<List<ExpensesByCategory>?>(data);
        }
        catch (Exception ex)
        {
            return new Response<List<ExpensesByCategory>?>(null, 500, ex.Message);
        }
    }

    public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(
        GetIncomesByCategoryRequest request)
    {
        try
        {
            var data = await context
                .IncomesByCategories
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .OrderByDescending(x => x.Year)
                .ThenBy(x => x.Category)
                .ToListAsync();

            return new Response<List<IncomesByCategory>?>(data);
        }
        catch (Exception ex)
        {
            return new Response<List<IncomesByCategory>?>(null, 500, ex.Message);
        }
    }

    public async Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request)
    {
        var startDate = DateTime.SpecifyKind(
            new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1),
            DateTimeKind.Utc
        );

        try
        {
            var incomes = await context.Transactions
                .Where(x => x.UserId == request.UserId
                            && x.PaidOrReceivedAt >= startDate
                            && x.PaidOrReceivedAt <= DateTime.UtcNow
                            && x.Type == ETransactionType.Deposit)
                .SumAsync(t => t.Amount);

            var expenses = await context.Transactions
                .Where(x => x.UserId == request.UserId
                            && x.PaidOrReceivedAt >= startDate
                            && x.PaidOrReceivedAt <= DateTime.UtcNow
                            && x.Type == ETransactionType.Withdraw)
                .SumAsync(t => t.Amount);

            var summary = new FinancialSummary(request.UserId, incomes, expenses);

            return new Response<FinancialSummary?>(summary);
        }
        catch (Exception ex)
        {
            return new Response<FinancialSummary?>(null, 500, ex.Message);
        }
    }

}