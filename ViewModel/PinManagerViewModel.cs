using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MFASeeker.Model;
using MFASeeker.Services;
using MFASeeker.View;
using System.Collections.ObjectModel;

namespace MFASeeker.ViewModel
{
    public partial class PinManagerViewModel : ObservableObject
    {
        public event Action<ToiletViewModel>? PinDeleted;
        public event Action<ToiletViewModel>? PinAdded;
        // НЕОБХОДИМО РЕАЛИЗОВАТЬ, ДЛЯ ТОГО ЧТОБЫ В VM отрисовывались обновления и наоборот
        // БЕЗ СВЯЗАННОСТИ КОДА
        public event Action<ObservableCollection<ToiletViewModel>>? ToiletsUpdated;

        private readonly JsonPinStorage jsonPinStorage = new();
        [ObservableProperty]
        private ObservableCollection<ToiletViewModel>? activePinList;
        [ObservableProperty]
        private ToiletViewModel? selectedToiletVM;
        [ObservableProperty]
        private int editToiletIndex;

        public PinManagerViewModel()
        {
            
            ActivePinList = [];
            _ = RefreshToilets();
        }

        [RelayCommand]
        private async Task RefreshToilets()
        {
            var temp = (await jsonPinStorage.GetMarkersAsync());
            ActivePinList = new ObservableCollection<ToiletViewModel>(temp.Select(t => new ToiletViewModel(t)));
            ToiletsUpdated?.Invoke(ActivePinList);
        }
        [RelayCommand]
        private async Task AddToilet(ToiletViewModel toilet)
        {
            if (toilet.Toilet == null) return;
            ActivePinList?.Add(toilet);
            await jsonPinStorage.SaveMarkerAsync(toilet.Toilet);

            ToiletsUpdated?.Invoke(ActivePinList);
            PinAdded?.Invoke(toilet);
        }
        [RelayCommand]
        private async Task DeleteToilet(object? value)
        {
            if (value is ToiletViewModel toiletVM)
            {
                // В принципе можно и весь объект передать, а смысл? 
                //await jsonPinStorage.DeleteMarkerAsync(marker: toilet);
                ActivePinList?.Remove(toiletVM);  // Удаляем из локальной коллекции
                await jsonPinStorage.DeleteMarkerAsync(guid: toiletVM.Toilet.Guid); // Асинхронно удаляем из хранилища
                PinDeleted?.Invoke(toiletVM); // Уведомляем об удалении
            }
        }
        [RelayCommand]
        private async Task EditToilet(object? value)
        {
            if (value is ToiletViewModel toilet && ActivePinList != null)
            {
                //Toilet cloneToilet = (Toilet)toilet.Clone();
                SelectedToiletVM = toilet; //(ToiletViewModel) [...] .Toilet.Clone 
                var popup = new NewPinPopup
                {
                    BindingContext = SelectedToiletVM
                };
                object? result = await Application.Current.MainPage.ShowPopupAsync(popup);
                // если нажал подтвердить в popup
                if (result is bool isConfirmed && isConfirmed)
                {
                    // обновление в интерфейсе
                    ToiletViewModel? item = ActivePinList?.FirstOrDefault(t => t.Toilet.Guid == toilet.Toilet.Guid);
                    if (item != null)
                    {
                        // Берем индекс
                        int index = ActivePinList.IndexOf(item);
                        ActivePinList[index] = SelectedToiletVM;
                        EditToiletIndex = index;

                        ToiletsUpdated?.Invoke(ActivePinList);
                    }
                    // обновление в памяти
                    await jsonPinStorage.UpdateMarker(SelectedToiletVM.Toilet);
                }
            }
        }
        [RelayCommand]
        private async Task ShowQR(object? value)
        {
            if (value is ToiletViewModel toilet) 
            {
                var popup = new ToiletQRpopup
                {
                    BindingContext = ToiletQRService.GenerateQRCode(toilet.Toilet)
                };
                object? result = await Application.Current.MainPage.ShowPopupAsync(popup);
            }
        }
    }
} 
