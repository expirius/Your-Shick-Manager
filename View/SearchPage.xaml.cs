using Mapsui.Tiling;
using Mapsui;
using Map = Mapsui.Map;
using Mapsui.Extensions;
using Mapsui.UI.Maui;
using Microsoft.Maui.ApplicationModel;
using Mapsui.Widgets.Zoom;
using MFASeeker.Services;
using MFASeeker.ViewModel;
 using CommunityToolkit.Mvvm.Input;

namespace MFASeeker.View;

public partial class SearchPage : ContentPage
{
    private SearchViewModel _searchViewModel;
    public SearchPage(SearchViewModel searchViewModel)
	{
        InitializeComponent();
        BindingContext = searchViewModel;
        _searchViewModel = searchViewModel;
    }


    // Удалить как будет разработан ChangeStatCheckboxCommand
    private void LocationSwitchCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        _searchViewModel.ChangeState();
    }
}