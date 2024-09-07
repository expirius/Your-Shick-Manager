using Mapsui.Tiling;
using Mapsui;
using Map = Mapsui.Map;
using Mapsui.Extensions;
using Mapsui.UI.Maui;
using Microsoft.Maui.ApplicationModel;
using Mapsui.Widgets.Zoom;
using MFASeeker.Services;
using MFASeeker.ViewModel;

namespace MFASeeker.View;

public partial class SearchPage : ContentPage
{
    private SearchViewModel _searchViewModel;
    public SearchPage(SearchViewModel searchViewModel)
	{
        BindingContext = searchViewModel;
        _searchViewModel = searchViewModel;
        InitializeComponent();
    }

    private void OnLocationSwitchCheckBox_Clicked(object sender, CheckedChangedEventArgs e)
    {
        _searchViewModel.ChangeState();
    }
}