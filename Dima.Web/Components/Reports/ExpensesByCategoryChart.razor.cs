﻿using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Components.Reports;

public partial class ExpensesByCategoryChartComponent : ComponentBase
{
    #region Properties
    public List<Double> Data { get; set; } = [];
    public List<string> Labels { get; set; } = [];
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
        return GetExpensesByCategoryAsync();
    }

    private async Task GetExpensesByCategoryAsync()
    {
        var request = new GetExpensesByCategoryRequests();
        var result = await Handler.GetExpensesByCategoryReportAsync(request);
        if (!result.IsSuccess || result.Data is null)
        {
            Snackbar.Add($"Error: {result.Message}", Severity.Error);
            return;
        }

        foreach (var item in result.Data)
        {
            Labels.Add($"{item.Category} ({item.Expenses:C})");
            Data.Add(-(double)item.Expenses);
        }
    }
    #endregion
}