﻿using Dima.Core.Handlers;
using Dima.Core.Requests.Reports;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Dima.Web.Components.Reports;

public partial class IncomesByCategoryChartComponent : ComponentBase
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
        return GetIncomesByCategoryAsync();
    }

    private async Task GetIncomesByCategoryAsync()
    {
        var request = new GetIncomesByCategoryRequest();
        var result = await Handler.GetIncomesByCategoryReportAsync(request);
        if (!result.IsSuccess || result.Data is null)
        {
            Snackbar.Add($"Error: {result.Message}", Severity.Error);
            return;
        }

        foreach (var item in result.Data)
        {
            Labels.Add($"{item.Category} ({item.Incomes:C})");
            Data.Add((double)item.Incomes);
        }
    }
    #endregion
}