using Light.Blazor;
using MudBlazor;

namespace Light.MudBlazor;

internal class DialogDisplay(IDialogService dialogService) : IDialogDisplay
{
    public async Task<bool> ShowWarning(string message) =>
        await dialogService.ShowMessageBox("Warning", message) ?? false;
}
