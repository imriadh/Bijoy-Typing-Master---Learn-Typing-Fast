using BijoyTypingMaster.Services;
using BijoyTypingMaster.Views;
using Microsoft.Extensions.Logging;

namespace BijoyTypingMaster;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("SutonnyMJ.ttf", "SutonnyMJ");
            });

        // Register Services
        builder.Services.AddSingleton<DatabaseManager>();
        builder.Services.AddSingleton<LicenseManager>();
        builder.Services.AddTransient<TypingEngine>();

        // Register Views
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<PracticeWindow>();
        builder.Services.AddTransient<PaymentWindow>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Initialize Database
        var app = builder.Build();
        var dbManager = app.Services.GetRequiredService<DatabaseManager>();
        dbManager.InitializeDatabase();

        return app;
    }
}
