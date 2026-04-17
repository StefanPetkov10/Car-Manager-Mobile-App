using Microsoft.Extensions.Logging;
using CarManagerApp.Data;
using CarManagerApp.ViewModels;
using CarManagerApp.Views;

namespace CarManagerApp
{
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
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // ── Data Layer ───────────────────────────────────────
            builder.Services.AddSingleton<DatabaseService>();

            // ── ViewModels ───────────────────────────────────────
            builder.Services.AddSingleton<VehiclesViewModel>();
            builder.Services.AddTransient<VehicleDetailViewModel>();
            builder.Services.AddTransient<AddVehicleViewModel>();
            builder.Services.AddTransient<AddFuelViewModel>();

            // ── Views ────────────────────────────────────────────
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<VehicleDetailPage>();
            builder.Services.AddTransient<AddVehiclePage>();
            builder.Services.AddTransient<AddFuelPage>();

            return builder.Build();
        }
    }
}
