using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace StudentClubsXml.Services;

public class MauiDialogService : IDialogService
{
    private static Page CurrentPage =>
        Application.Current?.Windows.FirstOrDefault()?.Page
        ?? throw new InvalidOperationException("Не вдалося знайти активну сторінку програми.");

    public Task ShowMessageAsync(string title, string message) =>
        CurrentPage.DisplayAlertAsync(title, message, "OK");

    public Task<bool> ConfirmAsync(string title, string message) =>
        CurrentPage.DisplayAlertAsync(title, message, "Так", "Ні");

    public async Task<string?> PickFileAsync(string fileType)
    {
        string extension = fileType.ToLowerInvariant() switch
        {
            "xml" => ".xml",
            "xsl" => ".xsl",
            _ => throw new ArgumentException("Непідтримуваний тип файлу.", nameof(fileType))
        };

        var allowedTypes = new FilePickerFileType(
            new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.WinUI, new[] { extension } }
            });

        var result = await FilePicker.Default.PickAsync(new PickOptions
        {
            PickerTitle = $"Виберіть файл {extension}",
            FileTypes = allowedTypes
        });

        if (result == null) return null;
        if (!string.Equals(Path.GetExtension(result.FileName), extension,
            StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException($"Потрібно вибрати файл із розширенням {extension}");

        if (string.IsNullOrWhiteSpace(result.FullPath))
            throw new InvalidOperationException("Не вдалося отримати шлях до вибраного файлу.");

        return result.FullPath;
    }
}
