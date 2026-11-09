
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using StudentClubsXml.Services;
using StudentClubsXml.ViewModels;

namespace StudentClubsXml;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont(
                    "OpenSans-Regular.ttf",
                    "OpenSansRegular");

                fonts.AddFont(
                    "OpenSans-Semibold.ttf",
                    "OpenSansSemibold");
            });

        builder.Services.AddSingleton<
            IDialogService,
            MauiDialogService>();

        builder.Services.AddSingleton<ExitService>();

        builder.Services.AddTransient<XmlSearchService>();

        builder.Services.AddTransient<StrategyFactory>();

        builder.Services.AddTransient<XmlTransformer>();

        builder.Services.AddTransient<MainViewModel>();

        builder.Services.AddTransient<MainPage>();

#if WINDOWS
        builder.ConfigureLifecycleEvents(events =>
        {
            events.AddWindows(windows =>
                windows.OnWindowCreated(window =>
                {
                    window.AppWindow.Closing += (sender, args) =>
                    {
                        var exit =
                            IPlatformApplication.Current!
                                .Services
                                .GetRequiredService<ExitService>();

                        // Після підтвердження не блокуємо закриття.
                        if (exit.IsConfirmed)
                            return;

                        // Скасовуємо початкове закриття.
                        args.Cancel = true;

                        // Запускаємо діалог через чергу UI.
                        window.DispatcherQueue.TryEnqueue(() =>
                        {
                            _ = exit.RequestExitAsync();
                        });
                    };
                }));
        });
#endif

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
