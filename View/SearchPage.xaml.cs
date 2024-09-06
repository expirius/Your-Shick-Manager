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

    public SearchPage(SearchViewModel searchViewModel)
	{
        BindingContext = searchViewModel;
        InitializeComponent();
        /*
        MapControl mapControl = new();
        mapControl.Map = MapService.CreateSearchMapAsync().Result;

        mapView.Content = mapControl.Content; //new MapControl
        */
    }
}