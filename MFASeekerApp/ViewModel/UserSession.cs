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
    public partial class UserSession : ObservableObject
    {
        //private static UserSession _instance;
        //public static UserSession Instance => _instance ??= new UserSession();
        [ObservableProperty]
        private User? authUser = null;
    }
}
