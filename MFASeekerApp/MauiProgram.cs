﻿using CommunityToolkit.Maui;
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
            builder.Services.AddSingleton<UserSession>();

            builder.Services.AddSingleton<SearchViewModel>();
            builder.Services.AddSingleton<SearchPage>();

            builder.Services.AddSingleton<PinManagerPage>();
            builder.Services.AddSingleton<PinManagerViewModel>();
            // Построение строки подключения
            //builder.Services.AddSingleton<ServerHttpFactory>();
            // Сессия пользователя
            builder.Services.AddSingleton<UserSession>();
            // Подключение к серверу
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("http://192.168.0.2:7226")
            });

            builder.Services.AddSingleton<UserService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}
