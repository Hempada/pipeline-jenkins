using BlazorBootstrap;
using Commons.Data.Results;
using Microsoft.AspNetCore.Components;
using WebUi.Extensions;
using WebUi.Services;
using WebUi.UiParts.App;
using static WebUi.UiParts.App.AppPage;

namespace WebUi.UiParts.Base;

public abstract class ListPageComponentBase<T> : PageComponentBase
{
    #region INJECTS
    [Inject] protected IODataService ODataService { get; init; } = default!;
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;
    #endregion INJECTS

    #region PROPS
    protected IEnumerable<T> Items { get; private set; } = Enumerable.Empty<T>();
    protected int TotalItems { get; private set; } = 0;
    #endregion PROPS

    protected abstract AppGrid<T> Grid();

    protected abstract ValueTask<Result<ODataCountValue<T>>> QueryItemsAsync(
        IODataService service, IEnumerable<FilterItem> filters, string queryStr);

    protected abstract Task<Result> DeleteItemAsync(Guid id);

    protected abstract string[] AddButtonPermissions();

    protected abstract string TypeName();

    protected abstract string TypeNamePlural();


    protected void ShowQueryItemsMessageError(Exception? e = null)
    {
        ShowMessageError($"Ocorreu um erro ao consultar a lista de {TypeNamePlural()}.", e);
    }

    protected void ShowDeleteItemSuccess()
    {
        ShowSuccess($"Exclusão de {TypeName()} concluída com sucesso.");
    }

    protected async Task<GridDataProviderResult<T>> DataProvider(GridDataProviderRequest<T> request)
    {
        await InternalQueryDataAsync(request);

        return await Task.FromResult(new GridDataProviderResult<T> { Data = Items, TotalCount = TotalItems });
    }

    private async Task InternalQueryDataAsync(GridDataProviderRequest<T> request)
    {
        Loading = true;

        Items = Enumerable.Empty<T>();
        TotalItems = 0;

        try
        {
            var queryStr = request.ODataQuery();
            var result = await QueryItemsAsync(ODataService, request.Filters, queryStr);

            if (result is not null && result.Data is not null)
            {
                Items = result.Data!.Value;
                TotalItems = result.Data.Count;

                Loading = false;
                return;
            }

            ShowQueryItemsMessageError();
        }
        catch (Exception e)
        {
            ShowQueryItemsMessageError(e);
        }

        Loading = false;
    }

    protected string PageTitle => TypeNamePlural();

    protected PageButtonNav AddButton =>
        new PageButtonNav($"Adicionar {TypeName()}", OnAdd, AddButtonPermissions());

    protected void OnAdd()
    {
        NavigationManager.NavigateTo(NavigationManager.Uri + "/add");
    }

    protected void OnEdit(Guid id)
    {
        NavigationManager.NavigateTo($"{NavigationManager.Uri}/edit/{id}");
    }

    protected async void OnDelete(Guid id, string Name)
    {
        if (!await DialogService.ConfirmDeletionAsync(Name))
        {
            return;
        }

        Loading = true;

        var result = await DeleteItemAsync(id);
        if (result.Valid)
        {
            ShowDeleteItemSuccess();
            await Grid().RefreshDataAsync();
        }
        else
        {
            ShowResultsError(result.Errors);
        }

        Loading = false;
    }
}
