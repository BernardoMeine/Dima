using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Components.Reports;

public class IncomesAndExpensesChartComponent : ComponentBase
{
    #region Properties
    public ChartOptions Options { get; set; } = new();
    public List<ChartSeries>? Series { get; set; }
    public List<string>? Labels { get; set; } = [];
    #endregion
    
    #region Services
    [Inject]
    public IReportHandler Handler { get; set; } = null!;
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    #endregion
    
    #region Overrides
    protected override Task OnInitializedAsync()
    {
        return GetIncomesAndExpensesAsync();
    }

    private async Task GetIncomesAndExpensesAsync()
    {
        var request = new GetIncomesAndExpensesRequest();
        var result = await Handler.GetIncomesAndExpensesReportAsync(request);
        if (!result.IsSuccess || result.Data is null)
        {
            Snackbar.Add($"Error: {result.Message}", Severity.Error);
            return;
        }

        var incomes = new List<double>();
        var expenses = new List<double>();

        foreach (var item in result.Data)
        {
            incomes.Add((double) item.Incomes);
            expenses.Add(-(double) item.Expenses);
            Labels.Add(GetMonthName(item.Month));
        }

        Options.YAxisTicks = 1000;
        Options.LineStrokeWidth = 5;
        Options.ChartPalette = ["#76FF01", Colors.Red.Default];
        Series =
        [
            new ChartSeries {Name = "Receitas", Data = incomes.ToArray()},
            new ChartSeries {Name = "Saídas", Data = incomes.ToArray()}
        ];
        StateHasChanged();
    }
    #endregion

    private static string GetMonthName(int month)
    {
        return new DateTime(DateTime.UtcNow.Year, month, 1).ToString("MMMM");
    }
}