using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MFASeeker.Services;
using MFASeeker.View;
using MFASeekerApp.Services;
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

        private readonly LocalToiletService jsonPinStorage = new();
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
            await Task.Run(async () =>
            {
                var temp = (await jsonPinStorage.GetAllToilets());
                ActivePinList = new ObservableCollection<ToiletViewModel>(temp.Select(t => new ToiletViewModel(t)));
                ToiletsUpdated?.Invoke(ActivePinList);
            });
        }
        [RelayCommand]
        private async Task AddToilet(ToiletViewModel toiletVM)
        {
            if (toiletVM.Toilet == null) return;
            ActivePinList?.Add(toiletVM);
            await jsonPinStorage.AddToilet(toiletVM.Toilet);

            ToiletsUpdated?.Invoke(ActivePinList);
            PinAdded?.Invoke(toiletVM);
        }
        [RelayCommand]
        private async Task DeleteToilet(object? value)
        {
            if (value is ToiletViewModel toiletVM)
            {
                // В принципе можно и весь объект передать, а смысл? 
                //await jsonPinStorage.DeleteMarkerAsync(marker: toilet);
                ActivePinList?.Remove(toiletVM);  // Удаляем из локальной коллекции
                await jsonPinStorage.DeleteToilet(guid: toiletVM.Toilet.Guid); // Асинхронно удаляем из хранилища
                PinDeleted?.Invoke(toiletVM); // Уведомляем об удалении
            }
        }
        [RelayCommand]
        private async Task EditToilet(object? value)
        {
            if (value is ToiletViewModel toiletVM && ActivePinList != null)
            {
                ToiletViewModel cloneToilet = (ToiletViewModel)toiletVM.Clone();
                SelectedToiletVM = cloneToilet; //(ToiletViewModel) [...] .Toilet.Clone 
                // проверка на пустого клона
                if (SelectedToiletVM.Toilet == null) return;
                var popup = new NewPinPopup
                {
                    BindingContext = toiletVM
                };
                object? result = await Application.Current.MainPage.ShowPopupAsync(popup);
                // если нажал подтвердить в popup
                if (result is bool isConfirmed && isConfirmed)
                {
                    // обновление в интерфейсе
                    ToiletViewModel? item = ActivePinList?.FirstOrDefault(t => t.Toilet.Guid == toiletVM.Toilet.Guid);

                    if (item != null)
                    {
                        // Берем индекс
                        int index = ActivePinList.IndexOf(item);
                        ActivePinList[index] = SelectedToiletVM;
                        EditToiletIndex = index;

                        ToiletsUpdated?.Invoke(ActivePinList);
                    }
                    // обновление в памяти
                    await jsonPinStorage.UpdateToilet(SelectedToiletVM.Toilet);
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
