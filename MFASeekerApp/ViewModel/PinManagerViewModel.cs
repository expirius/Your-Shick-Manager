﻿using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MFASeekerApp.View;
using MFASeekerApp.Services;
using System.Collections.ObjectModel;
using MFASeekerApp.Model;
using MFASeekerApp.Model.Interfaces;

namespace MFASeekerApp.ViewModel
{
    public partial class PinManagerViewModel : ObservableObject
    {
        public event Action<ToiletViewModel>? PinDeleted;
        public event Action<ToiletViewModel>? PinAdded;
        // НЕОБХОДИМО РЕАЛИЗОВАТЬ, ДЛЯ ТОГО ЧТОБЫ В VM отрисовывались обновления и наоборот
        // БЕЗ СВЯЗАННОСТИ КОДА
        public event Action<ObservableCollection<ToiletViewModel>>? ToiletsUpdated;

        [ObservableProperty]
        private ObservableCollection<ToiletViewModel>? activePinList;
        [ObservableProperty]
        private ToiletViewModel? selectedToiletVM;
        [ObservableProperty]
        private int editToiletIndex;

        private readonly UserSession _userSession;
        private readonly UserService _userService;
        private readonly ToiletApiService _toiletApiService;
        private readonly LocalToiletService _localToiletService = new();

        public PinManagerViewModel(UserSession userSession, UserService userService, ToiletApiService toiletApiService)
        {
            _userSession = userSession;
            _userService = userService;
            _toiletApiService = toiletApiService;
            ActivePinList = [];
            _ = RefreshToilets();
        }
        [RelayCommand]
        private async Task SetAuthUserSession()
        {
            var temp = await _userService.GetUsers();
            if (temp.Count > 0)
            {
                _userSession.AuthUser = temp.FirstOrDefault();
            }
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
        /// <summary>
        /// Добавление туалета из VM в локальную память
        /// </summary>
        /// <param name="toiletVM"></param>
        /// <returns></returns>
        [RelayCommand]
        private async Task AddToilet(ToiletViewModel toiletVM)
        {
            if (toiletVM.Toilet == null) return;
            ActivePinList?.Add(toiletVM);
            await _localToiletService.AddToilet(toiletVM.Toilet);
            await _toiletApiService.AddToilet(toiletVM.Toilet);
            ToiletsUpdated?.Invoke(ActivePinList);
            PinAdded?.Invoke(toiletVM);
        }
        //[RelayCommand]
        //private async Task AddToilet(ToiletViewModel toiletVM)
        //{
        //    if (toiletVM.Toilet == null) return;
        //    ActivePinList?.Add(toiletVM);
        //    await _toiletApiService.AddToilet(toiletVM.Toilet);
        //}
        /// <summary>
        /// Добавление картинки для туалета (UIT)
        /// </summary>
        /// <param name="toiletID"></param>
        /// <param name="image"></param>
        [RelayCommand] 
        private async Task AddImageToilet(int toiletID, ImageFile image)
        {
            // UIT - UserImageToilet, привазка картинки к туалету
            if (_userSession.AuthUser == null) return;
            var imageID = await _toiletApiService.AddImageFile(image); // добавляем пикчу на сервер
            if (imageID == null) return; // проверяем добавлена ли она
            //
            UserImageToilet toiletImages = new() // создаем UIT
            {
                ImageID = (int)imageID,
                UserID = _userSession.AuthUser.Id,
                ToiletID = toiletID
            };
            // добавить проверки в будущем
            await _toiletApiService.AddUserImageToilet(toiletImages); // добавляем в БД UIT
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
