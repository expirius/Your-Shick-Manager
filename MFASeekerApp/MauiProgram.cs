using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using MFASeekerApp.Services;
using MFASeekerApp.View;
using MFASeekerApp.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Reflection;
using System.Text.Json;
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
            // Сессия пользователя
            builder.Services.AddSingleton<UserSession>();

            builder.Services.AddSingleton<SearchViewModel>();
            builder.Services.AddSingleton<SearchPage>();

            builder.Services.AddSingleton<PinManagerPage>();
            builder.Services.AddSingleton<PinManagerViewModel>();
            // Построение строки подключения
            //builder.Services.AddSingleton<ServerHttpFactory>();
            // Подключение к серверу // 
            // !!! ИЗБАВИТЬСЯ ОТ ПРЯМОГО ПОДКЛЮЧЕНИЯ ПО IP и использовать appsettings.json
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("http://95.106.241.117:7226")
            });

            builder.Services.AddSingleton<UserService>();
            builder.Services.AddSingleton<ToiletApiService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
