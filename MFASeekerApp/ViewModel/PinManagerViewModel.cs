using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MFASeekerApp.View;
using MFASeekerApp.Services;
using System.Collections.ObjectModel;
using MFASeekerApp.Model;
using MFASeekerApp.Model.Interfaces;
using CommunityToolkit.Maui.Core.Extensions;

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

            ToiletsUpdated += OnToiletsUpdated;
        }
        [RelayCommand]
        private async Task LoadPhotosDb(ToiletViewModel toiletVM)
        {
            // Получаем список фотографий для туалета с сервера
            if (toiletVM.Toilet == null) return;
            var userImageToilets = await _toiletApiService.GetPhotos(toiletVM.Toilet.Guid);
            if (userImageToilets == null) return;

            // Извлекаем пути изображений и добавляем их в ImagePaths
            foreach (var image in userImageToilets)
            {
                toiletVM.ImagePaths.Add(image.ImageFile.Path);
            }
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
            var temp = await _toiletApiService.GetAllToilets();
            ActivePinList = new ObservableCollection<ToiletViewModel>(temp.Select(t => new ToiletViewModel(t)));
            ToiletsUpdated?.Invoke(ActivePinList);
            //await Task.Run(async () =>
            //{
            //    var temp = (await _localToiletService.GetAllToilets());
            //    ActivePinList = new ObservableCollection<ToiletViewModel>(temp.Select(t => new ToiletViewModel(t)));
            //    ToiletsUpdated?.Invoke(ActivePinList);
            // });
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
            //await _localToiletService.AddToilet(toiletVM.Toilet); // local
            var toiletId = await _toiletApiService.AddToilet(toiletVM.Toilet); // db
            if (toiletId == null) return;
            foreach(var item in toiletVM.Toilet.Images)
            {
                if (item != null)
                {
                    var imageId = await _toiletApiService.AddImageFile(item);
                    UserImageToilet imageToilet = new()
                    {
                        ImageID = (int)imageId,
                        UserID = _userSession.AuthUser.Id,
                        ToiletID = (int)toiletId
                    };
                    await _toiletApiService.AddUserImageToilet(imageToilet);
                }
            }
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
                await _localToiletService.DeleteToilet(guid: toiletVM.Toilet.Guid); // Асинхронно удаляем из хранилища
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
                    await _localToiletService.UpdateToilet(SelectedToiletVM.Toilet);
                    // Обновление в БД
                    _toiletApiService.UpdateToilet(SelectedToiletVM.Toilet);
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

        private async void OnToiletsUpdated(ObservableCollection<ToiletViewModel> updatedToilets)
        {
            // Здесь можно обновить данные в зависимости от обновленных туалетов
            // Например, обновить коллекцию туалетов или UI

            if (updatedToilets != null)
                foreach (var updatedToilet in updatedToilets)
                {
                    if (updatedToilet.Toilet.Images != null)
                        await LoadPhotosDb(updatedToilet);
                }
        }
    }
} 
