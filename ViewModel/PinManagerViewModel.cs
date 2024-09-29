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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MFASeeker.Model;

namespace MFASeeker.ViewModel
{
    public partial class PinManagerViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Toilet>? activePinList;

        public PinManagerViewModel()
        {
            ActivePinList = [];
            JsonPinStorage jsonPinStorage = new();
            _ = UpdatePins();
        }

        [RelayCommand]
        private async Task UpdatePins()
        {
            JsonPinStorage jsonPinStorage = new();
            ActivePinList?.Clear();

            ActivePinList = (await jsonPinStorage.GetMarkersAsync())
                .OrderByDescending(toilet => toilet.CreatedDate)
                .ToObservableCollection(); 
        }
        [RelayCommand]
        private void DeletePin()
        {
            
        }
        [RelayCommand]
        private void EditPin()
        {

        }
    }
} 
