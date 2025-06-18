namespace Light.Blazor;

public interface IDialogDisplay
{
    Task<bool> ShowWarning(string message);
}
