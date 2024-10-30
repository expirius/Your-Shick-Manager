using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using MFASeekerApp.Services;
using MFASeekerApp.View;
using MFASeekerApp.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using SkiaSharp.Views.Maui.Controls.Hosting;
//using Material.Components.Maui.Extensions;

namespace MFASeekerApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            // MAPSUI
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp(true) // Карты MAPSUI
                .UseFFImageLoading() // FFImageLoading
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            // Community Toolkit MAUI от mc с кастомными элементами
            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit();
            //
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<UserSession>();

            builder.Services.AddSingleton<SearchViewModel>();
            builder.Services.AddSingleton<SearchPage>();

            builder.Services.AddSingleton<PinManagerPage>();
            builder.Services.AddSingleton<PinManagerViewModel>();

            // Сессия пользователя
            builder.Services.AddSingleton<UserSession>();
            builder.Services.AddHttpClient(); //"maui-to-api-http-localhost", httpClient =>
            //{
            //    var baseAdress = "http://localhost:5252";
            //    httpClient.BaseAddress = new Uri(baseAdress);
            //});

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
