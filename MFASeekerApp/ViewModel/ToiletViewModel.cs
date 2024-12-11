using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MFASeekerApp.Services;
using MFASeekerApp.Model;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.CompilerServices;

namespace MFASeekerApp.ViewModel
{
    public partial class ToiletViewModel : ObservableObject, ICloneable
    {
        private readonly LocalImageService _localImageService = new();
        [ObservableProperty]
        private string adress;
        [ObservableProperty]
        private Toilet? toilet;
        [ObservableProperty]
        private ObservableCollection<ImageSource> imageSources;
        [ObservableProperty]
        private ObservableCollection<string> imagePaths;
        [ObservableProperty]
        private ImageSource? imagePreviewSource = null;

        public ToiletViewModel(Toilet _toilet)
        {
            Toilet = _toilet;
            Adress = string.Empty;
            ImageSources = [];
            ImagePaths = [];
        }
        [RelayCommand]
        private async Task AddLocalImageAsync()
        {
            var fileResult = await _localImageService.TakePhoto();
            if (fileResult != null)
            {
                
                ImageFile? imageFile = await _localImageService.Upload(fileResult);
                if (imageFile != null)
                {
                    Toilet?.Images?.Add(imageFile);
                    AddImageSource(imageFile);
                }

            }
        }
        public async void AddImageSource(ImageFile imageFile)
        {
            await Task.Run(() =>
            {
                LocalImageService imageService = new();
                var temp = ImageSource.FromStream(() => imageService.ByteArrayToStream(Convert.FromBase64String(imageFile.ByteBase64)));
                ImageSources.Add(temp);
            });
        }
        /* LOADLOCALIMAGESOURCES
        public async void LoadLocalImageSources()
        {
            await Task.Run(() =>
            {
                LocalImageService imageService = new();
                foreach (var image in this.Toilet.Images)
                {
                    var imageSource = ImageSource.FromStream(() => imageService.ByteArrayToStream(Convert.FromBase64String(image.ByteBase64)));
                    this.ImageSources.Add(imageSource);
                }
            });
        } */
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
