using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using System.Net.Http.Json;

namespace Dima.Web.Handlers;

public class TransactionHandler(IHttpClientFactory httpClientFactory) : ITransactionHandler
{
    private readonly HttpClient _clientFactory = httpClientFactory.CreateClient(Configuration.HttpClientName);

    public async Task<Response<Transaction?>> CreateAsync(CreateTransactionRequest request)
    {
        var result = await _clientFactory.PostAsJsonAsync("v1/transactions", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
            ?? new Response<Transaction?>(null, 400, "Falha ao criar a transação");
    }

    public async Task<Response<Transaction?>> DeleteAsync(DeleteTransactionRequest request)
    {
        var result = await _clientFactory.DeleteAsync($"v1/transactions/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
            ?? new Response<Transaction?>(null, 400, "Falha ao excluir a transação");
    }

    public async Task<Response<Transaction?>> GetByIdAsync(GetTransactionByIdRequest request)
    {
        return await _clientFactory.GetFromJsonAsync<Response<Transaction?>>($"v1/transactions/{request.Id}")
        ?? new Response<Transaction?>(null, 400, "Falha ao excluir a categoria");
    }

    public async Task<PagedResponse<List<Transaction>?>> GetByPeriodAsync(GetTransactionsByPeriodRequest request)
    {
        const string format = "yyyy-MM-dd";

        // Verifica e converte StartDate para UTC se não for nulo
        string startDate = request.StartDate is not null
            ? request.StartDate.Value.ToUniversalTime().ToString(format)  // Converte para UTC
            : DateTime.UtcNow.GetFirstDay().ToString(format);  // Utiliza UtcNow para já garantir UTC

        // Verifica e converte EndDate para UTC se não for nulo
        string endDate = request.EndDate is not null
            ? request.EndDate.Value.ToUniversalTime().ToString(format)  // Converte para UTC
            : DateTime.UtcNow.GetLastDay().ToString(format);  // Utiliza UtcNow para garantir UTC

        string url = $"v1/transactions?startDate={startDate}&endDate={endDate}";

        // Realiza a requisição
        return await _clientFactory.GetFromJsonAsync<PagedResponse<List<Transaction>?>>(url)
            ?? new PagedResponse<List<Transaction>?>(null, 400, "Não foi possível obter as transações");
    }

    public async Task<Response<Transaction?>> UpdateAsync(UpdateTransactionRequest request)
    {
        var result = await _clientFactory.PutAsJsonAsync($"v1/transactions/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Transaction?>>()
            ?? new Response<Transaction?>(null, 400, "Falha ao atualizar a transação");
    }
}
