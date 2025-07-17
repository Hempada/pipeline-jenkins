using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace WebUi.UiParts
{
    public interface IAppSnackbar
    {
        public void ShowSuccess(string message);

        public void ShowError(string message, Exception? e = null);

        public void ShowError(string[] messages);
    }

    public class AppSnackbar : IAppSnackbar
    {
        private ISnackbar Snackbar { get; init; }

        public AppSnackbar(ISnackbar snackbar)
        {
            Snackbar = snackbar;
        }

        public void ShowSuccess(string message)
        {
            Snackbar.Add(message, Severity.Success);
        }

        public void ShowError(string message, Exception? e = null)
        {
            if (e is not null)
            {
                Snackbar.Add(new MarkupString(message + "<br><span style=\"font-size:10px\">" + e.Message + "</span>"), Severity.Error);
                return;
            }
            Snackbar.Add(message, Severity.Error);
        }

        public void ShowError(string[] messages)
        {
            Snackbar.Add(new MarkupString(string.Join("<br>", messages)), Severity.Error);
        }
    }
}
