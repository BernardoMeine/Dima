using Dima.Core.Handlers;
using Dima.Core.Requests.Categories;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;

namespace Dima.Web.Pages.Categories;

public partial class EditCategoryPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public UpdateCategoryRequest InputModel { get; set; } = new();

    #endregion

    #region Parameters

    [Parameter]
    public string Id { get; set; } = string.Empty;

    #endregion

    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;

    #endregion

    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        if (!long.TryParse(Id, out var categoryId))
        {
            Snackbar.Add("Parâmetro inválido", Severity.Error);
            return;
        }

        var request = new GetCategoryByIdRequest
        {
            Id = categoryId
        };

        IsBusy = true;
        try
        {
            var category = await Handler.GetByIdAsync(request);

            if (category != null)
            {
                InputModel = new UpdateCategoryRequest
                {
                    Id = category.Id,
                    Title = category.Title,
                    Description = category.Description
                };
            }
            else
            {
                Snackbar.Add("Erro ao buscar a categoria", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
            Snackbar.Add("Ocorreu um erro ao tentar buscar a categoria", Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion

    #region Methods

    public async Task OnValidSubmitAsync()
    {
        IsBusy = true;

        try
        {
            var result = await Handler.UpdateAsync(InputModel);
            if (result.IsSuccess)
            {
                Snackbar.Add("Categoria atualizada", Severity.Success);
                NavigationManager.NavigateTo("/categorias");
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
}
