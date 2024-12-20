using Mapsui.Tiling;
using Mapsui;
using Map = Mapsui.Map;
using Mapsui.Extensions;
using Mapsui.UI.Maui;
using Microsoft.Maui.ApplicationModel;
using Mapsui.Widgets.Zoom;
using MFASeekerApp.Services;
using MFASeekerApp.ViewModel;
 using CommunityToolkit.Mvvm.Input;
using MFASeekerApp.View.Controls;
using CommunityToolkit.Maui.Views;

namespace MFASeekerApp.View;

public partial class SearchPage : ContentPage
{
    private SearchViewModel _searchViewModel;
    public SearchPage(SearchViewModel searchViewModel)
	{
        InitializeComponent();
        BindingContext = searchViewModel;
        _searchViewModel = searchViewModel;
    }
}