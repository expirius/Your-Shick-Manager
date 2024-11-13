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

        public ToiletViewModel(Toilet toilet)
        {
            Adress = string.Empty;
            ImageSources = [];
            Toilet = toilet;
            ImagePaths = [];
            //SetPreviewImageSource();
            //LoadLocalImageSources();
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
        public async void SetPreviewImageSource()
        {
            await Task.Run(() =>
            {
                if (this.Toilet?.Images.Count == 0)
                {

                    this.ImagePreviewSource = ImageSource.FromFile("Images/toilet_undefined.png");
                }
                else
                {
                    LocalImageService imageService = new();
                    var previewIS = ImageSource.FromStream(() => imageService.ByteArrayToStream(Convert.FromBase64String(this.Toilet.Images[0].ByteBase64)));
                    this.ImagePreviewSource = previewIS;
                }
            });
        }
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
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
