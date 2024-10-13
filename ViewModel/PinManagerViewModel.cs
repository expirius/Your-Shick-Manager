using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MFASeeker.Model;
using MFASeeker.View;

namespace MFASeeker.ViewModel
{
    public partial class PinManagerViewModel : ObservableObject
    {
        public event Action<Toilet>? PinDeleted;
        public event Action<Toilet>? PinAdded;
        // НЕОБХОДИМО РЕАЛИЗОВАТЬ, ДЛЯ ТОГО ЧТОБЫ В VM отрисовывались обновления и наоборот
        // БЕЗ СВЯЗАННОСТИ КОДА
        public event Action<ObservableCollection<Toilet>>? ToiletsUpdated;

        private readonly JsonPinStorage jsonPinStorage = new();
        [ObservableProperty]
        private ObservableCollection<Toilet>? activePinList;
        [ObservableProperty]
        private Toilet? selectedToilet;

        public PinManagerViewModel()
        {
            ActivePinList = [];
        }

        [RelayCommand]
        private async Task RefreshToilets()
        {
            ActivePinList = (await jsonPinStorage.GetMarkersAsync())
                .OrderByDescending(toilet => toilet.CreatedDate)
                .ToObservableCollection();
        }
        [RelayCommand]
        private async Task AddToilet(Toilet toilet)
        {
            await jsonPinStorage.SaveMarkerAsync(toilet);
            await RefreshToilets();

            PinAdded?.Invoke(toilet);
        }
        [RelayCommand]
        private async Task DeleteToilet(object? value)
        {
            if (value is Toilet toilet)
            {
                // В принципе можно и весь объект передать, а смысл? 
                //await jsonPinStorage.DeleteMarkerAsync(marker: toilet);
                await jsonPinStorage.DeleteMarkerAsync(guid: toilet.Guid);

                // Обновляем активный лист
                await RefreshToilets();
                // Уведомляем об удалении
                PinDeleted?.Invoke(toilet);
            }
        }
        [RelayCommand]
        private async void EditToilet(object? value)
        {
            if (value is Toilet toilet)
            {
                var popup = new NewPinPopup
                {
                    BindingContext = value
                };
                object? result = await Application.Current.MainPage.ShowPopupAsync(popup);
                if (result is bool isConfirmed)
                {
                    await jsonPinStorage.UpdateMarker((Toilet)value);
                }
            }
        }
    }
} 
