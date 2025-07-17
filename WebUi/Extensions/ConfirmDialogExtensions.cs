using MudBlazor;
using WebUi.UiParts.Dialog;

namespace WebUi.Extensions
{
    public static class DialogExtensions
    {
        public async static Task<bool> ConfirmCancellationAsync(this IDialogService DialogService)
        {
            return await ConfirmDialog.ShowAsync(DialogService, "As alterações não salvas serão perdidas.", "Confirmar");
        }

        public async static Task<bool> ConfirmDeletionAsync(this IDialogService DialogService, string name)
        {
            return await ConfirmDialog.ShowAsync(DialogService, $"Deseja excluir o item '{name}'?", "Excluir", UiParts.AppButtonType.Danger);
        }
    }
}
