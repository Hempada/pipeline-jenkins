using Commons.Data.Results;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace WebUi.UiParts.Base;

public abstract class PageComponentBase : ComponentBase
{
    [Inject] protected IDialogService DialogService { get; init; } = default!;
    [Inject] protected IAppSnackbar Snackbar { get; set; } = default!;

    private bool loading = false;
    public bool Loading
    {
        get => loading;
        set
        {
            loading = value;
            StateHasChanged();
        }
    }

    protected virtual void ShowMessageError(string? message, Exception? e = null)
    {
        if (message is null)
        {
            message = $"Ocorreu um erro desconhecido.";
        }
        Snackbar.ShowError(message, e);
    }

    protected virtual void ShowResultsError(IEnumerable<Error> errors)
    {
        var strErrors = errors.Select(x => x.Description).ToArray();
        Snackbar.ShowError(strErrors);
    }

    protected virtual void ShowSuccess(string message)
    {
        Snackbar.ShowSuccess(message);
    }
}
