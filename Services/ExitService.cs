using Microsoft.Maui.Controls;

namespace StudentClubsXml.Services;

public class ExitService
{
    private readonly IDialogService _dialogService;
    private bool _isAsking;

    public bool IsConfirmed { get; private set; }

    public ExitService(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public async Task RequestExitAsync()
    {
        if (_isAsking || IsConfirmed)
            return;

        _isAsking = true;
        try
        {
            bool confirmed = await _dialogService.ConfirmAsync(
                "Підтвердження виходу",
                "Чи дійсно ви хочете завершити роботу з програмою?");

            if (!confirmed)
                return;

            var app = Application.Current;
            var window = app?.Windows.FirstOrDefault();
            if (app == null || window == null)
                return;

            IsConfirmed = true;
            try
            {
                app.CloseWindow(window);
            }
            catch
            {
                IsConfirmed = false;
                throw;
            }
        }
        finally
        {
            _isAsking = false;
        }
    }
}
