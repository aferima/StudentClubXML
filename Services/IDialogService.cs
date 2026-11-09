namespace StudentClubsXml.Services;

public interface IDialogService
{
    Task ShowMessageAsync(string title, string message);

    Task<bool> ConfirmAsync(string title, string message);

    Task<string?> PickFileAsync(string fileType);
}
