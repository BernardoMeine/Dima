using System.Net.Http.Json;
using Dima.Core.Handlers;
using Dima.Core.Models.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using Microsoft.VisualBasic;

namespace Dima.Web.Handlers;

public class ReportHandler(IHttpClientFactory factory) : IReportHandler
{
    private readonly HttpClient _httpClient = factory.CreateClient(Configuration.HttpClientName);
    public async Task<Response<List<IncomesAndExpenses>?>> GetIncomesAndExpensesReportAsync(GetIncomesAndExpensesRequest request)
    {
        return await _httpClient.GetFromJsonAsync<Response<List<IncomesAndExpenses>?>>("v1/reports/incomes-expenses")
               ?? new Response<List<IncomesAndExpenses>?>(null,400, "Não foi possível obter os dados");
    }

    public async Task<Response<List<ExpensesByCategory>?>> GetExpensesByCategoryReportAsync(GetExpensesByCategoryRequests request)
    {
        return await _httpClient.GetFromJsonAsync<Response<List<ExpensesByCategory>?>>("v1/reports/expenses")
               ?? new Response<List<ExpensesByCategory>?>(null,400, "Não foi possível obter os dados");
    }

    public async Task<Response<List<IncomesByCategory>?>> GetIncomesByCategoryReportAsync(GetIncomesByCategoryRequest request)
    {
        return await _httpClient.GetFromJsonAsync<Response<List<IncomesByCategory>?>>("v1/reports/incomes")
               ?? new Response<List<IncomesByCategory>?>(null,400, "Não foi possível obter os dados");
    }

    public async Task<Response<FinancialSummary?>> GetFinancialSummaryReportAsync(GetFinancialSummaryRequest request)
    {
        return await _httpClient.GetFromJsonAsync<Response<FinancialSummary?>>("v1/reports/summary")
               ?? new Response<FinancialSummary?>(null,400, "Não foi possível obter os dados");
    }
}