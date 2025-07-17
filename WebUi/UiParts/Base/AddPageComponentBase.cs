using Commons.Data.Results;
using Microsoft.AspNetCore.Components;
using WebUi.Extensions;
using WebUi.Services;

namespace WebUi.UiParts.Base;

public abstract class AddPageComponentBase<T> : PageComponentBase
{
    #region INJECTS
    [Inject] protected IODataService ODataService { get; init; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; init; } = default!;
    #endregion INJECTS

    #region PARAMETERS
    [Parameter] public string Id { get; set; } = default!;
    #endregion PARAMETERS

    #region PROPS
    protected T Item { get; private set; } = default!;
    protected bool Edit { get; private set; } = false;
    private bool dataChanged = false;
    protected bool DataChanged
    {
        get => dataChanged;
        set
        {
            dataChanged = value;
            StateHasChanged();
        }
    }
    protected Guid? ItemId => string.IsNullOrEmpty(Id) ? null : Guid.Parse(Id);
    #endregion PROPS

    protected abstract string TypeName();

    protected abstract ValueTask<Result<ODataCountValue<T>>> QueryItemAsync(
        IODataService service, Guid id);

    protected abstract Task<Result> SaveItemAsync();

    protected virtual void OnLoadedItem(T Item) { }

    protected virtual Task OnLoadedItemAsync(T Item) => Task.CompletedTask;


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        Edit = !string.IsNullOrEmpty(Id);

        if (Edit)
        {
            await InternalQueryDataAsync();
        }
    }

    private async Task InternalQueryDataAsync()
    {
        Loading = true;

        try
        {
            var result = await QueryItemAsync(ODataService, Guid.Parse(Id));

            if (result is not null && result.Data is not null)
            {
                if (result.Data.Value.Any())
                {
                    Item = result.Data!.Value.First();
                    OnLoadedItem(Item);
                    await OnLoadedItemAsync(Item);
                }
                else
                {
                    ShowMessageError($"{TypeName()} não existente.");
                }

                Loading = false;
                return;
            }

            ShowQueryItemMessageError(null);
        }
        catch (Exception e)
        {
            ShowQueryItemMessageError(e);
        }

        Loading = false;
    }

    protected void ShowQueryItemMessageError(Exception? e = null)
    {
        ShowMessageError($"A consulta por {TypeName()} falhou.", e);
    }

    protected void ShowSaveItemMessageError(Exception? e = null)
    {
        var action = Edit ? "Edição" : "Criação";
        ShowMessageError($"{action} de {TypeName()} não concluída.", e);
    }

    protected void ShowSaveItemSuccess()
    {
        var action = Edit ? "Edição" : "Criação";
        ShowSuccess($"{action} de {TypeName()} concluída com sucesso.");
    }

    protected string PageTitle => Edit ? $"Editar {TypeName()}" : $"Adicionar {TypeName()}";

    protected void OnDataChanged()
    {
        DataChanged = true;
    }

    protected async void OnSave()
    {
        Loading = true;

        var result = await SaveItemAsync();
        if (result.Valid)
        {
            ShowSaveItemSuccess();
            DataChanged = false;
        }
        else
        {
            ShowResultsError(result.Errors);
        }

        Loading = false;
    }

    protected async void OnCancel()
    {
        if (DataChanged && !await DialogService.ConfirmCancellationAsync()) 
        {
            return;
        }

        string listUri = NavigationManager.Uri.Substring(0, NavigationManager.Uri.LastIndexOf('/'));
        if (Edit)
        {
            listUri = listUri.Substring(0, listUri.LastIndexOf('/'));
        }
        NavigationManager.NavigateTo(listUri);
    }
}
