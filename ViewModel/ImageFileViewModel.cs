using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MFASeeker.Model;
using MFASeeker.Services;

namespace MFASeeker.ViewModel
{
    public partial class ImageFileViewModel : ObservableObject
    {
        private readonly ImageFile _imageFile;

        public ImageFileViewModel(ImageFile imageFile)
        {
            _imageFile = imageFile;
        }

        [ObservableProperty]
        private ImageSource? imageSource;
        [ObservableProperty]
        private bool isLoading;
        [ObservableProperty]
        private string? errorMessage;

        // Команда для загрузки изображения
        [RelayCommand]
        public async Task LoadImageAsync()
        {
            IsLoading = true;
            ErrorMessage = null;

            try
            { ImageSource = await Task.Run(() => GetImageSource()); }
            catch (System.Exception ex)
            { ErrorMessage = $"Ошибка при загрузке изображения: {ex.Message}"; }
            finally
            { IsLoading = false; }
        }

        // Возвращаем путь к кэшированному изображению
        private string GetImageCachePath()
        {
            var cachePath = Path.Combine(FileSystem.CacheDirectory, _imageFile.FileName);
            if (!File.Exists(cachePath)) // если нет в директории
            {
                return "";
                /*LocalImageService imageService = new();
                //return Task.Run(async () =>
                //{
                //    return await imageService.SaveByteArrayToFile(Convert.FromBase64String(_imageFile.ByteBase64), _imageFile.FileName);
                //}).GetAwaiter().GetResult(); */
            }
            else { return cachePath; }
        }

        // Получаем ImageSource для отображения
        private ImageSource GetImageSource()
        {
            string cachePath = GetImageCachePath();
            return ImageSource.FromFile(cachePath);
        }
    }
}
