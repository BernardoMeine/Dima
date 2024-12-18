using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;

namespace Dima.Web.Handlers;

public class CategoryHandler(IHttpClientFactory httpClientFactory) : ICategoryHandler
{
    private readonly HttpClient _clientFactory = httpClientFactory.CreateClient(Configuration.HttpClientName);

    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        var result = await _clientFactory.PostAsJsonAsync("v1/categories", request);
        return await result.Content.ReadFromJsonAsync<Response<Category?>>() 
            ?? new Response<Category?>(null, 400, "Falha ao criar a categoria");
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        var result = await _clientFactory.DeleteAsync($"v1/categories/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Category?>>()
            ?? new Response<Category?>(null, 400, "Falha ao excluir a categoria");
    }

    public async Task<PagedResponse<List<Category>>> GetAllAsync(GetAllCategoriesRequest request)
    {
        return await _clientFactory.GetFromJsonAsync<PagedResponse<List<Category>>>("v1/categories")
            ?? new PagedResponse<List<Category>>(null, 400, "Ñão foi possível obter as categorias");
    }

    public async Task<Category?> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var response = await _clientFactory.GetAsync($"v1/categories/{request.Id}");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Conteúdo da resposta: {content}");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Ignorar diferenças de maiúsculas/minúsculas
            };

            // Deserializando diretamente para a classe Category
            var category = JsonSerializer.Deserialize<Category>(content, options);

            if (category != null)
            {
                Console.WriteLine($"Categoria ID: {category.Id}");
                Console.WriteLine($"Categoria Title: {category.Title}");
            }
            else
            {
                Console.WriteLine("Erro: Categoria não encontrada ou deserialização falhou.");
            }

            return category;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro no GetByIdAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        var result = await _clientFactory.PutAsJsonAsync($"v1/categories/{request.Id}", request);

        // Verifique o retorno da requisição
        var response = await result.Content.ReadFromJsonAsync<Response<Category?>>()
                       ?? new Response<Category?>(null, 400, "Falha ao atualizar a categoria");

        Console.WriteLine($"Resposta: {response.Message}");
        return response;
    }
}
