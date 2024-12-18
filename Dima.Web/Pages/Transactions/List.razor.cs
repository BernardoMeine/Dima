using Dima.Core.Common.Extensions;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Requests.Transactions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Pages.Transactions;

public partial class ListTransactionPage : ComponentBase
{
    #region Properties

    public bool IsBusy { get; set; } = false;
    public List<Transaction> Transactions { get; set; } = [];
    public string SearchTerm { get; set; } = string.Empty;
    public int CurrentYear { get; set; } = DateTime.UtcNow.Year;
    public int CurrentMonth { get; set; } = DateTime.UtcNow.Month;

    public int[] Years { get; set; } =
    [
        DateTime.UtcNow.Year,
        DateTime.UtcNow.AddYears(-1).Year,
        DateTime.UtcNow.AddYears(-2).Year,
        DateTime.UtcNow.AddYears(-3).Year,
    ];

    #endregion

    #region Services
    [Inject]
    public ISnackbar SnackBar { get; set; } = null!;
    [Inject]
    public IDialogService DialogService { get; set; } = null!;
    [Inject]
    public ITransactionHandler Handler { get; set; } = null!;
    #endregion

    #region Overrides
    protected override async Task OnInitializedAsync()
    {
        try
        {
            await GetTransactionsAsync();
        }
        catch (Exception ex)
        {
            SnackBar.Add($"Erro ao carregar transações: {ex.Message}", Severity.Error);
        }
    }
    #endregion

    #region Methods
    public async Task OnSearchAsync()
    {
        await GetTransactionsAsync();
        StateHasChanged();
    }

    private async Task GetTransactionsAsync()
    {
        IsBusy = true;

        try
        {
            var request = new GetTransactionsByPeriodRequest
            {
                StartDate = DateTime.UtcNow.GetFirstDay(CurrentYear, CurrentMonth).ToUniversalTime(),
                EndDate = DateTime.UtcNow.GetLastDay(CurrentYear, CurrentMonth).ToUniversalTime(),
                PageNumber = 1,
                PageSize = 1000,
            };
            var result = await Handler.GetByPeriodAsync(request);
            if (result.IsSuccess)
                Transactions = result.Data ?? [];
        }
        catch (Exception ex)
        {
            SnackBar.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async void OnDeleteButtonClickedAsync(long id, string title)
    {
        var result = await DialogService.ShowMessageBox(
            "ATENÇÃO",
            $"Ao prosseguir, a transação {title} será excluída, deseja prosseguir?",
            yesText: "Excluir",
            cancelText: "Cancelar");

        if (result is true)
            await OnDeleteAsync(id, title);

        StateHasChanged();
    }

    public async Task OnDeleteAsync(long id, string title)
    {
        IsBusy = true;

        try
        {
            await Handler.DeleteAsync(new DeleteTransactionRequest { Id = id });
            Transactions.RemoveAll(x => x.Id == id);
            SnackBar.Add($"Transação {title} removida!", Severity.Success);
        }
        catch (Exception ex)
        {
            SnackBar.Add(ex.Message, Severity.Error);
        }

        IsBusy = false;
    }

    public Func<Transaction, bool> Filter => transaction =>
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
            return true;

        var term = SearchTerm.Trim();
        return transaction.Id.ToString().Contains(term, StringComparison.OrdinalIgnoreCase)
            || transaction.Title.Contains(term, StringComparison.OrdinalIgnoreCase);
    };
    #endregion
}
