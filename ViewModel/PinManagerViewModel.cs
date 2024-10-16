using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MFASeeker.Model;
using MFASeeker.View;
using System.Collections.ObjectModel;

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
            _ = RefreshToilets();
        }

        [RelayCommand]
        private async Task RefreshToilets()
        {
            ActivePinList = (await jsonPinStorage.GetMarkersAsync())
                .OrderByDescending(toilet => toilet.CreatedDate)
                .ToObservableCollection();
            ToiletsUpdated?.Invoke(ActivePinList);
        }
        [RelayCommand]
        private async Task AddToilet(Toilet toilet)
        {
            ActivePinList?.Add(toilet);
            await jsonPinStorage.SaveMarkerAsync(toilet);

            ToiletsUpdated?.Invoke(ActivePinList);
            PinAdded?.Invoke(toilet);
        }
        [RelayCommand]
        private async Task DeleteToilet(object? value)
        {
            if (value is Toilet toilet)
            {
                // В принципе можно и весь объект передать, а смысл? 
                //await jsonPinStorage.DeleteMarkerAsync(marker: toilet);
                ActivePinList?.Remove(toilet);  // Удаляем из локальной коллекции
                await jsonPinStorage.DeleteMarkerAsync(guid: toilet.Guid); // Асинхронно удаляем из хранилища
                PinDeleted?.Invoke(toilet); // Уведомляем об удалении
            }
        }
        [RelayCommand]
        private async Task EditToilet(object? value)
        {
            if (value is Toilet toilet)
            {

                Toilet cloneToilet = (Toilet)toilet.Clone();
                var popup = new NewPinPopup
                {
                    BindingContext = cloneToilet
                };
                object? result = await Application.Current.MainPage.ShowPopupAsync(popup);
                if (result is bool isConfirmed)
                {
                    Toilet? item = ActivePinList?.FirstOrDefault(t => t.Guid == toilet.Guid);
                    if (item != null && ActivePinList != null)
                    {
                        int index = ActivePinList.IndexOf(item);
                        ActivePinList[index] = cloneToilet;
                    }
                    await jsonPinStorage.UpdateMarker(toilet);
                    ToiletsUpdated?.Invoke(ActivePinList);
                }
            }
        }
    }
} 
