using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MFASeeker.Model;
using MFASeeker.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MFASeeker.ViewModel
{
    public partial class ToiletViewModel : ObservableObject, System.ICloneable
    {
        private readonly LocalImageService _localImageService = new();
        public Toilet? Toilet { get; private set; }

        public ToiletViewModel(Toilet toilet)
        {
            Toilet = toilet;
        }

        [RelayCommand]
        private async Task AddImageAsync()
        {
            var fileResult = await _localImageService.TakePhoto();
            if (fileResult != null)
            {
                ImageFile? imageFile = await _localImageService.Upload(fileResult);
                if (imageFile != null)
                {
                    Toilet?.Images?.Add(imageFile);
                    Console.WriteLine("Image added: " + imageFile.FileName);
                }
            }
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
