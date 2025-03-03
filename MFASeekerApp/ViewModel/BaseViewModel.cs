using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Entities;
using Mapsui.UI.Maui;
using MFASeekerApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeekerApp.ViewModel
{
    public partial class BaseViewModel : ObservableObject
    {
        //private static BaseViewModel _instance;
        //public static BaseViewModel Instance => _instance ??= new BaseViewModel();
        [ObservableProperty]
        private User? authUser = null;
    }
}
